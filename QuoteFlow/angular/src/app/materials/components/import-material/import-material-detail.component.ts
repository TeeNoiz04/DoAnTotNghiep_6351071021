import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { ApprovalCommentModalComponent } from '@app/shared/components/approval-comment/approval-comment.component';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { TitleService } from '@app/shared/services/title/title.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbNavModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { finalize } from 'rxjs';
import { ImportMaterialDetailViewService } from '../../services/import-material-detail.service';
import { ImportMaterialColumns, ImportMaterialManagementType } from './import-material.type';

@Component({
  selector: 'app-import-material-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    PageModule,
    ReactiveFormsModule,
    NgbNavModule,
    HistoryModalComponent,
    StatusLabelComponent,
    ApprovalCommentModalComponent,
    ApproversModalComponent,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    NgbTooltip,
  ],
  providers: [ListService, ImportMaterialDetailViewService],
  templateUrl: './import-material-detail.component.html',
  styleUrls: ['./import-material-detail.component.scss'],
})
export class ImportMaterialDetailComponent implements OnInit {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly toast = inject(ToasterService);
  public readonly service = inject(ImportMaterialDetailViewService);
  public readonly titleService = inject(TitleService);

  protected readonly title = 'Import Material Detail';

  form: FormGroup = new FormGroup({});
  showHistoryModal = false;
  approvalHistories = [];

  // Column configurations for different import types
  columnConfigs = {
    'MATERIAL.NEW': [
      'materialStatus',
      'golfaCode',
      'model',
      'registrationDate',
      'validFrom',
      'validTo',
      'sap_Code',
      'spec1',
      'spec2',
      'spec3',
      'spec4',
      'description_EN',
      'description_VN',
      'materialType',
      'unit',
      'materialClass',
      'material_SEC_Classification',
      'material_Group',
      // 'materialGroupCategory',
      'sapMatGroup',
      'product_Hierarchy',
      'productHierarchyDescription',
      'countryOfOrigin',
      'referenceLeadTime',
      'warrantyTime',
      'inventoryCategory',
      'cargoNote',
      'weight',
      'size',
      'qrCode',
      'maxlot',
      'stockWarning',
      'vat',
      'hS_Code',
      'supplierCode',
      'supplierBUCode',
      'factory_Text',
      'input_Price',
      'inputCurrency',
      'incoterms',
      'epa',
      'importDuty',
      'appliedExchangeRate',
      'landedCost',
      'maxSalesOfferPrice',
      'maxMangerOfferPrice',
      'standard_Price',
      'sellingPrice1',
      'sellingPrice2',
      'sellingPrice3',
      'sellingPrice4',
      'sellingPrice5',
    ],
    'MATERIAL.PRICE': [
      'materialStatus',
      'golfaCode',
      'model',
      'spec1',
      'materialType',
      'material_Group',
      'validFrom',
      'validTo',
      'input_Price',
      'inputCurrency',
      'incoterms',
      'epa',
      'importDuty',
      'appliedExchangeRate',
      'landedCost',
      'maxSalesOfferPrice',
      'maxMangerOfferPrice',
      'standard_Price',
      'sellingPrice1',
      'sellingPrice2',
      'sellingPrice3',
      'sellingPrice4',
      'sellingPrice5',
    ],
    'MATERIAL.WITHOUT_PRICE': [
      'materialStatus',
      'golfaCode',
      'model',
      'registrationDate', // "Date"
      'validFrom',
      'validTo',
      'spec1',
      'spec2',
      'spec3',
      'spec4',
      'description_EN',
      'description_VN',
      'supplierCode',
      'supplierBUCode',
      'factory_Text',
      'materialType',
      'unit',
      'material_Group',
      'sapMatGroup',
      'productHierarchyDescription',
      'countryOfOrigin',
      'referenceLeadTime',
      'warrantyTime',
      'inventoryCategory',
      'cargoNote',
      'weight',
      'size',
      'qrCode',
      'maxlot',
      'stockWarning',
      'stockQty',
      'hS_Code',
    ],

    'MATERIAL.STATUS': [
      'materialStatus',
      'golfaCode',
      'model',
      'finalDPOAcceptanceDate',
      'action',
      'actionDate',
      'source',
      'reason',
      'factoryRefDoc',
    ],

    'MATERIAL.INVENTORY_PLANNING': [
      'materialStatus',
      'golfaCode',
      'model',
      'inventoryCategory',
      'currentStockWarning',
      'stockWarning',
    ],

    'MATERIAL.LEADTIME': ['materialStatus', 'golfaCode', 'model', 'referenceLeadTime', 'countryOfOrigin', 'maxlot'],

    'MATERIAL.SAP_CODE': [
      'materialStatus',
      'golfaCode',
      'model',
      'sap_Code',
      'description_VN',
      'product_Hierarchy',
      'vat',
    ],
  };
  requestId: string | null;
  importType = ImportMaterialManagementType;
  columns = ImportMaterialColumns;
  // Formatter functions for advanced data table
  formatRowNumber = (value: any, row: any): string => {
    const data = this.service?.selected.materialApprovalDetails || [];
    const index = data.indexOf(row) + 1;
    return index.toString();
  };

  formatCurrency = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2,
        }).format(value)}`
      : '';
  };
  formatCurrencyDelete = (value: any): string => {
    if (value === -1) return 'DELETE VALUE'; // null or undefined
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 0,
        }).format(value)}`
      : '';
  };

  formatNegative(value: number): string {
    if (value == null) return ''; // null or undefined

    if (value < 0) {
      return 'KCT';
    }

    return new Intl.NumberFormat('en-US', {
      style: 'decimal',
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
      useGrouping: true,
    }).format(value);
  }

  formatNumber = (value: any): string => {
    if (value == -1) return 'DELETE VALUE';
    return value != null && value !== '' ? new Intl.NumberFormat('en-US').format(value) : '';
  };

  formatDate = (value: any): string => {
    return value ? new Date(value).toLocaleDateString('en-GB') : '';
  };

  formatDateTimeDelete = (value: any, time?: boolean): string => {
    if (!value && value !== 0) return '';

    if (value === -1) return 'DELETE VALUE';

    const date = new Date(value);

    // check DateTime.MinValue (0001-01-01)
    if (date.getFullYear() === 1 && date.getMonth() === 0 && date.getDate() === 1) {
      return 'DELETE VALUE';
    }

    return time
      ? date.toLocaleDateString('en-GB') +
          ' ' +
          date.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
      : date.toLocaleDateString('en-GB');
  };
  formatStringDelete = (value: any): string => {
    if (!value) {
      return '';
    }
    if (value === '-1') {
      return 'DELETE VALUE';
    }
    return value;
  };

  formatBoolean = (value: any): string => {
    return value ? '✓' : '-';
  };

  isStockWarningChanged(row: any): boolean {
    if (!row) return false;
    const current = row.currentStockWarning;
    const updated = row.stockWarning;
    return current !== updated;
  }

  // Text class function for Updated Stock Warning column - highlights when value differs from current
  getTextClassForStockWarning = (value: any, row: any): string => {
    return this.isStockWarningChanged(row) ? 'text-primary fw-bold' : '';
  };

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;
  @ViewChild('customCellTemplate', { static: true }) customCellTemplate: TemplateRef<any>;
  ImportMaterialManagementType: any;

  ngOnInit() {
    this.titleService.setTitle(this.title);

    this.requestId = this.route.snapshot.paramMap.get('id');
    if (this.requestId) {
      this.service.isBusy = true;

      this.service
        .loadDetailsAndImports(this.requestId)
        .pipe(finalize(() => (this.service.isBusy = false)))
        .subscribe({
          next: ({ details }) => {
            this.service.selected = details;
            this.service.buildForm();
            const importType = this.service.form?.get('importType')?.value;
            const status = this.service.selected?.status;

            // Hide currentStockWarning column if not IN_PROGRESS
            if (importType === 'MATERIAL.INVENTORY_PLANNING' && status !== 'IN_PROGRESS') {
              this.columnConfigs['MATERIAL.INVENTORY_PLANNING'] = this.columnConfigs[
                'MATERIAL.INVENTORY_PLANNING'
              ].filter(col => col !== 'currentStockWarning');
            }
          },
          error: error => {
            console.error('Error loading data', error);
          },
        });
    }
  }

  formatCurrentStockWarning = (value: any): string => {
    return value !== undefined && value !== null ? new Intl.NumberFormat('en-US').format(value) : '-';
  };

  public searchText: string = '';
  searchColumns = ['golfaCode', 'model'];
  private readonly tableFilterPipe = new TableFilterPipe();
  get filteredMaterials() {
    let data = this.service?.selected.materialApprovalDetails || [];
    if (this.searchText && this.searchText.trim() !== '') {
      data = this.tableFilterPipe.transform(data, this.searchText, this.searchColumns);
    }

    return data;
  }
  get filteredItemsCount(): number {
    return this.filteredMaterials.length;
  }
  get totalItemsCount(): number {
    return this.service?.selected.materialApprovalDetails.length || 0;
  }
  showHistory() {
    this.showHistoryModal = true;
    this.approvalHistories = this.service.selected?.materialHistories;
  }

  closeHistoryDialog(): void {
    this.showHistoryModal = false;
    this.approvalHistories = [];
  }

  performAction(action: string) {
    this.service.performAction(action);
  }

  goBack(): void {
    const currentUrl = this.router.url;

    const routesMap = [
      {
        condition:
          currentUrl.includes(AppRoutes.MATERIAL_STOCK.BASE) &&
          currentUrl.includes(AppRoutes.MATERIAL_STOCK.DETAILS.BASE) &&
          currentUrl.includes(AppRoutes.MATERIAL_STOCK.MY_APPROVALS.BASE),
        route: [AppRoutes.MATERIAL_STOCK.BASE, AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.BASE],
      },
      {
        condition:
          currentUrl.includes(AppRoutes.MATERIAL_STOCK.BASE) &&
          currentUrl.includes(AppRoutes.MATERIAL_STOCK.DETAILS.BASE),
        route: [AppRoutes.MATERIAL_STOCK.BASE, AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.BASE],
      },
    ];

    for (const route of routesMap) {
      if (route.condition) {
        this.router.navigate(route.route);
        break;
      }
    }
  }

  loadApprovalList() {
    if (this.service?.selected?.id) {
      this.service.getListApprovers(this.service?.selected?.id).subscribe({
        next: approvers => {
          this.approversModalComponent.openModal(approvers);
        },
        error: () => {
          this.toast.error('Failed to load approvers.');
        },
      });
    }
  }

  onHistoryClick(event: any) {}
}
