import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { CurrencyCategoryComponent } from './currency/components/currency-category.component';
import { MaterialGroupCategoryComponent } from './material-group/components/material-group-category.component';
import { StorageLocationCategoryComponent } from './storage-location/components/storage-location-category.component';
import { BuyerTypeCategoryComponent } from './buyer-type/components/buyer-type-category.component';
import { CustomerComponent } from '@app/customer/components/customer.component';
import { VendorCategoryComponent } from './vendor/components/vendor-category.component';
import { FactoryCategoryComponent } from './factory/components/factory-category.component';
import { BuyerComponent } from '@app/buyer/components/buyer.component';

export const routes: Routes = [
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.STORAGE_LOCATION.BASE}`,
    component: StorageLocationCategoryComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.STORAGE_LOCATION.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.CURRENCY.BASE}`,
    component: CurrencyCategoryComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SYSTEM_CATEGORY.CURRENCY.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.MATERIAL_GROUP.BASE}`,
    component: MaterialGroupCategoryComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SYSTEM_CATEGORY.MATERIAL_GROUP.TITLE,
    },
  },

  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.BUYER_TYPE.BASE}`,
    component: BuyerTypeCategoryComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.BUYER_TYPE.TITLE,
    },
  },

  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.CUSTOMER.BASE}`,
    component: CustomerComponent,
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.CUSTOMER.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.SUPPLIER.BASE}`,
    component: VendorCategoryComponent,
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.SUPPLIER.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.SUPPLIER_BU.BASE}`,
    component: FactoryCategoryComponent,
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.SUPPLIER_BU.TITLE,
    },
  },
  {
    path: `${AppRoutes.APPLICATION_CATEGORIES.BUYER.BASE}`,
    component: BuyerComponent,
    data: {
      title: AppRoutes.APPLICATION_CATEGORIES.BUYER.TITLE,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApplicationCategoryRoutingModule {}
