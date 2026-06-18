import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferWorkflowComponent } from './components/price-offer-workflow/price-offer-workflow.component';
import { MaterialStockWorkflowComponent } from './components/material-stock-workflow/material-stock-workflow.component';

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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkflowConfigurationRoutingModule {}
