import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from './app.routes';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard/base',
    pathMatch: 'full',
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'account',
    loadChildren: () =>
      import('@volo/abp.ng.account/public').then(m => m.AccountPublicModule.forLazy()),
  },
  // {
  //   path: 'gdpr',
  //   loadChildren: () => import('@volo/abp.ng.gdpr').then(m => m.GdprModule.forLazy()),
  // },
  {
    path: 'identity',
    loadChildren: () => import('@volo/abp.ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'language-management',
    loadChildren: () =>
      import('@volo/abp.ng.language-management').then(m => m.LanguageManagementModule.forLazy()),
  },
  {
    path: 'audit-logs',
    loadChildren: () =>
      import('@volo/abp.ng.audit-logging').then(m => m.AuditLoggingModule.forLazy()),
  },
  {
    path: 'openiddict',
    loadChildren: () =>
      import('@volo/abp.ng.openiddictpro').then(m => m.OpeniddictproModule.forLazy()),
  },
  {
    path: 'text-template-management',
    loadChildren: () =>
      import('@volo/abp.ng.text-template-management').then(m =>
        m.TextTemplateManagementModule.forLazy(),
      ),
  },
  {
    path: 'file-management',
    loadChildren: () =>
      import('@volo/abp.ng.file-management').then(m => m.FileManagementModule.forLazy()),
  },
  {
    path: 'gdpr-cookie-consent',
    loadChildren: () =>
      import('./gdpr-cookie-consent/gdpr-cookie-consent.module').then(
        m => m.GdprCookieConsentModule,
      ),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  {
    path: `${AppRoutes.KEY_ACCOUNTS.BASE}`,
    loadChildren: () => import('./key-accounts/key-account.module').then(m => m.KeyAccountModule),
  },
  {
    path: `${AppRoutes.SYSTEM_CATEGORY.BASE}`,
    loadChildren: () =>
      import('./system-categories/application-category.module').then(
        m => m.ApplicationCategoryModule,
      ),
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.BASE}`,
    loadChildren: () =>
      import('./system-categories/application-category.module').then(
        m => m.ApplicationCategoryModule,
      ),
  },
  {
    path: `${AppRoutes.APPLICATION_SETTING.BASE}`,
    loadChildren: () =>
      import('./system-categories/application-setting.module').then(
        m => m.ApplicationSettingModule,
      ),
  },
  {
    path: `${AppRoutes.FA_ADMIN.BASE}`,
    loadChildren: () => import('./system-categories/fa-admin.module').then(m => m.FAAdminModule),
  },
  {
    path: `${AppRoutes.STOCK_TRACING.BASE}`,
    loadChildren: () =>
      import('./stock-tracings/stock-tracing/stock-tracing.module').then(m => m.StockTracingModule),
  },
  {
    path: `${AppRoutes.MATERIAL_STOCK.BASE}`,
    loadChildren: () => import('./materials/material.module').then(m => m.MaterialModule),
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}`,
    loadChildren: () => import('./price-offers/price-offer.module').then(m => m.PriceOfferModule),
  },
  {
    path: `${AppRoutes.CARGO_DATA.BASE}`,
    loadChildren: () => import('./cargos/cargo.module').then(m => m.CargoModule),
  },
  {
    path: `${AppRoutes.PSI.BASE}`,
    loadChildren: () => import('./psis/psi.module').then(m => m.PSIModule),
  },
  {
    path: `${AppRoutes.CUSTOMERS.BASE}`,
    loadChildren: () => import('./customer/customer.module').then(m => m.CustomerModule),
  },
  {
    path: `${AppRoutes.BUYERS.BASE}`,
    loadChildren: () => import('./buyer/buyer.module').then(m => m.BuyerModule),
  },
  {
    path: `${AppRoutes.DPO.BASE}`,
    loadChildren: () => import('./dpos/dpo.module').then(m => m.DPOModule),
  },
  {
    path: `${AppRoutes.GIC.BASE}`,
    loadChildren: () => import('./gics/gic.module').then(m => m.GICModule),
  },
  {
    path: `${AppRoutes.GKR.BASE}`,
    loadChildren: () => import('./gkrs/gkr.module').then(m => m.GKRModule),
  },
  {
    path: `${AppRoutes.STOCK_MANAGEMENT.BASE}`,
    loadChildren: () =>
      import('./stock-management/stock-management.module').then(m => m.StockManagementModule),
  },
  {
    path: `${AppRoutes.IMPORT_ALLOCATION.BASE}`,
    loadChildren: () =>
      import('./import-allocations/import-allocation.module').then(m => m.ImportAllocationModule),
  },
  {
    path: `${AppRoutes.PURCHASE_ORDERS_MANAGEMENT.BASE}`,
    loadChildren: () =>
      import('./purchase-orders/purchase-orders.module').then(
        m => m.PurchaseOrdersManagementModule,
      ),
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.BASE}`,
    loadChildren: () =>
      import('./system-categories/workflow-configuration/workflow-configuration.module').then(
        m => m.WorkflowConfigurationModule,
      ),
  },
  {
    path: `${AppRoutes.REPORT.BASE}`,
    loadChildren: () => import('./report/report.module').then(m => m.ReportModule),
  },
  {
    path: `${AppRoutes.SPECIAL_INPUT_PRICE.BASE}`,
    loadChildren: () =>
      import('./system-categories/special-input-prices/special-input-price.module').then(
        m => m.SpecialInputPriceModule,
      ),
  },
  {
    path: AppRoutes.SALE_ORDERS_MANAGEMENT.BASE,
    loadChildren: () => import('./sale-orders/sale-orders.module').then(m => m.SaleOrdersModule),
    canActivate: [authGuard, permissionGuard],
  },

  {
    path: AppRoutes.SALE_ORDERS_GIC_MANAGEMENT.BASE,
    loadChildren: () =>
      import('./sale-orders-gic/sale-orders-gic.module').then(m => m.SaleOrdersGicModule),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.ASSET_AUDIT.BASE}`,
    loadChildren: () =>
      import('./asset-management/asset-stock-audit/asset-stock-audit.module').then(
        m => m.AssetStockAuditModule,
      ),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.INTERNAL_WAREHOUSE_TRANSFER.BASE}`,
    loadChildren: () =>
      import('./asset-management/asset-stock-transfer/asset-stock-transfer.module').then(
        m => m.AssetStockTransferModule,
      ),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.ASSET_CATALOG.BASE}`,
    loadChildren: () =>
      import('./asset-management/asset-catalog/asset-catalog.module').then(
        m => m.AssetCatalogModule,
      ),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.ASSET_APPROVAL.BASE}`,
    loadChildren: () =>
      import('./asset-management/my-approval/my-approvals.module').then(m => m.MyApprovalsModule),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.PIC_TRANSFER.BASE}`,
    loadChildren: () =>
      import('./asset-management/pic-transfer/pic-transfer.module').then(m => m.PicTransferModule),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.ASSET_LIQUIDATION.BASE}`,
    loadChildren: () =>
      import('./asset-management/asset-liquidation/asset-liquidation.module').then(
        m => m.AssetLiquidationModule,
      ),
  },
  {
    path: `${AppRoutes.ASSET_MANAGEMENT.ASSET_LENDING.BASE}`,
    loadChildren: () =>
      import('./asset-management/asset-lending/asset-lending.module').then(
        m => m.AssetLendingModule,
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
