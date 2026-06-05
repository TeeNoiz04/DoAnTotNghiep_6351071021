import { PageModule } from '@abp/ng.components/page';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { DashboardService } from '@app/proxy/dashboards';
import { ApprovalDashboardItemDto } from '@app/proxy/dashboards/models';
import { TitleService } from '@app/shared/services/title/title.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-approval-dashboard',
  templateUrl: './approval-dashboard.component.html',
  styleUrls: ['./approval-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, PageModule, RouterModule],
})
export class ApprovalDashboardComponent implements OnInit {
  private readonly titleService = inject(TitleService);
  private readonly dashboardService = inject(DashboardService);
  private readonly router = inject(Router);

  dashboardItems: ApprovalDashboardItemDto[] = [];
  loading: boolean = false;

  ngOnInit(): void {
    this.titleService.setTitle('Approval Dashboard');
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loading = true;
    this.dashboardService
      .getApprovalDashboard()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (response: ApprovalDashboardItemDto[]) => {
          this.dashboardItems = response;
        },
        error: error => {
          console.error('Failed to load approval dashboard:', error);
        },
      });
  }

  navigateToInApproval(title: string): void {
    const route = this.getInApprovalRouteForTitle(title);

    if (route) {
      // 1. Get the full URL from the Angular Router
      const url = this.router.serializeUrl(this.router.createUrlTree(route.path, { queryParams: route.params }));

      // 2. Open the URL in a new tab
      // The second parameter '_blank' tells the browser to open a new tab/window.
      window.open(url, '_blank');
    }
  }

  navigateToSetProjectResult(): void {
    // Navigate to price offer list with PreOrder filter
    this.router.navigate(['/', AppRoutes.SPECIAL_PRICE_OFFERS.BASE, AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE], {
      queryParams: {
        project_result_status: 'PRE_ORDER',
      },
    });
  }

  private getInApprovalRouteForTitle(title: string): { path: string[]; params: any } | null {
    switch (title) {
      case 'SPO':
        return {
          path: ['/', AppRoutes.SPECIAL_PRICE_OFFERS.BASE, AppRoutes.SPECIAL_PRICE_OFFERS.MY_APPROVALS.BASE],
          params: {},
        };
      case 'DPO':
        return {
          path: ['/', AppRoutes.DPO.BASE, AppRoutes.DPO.LIST.BASE],
          params: { status: 'SUBMITTED' },
        };
      case 'GKR':
        return {
          path: ['/', AppRoutes.GKR.BASE, AppRoutes.GKR.APPROVALS.BASE],
          params: {},
        };
      case 'Material Data Import':
        return {
          path: ['/', AppRoutes.MATERIAL_STOCK.BASE, AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.BASE],
          params: {},
        };
      case 'Key Account':
        return {
          path: ['/', AppRoutes.KEY_ACCOUNTS.BASE, AppRoutes.KEY_ACCOUNTS.MY_APPROVALS.BASE],
          params: {},
        };
      case 'PSI':
        return {
          path: ['/', AppRoutes.PSI.BASE, AppRoutes.PSI.MY_APPROVALS.BASE],
          params: {},
        };
      default:
        return null;
    }
  }

  getCardClass(title: string): string {
    const classMap: { [key: string]: string } = {
      SPO: 'border-primary',
      DPO: 'border-success',
      GKR: 'border-info',
      'Material Data Import': 'border-warning',
      'Key Account': 'border-danger',
      PSI: 'border-secondary',
    };
    return classMap[title] || 'border-light';
  }

  getIconClass(title: string): string {
    const iconMap: { [key: string]: string } = {
      SPO: 'bi bi-file-earmark-text text-primary',
      DPO: 'bi bi-cart-check text-success',
      GKR: 'bi bi-box-seam text-info',
      'Material Data Import': 'fas fa-boxes text-warning',
      'Key Account': 'bi bi-people text-danger',
      PSI: 'bi bi-graph-up text-secondary',
    };
    return iconMap[title] || 'bi bi-clipboard-data';
  }
}
