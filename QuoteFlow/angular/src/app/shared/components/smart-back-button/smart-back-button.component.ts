import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { NavigationHistoryService } from '../../services/navigation-history/navigation-history.service';
import { NavigationFallbackService } from '../../services/navigation-history/navigation-fallback.service';

@Component({
  selector: 'app-smart-back-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './smart-back-button.component.html',
  styleUrls: ['./smart-back-button.component.scss'],
})
export class SmartBackButtonComponent implements OnInit, OnDestroy {
  @Input() fallbackUrl?: string | string[];
  @Input() buttonText: string = 'Back';
  @Input() buttonClass: string = 'btn btn-secondary';
  @Input() iconClass: string = 'fas fa-arrow-left';
  @Input() disabled: boolean = false;
  @Input() showIcon: boolean = true;

  canGoBack = false;
  private destroy$ = new Subject<void>();

  constructor(
    private navigationHistoryService: NavigationHistoryService,
    private navigationFallbackService: NavigationFallbackService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    // Subscribe to can go back state
    this.navigationHistoryService.canGoBack.pipe(takeUntil(this.destroy$)).subscribe(canGoBack => {
      this.canGoBack = canGoBack;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Handle back button click
   */
  onBackClick(): void {
    if (this.disabled) {
      return;
    }

    // Use provided fallback URL or auto-determine from current context
    const fallbackUrl = this.fallbackUrl || this.navigationFallbackService.getSmartFallbackUrl(this.router.url);
    this.navigationHistoryService.smartBack(fallbackUrl);
  }

  /**
   * Get preview of where the back button will navigate
   */
  getBackPreview(): string {
    const baseContext = this.navigationHistoryService.getCurrentBaseContext();
    const fallbackUrl = this.fallbackUrl || this.navigationFallbackService.getSmartFallbackUrl(this.router.url);
    return this.navigationHistoryService.generateSmartBackUrl(baseContext, fallbackUrl);
  }
}
