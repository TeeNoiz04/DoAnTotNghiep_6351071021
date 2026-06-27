import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from './app.routes';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'materials/list',
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
    path: `${AppRoutes.STOCK_MANAGEMENT.BASE}`,
    loadChildren: () =>
      import('./stock-management/stock-management.module').then(m => m.StockManagementModule),
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.BASE}`,
    loadChildren: () =>
      import('./system-categories/workflow-configuration/workflow-configuration.module').then(
        m => m.WorkflowConfigurationModule,
      ),
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
