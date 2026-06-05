import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ErrorDisplayComponent } from '@app/shared/components/error-display';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import {
  AbstractStockReportComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './stock-report.abstract.component';
import { DPODataReportDto } from '@proxy/dpos';
import { NgxDatatableModule, ColumnMode, TableColumn, SortType } from '@swimlane/ngx-datatable';
import { StockReportViewService } from '@app/stock-management/services/stock-report.service';

@Component({
  selector: 'app-stock-report',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ErrorDisplayComponent,
    NgbTooltipModule,
    NgxDatatableModule,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, StockReportViewService],
  templateUrl: './stock-report.component.html',
  styleUrls: ['./stock-report.component.scss'],
  encapsulation: ViewEncapsulation.Emulated,
})
export class StockReportComponent extends AbstractStockReportComponent {}
