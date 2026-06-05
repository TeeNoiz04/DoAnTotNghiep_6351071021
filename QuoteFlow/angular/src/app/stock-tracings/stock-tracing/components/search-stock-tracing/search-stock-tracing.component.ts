import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ReportTypePipe } from '@shared/pipes/report-type.pipe';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { StockTracingDetailViewService } from '../../services/stock-tracing-detail.service';
import { StockTracingViewService } from '../../services/stock-tracing.service';
import { SearchStockTracingFilterComponent } from './search-stock-tracing-filter/search-stock-tracing-filter.component';
import {
  AbstractSearchStockTracingComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './search-stock-tracing.abstract.component';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';

@Component({
  selector: 'app-search-stock-tracing',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
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
    SearchStockTracingFilterComponent,
    ReportTypePipe,
    TableEdgeScrollerComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    StockTracingViewService,
    StockTracingDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './search-stock-tracing.component.html',
  styleUrls: ['./search-stock-tracing.component.scss'],
})
export class SearchStockTracingComponent extends AbstractSearchStockTracingComponent {}
