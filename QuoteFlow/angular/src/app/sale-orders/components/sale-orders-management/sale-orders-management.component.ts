import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SaleOrdersManagementDetailViewService } from '@app/sale-orders/services/sale-orders-management-detail.service';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { ErrorDisplayComponent } from '@app/shared/components/error-display';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
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
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ImportSaleOrderModalComponent } from './components/import-sale-order-modal/import-sale-order-modal.component';
import { ResultImportSaleOrderComponent } from './components/result-import-sale-order/result-import-sale-order.component';
import {
  AbstractSaleOrdersManagementComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './sale-orders-management.abstract.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';

@Component({
  selector: 'app-sale-orders-management',
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
    ErrorDisplayComponent,
    ImportSaleOrderModalComponent,
    ResultImportSaleOrderComponent,
    TableEdgeScrollerComponent,
    AuditInfoColumnComponent,
    HistoryModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SaleOrdersManagementViewService,
    SaleOrdersManagementDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './sale-orders-management.component.html',
  styleUrls: [`sale-orders-management.component.scss`],
})
export class SaleOrdersManagementComponent extends AbstractSaleOrdersManagementComponent {}
