import { NgModule } from '@angular/core';
import { ReportRoutingModule } from './report-routing.module';
import { CommonModule } from '@angular/common';
import { ReportMenuService } from './services/report-menu.service';
import { MenuClickHandlerService } from '@app/shared/services/menu-click-handler.service';

@NgModule({
  declarations: [],
  imports: [CommonModule, ReportRoutingModule],
  providers: [ReportMenuService, MenuClickHandlerService],
})
export class ReportModule {}
