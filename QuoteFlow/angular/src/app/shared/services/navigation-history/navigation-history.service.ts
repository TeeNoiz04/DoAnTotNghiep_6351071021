import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { filter, map } from 'rxjs/operators';

interface NavigationHistoryItem {
  url: string;
  timestamp: number;
  baseContext?: string;
}

@Injectable({
  providedIn: 'root',
})
export class NavigationHistoryService {
  private readonly maxHistorySize = 50;
  private navigationHistory: NavigationHistoryItem[] = [];
  private canGoBack$ = new BehaviorSubject<boolean>(false);

  constructor(
    private router: Router,
    private location: Location,
  ) {
    this.initializeHistoryTracking();
  }

  /**
   * Initialize navigation history tracking
   */
  private initializeHistoryTracking(): void {
    // First, try to restore from session storage
    this.restoreFromSessionStorage();

    // Add current URL to history if not already present
    const currentUrl = this.router.url;

    if (
      currentUrl &&
      (this.navigationHistory.length === 0 ||
        this.navigationHistory[this.navigationHistory.length - 1].url !== currentUrl)
    ) {
      this.addToHistory(currentUrl);
    }

    // Track navigation events
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(event => event as NavigationEnd),
      )
      .subscribe(event => {
        this.addToHistory(event.url);
      });

    // Check initial browser history state
    this.updateCanGoBackState();
  }

  /**
   * Add a URL to navigation history
   */
  private addToHistory(url: string): void {
    const historyItem: NavigationHistoryItem = {
      url,
      timestamp: Date.now(),
      baseContext: this.extractBaseContext(url),
    };

    // Remove duplicate consecutive entries
    if (
      this.navigationHistory.length > 0 &&
      this.navigationHistory[this.navigationHistory.length - 1].url === url
    ) {
      return;
    }

    this.navigationHistory.push(historyItem);

    // Keep history size manageable
    if (this.navigationHistory.length > this.maxHistorySize) {
      this.navigationHistory.shift();
    }

    this.updateCanGoBackState();
    this.saveToSessionStorage();
  }

  /**
   * Save navigation history to session storage
   */
  private saveToSessionStorage(): void {
    try {
      const dataToSave = {
        history: this.navigationHistory,
        timestamp: Date.now(),
      };
      sessionStorage.setItem('navigation-history', JSON.stringify(dataToSave));
    } catch (error) {
      console.warn('Failed to save navigation history to session storage:', error);
    }
  }

  /**
   * Extract base context from URL (e.g., 'materials' from '/materials/list')
   */
  private extractBaseContext(url: string): string {
    const segments = url.split('/').filter(segment => segment.length > 0);
    return segments[0] || '';
  }

  /**
   * Check if we can go back within the same base context
   */
  private canGoBackWithinContext(baseContext: string): boolean {
    if (this.navigationHistory.length < 2) {
      return false;
    }

    const current = this.navigationHistory[this.navigationHistory.length - 1];
    const previous = this.navigationHistory[this.navigationHistory.length - 2];

    return current.baseContext === baseContext && previous.baseContext === baseContext;
  }

  /**
   * Update the can go back state
   */
  private updateCanGoBackState(): void {
    const canGoBack = this.navigationHistory.length > 1;
    this.canGoBack$.next(canGoBack);
  }

  /**
   * Get the previous URL from history within the same base context
   * Excludes detail pages to prevent back navigation between detail pages
   */
  private getPreviousUrlInContext(baseContext: string): string | null {
    if (this.navigationHistory.length < 2) {
      return null;
    }

    // Work backwards through history to find the last URL in the same context
    // that is NOT a detail page
    for (let i = this.navigationHistory.length - 2; i >= 0; i--) {
      const item = this.navigationHistory[i];
      if (item.baseContext === baseContext && !this.isDetailPageUrl(item.url)) {
        return item.url;
      }
    }

    return null;
  }

  /**
   * Check if browser has history (not a fresh page load)
   */
  private hasBrowserHistory(): boolean {
    return window.history.length > 1;
  }

  /**
   * Validate URL format and ensure it's a valid application route
   */
  private isValidApplicationRoute(url: string): boolean {
    try {
      const urlObj = new URL(url, window.location.origin);

      // Check if it's the same origin
      if (urlObj.origin !== window.location.origin) {
        return false;
      }

      // Check if it follows our application route patterns
      const pathname = urlObj.pathname.substring(1); // Remove leading slash
      const segments = pathname.split('/').filter(segment => segment.length > 0);

      // Should have at least one segment for base context
      if (segments.length === 0) {
        return false;
      }

      // Check against known route patterns
      const knownBaseRoutes = [
        'materials',
        'price-offer',
        'dpo',
        'gic',
        'psis',
        'key-accounts',
        'stock-management',
        'cargos',
        'system-categories',
        'buyers',
        'customers',
        'import-allocations',
        'home',
        'dashboard',
      ];

      return knownBaseRoutes.includes(segments[0]);
    } catch {
      return false;
    }
  }

  /**
   * Smart back navigation with fallback logic
   */
  public smartBack(fallbackUrl: string | string[]): void {
    const currentUrl = this.router.url;
    const baseContext = this.extractBaseContext(currentUrl);

    // Strategy 1: Try to use previous URL from our tracked history within the same context
    let previousUrl = this.getPreviousUrlInContext(baseContext);

    if (previousUrl && previousUrl.includes('/new')) {
      previousUrl = `/${baseContext}/list`;
      this.router.navigateByUrl(previousUrl);
      return;
    }

    if (previousUrl && previousUrl !== currentUrl && this.isValidApplicationRoute(previousUrl)) {
      this.router.navigateByUrl(previousUrl);
      return;
    }

    // Strategy 2: If we have browser history and we're in the same context, try browser back
    // This handles cases where our tracking might have missed the navigation
    if (
      this.hasBrowserHistory() &&
      window.history.length > 1 &&
      !this.isLikelyDirectDetailAccess(currentUrl)
    ) {
      this.tryBrowserBackWithSafetyCheck(fallbackUrl, baseContext);
      return;
    }

    // Strategy 3: Fallback to provided URL with history replacement for direct access
    if (this.isLikelyDirectDetailAccess(currentUrl)) {
      this.navigateToFallbackWithReplacement(fallbackUrl);
    } else {
      this.navigateToFallback(fallbackUrl);
    }
  }

  /**
   * Try browser back with safety check to ensure we stay in the same context
   * and don't navigate to another detail page
   */
  private tryBrowserBackWithSafetyCheck(
    fallbackUrl: string | string[],
    expectedContext: string,
  ): void {
    const currentUrl = this.router.url;

    // Listen for the next navigation event to check if we went to the right place
    const navigationSub = this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(event => event as NavigationEnd),
      )
      .subscribe(event => {
        navigationSub.unsubscribe();

        const newContext = this.extractBaseContext(event.url);
        const isDetailPage = this.isDetailPageUrl(event.url);

        // If we didn't go to the right context OR we went to another detail page, navigate to fallback
        if (newContext !== expectedContext || isDetailPage) {
          this.navigateToFallback(fallbackUrl);
        }
      });

    // Set up a timeout in case navigation doesn't happen
    setTimeout(() => {
      navigationSub.unsubscribe();
      if (this.router.url === currentUrl) {
        this.navigateToFallback(fallbackUrl);
      }
    }, 500);

    // Perform the browser back
    this.location.back();
  }

  /**
   * Navigate to fallback URL
   */
  private navigateToFallback(fallbackUrl: string | string[]): void {
    if (Array.isArray(fallbackUrl)) {
      this.router.navigate(fallbackUrl);
    } else {
      this.router.navigateByUrl(fallbackUrl);
    }
  }

  /**
   * Navigate to fallback URL with history replacement to prevent back to new tab
   */
  private navigateToFallbackWithReplacement(fallbackUrl: string | string[]): void {
    if (Array.isArray(fallbackUrl)) {
      this.router.navigate(fallbackUrl, { replaceUrl: true });
    } else {
      this.router.navigateByUrl(fallbackUrl, { replaceUrl: true });
    }
  }

  /**
   * Check if a URL is a detail page
   */
  private isDetailPageUrl(url: string): boolean {
    // Check for common detail page patterns:
    // 1. Contains UUID pattern followed by 'details' (e.g., /price-offer/uuid/details)
    // 2. Contains UUID pattern at the end (e.g., /materials/uuid)
    const hasDetailPattern =
      /\/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\/details/.test(url);
    const hasUuidAtEnd = /\/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/.test(
      url,
    );
    return hasDetailPattern || hasUuidAtEnd;
  }

  /**
   * Check if the current URL is likely a direct detail page access
   */
  private isLikelyDirectDetailAccess(currentUrl: string): boolean {
    // Check if this is a detail page (contains UUID pattern) and we have minimal history
    const isDetailPage = this.isDetailPageUrl(currentUrl);
    const hasMinimalHistory = this.navigationHistory.length <= 2;
    return isDetailPage && hasMinimalHistory;
  }

  /**
   * Restore navigation history from session storage
   */
  private restoreFromSessionStorage(): void {
    try {
      const storedData = sessionStorage.getItem('navigation-history');
      if (storedData) {
        const data = JSON.parse(storedData);

        // Handle both old and new format
        if (Array.isArray(data)) {
          // Old format - direct array
          this.navigationHistory = data.filter(
            item => Date.now() - item.timestamp < 3600000, // Only keep items from last hour
          );
        } else if (data.history && Array.isArray(data.history)) {
          // New format - object with history and timestamp
          const timeSinceLastSave = Date.now() - (data.timestamp || 0);
          if (timeSinceLastSave < 3600000) {
            // Only restore if less than 1 hour old
            this.navigationHistory = data.history.filter(
              item => Date.now() - item.timestamp < 3600000,
            );
          }
        }
      }
    } catch (error) {
      console.warn('Failed to restore navigation history from session storage:', error);
    }
  }

  /**
   * Get the current navigation history
   */
  public getHistory(): NavigationHistoryItem[] {
    return [...this.navigationHistory];
  }

  /**
   * Get can go back observable
   */
  public get canGoBack() {
    return this.canGoBack$.asObservable();
  }

  /**
   * Clear navigation history
   */
  public clearHistory(): void {
    this.navigationHistory = [];
    this.updateCanGoBackState();
  }

  /**
   * Get base context from current URL
   */
  public getCurrentBaseContext(): string {
    return this.extractBaseContext(this.router.url);
  }

  /**
   * Check if a URL is within the same base context
   */
  public isInSameContext(url: string, baseContext?: string): boolean {
    const urlContext = this.extractBaseContext(url);
    const currentContext = baseContext || this.getCurrentBaseContext();
    return urlContext === currentContext;
  }

  /**
   * Generate smart back URL for a given context and fallback
   */
  public generateSmartBackUrl(baseContext: string, fallbackUrl: string | string[]): string {
    const previousUrl = this.getPreviousUrlInContext(baseContext);
    if (previousUrl) {
      return previousUrl;
    }

    if (Array.isArray(fallbackUrl)) {
      return '/' + fallbackUrl.join('/');
    }

    return fallbackUrl;
  }
}
