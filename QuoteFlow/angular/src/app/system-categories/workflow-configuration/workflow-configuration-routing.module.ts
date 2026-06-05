import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferWorkflowComponent } from './components/price-offer-workflow/price-offer-workflow.component';
import { MaterialStockWorkflowComponent } from './components/material-stock-workflow/material-stock-workflow.component';
import { PSIWorkflowComponent } from './components/psi-workflow/psi-workflow.component';
import { KeyAccountWorkflowComponent } from './components/key-account-workflow/key-account-workflow.component';
import { GKRWorkflowComponent } from './components/gkr/gkr-workflow.component';
import { AssetManagementComponent } from './components/asset-management/asset-management.component';

export const routes: Routes = [
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.PRICE_OFFER_WORKFLOW.BASE}`,
    component: PriceOfferWorkflowComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.PRICE_OFFER_WORKFLOW.TITLE,
    },
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.MATERIAL_STOCK_WORKFLOW.BASE}`,
    component: MaterialStockWorkflowComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.MATERIAL_STOCK_WORKFLOW.TITLE,
    },
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.PSI_WORKFLOW.BASE}`,
    component: PSIWorkflowComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.PSI_WORKFLOW.TITLE,
    },
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.KEY_ACCOUNT_WORKFLOW.BASE}`,
    component: KeyAccountWorkflowComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.KEY_ACCOUNT_WORKFLOW.TITLE,
    },
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.GKR_WORKFLOW.BASE}`,
    component: GKRWorkflowComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.GKR_WORKFLOW.TITLE,
    },
  },
  {
    path: `${AppRoutes.WORKFLOW_CONFIGURATION.ASSET_MANAGEMENT.BASE}`,
    component: AssetManagementComponent,
    data: {
      title: AppRoutes.WORKFLOW_CONFIGURATION.ASSET_MANAGEMENT.TITLE,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkflowConfigurationRoutingModule {}
