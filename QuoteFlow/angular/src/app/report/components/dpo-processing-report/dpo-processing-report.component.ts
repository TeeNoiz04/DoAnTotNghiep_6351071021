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
  AbstractDPOProcessingReportComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './dpo-processing-report.abstract.component';
import { DPOReportViewService } from '@app/report/services/dpo-report.service';
import { DPOProcessingReportFilterComponent } from './components/dpo-report-filter/dpo-processing-report-filter.component';
import { DPODataReportDto } from '@proxy/dpos';
import { NgxDatatableModule, ColumnMode, TableColumn, SortType } from '@swimlane/ngx-datatable';
import { DPOProcessingReportViewService } from '@app/report/services/dpo-processing-report.service';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';

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
    DPOProcessingReportFilterComponent,
    NgxDatatableModule,
    TableEdgeScrollerComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    DPOProcessingReportViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './dpo-processing-report.component.html',
  styleUrls: ['./dpo-processing-report.component.scss'],
  encapsulation: ViewEncapsulation.Emulated,
})
export class DPOProcessingReportComponent extends AbstractDPOProcessingReportComponent {
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
  }
}
