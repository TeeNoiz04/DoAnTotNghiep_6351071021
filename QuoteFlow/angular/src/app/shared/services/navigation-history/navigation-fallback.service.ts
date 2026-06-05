import { Injectable } from '@angular/core';
import { AppRoutes } from '@app/app.routes';

@Injectable({
  providedIn: 'root',
})
export class NavigationFallbackService {
  /**
   * Get the fallback URL for a given module/context
   */
  public getFallbackUrl(baseContext: string): string[] {
    switch (baseContext) {
      case 'materials':
        return [AppRoutes.MATERIAL_STOCK.BASE, AppRoutes.MATERIAL_STOCK.LIST.BASE];

      case 'price-offer':
        return [AppRoutes.SPECIAL_PRICE_OFFERS.BASE, AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE];

      case 'dpo':
        return [AppRoutes.DPO.BASE, AppRoutes.DPO.LIST.BASE];

      case 'gic':
        return [AppRoutes.GIC.BASE, AppRoutes.GIC.LIST.BASE];

      case 'key-accounts':
        return [AppRoutes.KEY_ACCOUNTS.BASE, AppRoutes.KEY_ACCOUNTS.LIST.BASE];

      case 'customers':
        return [AppRoutes.CUSTOMERS.BASE];

      case 'import-allocation':
        return [AppRoutes.IMPORT_ALLOCATION.BASE, AppRoutes.IMPORT_ALLOCATION.IMPORT_LIST.BASE];

      case 'stock-tracing':
        return [AppRoutes.STOCK_TRACING.BASE, AppRoutes.STOCK_TRACING.SEARCH.BASE];

      case 'cargo-data':
        return [AppRoutes.CARGO_DATA.BASE, AppRoutes.CARGO_DATA.IMPORT.BASE];

      case 'psis':
        return [AppRoutes.PSI.BASE, AppRoutes.PSI.LIST.BASE];

      case 'home':
      case 'dashboard':
        return [AppRoutes.HOME.BASE];

      default:
        return [AppRoutes.HOME.BASE];
    }
  }

  /**
   * Get the my-approvals fallback URL for a given module/context
   */
  public getMyApprovalsFallbackUrl(baseContext: string): string[] {
    switch (baseContext) {
      case 'materials':
        return [AppRoutes.MATERIAL_STOCK.BASE, AppRoutes.MATERIAL_STOCK.MY_APPROVALS.BASE];

      case 'price-offer':
        return [
          AppRoutes.SPECIAL_PRICE_OFFERS.BASE,
          AppRoutes.SPECIAL_PRICE_OFFERS.MY_APPROVALS.BASE,
        ];

      case 'key-accounts':
        return [AppRoutes.KEY_ACCOUNTS.BASE, AppRoutes.KEY_ACCOUNTS.MY_APPROVALS.BASE];

      default:
        return this.getFallbackUrl(baseContext);
    }
  }

  /**
   * Get smart fallback URL based on current URL context
   */
  public getSmartFallbackUrl(currentUrl: string): string[] {
    const baseContext = this.extractBaseContext(currentUrl);

    // Check if current URL indicates my-approvals context
    if (currentUrl.includes('my-approvals')) {
      return this.getMyApprovalsFallbackUrl(baseContext);
    }

    return this.getFallbackUrl(baseContext);
  }

  /**
   * Extract base context from URL (e.g., 'materials' from '/materials/list')
   */
  private extractBaseContext(url: string): string {
    const segments = url.split('/').filter(segment => segment.length > 0);
    return segments[0] || '';
  }
}
