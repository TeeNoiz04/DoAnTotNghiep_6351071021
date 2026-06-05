import { Directive, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { Confirmation, ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { SaleOrdersManagementDetailViewService } from '@app/sale-orders/services/sale-orders-management-detail.service';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { downloadBlob } from '@app/shared/helpers/file-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { SaleOrderDto } from '@proxy/sale-orders';
import { SaleOrderExcelDto } from '@proxy/sale-orders/excel';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { filter, switchMap } from 'rxjs';
import { SaleOrdersDetailComponent } from '../sale-orders-details/sale-orders-details.component';
import { SaleOrderTypes } from '../sale-orders.model';
import { ImportSaleOrderModalComponent } from './components/import-sale-order-modal/import-sale-order-modal.component';
import { SaleOrdersManagementFilterComponent } from './components/sale-orders-management-filter/sale-orders-management-filter.component';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [SaleOrdersManagementFilterComponent, SaleOrdersDetailComponent];

@Directive()
export abstract class AbstractSaleOrdersManagementComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SaleOrdersManagementViewService);
  public readonly serviceDetail = inject(SaleOrdersManagementDetailViewService);
  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly templateService = inject(TemplateService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);

  protected title = 'Sale Orders (DPO)';
  protected isActionButtonVisible: boolean | null = null;
  protected showImport: boolean = false;
  protected showResultImport = false;
  protected importInformation = {};
  protected resultImports: ExcelValidationResult<SaleOrderExcelDto> | undefined;
  protected soRoutes = AppRoutes.SALE_ORDERS_MANAGEMENT;
  protected AppPermissions = AppPermissions;

  @ViewChild('importForm') importForm: ImportSaleOrderModalComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle('Sale Orders (DPO)');
    // this.service.loadBuyers();
    this.service.loadBuyerTypes();
    this.service.loadMaterialType();
    this.service.loadMaterialGroup();
    this.checkActionButtonVisibility();
  }

  onCreate() {
    this.router.navigate([`/${AppRoutes.SALE_ORDERS_MANAGEMENT.BASE}/${AppRoutes.SALE_ORDERS_MANAGEMENT.NEW.BASE}`]);
  }

  clearFilters() {
    this.service.clearFilters();
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  exportReportExcel() {
    this.service.exportReportExcel();
  }

  onSearch() {
    const values = this.service.searchForm.getRawValue();
    const previousValues = this.service.searchForm.value;
    const filters = {
      ...this.service.filters,
      ...values,
      soType: SaleOrderTypes.DPO,
      skipCount: 0,
    };
    this.service.filters = filters;
    this.service.saveFiltersToStorage();
    this.service.routeStateService.saveFilters(filters, previousValues, ['skipCount', 'maxResultCount'], false);
    this.service.soCheckedList = [];
    this.list.get();
    this.service.loadAllData();
  }

  onDelete(val: SaleOrderDto): void {
    this.service.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.proxyService.delete(val.id)),
      )
      .subscribe(() => {
        this.service.toast.success('Sale order deleted successfully', 'Success');
        this.list.get();
        this.service.loadAllData();
      });
  }

  onReopen(val: SaleOrderDto): void {
    this.service.confirmationService
      .warn('::ReopenConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.proxyService.reOpenSO(val.id)),
      )
      .subscribe(() => {
        this.service.toast.success('Sale order reopened successfully', 'Success');
        this.list.get();
        this.service.loadAllData();
      });
  }

  showHistoryModal = false;
  viewHistory(record: SaleOrderDto) {
    this.service.viewHistory(record);
    this.showHistoryModal = true;
  }

  closeHistoryDialog() {
    this.showHistoryModal = false;
  }

  onOpenImport() {
    this.showImport = true;
  }

  onOpenDownload() {
    this.templateService.getSOTemplate().subscribe({
      next: (blob: Blob) => downloadBlob(blob, `Template_Sale_Order.xlsx`),
      error: err => console.error('Error downloading template:', err),
    });
  }

  onCloseImportModal(): void {
    this.showImport = false;
    this.importForm?.importForm.reset();
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.resultImports = undefined;
    this.importForm?.clearSelectedFile();
  }

  verifyData() {
    const file = this.importForm?.fileImport;
    const values = this.importForm?.importForm.getRawValue();

    if (!file || !(file instanceof File)) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.importInformation = { file };

    this.loadingService.show();
    const formData = new FormData();
    formData.append('file', file);

    const handler = () => this.service.proxyService.validateAndParse(formData);

    if (!handler) {
      this.toast.error('Invalid import type.');
      this.loadingService.hide();
      return;
    }

    handler().subscribe({
      next: result => {
        result.listData = result.listData.map(row => ({
          ...row,
          rowData: {
            ...row.rowData,
          },
        }));

        this.resultImports = result;
        this.showResultImport = true;

        this.loadingService.hide();
      },
      error: this.handleImportError.bind(this),
    });
  }

  onBackFromResult() {
    // Clear the result import data
    this.resultImports = undefined;
    this.showResultImport = false;

    // Clear the file from the import form to prevent errors
    if (this.importForm) {
      this.importForm?.clearSelectedFile();
    }
  }

  onSubmitImport() {
    const values = this.importForm?.importForm.getRawValue();
    const resultImport = this.resultImports;

    const requestFn = () => this.service.proxyService.importSO(resultImport, values.note);

    if (!requestFn) {
      this.toast.error('Invalid import type.');
      return;
    }

    requestFn().subscribe({
      next: () => {
        this.toast.success('Sale order imported successfully.');
        this.resultImports = undefined;
        this.showImport = false;
        this.showResultImport = false;
        this.service.hookToQuery();
      },
      error: () => {
        this.toast.error('Failed to import sale order. Please try again.');
      },
    });
  }

  private handleImportError() {
    this.loadingService.hide();
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  // Method to toggle all rows
  toggleSelectAll(event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.service.soCheckedList = isChecked ? [...this.service.selectedAllDatas] : [];
  }

  // Method to check if all rows are soCheckedList
  isAllSelected(): boolean {
    return (
      this.service?.soCheckedList?.length > 0 &&
      this.service?.soCheckedList?.length === this.service.selectedAllDatas.length
    );
  }
  toggleRowSelection(row: any): void {
    const index = this.service.soCheckedList.indexOf(row);

    if (index > -1) {
      this.service.soCheckedList.splice(index, 1);
    } else {
      this.service.soCheckedList.push(row);
    }
  }
  isRowSelected(row: any): boolean {
    if (!this.service?.soCheckedList) return false;
    return this.service.soCheckedList.some(x => x.id === row.id);
  }
  protected checkActionButtonVisibility() {
    this.isActionButtonVisible =
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.ImportSAPSO) ||
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.ExportSAPData) ||
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.ExportReportData);
  }
}
