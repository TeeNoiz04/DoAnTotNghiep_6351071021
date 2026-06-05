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
  AbstractSaleReportR05Component,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './sale-report-r05.abstract.component';
import { SaleReportR05FilterComponent } from './sale-report-general-filter/sale-report-r05-filter.component';
import { NgxDatatableModule, ColumnMode, TableColumn, SortType } from '@swimlane/ngx-datatable';
import { SaleReportR05ViewService } from '@app/stock-management/services/sale-report-r05.service';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';

@Component({
  selector: 'app-sale-report-r05',
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
    SaleReportR05FilterComponent,
    NgxDatatableModule,
    TableEdgeScrollerComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SaleReportR05ViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './sale-report-r05.component.html',
  // styleUrls: ['./sale-report-r05.component.scss'],
  encapsulation: ViewEncapsulation.Emulated,
})
export class SaleReportR05Component extends AbstractSaleReportR05Component {
  @ViewChild('materialTypeTpl', { static: true }) materialTypeTpl: TemplateRef<any>;
  @ViewChild('numberTpl', { static: true }) numberTpl: TemplateRef<any>;
  @ViewChild('emptyTemplate', { static: true }) emptyTemplate: TemplateRef<any>;

  columns: TableColumn[] = [];
  rows: any[] = [];
  ColumnMode = ColumnMode;
  SortType = SortType;

  override ngOnInit() {
    super.ngOnInit();
    this.service.data.items = [];
  }

  getBadgeClass(keyAccountClass: string): string {
    if (!keyAccountClass) {
      return 'badge-secondary';
    }

    const normalizedClass = keyAccountClass.toLowerCase().replace(/[_\s]/g, '');

    switch (normalizedClass) {
      case 'signature':
        return 'badge-signature';
      case 'premium':
        return 'badge-premium';
      case 'deluxe':
        return 'badge-deluxe';
      case 'diamond':
        return 'badge-diamond';
      case 'platinum':
        return 'badge-platinum';
      case 'gold':
        return 'badge-gold';
      case 'silver':
        return 'badge-silver';
      case 'globalaccount':
      case 'global_account':
        return 'badge-global-account';
      case 'eu':
      case 'euna':
        return 'badge-eu';
      case 'sioem':
        return 'badge-si-oem';
      case 'downgraded':
        return 'downgraded';
      default:
        return 'bg-light';
    }
  }

  getDisplayText(keyAccountClass: string): string {
    if (!keyAccountClass) {
      return '';
    }

    const keyAccountClassMap: { [key: string]: string } = {
      signature: 'Signature',
      premium: 'Premium',
      deluxe: 'Deluxe',
      diamond: 'Diamond',
      platinum: 'Platinum',
      gold: 'Gold',
      silver: 'Silver',
      global_account: 'Global Account',
      eu: 'E/U',
      eu_na: 'EU / NA',
      si_oem: 'SI/OEM',
      downgraded: 'downgraded',
    };

    return keyAccountClassMap[keyAccountClass.toLowerCase()] || keyAccountClass;
  }
}
