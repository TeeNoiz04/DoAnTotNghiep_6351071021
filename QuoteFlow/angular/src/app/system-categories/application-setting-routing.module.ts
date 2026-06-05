import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { BuyerComponent } from '@app/buyer/components/buyer.component';
import { CustomerComponent } from '@app/customer/components/customer.component';
import { FactoryCategoryComponent } from './factory/components/factory-category.component';
import { BuyerTypeCategoryComponent } from './buyer-type/components/buyer-type-category.component';
import { SaleBuyerComponent } from './sale-of-buyer/components/sale-buyer.component';
import { VendorCategoryComponent } from './vendor/components/vendor-category.component';
import { SystemConfigurationCategoryComponent } from './system-configuration/components/system-configuration-category.component';
import { DistributorTargetComponent } from './distributor-target/components/distributor-target.component';

export const routes: Routes = [
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.FILE_TEMPLATE.BASE}`,
  //   component: FileManagementComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.FILE_TEMPLATE.TITLE,
  //   },
  // },
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.CUSTOMER.BASE}`,
  //   component: CustomerComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.CUSTOMER.TITLE,
  //   },
  // },
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.SUPPLIER.BASE}`,
  //   component: VendorCategoryComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.SUPPLIER.TITLE,
  //   },
  // },
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.SUPPLIER_BU.BASE}`,
  //   component: FactoryCategoryComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.SUPPLIER_BU.TITLE,
  //   },
  // },
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.BUYER.BASE}`,
  //   component: BuyerComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.BUYER.TITLE,
  //   },
  // },
  // {
  //   path: `${AppRoutes.APPLICATION_SETTING.DISTRIBUTOR_TARGET.BASE}`,
  //   component: DistributorTargetComponent,
  //   data: {
  //     title: AppRoutes.APPLICATION_SETTING.DISTRIBUTOR_TARGET.TITLE,
  //   },
  // },
  {
    path: `${AppRoutes.APPLICATION_SETTING.SALE_TEAM.BASE}`,
    component: SaleBuyerComponent,
    data: {
      title: AppRoutes.APPLICATION_SETTING.SALE_TEAM.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_SETTING.BUYER_TYPE.BASE}`,
    component: BuyerTypeCategoryComponent,
    data: {
      title: AppRoutes.APPLICATION_SETTING.BUYER_TYPE.TITLE,
    },
  },
  {
    path: `${AppRoutes.FA_ADMIN.SYSTEM_CONFIGURATION.BASE}`,
    component: SystemConfigurationCategoryComponent,
    data: {
      title: AppRoutes.FA_ADMIN.SYSTEM_CONFIGURATION.TITLE,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApplicationSettingRoutingModule {}
