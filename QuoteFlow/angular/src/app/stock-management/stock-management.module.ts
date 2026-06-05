import { NgModule } from '@angular/core';
import { StockManagementRoutingModule } from './stock-management-routing.module';
import { ReportStockPlaceholderComponent } from './components/placeholder/placeholder.component';
import { StockReportViewService } from './services/stock-report.service';

@NgModule({
  declarations: [],
  imports: [StockManagementRoutingModule, ReportStockPlaceholderComponent],
  providers: [StockReportViewService],
})
export class StockManagementModule {}
