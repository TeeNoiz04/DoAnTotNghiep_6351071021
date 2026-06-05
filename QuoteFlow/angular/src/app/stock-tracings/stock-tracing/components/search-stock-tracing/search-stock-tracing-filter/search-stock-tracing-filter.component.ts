import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { StockTracingDetailViewService } from '@app/stock-tracings/stock-tracing/services/stock-tracing-detail.service';
import { ReportType } from '@proxy/stock-tracings';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-search-stock-tracing-filter',
  templateUrl: './search-stock-tracing-filter.component.html',
  standalone: true,
  styleUrls: ['./search-stock-tracing-filter.component.scss'],
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule, TrimDirective],
})
export class SearchStockTracingFilterComponent {
  public readonly service = inject(StockTracingDetailViewService);
  reportTypeOptions = [
    // { value: 0, label: 'All' },
    { value: ReportType.Delivery, label: 'Delivery' },
    { value: ReportType.Receipt, label: 'Receipt' },
    { value: ReportType.Inventory, label: 'Inventory' },
  ];

  materialTypeOptions = [
    { value: 'LVS', label: 'LVS' },
    { value: 'FA', label: 'FA' },
  ];
}
