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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractStockManagementComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './stock-management.abstract.component';
import { StockManagementViewService } from '../../services/stock-management.service';
import { StockManagementDetailViewService } from '../../services/stock-management-detail.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { LockShipmentDetailModalComponent } from './components/lock-shipment-detail-modal/lock-shipment-detail-modal.component';
import { OnOrderStockDetailModalComponent } from './components/on-order-stock-detail-modal/on-order-stock-detail-modal.component';
import { LockStockSODetailModalComponent } from './components/lock-stock-so-detail-modal/lock-stock-so-detail-modal.component';
import { StockLockedDetailModalComponent } from './components/stock-locked-detail-modal/stock-locked-detail-modal.component';
import { StockQtyDetailModalComponent } from './components/stock-qty-detail-modal/stock-qty-detail-modal.component';
import { StockHistoryDetailModalComponent } from './components/stock-history-detail-modal/stock-history-detail-modal.component';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';

@Component({
  selector: 'app-stock-management',
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
    LockShipmentDetailModalComponent,
    OnOrderStockDetailModalComponent,
    LockStockSODetailModalComponent,
    StockQtyDetailModalComponent,
    StockLockedDetailModalComponent,
    StockHistoryDetailModalComponent,
    TableEdgeScrollerComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    StockManagementViewService,
    StockManagementDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './stock-management.component.html',
  styleUrls: ['./stock-management.component.scss'],
})
export class StockManagementComponent extends AbstractStockManagementComponent {}
