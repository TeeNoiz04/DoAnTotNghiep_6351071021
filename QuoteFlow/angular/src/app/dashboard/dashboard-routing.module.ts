import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApprovalDashboardComponent } from './approval-dashboard/approval-dashboard.component';
import { DashboardComponent } from './dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: 'base', pathMatch: 'full' },
  { path: 'base', component: DashboardComponent, data: { title: 'Dashboard' } },
  {
    path: 'approval',
    component: ApprovalDashboardComponent,
    data: { title: 'Approval Dashboard' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule {}
