import { enableProdMode, importProvidersFrom, inject, provideAppInitializer } from '@angular/core';

import { CoreModule, LocalStorageListenerService, provideAbpCore, withOptions } from '@abp/ng.core';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideSettingManagementConfig } from '@abp/ng.setting-management/config';
import {
  CUSTOM_ERROR_HANDLERS,
  ThemeSharedModule,
  provideAbpThemeShared,
  withHttpErrorConfig,
  withValidationBluePrint,
} from '@abp/ng.theme.shared';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { BUYER_ROUTE_PROVIDER } from '@app/buyer/providers/buyer-route.provider';
import { CUSTOMER_ROUTE_PROVIDER } from '@app/customer/providers/customer-route.provider';
import { DPO_ROUTE_PROVIDERS } from '@app/dpos/providers/dpo-route.provider';
import { MATERIALS_MATERIAL_ROUTE_PROVIDER } from '@app/materials/providers/material-route.provider';
import { PRICE_OFFERS_PRICE_OFFER_ROUTE_PROVIDER } from '@app/price-offers/providers/price-offer-route.provider';
import { REPORT_ROUTE_PROVIDER } from '@app/report/providers/report-route.provider';
import { SALE_ORDERS_MANAGEMENT_ROUTE_PROVIDER } from '@app/sale-orders/providers/sale-orders-route.provider';
import { CustomNgbDatePickerFormatter } from '@app/shared/formatters/custom-ngb-datepicker';
import { InternalServerErrorHandlerService } from '@app/shared/http-error-handler/internal-error-handler';
import { RestInterceptor } from '@app/shared/interceptors/rest.interceptor';
import { STOCK_MANAGEMENT_ROUTE_PROVIDER } from '@app/stock-management/providers/stock-management-route.provider';
import { STOCK_TRACINGS_STOCK_TRACING_ROUTE_PROVIDER } from '@app/stock-tracings/stock-tracing/providers/stock-tracing-route.provider';
import { APPLICATION_SETTING_ROUTE_PROVIDER } from '@app/system-categories/providers/application-setting-route.provider';
import { FA_ADMIN_ROUTE_PROVIDER } from '@app/system-categories/providers/fa-admin-route.provider';
import { SPECIAL_INPUT_PRICE_ROUTE_PROVIDER } from '@app/system-categories/special-input-prices/providers/special-input-price-route.provider';
import { WORKFLOW_CONFIGURATION_ROUTE_PROVIDER } from '@app/system-categories/workflow-configuration/providers/workflow-configuration-route.provider';
import { TokenSyncService } from '@app/token-sync.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { provideCommercialUiConfig } from '@volo/abp.commercial.ng.ui/config';
import {
  AccountAdminConfigModule,
  provideAccountAdminConfig,
} from '@volo/abp.ng.account/admin/config';
import { provideAccountPublicConfig } from '@volo/abp.ng.account/public/config';
import {
  AuditLoggingConfigModule,
  provideAuditLoggingConfig,
} from '@volo/abp.ng.audit-logging/config';
import { provideFileManagementConfig } from '@volo/abp.ng.file-management/config';
import {
  GdprConfigModule,
  provideGdprConfig,
  withCookieConsentOptions,
} from '@volo/abp.ng.gdpr/config';
import { IdentityConfigModule, provideIdentityConfig } from '@volo/abp.ng.identity/config';
import { provideLanguageManagementConfig } from '@volo/abp.ng.language-management/config';
import { registerLocale } from '@volo/abp.ng.language-management/locale';
import { provideOpeniddictproConfig } from '@volo/abp.ng.openiddictpro/config';
import { provideTextTemplateManagementConfig } from '@volo/abp.ng.text-template-management/config';
import { LPX_STYLE_FINAL, LpxNavbarModule } from '@volo/ngx-lepton-x.core';
import { HttpErrorComponent, ThemeLeptonXModule } from '@volosoft/abp.ng.theme.lepton-x';
import { AccountLayoutModule } from '@volosoft/abp.ng.theme.lepton-x/account';
import { SideMenuLayoutModule } from '@volosoft/abp.ng.theme.lepton-x/layouts';
import { ToolbarContainerComponent } from '@volosoft/ngx-lepton-x/layouts';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { AppRoutingModule } from './app/app-routing.module';
import { AppComponent } from './app/app.component';
import { APP_ROUTE_PROVIDER } from './app/route.provider';
import { APPLICATION_CATEGORIES_APPLICATION_CATEGORY_ROUTE_PROVIDER } from './app/system-categories/providers/application-category-route.provider';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    provideCharts(withDefaultRegisterables()),
    importProvidersFrom(
      CoreModule,
      BrowserModule,
      AppRoutingModule,
      ThemeSharedModule,
      AccountAdminConfigModule,
      IdentityConfigModule,
      GdprConfigModule,
      AuditLoggingConfigModule,
      ThemeLeptonXModule.forRoot(),
      SideMenuLayoutModule.forRoot(),
      AccountLayoutModule.forRoot(),
      LpxNavbarModule.forRoot({
        contentAfterRoutes: [ToolbarContainerComponent],
      }),
    ),
    APP_ROUTE_PROVIDER,
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale(),
      }),
    ),
    provideAbpOAuth(),
    provideIdentityConfig(),
    provideSettingManagementConfig(),
    provideFeatureManagementConfig(),
    provideAccountAdminConfig(),
    provideAccountPublicConfig(),
    provideCommercialUiConfig(),
    provideAbpThemeShared(
      withHttpErrorConfig({
        errorScreen: {
          component: HttpErrorComponent,
          forWhichErrors: [401, 403, 404, 500],
          hideCloseIcon: true,
        },
      }),
      withValidationBluePrint({
        wrongPassword: 'Please choose 1q2w3E*',
      }),
    ),
    // provideGdprConfig(
    //   withCookieConsentOptions({
    //     cookiePolicyUrl: '/gdpr-cookie-consent/cookie',
    //     privacyPolicyUrl: '/gdpr-cookie-consent/privacy',
    //   }),
    // ),
    provideLanguageManagementConfig(),
    provideFileManagementConfig(),
    provideAuditLoggingConfig(),
    provideOpeniddictproConfig(),
    provideTextTemplateManagementConfig(),
    BUYER_ROUTE_PROVIDER,
    CUSTOMER_ROUTE_PROVIDER,
    MATERIALS_MATERIAL_ROUTE_PROVIDER,
    APPLICATION_CATEGORIES_APPLICATION_CATEGORY_ROUTE_PROVIDER,
    STOCK_TRACINGS_STOCK_TRACING_ROUTE_PROVIDER,
    PRICE_OFFERS_PRICE_OFFER_ROUTE_PROVIDER,
    APPLICATION_SETTING_ROUTE_PROVIDER,
    FA_ADMIN_ROUTE_PROVIDER,
    DPO_ROUTE_PROVIDERS,
    SPECIAL_INPUT_PRICE_ROUTE_PROVIDER,
    WORKFLOW_CONFIGURATION_ROUTE_PROVIDER,
    STOCK_MANAGEMENT_ROUTE_PROVIDER,
    REPORT_ROUTE_PROVIDER,
    SALE_ORDERS_MANAGEMENT_ROUTE_PROVIDER,
    provideAnimations(),
    provideAppInitializer(() => {
      const styles = inject(LPX_STYLE_FINAL);
      const index = styles.findIndex(f => f.bundleName === 'font-bundle');
      if (index !== -1) {
        styles.splice(index, 1);
      }
    }),
    {
      provide: LocalStorageListenerService,
      useClass: TokenSyncService,
    },
    {
      provide: CUSTOM_ERROR_HANDLERS,
      useExisting: InternalServerErrorHandlerService,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RestInterceptor,
      multi: true,
    },
    // Global NgBootstrap date formatter
    {
      provide: NgbDateParserFormatter,
      useClass: CustomNgbDatePickerFormatter,
    },
  ],
}).catch(err => console.error(err));
