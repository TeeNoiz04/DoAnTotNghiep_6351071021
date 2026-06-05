import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const APPLICATION_CATEGORY_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-cogs', // system category settings
    name: '::Menu:SystemCategories',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.Default}`,
    breadcrumbText: '::SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.ORDER,
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.CURRENCY.BASE}`,
    iconClass: 'fas fa-coins', // currency-related
    name: '::Menu:SystemCategories:Currency',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.SYSTEM_CATEGORY.CURRENCY.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewCurrency}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.MATERIAL_GROUP.BASE}`,
    iconClass: 'fas fa-cubes', // represents grouped materials
    name: '::Menu:SystemCategories:MaterialGroup',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.MATERIAL_GROUP.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewMaterialGroup}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.STORAGE_LOCATION.BASE}`,
    iconClass: 'fas fa-warehouse', // inventory/stock
    name: '::Menu:SystemCategories:StorageLocation',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.STORAGE_LOCATION.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewStorageLocation}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.BUYER_TYPE.BASE}`,
    iconClass: 'fas fa-user-tie', // professional/buyer type
    name: '::Menu:SystemCategories:BuyerType',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.BUYER_TYPE.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewBuyerType}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.CUSTOMER.BASE}`,
    iconClass: 'fas fa-user-friends', // represents customers
    name: '::Menu:SystemCategories:Customer',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.CUSTOMER.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewCustomer}`,
    breadcrumbText: '::SystemCategories',
  },
  // {
  //   path: `/${AppRoutes.APPLICATION_SETTING.BASE}/${AppRoutes.APPLICATION_SETTING.DISTRIBUTOR_TARGET.BASE}`,
  //   iconClass: 'fas fa-bullseye', // buyer target / goal
  //   name: '::Menu:SystemCategories:BuyerTarget',
  //   parentName: '::Menu:SystemCategories',
  //   order: AppRoutes.APPLICATION_SETTING.DISTRIBUTOR_TARGET.ORDER,
  //   layout: eLayoutType.application,
  //   requiredPolicy: `${AppPermissions.MasterDatas.ViewBuyerTarget}`,
  //   breadcrumbText: '::SystemCategories',
  // },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.BUYER.BASE}`,
    iconClass: 'fas fa-user-tag', // buyer / tagged user
    name: '::Menu:SystemCategories:Buyer',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.BUYER.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewBuyer}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.SUPPLIER.BASE}`,
    iconClass: 'fas fa-handshake', // supplier / partner
    name: '::Menu:SystemCategories:Supplier',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.SUPPLIER.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewSupplier}`,
    breadcrumbText: '::SystemCategories',
  },
  {
    path: `/${AppRoutes.APPLICATION_CATEGORIES.BASE}/${AppRoutes.APPLICATION_CATEGORIES.SUPPLIER_BU.BASE}`,
    iconClass: 'fas fa-network-wired', // business unit / organization
    name: '::Menu:SystemCategories:SupplierBU',
    parentName: '::Menu:SystemCategories',
    order: AppRoutes.APPLICATION_CATEGORIES.SUPPLIER_BU.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewSupplierBU}`,
    breadcrumbText: '::SystemCategories',
  },
];
