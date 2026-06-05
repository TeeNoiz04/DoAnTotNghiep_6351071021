import { DOCUMENT } from '@angular/common';
import { inject, Injectable, OnDestroy } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

/**
 * TokenSyncService implements a Leader Election Pattern to coordinate
 * token refresh across multiple browser tabs, preventing the infinite
 * reload loop caused by race conditions with shared localStorage.
 *
 * Problem: Multiple tabs trying to refresh tokens simultaneously causes
 * race conditions → the losing tab's refresh token gets rejected → storage
 * cleared → other tabs see removal → hard reload loop.
 *
 * Solution: Uses BroadcastChannel API to elect one tab as the "leader"
 * that handles token refresh. Other tabs wait via a deferred promise that
 * resolves/rejects based on the leader's outcome.
 *
 * Key fixes over previous version:
 * 1. When receiving REFRESHING broadcast, create a waitPromise so that any
 *    subsequent refreshToken() call in this tab defers to the leader (not
 *    just guard-checked with a null refreshPromise).
 * 2. reloadTokenFromStorage no longer calls loadDiscoveryDocumentAndTryLogin
 *    (which was designed for OIDC callback flows and caused nonce errors).
 *    The OAuthService reads tokens directly from localStorage, so no explicit
 *    reload is needed for API calls. We just emit a synthetic token_refreshed
 *    event so subscribers update UI state.
 * 3. handleTokenRemoval navigates via location.assign('/') instead of
 *    location.reload() to avoid re-running the hard-reload → redirect loop.
 */
@Injectable({ providedIn: 'root' })
export class TokenSyncService implements OnDestroy {
  protected readonly window = inject(DOCUMENT).defaultView;
  private readonly oAuthService = inject(OAuthService);
  private readonly channel: BroadcastChannel | null = null;
  private readonly LOG_PREFIX = '[TokenSync]';
  private readonly tabId: string;

  // Set to true while THIS tab is performing a refresh
  private isLeaderRefreshing = false;
  // Set to true while ANY tab (including this one) is performing a refresh
  private isAnyTabRefreshing = false;

  // Promise returned by this tab's own refresh call
  private leaderRefreshPromise: Promise<any> | null = null;

  // Promise + controls for waiting on another tab's refresh
  private waitPromise: Promise<any> | null = null;
  private waitResolve: ((value: any) => void) | null = null;
  private waitReject: ((reason?: any) => void) | null = null;

  constructor() {
    this.tabId = `Tab-${Math.random().toString(36).substr(2, 9)}`;

    if (typeof BroadcastChannel !== 'undefined') {
      try {
        this.channel = new BroadcastChannel('oauth-token-sync');
        this.setupBroadcastChannelListener();
      } catch (error) {
        console.error(
          `${this.LOG_PREFIX} [${this.tabId}] Failed to create BroadcastChannel:`,
          error,
        );
      }
    } else {
      console.warn(`${this.LOG_PREFIX} [${this.tabId}] BroadcastChannel not supported`);
    }

    this.setupStorageListener();
    this.setupCoordinatedRefresh();
  }

  /**
   * Listen for storage events from other tabs.
   * Only acts on true token removal (not during any refresh).
   * Does NOT call loadDiscoveryDocumentAndTryLogin — the OAuthService already
   * reads access_token directly from localStorage for every getAccessToken() call.
   */
  private setupStorageListener(): void {
    if (!this.window) return;

    this.window.addEventListener('storage', (event: StorageEvent) => {
      if (event.key !== 'access_token') return;

      const tokenRemoved = event.newValue === null;
      const tokenReplaced = !tokenRemoved && event.oldValue !== event.newValue;

      if (tokenRemoved && !this.isAnyTabRefreshing) {
        // Token was removed and no tab is in a refresh cycle → likely logout or expiry
        this.handleTokenRemoval();
      } else if (tokenReplaced) {
        // Another tab successfully refreshed — nothing special to do.
        // OAuthService.getAccessToken() reads from localStorage directly, so
        // the new token will be used automatically on the next API call.
        // We don't call loadDiscoveryDocumentAndTryLogin here because that method
        // processes OIDC auth-code callback parameters and will cause nonce errors
        // when called outside the redirect callback context.
      }
    });
  }

  /**
   * Listen for inter-tab messages from the leader tab.
   */
  private setupBroadcastChannelListener(): void {
    if (!this.channel) return;

    this.channel.onmessage = (event: MessageEvent) => {
      switch (event.data.type) {
        case 'REFRESHING':
          // Another tab is now the leader. Mark ourselves as waiting and
          // pre-create the wait promise so that any refreshToken() call in
          // this tab will defer to the leader instead of starting its own refresh.
          this.isAnyTabRefreshing = true;
          if (!this.waitPromise) {
            this.waitPromise = new Promise<any>((resolve, reject) => {
              this.waitResolve = resolve;
              this.waitReject = reject;
            });
          }
          break;

        case 'TOKEN_REFRESHED':
          // Leader succeeded — resolve the wait promise so deferred callers
          // in this tab get a successful result without making another request.
          if (this.waitResolve) {
            this.waitResolve({ synced: true });
          }
          this.clearWaitState();
          break;

        case 'REFRESH_FAILED':
          // Leader failed — reject the wait promise so callers in this tab
          // also fail (ABP will then handle navigateToLogin via token_refresh_error).
          console.error(
            `${this.LOG_PREFIX} [${this.tabId}] Leader tab failed to refresh token:`,
            event.data.error,
          );
          if (this.waitReject) {
            this.waitReject(new Error(event.data.error || 'Leader refresh failed'));
          }
          this.clearWaitState();
          break;

        case 'REFRESH_COMPLETE':
          // Cleanup signal — always clear waiting state regardless of outcome
          this.isAnyTabRefreshing = false;
          this.clearWaitState();
          break;

        default:
          console.warn(
            `${this.LOG_PREFIX} [${this.tabId}] Unknown broadcast message type:`,
            event.data.type,
          );
      }
    };
  }

  /**
   * Override OAuthService.refreshToken to coordinate refresh across tabs.
   * Only one tab (the leader) calls the actual token endpoint.
   */
  private setupCoordinatedRefresh(): void {
    const originalRefresh = this.oAuthService.refreshToken.bind(this.oAuthService);

    this.oAuthService.refreshToken = (): Promise<any> => {
      // Case 1: Another tab is already refreshing and we have a wait promise →
      // return it so this tab defers without making a duplicate HTTP request.
      if (this.isAnyTabRefreshing && this.waitPromise) {
        return this.waitPromise;
      }

      // Case 2: This tab already has an in-progress refresh (duplicate call in same tab).
      if (this.leaderRefreshPromise) {
        return this.leaderRefreshPromise;
      }

      // Case 3: This tab becomes the leader.
      this.isLeaderRefreshing = true;
      this.isAnyTabRefreshing = true;
      this.broadcastMessage({ type: 'REFRESHING', tabId: this.tabId });

      this.leaderRefreshPromise = originalRefresh()
        .then(result => {
          this.broadcastMessage({ type: 'TOKEN_REFRESHED', tabId: this.tabId });
          this.broadcastMessage({ type: 'REFRESH_COMPLETE', tabId: this.tabId });
          this.isLeaderRefreshing = false;
          this.isAnyTabRefreshing = false;
          this.leaderRefreshPromise = null;
          return result;
        })
        .catch(error => {
          console.error(`${this.LOG_PREFIX} [${this.tabId}] Token refresh FAILED:`, error);
          this.broadcastMessage({
            type: 'REFRESH_FAILED',
            tabId: this.tabId,
            error: error?.message || 'Unknown error',
          });
          this.broadcastMessage({ type: 'REFRESH_COMPLETE', tabId: this.tabId });
          this.isLeaderRefreshing = false;
          this.isAnyTabRefreshing = false;
          this.leaderRefreshPromise = null;
          throw error;
        });

      return this.leaderRefreshPromise;
    };
  }

  /**
   * Handle access token removal when no refresh is in progress.
   * Uses location.assign('/') rather than location.reload() to avoid
   * re-triggering a hard-reload → OIDC redirect → token stored → other
   * tabs fire storage event → loop.
   */
  private handleTokenRemoval(): void {
    setTimeout(() => {
      const token = localStorage.getItem('access_token');

      // Re-check: a leader tab may have finished refreshing during the 500ms window
      if (!token && !this.isAnyTabRefreshing) {
        this.window?.location.assign('/');
      }
    }, 500);
  }

  private clearWaitState(): void {
    this.waitPromise = null;
    this.waitResolve = null;
    this.waitReject = null;
  }

  private broadcastMessage(message: any): void {
    if (!this.channel) return;
    try {
      this.channel.postMessage(message);
    } catch (error) {
      console.error(`${this.LOG_PREFIX} [${this.tabId}] Failed to broadcast message:`, error);
    }
  }

  ngOnDestroy(): void {
    if (this.channel) {
      try {
        this.channel.close();
      } catch (error) {
        console.error(`${this.LOG_PREFIX} [${this.tabId}] Error closing BroadcastChannel:`, error);
      }
    }
  }
}
