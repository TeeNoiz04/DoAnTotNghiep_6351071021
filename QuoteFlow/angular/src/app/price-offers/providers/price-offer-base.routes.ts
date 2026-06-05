import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const PRICE_OFFER_BASE_ROUTES: ABP.Route[] = [
  {
    // path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}`,
    iconClass: 'fas fa-file-alt',
    name: '::Menu:PriceOffers',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PriceOffers.Default}`,
    breadcrumbText: '::PriceOffers',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.ORDER,
  },
  {
    path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE}`,
    iconClass: 'fas fa-list',
    name: '::Menu:PriceOffers:List',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PriceOffers.PriceOfferList}`,
    breadcrumbText: '::PriceOffersList',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.LIST.ORDER,
    parentName: '::Menu:PriceOffers',
  },
  {
    path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${AppRoutes.SPECIAL_PRICE_OFFERS.MY_APPROVALS.BASE}`,
    iconClass: 'fas fa-user-check',
    name: '::Menu:PriceOffers:MyApprovals',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PriceOffers.PriceOfferList}`,
    breadcrumbText: '::MyApprovals',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.MY_APPROVALS.ORDER,
    parentName: '::Menu:PriceOffers',
  },
  {
    path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE}`,
    iconClass: 'fas fa-chart-bar',
    name: '::Menu:PriceOffers:GeneralReport',
    layout: eLayoutType.application,
    breadcrumbText: '::GeneralReport',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.ORDER,
    parentName: '::Menu:PriceOffers',
    requiredPolicy: `${AppPermissions.PriceOffers.GeneralReport}`,
  },
  {
    path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE}`,
    iconClass: 'fas fa-chart-bar',
    name: '::Menu:PriceOffers:DetailReport',
    layout: eLayoutType.application,
    breadcrumbText: '::SPODetailedReport',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.ORDER,
    parentName: '::Menu:PriceOffers',
    requiredPolicy: `${AppPermissions.PriceOffers.DetailedReport}`,
  },

  {
    path: `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${AppRoutes.SPECIAL_PRICE_OFFERS.BATCH_REQUEST.BASE}`,
    iconClass: 'fas fa-file-upload',
    name: '::Menu:PriceOffers:BatchRequest',
    layout: eLayoutType.application,
    breadcrumbText: '::BatchRequest',
    order: AppRoutes.SPECIAL_PRICE_OFFERS.BATCH_REQUEST.ORDER,
    parentName: '::Menu:PriceOffers',

    requiredPolicy: `${AppPermissions.PriceOffers.BatchRequest}`,
  },
];
