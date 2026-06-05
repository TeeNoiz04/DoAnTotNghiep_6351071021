import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface KeyboardNavigationConfig {
  keys: KeyInfo[];
  position?: 'top-right' | 'bottom-left' | 'bottom-right' | 'center-bottom';
  showMode?: 'always' | 'first-time' | 'smart' | 'toggleable';
  autoHideDelay?: number; // in milliseconds
  storageKey?: string; // for remembering user preferences
}

export interface KeyInfo {
  key: string;
  icon?: string;
  description: string;
  action?: string;
}

@Injectable({
  providedIn: 'root',
})
export class KeyboardNavigationHintService {
  private readonly STORAGE_PREFIX = 'keyboard_hint_';
  private hintVisibility$ = new BehaviorSubject<{ [modalId: string]: boolean }>({});

  /**
   * Default keyboard navigation configurations for common modal types
   */
  static readonly PRESETS: { [key: string]: KeyboardNavigationConfig } = {
    NAVIGATION_MODAL: {
      keys: [
        { key: '←', icon: 'bi-arrow-left', description: 'Previous Item', action: 'previous' },
        { key: '→', icon: 'bi-arrow-right', description: 'Next Item', action: 'next' },
      ],
      position: 'bottom-right' as const,
      showMode: 'always' as const,
      autoHideDelay: 5000,
    },
    EXTENDED_NAVIGATION: {
      keys: [
        { key: '←', icon: 'bi-arrow-left', description: 'Previous', action: 'previous' },
        { key: '→', icon: 'bi-arrow-right', description: 'Next', action: 'next' },
        { key: 'PgUp', icon: 'bi-arrow-up-square', description: 'Previous', action: 'previous' },
        { key: 'PgDn', icon: 'bi-arrow-down-square', description: 'Next', action: 'next' },
        { key: 'Esc', icon: 'bi-x-circle', description: 'Close', action: 'close' },
      ],
      position: 'bottom-right' as const,
      showMode: 'smart' as const,
      autoHideDelay: 6000,
    },
  };

  /**
   * Check if hint should be shown based on configuration
   */
  shouldShowHint(modalId: string, config: KeyboardNavigationConfig): boolean {
    const storageKey = config.storageKey || `${this.STORAGE_PREFIX}${modalId}`;

    switch (config.showMode) {
      case 'always':
        return true;

      case 'first-time': {
        const hasBeenShown = localStorage.getItem(storageKey);
        if (!hasBeenShown) {
          localStorage.setItem(storageKey, 'true');
          return true;
        }
        return false;
      }

      case 'smart': {
        const dismissCount = parseInt(localStorage.getItem(`${storageKey}_dismiss_count`) || '0');
        const lastShown = parseInt(localStorage.getItem(`${storageKey}_last_shown`) || '0');
        const now = Date.now();

        // Show for first 3 times, then show again after 7 days
        if (dismissCount < 3) {
          return true;
        } else if (now - lastShown > 7 * 24 * 60 * 60 * 1000) {
          // 7 days
          return true;
        }
        return false;
      }

      case 'toggleable': {
        const currentVisibility = this.hintVisibility$.value;
        return currentVisibility[modalId] !== false;
      } // default to true

      default:
        return true;
    }
  }

  /**
   * Mark hint as dismissed
   */
  dismissHint(modalId: string, config: KeyboardNavigationConfig): void {
    const storageKey = config.storageKey || `${this.STORAGE_PREFIX}${modalId}`;

    if (config.showMode === 'smart') {
      const dismissCount = parseInt(localStorage.getItem(`${storageKey}_dismiss_count`) || '0');
      localStorage.setItem(`${storageKey}_dismiss_count`, (dismissCount + 1).toString());
      localStorage.setItem(`${storageKey}_last_shown`, Date.now().toString());
    } else if (config.showMode === 'toggleable') {
      const currentVisibility = this.hintVisibility$.value;
      this.hintVisibility$.next({
        ...currentVisibility,
        [modalId]: false,
      });
    }
  }

  /**
   * Toggle hint visibility (for toggleable mode)
   */
  toggleHint(modalId: string): void {
    const currentVisibility = this.hintVisibility$.value;
    const newVisibility = !currentVisibility[modalId];

    this.hintVisibility$.next({
      ...currentVisibility,
      [modalId]: newVisibility,
    });
  }

  /**
   * Check if hint is currently visible (for toggleable mode)
   */
  isHintVisible(modalId: string): boolean {
    return this.hintVisibility$.value[modalId] !== false;
  }

  /**
   * Reset all stored preferences (for testing/admin purposes)
   */
  resetAllHints(): void {
    const keys = Object.keys(localStorage).filter(key => key.startsWith(this.STORAGE_PREFIX));
    keys.forEach(key => localStorage.removeItem(key));
    this.hintVisibility$.next({});
  }
}
