import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import { Subject, takeUntil, timer } from 'rxjs';
import {
  KeyboardNavigationConfig,
  KeyboardNavigationHintService,
} from '../../services/keyboard-navigation-hint.service';

@Component({
  selector: 'app-keyboard-navigation-hint',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      *ngIf="isVisible"
      class="keyboard-navigation-hint"
      [class]="'position-' + config.position"
      [attr.data-modal-id]="modalId">
      <!-- Approach 1: Minimalist floating tooltip -->
      <div *ngIf="displayStyle === 'minimal'" class="hint-minimal">
        <div class="hint-keys">
          <span *ngFor="let keyInfo of config.keys; let last = last" class="key-combo">
            <kbd class="key">{{ keyInfo.key }}</kbd>
            <span class="action">{{ keyInfo.description }}</span>
            <span *ngIf="!last" class="separator">•</span>
          </span>
        </div>
        <button
          *ngIf="config.showMode === 'toggleable'"
          type="button"
          class="btn-close-hint"
          (click)="dismiss()"
          title="Hide keyboard hints">
          <i class="bi bi-x"></i>
        </button>
      </div>

      <!-- Approach 2: Card-style with animations -->
      <div *ngIf="displayStyle === 'card'" class="hint-card">
        <div class="hint-header">
          <i class="bi bi-keyboard hint-icon"></i>
          <span class="hint-title">Keyboard Shortcuts</span>
          <button type="button" class="btn-close-hint" (click)="dismiss()" title="Hide keyboard hints">
            <i class="bi bi-x"></i>
          </button>
        </div>
        <div class="hint-body">
          <div *ngFor="let keyInfo of config.keys" class="key-item">
            <kbd class="key">
              <i *ngIf="keyInfo.icon" [class]="keyInfo.icon"></i>
              {{ keyInfo.key }}
            </kbd>
            <span class="description">{{ keyInfo.description }}</span>
          </div>
        </div>
      </div>

      <!-- Approach 3: Interactive overlay with help toggle -->
      <div *ngIf="displayStyle === 'interactive'" class="hint-interactive">
        <button
          type="button"
          class="btn-hint-toggle"
          [class.expanded]="isExpanded"
          (click)="toggleExpanded()"
          title="Toggle keyboard shortcuts">
          <i class="bi bi-keyboard"></i>
          <span *ngIf="!isExpanded" class="badge">{{ config.keys.length }}</span>
        </button>

        <div *ngIf="isExpanded" class="hint-popup">
          <div class="hint-popup-header">
            <span class="title">Keyboard Shortcuts</span>
            <button type="button" class="btn-close" (click)="toggleExpanded()">
              <i class="bi bi-x"></i>
            </button>
          </div>
          <div class="hint-popup-body">
            <div *ngFor="let keyInfo of config.keys" class="shortcut-item">
              <div class="shortcut-key">
                <kbd>
                  <i *ngIf="keyInfo.icon" [class]="keyInfo.icon + ' me-1'"></i>
                  {{ keyInfo.key }}
                </kbd>
              </div>
              <div class="shortcut-desc">{{ keyInfo.description }}</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Approach 4: Bottom bar notification style -->
      <div *ngIf="displayStyle === 'notification'" class="hint-notification">
        <div class="notification-content">
          <i class="bi bi-info-circle notification-icon"></i>
          <div class="notification-text">
            <strong>Tip:</strong> Use
            <span *ngFor="let keyInfo of config.keys; let last = last">
              <kbd>{{ keyInfo.key }}</kbd> for {{ keyInfo.description.toLowerCase() }}<span *ngIf="!last">, </span>
            </span>
          </div>
        </div>
        <div class="notification-actions">
          <button type="button" class="btn-got-it" (click)="dismiss()">Got it</button>
          <button type="button" class="btn-close-notification" (click)="dismiss()">
            <i class="bi bi-x"></i>
          </button>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./keyboard-navigation-hint.component.scss'],
})
export class KeyboardNavigationHintComponent implements OnInit, OnDestroy {
  private readonly hintService = inject(KeyboardNavigationHintService);
  private readonly destroy$ = new Subject<void>();

  @Input() modalId!: string;
  @Input() config!: KeyboardNavigationConfig;
  @Input() displayStyle: 'minimal' | 'card' | 'interactive' | 'notification' = 'minimal';

  isVisible = false;
  isExpanded = false;

  ngOnInit(): void {
    this.isVisible = this.hintService.shouldShowHint(this.modalId, this.config);

    // Auto-hide functionality
    if (this.isVisible && this.config.autoHideDelay && this.config.showMode !== 'toggleable') {
      timer(this.config.autoHideDelay)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.isVisible = false;
          this.hintService.dismissHint(this.modalId, this.config);
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dismiss(): void {
    this.isVisible = false;
    this.hintService.dismissHint(this.modalId, this.config);
  }

  toggleExpanded(): void {
    this.isExpanded = !this.isExpanded;
  }
}
