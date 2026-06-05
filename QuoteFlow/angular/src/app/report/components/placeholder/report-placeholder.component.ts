import { CommonModule, Location } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ReportMenuService } from '../../services/report-menu.service';
import { AppRoutes } from '@app/app.routes';

@Component({
  selector: 'app-report-placeholder',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="d-flex justify-content-center align-items-center p-5">
      <div class="text-center">
        <i class="bi spinner-border text-primary mb-3" style="font-size: 2rem"></i>
        <p class="text-muted">Redirecting...</p>
      </div>
    </div>
  `,
})
export class ReportPlaceholderComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly location = inject(Location);
  private readonly reportMenuService = inject(ReportMenuService);

  ngOnInit(): void {
    const currentUrl = this.router.url;
    if (!currentUrl.includes('spo-detailed-report') && !currentUrl.includes('general-report')) {
      this.location.back();
      return;
    }
    this.reportMenuService.openReportDialog(currentUrl);

    setTimeout(() => {
      this.location.back();
    }, 100);
  }
}
