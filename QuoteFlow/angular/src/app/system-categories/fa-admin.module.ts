import { CommonModule } from '@angular/common'; // Always include CommonModule for feature modules
import { NgModule } from '@angular/core';
import { FileManagementModule } from '@volo/abp.ng.file-management';
import { ApplicationSettingRoutingModule } from './application-setting-routing.module';
import { FAAdminRoutingModule } from './fa-admin-routing.module';

@NgModule({
  declarations: [
    // Add any other components declared in this module
  ],
  imports: [
    CommonModule,
    ApplicationSettingRoutingModule,
    FAAdminRoutingModule,
    FileManagementModule,
  ],
})
export class FAAdminModule {}
