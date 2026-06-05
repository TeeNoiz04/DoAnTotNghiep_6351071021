import { PermissionDirective } from '@abp/ng.core';
import { Component } from '@angular/core';
import { AppPermissions } from '@app/app.permissions';
import { HostDashboardComponent } from './host-dashboard/host-dashboard.component';

@Component({
  selector: 'app-dashboard',
  template: ` <app-host-dashboard *abpPermission="AppPermissions.Dashboard.Default"></app-host-dashboard> `,
  imports: [PermissionDirective, HostDashboardComponent],
})
export class DashboardComponent {
  AppPermissions = AppPermissions;
}
