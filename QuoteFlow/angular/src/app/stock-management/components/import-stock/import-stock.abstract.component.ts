import { Directive, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { StockManagementService, StockManagementUploadDto } from '@proxy/stock-managements';
import { Observable, Subscription } from 'rxjs';
import { ImportStockViewService } from '../../services/import-stock.service';
import { ImportStockModalComponent } from './import-stock-modal/import-stock-modal.component';
import {
  ImportDownloadTemplateType,
  ImportMaterialOptions,
  ImportResultMap,
  ImportStockManagementType,
  ImportStockManagementTypeOption,
} from './import-stock.type';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractImportStockComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(ImportStockViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly templateService = inject(TemplateService);
  public readonly toast = inject(ToasterService);
  public readonly proxyService = inject(StockManagementService);

  protected title = '::Upload Material Stock';

  importMode = '';

  importMaterialInformation = {};
  showImportMaterial = false;
  showResultImport = false;
  importMaterialTypeSelected: ImportStockManagementTypeOption | undefined;
  importMaterialOptions = ImportMaterialOptions;
  resultImports: Partial<ImportResultMap> = {};

  private subscriptions: Subscription[] = [];

  @ViewChild('importMaterial') importMaterial: ImportStockModalComponent | undefined;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('Upload Material Stock');
    this.service.hookToQuery();
    this.checkImportOptionPermission();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  clearFilters() {
    this.service.clearFilters();
  }

  onSearch() {
    this.list.get();
  }

  getDetailUrl(request: StockManagementUploadDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.STOCK_MANAGEMENT.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.STOCK_MANAGEMENT.DETAILS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  onCloseImportModal(): void {
    this.showImportMaterial = false;
    this.importMaterial?.importForm.reset();
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.resultImports = {} as Partial<ImportResultMap>;
    this.importMaterial?.clearSelectedFile();
  }

  verifyData() {
    const file = this.importMaterial?.fileImport;
    const values = this.importMaterial?.importForm.getRawValue();

    const { note } = values || {};

    if (!file || !(file instanceof File) || !note) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.importMaterialInformation = { file, note };

    this.loadingService.show();
    const formData = new FormData();
    formData.append('file', file);

    const importHandlers = {
      [ImportStockManagementType.StockInventory]: () => this.proxyService.validateAndParseStockInventory(formData),
      [ImportStockManagementType.StockTransfer]: () => this.proxyService.validateAndParseStockTransfer(formData),
    };

    const handler = importHandlers[this.importMode];

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

        this.resultImports[this.importMode] = result;
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
    if (this.importMaterial) {
      this.importMaterial?.clearSelectedFile();
    }
  }

  onSubmitImport() {
    const values = this.importMaterial?.importForm.getRawValue();
    const resultImport = this.resultImports[this.importMode];

    const importHandlers: Partial<Record<ImportStockManagementType, () => Observable<any>>> = {
      [ImportStockManagementType.StockInventory]: () =>
        this.proxyService.importMaterialStockInventory(resultImport, values.note),
      [ImportStockManagementType.StockTransfer]: () =>
        this.proxyService.importMaterialStockTransfer(resultImport, values.note),
    };

    const requestFn = importHandlers[this.importMode];

    if (!requestFn) {
      this.toast.error('Invalid import type.');
      return;
    }

    requestFn().subscribe({
      next: () => {
        this.toast.success('Stock imported successfully.');
        this.resultImports = {} as Partial<ImportResultMap>;
        this.showImportMaterial = false;
        this.showResultImport = false;
        this.service.hookToQuery();
      },
      error: () => {
        this.toast.error('Failed to import Stock. Please try again.');
      },
    });
  }

  private handleImportError() {
    this.loadingService.hide();
    this.resultImports = {} as Partial<ImportResultMap>;
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  onOpenImportMaterial(val: ImportStockManagementTypeOption) {
    this.importMode = val.value;
    this.importMaterialTypeSelected = val;
    this.showImportMaterial = true;
  }

  onOpenDownloadImportMaterial(option: ImportStockManagementTypeOption) {
    if (!option || !option.value) {
      this.toast.error('Invalid import type selected.');
      return;
    }

    const importTypes = ImportDownloadTemplateType[option.value];
    this.templateService.getTemplateMaterial(importTypes).subscribe({
      next: (blob: Blob) => this.downloadBlob(blob, `Template_${option.value}.xlsx`),
      error: err => console.error('Error downloading template:', err),
    });
  }

  private downloadBlob(blob: Blob, fileName: string) {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  checkImportOptionPermission() {
    this.importMaterialOptions = this.importMaterialOptions.filter(option =>
      this.permissionService.getGrantedPolicy(option.requiredPolicy),
    );
  }
}
