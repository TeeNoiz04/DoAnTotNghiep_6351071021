import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ReportMenuService } from '../services/report-menu.service';
import { AppRoutes } from '@app/app.routes';
import { StockReportDialogService } from '@app/stock-management/services/stock-report-dialog.service';
import { PSIReportDialogService } from '@app/psis/services/psi-report-dialog.service';
import { SaleReportDialogService } from '@app/stock-management/services/sale-report-dialog.service';
import { SaleReportGeneralDialogService } from '@app/stock-management/services/sale-report-general-dialog.service';

/**
 * Guard that intercepts navigation to report routes and opens modal instead
 * This prevents any visual navigation flash
 */
export const reportModalGuard: CanActivateFn = (route, state) => {
  // Check which report route was accessed
  const path = route.routeConfig?.path;

  if (
    path === AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE ||
    path === AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE
  ) {
    const reportMenuService = inject(ReportMenuService);
    reportMenuService.openSaleReportDialog(path);
    return false;
  }

  if (path === AppRoutes.REPORT.INVENTORY_REPORT.INVENTORY_REPORT.BASE) {
    try {
      const stockReportService = inject(StockReportDialogService);
      stockReportService.openInventoryReportDialog();
      return false;
    } catch (error) {
      console.error('Failed to open stock report dialog:', error);
      return false;
    }
  }
  if (path === AppRoutes.REPORT.CUSTOMER_REPORT.SALE.BASE) {
    try {
      const stockReportService = inject(SaleReportDialogService);
      stockReportService.openSaleReportDialog();
      return false;
    } catch (error) {
      console.error('Failed to open stock report dialog:', error);
      return false;
    }
  }

  if (path === AppRoutes.REPORT.CUSTOMER_REPORT.SALE_R05.BASE) {
    try {
      const stockReportService = inject(SaleReportGeneralDialogService);
      stockReportService.openSaleReportDialog();
      return false;
    } catch (error) {
      console.error('Failed to open stock report dialog:', error);
      return false;
    }
  }

  // if (path === AppRoutes.PSI.PSI_REPORT.BASE) {
  //   try {
  //     const psiReportService = inject(PSIReportDialogService);
  //     psiReportService.openPSIReportDialog();
  //     return false;
  //   } catch (error) {
  //     console.error('Failed to open PSI report dialog:', error);
  //     return false;
  //   }
  // }

  // // Allow other routes
  // return true;
};
