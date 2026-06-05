import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { ReportType } from '@proxy/stock-tracings';
import { debounceTime, Subject } from 'rxjs';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-stock-tracing-filter',
  templateUrl: './stock-tracing-filter.component.html',
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule, TrimDirective],
})
export class StockTracingFilterComponent implements OnInit {
  @Input() filters: any = {};
  @Output() filterChange = new EventEmitter<any>();

  private filterSubject = new Subject<void>();

  reportTypeOptions = [
    // { value: 0, label: 'All' },
    { value: ReportType.Delivery, label: 'Delivery' },
    { value: ReportType.Receipt, label: 'Receipt' },
    { value: ReportType.Inventory, label: 'Inventory' },
  ];

  ngOnInit() {
    this.filterSubject.pipe(debounceTime(300)).subscribe(() => {
      this.filterChange.emit(this.filters);
    });
  }

  applyFilter() {
    this.filterChange.emit(this.filters);
  }

  onReportTypeChange($event: any) {
    this.onFiltersChange();

    this.applyFilter();
  }

  onFiltersChange() {
    this.filterSubject.next();
  }
}
