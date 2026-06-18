import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const WORKFLOW_CONFIGURATION_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-sliders-h', // general application settings
    name: '::Menu:WorkflowConfiguration',
    layout: eLayoutType.application,
    parentName: 'AbpUiNavigation::Menu:Administration',
    requiredPolicy: `${AppPermissions.WorkflowConfigurations.View}`,
    breadcrumbText: '::WorkflowConfiguration',
    order: AppRoutes.WORKFLOW_CONFIGURATION.ORDER,
  },
  {
    path: `/${AppRoutes.WORKFLOW_CONFIGURATION.BASE}/${AppRoutes.WORKFLOW_CONFIGURATION.PRICE_OFFER_WORKFLOW.BASE}`,
    iconClass: 'fas fa-file-alt',
    name: '::Menu:ApplicationSetting:PriceOfferWorkflow',
    parentName: '::Menu:WorkflowConfiguration',
    order: AppRoutes.WORKFLOW_CONFIGURATION.PRICE_OFFER_WORKFLOW.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.WorkflowConfigurations.View}`,
    breadcrumbText: '::PriceOfferWorkflow',
  },
  {
    path: `/${AppRoutes.WORKFLOW_CONFIGURATION.BASE}/${AppRoutes.WORKFLOW_CONFIGURATION.MATERIAL_STOCK_WORKFLOW.BASE}`,
    iconClass: 'fas fa-cube',
    name: '::Menu:ApplicationSetting:MaterialStockWorkflow',
    parentName: '::Menu:WorkflowConfiguration',
    order: AppRoutes.WORKFLOW_CONFIGURATION.MATERIAL_STOCK_WORKFLOW.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.WorkflowConfigurations.View}`,
    breadcrumbText: '::MaterialStockWorkflow',
  },
];
