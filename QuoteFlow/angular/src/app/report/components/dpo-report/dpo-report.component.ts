import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { ErrorDisplayComponent } from '@app/shared/components/error-display';
import {
  AbstractDPOReportComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './dpo-report.abstract.component';
import { DPOReportViewService } from '@app/report/services/dpo-report.service';
import { DPOReportFilterComponent } from './components/dpo-report-filter/dpo-report-filter.component';
import { DPODataReportDto } from '@proxy/dpos';
import { NgxDatatableModule, ColumnMode, TableColumn, SortType } from '@swimlane/ngx-datatable';

@Component({
  selector: 'app-dpo-report',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    MatCheckboxModule,
    StatusLabelComponent,
    NgbTooltipModule,
    UsernamePipe,
    ErrorDisplayComponent,
    DPOReportFilterComponent,
    NgxDatatableModule,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    DPOReportViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './dpo-report.component.html',
  styleUrls: ['./dpo-report.component.scss'],
  encapsulation: ViewEncapsulation.Emulated,
})
export class DPOReportComponent extends AbstractDPOReportComponent {
  @ViewChild('materialTypeTpl', { static: true }) materialTypeTpl: TemplateRef<any>;
  @ViewChild('numberTpl', { static: true }) numberTpl: TemplateRef<any>;
  @ViewChild('totalTpl', { static: true }) totalTpl: TemplateRef<any>;
  @ViewChild('emptyTemplate', { static: true }) emptyTemplate: TemplateRef<any>;

  columns: TableColumn[] = [];
  rows: any[] = [];
  ColumnMode = ColumnMode;
  SortType = SortType;

  // Summaries
  grandTotal: any = {};
  faTotal: any = {};
  lvsTotal: any = {};

  override ngOnInit() {
    super.ngOnInit();
    this.initializeColumns();
    this.service.data.items = [];
  }

  private initializeColumns() {
    this.columns = [
      {
        prop: 'buyerType',
        name: 'Buyer Type',
        width: 120,
        canAutoResize: false,
      },
      {
        prop: 'buyerShortName',
        name: 'Buyer',
        width: 120,
        canAutoResize: false,
      },
      {
        prop: 'materialType',
        name: 'Material Type',
        width: 120,
        canAutoResize: false,
      },
    ];

    const months = [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
    ];

    months.forEach(month => {
      this.columns.push({
        prop: month.toLowerCase(),
        name: month,
        width: 120,
        headerClass: 'text-end',
        cellClass: this.getCellClass.bind(this),
        cellTemplate: this.numberTpl,
      });
    });

    this.columns.push({
      prop: 'total',
      name: 'Total',
      width: 130,
      headerClass: 'text-end',
      cellClass: 'text-end fw-bold',
      cellTemplate: this.totalTpl,
    });
  }

  prepareTableData() {
    this.initializeSummaryRows();
    this.rows = [...this.service.data.items];
    this.rows.forEach(row => {
      row.total = this.getRowTotal(row);
      this.updateSummaryRow(this.grandTotal, row);
      if (row.materialType === 'FA') {
        this.updateSummaryRow(this.faTotal, row);
      } else if (row.materialType === 'LVS') {
        this.updateSummaryRow(this.lvsTotal, row);
      }
    });
  }

  private initializeSummaryRows() {
    this.grandTotal = {
      buyerType: 'Grand Total',
      buyerShortName: '',
      materialType: '',
      january: 0,
      february: 0,
      march: 0,
      april: 0,
      may: 0,
      june: 0,
      july: 0,
      august: 0,
      september: 0,
      october: 0,
      november: 0,
      december: 0,
      total: 0,
    };

    this.faTotal = {
      buyerType: 'Total FA',
      buyerShortName: '',
      materialType: 'FA',
      january: 0,
      february: 0,
      march: 0,
      april: 0,
      may: 0,
      june: 0,
      july: 0,
      august: 0,
      september: 0,
      october: 0,
      november: 0,
      december: 0,
      total: 0,
    };

    this.lvsTotal = {
      buyerType: 'Total LVS',
      buyerShortName: '',
      materialType: 'LVS',
      january: 0,
      february: 0,
      march: 0,
      april: 0,
      may: 0,
      june: 0,
      july: 0,
      august: 0,
      september: 0,
      october: 0,
      november: 0,
      december: 0,
      total: 0,
    };
  }

  private updateSummaryRow(summary: any, row: any) {
    const months = [
      'january',
      'february',
      'march',
      'april',
      'may',
      'june',
      'july',
      'august',
      'september',
      'october',
      'november',
      'december',
    ];

    months.forEach(month => {
      summary[month] += row[month] || 0;
    });

    summary.total += this.getRowTotal(row);
  }
  getCellClass({ row, column, value }): string {
    return value > 0 ? 'highlight-cell text-end' : 'text-end';
  }

  override onSearch() {
    super.onSearch();
    setTimeout(() => {
      this.prepareTableData();
    }, 500);
  }

  getTotalAmount(): number {
    return this.service.data.items.reduce((sum, item) => sum + this.getRowTotal(item), 0);
  }

  getTotalByType(type: string): number {
    return this.service.data.items
      .filter(item => item.materialType === type)
      .reduce((sum, item) => sum + this.getRowTotal(item), 0);
  }

  getRowTotal(item: DPODataReportDto): number {
    return (
      (item.january || 0) +
      (item.february || 0) +
      (item.march || 0) +
      (item.april || 0) +
      (item.may || 0) +
      (item.june || 0) +
      (item.july || 0) +
      (item.august || 0) +
      (item.september || 0) +
      (item.october || 0) +
      (item.november || 0) +
      (item.december || 0)
    );
  }

  getMonthlyTotal(month: string): number {
    return this.service.data.items.reduce((sum, item) => sum + (item[month] || 0), 0);
  }

  getMonthlyTotalByType(month: string, type: string): number {
    return this.service.data.items
      .filter(item => item.materialType === type)
      .reduce((sum, item) => sum + (item[month] || 0), 0);
  }
}
