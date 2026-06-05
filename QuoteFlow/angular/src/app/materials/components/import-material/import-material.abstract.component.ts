import { Directive, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { downloadBlob } from '@app/shared/helpers/file-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { MaterialService } from '@proxy/materials';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests';
import { Observable, Subscription } from 'rxjs';
import { ImportMaterialViewService } from '../../services/import-material.service';
import { ImportMaterialModalComponent } from './import-material-modal/import-material-modal.component';
import {
  ImportMaterialManagementType,
  ImportMaterialManagementTypeOption,
  ImportMaterialOptions,
  ImportResultMap,
} from './import-material.type';

export const ChildTabDependencies = [];
export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractImportMaterialComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(ImportMaterialViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly templateService = inject(TemplateService);
  public readonly toast = inject(ToasterService);
  public readonly proxyService = inject(MaterialService);

  protected title = '::Upload Material Data';

  importMode = '';

  showDetails = false;
  showHistoryModal = false;
  approvalHistories = [];
  importMaterialInformation = {};

  showImportMaterial = false;
  showResultImport = false;
  importMaterialTypeSelected: ImportMaterialManagementTypeOption | undefined;
  importMaterialOptions = ImportMaterialOptions;
  resultImports: Partial<ImportResultMap> = {};
  totalImportErrors: number = 0;

  showWarningConfirmModal = false;
  warningConfirmCount = 0;
  private warningConfirmResolve: ((value: boolean) => void) | null = null;

  submitting = false;

  private subscriptions: Subscription[] = [];

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;
  @ViewChild('importMaterial') importMaterial: ImportMaterialModalComponent | undefined;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('Upload Material Data');
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

  performAction(action: string, row: MaterialApprovalRequestDto) {
    this.service.performAction(action, row);
  }

  getDetailUrl(request: MaterialApprovalRequestDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.MATERIAL_STOCK.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.MATERIAL_STOCK.DETAILS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  showHistory(record: MaterialApprovalRequestDto) {
    this.showHistoryModal = true;
    this.approvalHistories = record.materialHistories;
  }

  closeHistoryDialog(): void {
    this.showHistoryModal = false;
    this.approvalHistories = [];
  }

  onCloseImportModal(): void {
    this.showImportMaterial = false;
    this.importMaterial?.importForm?.reset();
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.totalImportErrors = 0;
    this.resultImports = {} as Partial<ImportResultMap>;
    this.importMaterial?.resetForm();
  }

  onBack() {
    setTimeout(() => {
      this.importMaterial?.resetForm();
    });
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
    this.totalImportErrors = 0;
    const formData = new FormData();
    formData.append('file', file);

    const importHandlers = {
      [ImportMaterialManagementType.NewMaterial]: () => this.proxyService.validateAndParseNewRegistration(formData),
      [ImportMaterialManagementType.ApprovalPrice]: () => this.proxyService.validateAndParseUpdatePrice(formData),
      [ImportMaterialManagementType.WithoutPrice]: () => this.proxyService.validateAndParseUpdateWithoutPrice(formData),
      [ImportMaterialManagementType.Status]: () => this.proxyService.validateAndParseStatus(formData),
      [ImportMaterialManagementType.InventoryPlanning]: () =>
        this.proxyService.validateAndParseUpdateInventoryPlan(formData),
      [ImportMaterialManagementType.Leadtime]: () => this.proxyService.validateAndParseFactory(formData),
      [ImportMaterialManagementType.SAPCode]: () => this.proxyService.validateAndParseSAP(formData),
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
            productionDate: this.formatDate(row.rowData.productionDate),
          },
        }));

        this.resultImports[this.importMode] = result;
        this.showResultImport = true;
        this.totalImportErrors = result?.listData?.reduce((acc, item) => acc + item.errors.length, 0);
        this.loadingService.hide();
      },
      error: err => {
        this.loadingService.hide();
        this.toast.error('Failed to process the file.');
        this.handleImportError();
      },
    });
  }

  async onSubmitImport() {
    if (this.importMode === ImportMaterialManagementType.Leadtime) {
      const resultImport = this.resultImports[this.importMode];
      const hasWarnings = (resultImport as any)?.hasNotFoundWarnings;

      if (hasWarnings) {
        const warningCount = (resultImport as any)?.listData?.filter((x: any) => x.warnings?.length > 0).length ?? 0;
        const confirmed = await this.showWarningConfirm(warningCount);
        if (!confirmed) return;
      }
    }

    this.submitting = true;
    const values = this.importMaterial?.importForm.getRawValue();
    const resultImport = this.resultImports[this.importMode];

    const importHandlers: Partial<Record<ImportMaterialManagementType, () => Observable<any>>> = {
      [ImportMaterialManagementType.NewMaterial]: () =>
        this.proxyService.importMaterialNewRegistration(resultImport, values.note),
      [ImportMaterialManagementType.ApprovalPrice]: () =>
        this.proxyService.importMaterialUpdatePrice(resultImport, values.note),
      [ImportMaterialManagementType.WithoutPrice]: () =>
        this.proxyService.importMaterialUpdateWithoutPrice(resultImport, values.note),
      [ImportMaterialManagementType.Status]: () => this.proxyService.updateMaterialStatus(resultImport, values.note),
      [ImportMaterialManagementType.InventoryPlanning]: () =>
        this.proxyService.materialUpdateInventoryPlan(resultImport, values.note),
      [ImportMaterialManagementType.Leadtime]: () => this.proxyService.updateMaterialFactory(resultImport, values.note),
      [ImportMaterialManagementType.SAPCode]: () => this.proxyService.updateMaterialSAP(resultImport, values.note),
    };

    const requestFn = importHandlers[this.importMode];

    if (!requestFn) {
      this.toast.error('Invalid import type.');
      this.submitting = false;
      return;
    }

    requestFn().subscribe({
      next: () => {
        this.toast.success('Material imported successfully.');
        this.resultImports = {} as Partial<ImportResultMap>;
        this.showImportMaterial = false;
        this.showResultImport = false;
        this.submitting = false;
        this.service.hookToQuery();
      },
      error: () => {
        this.toast.error('Failed to import Material. Please try again.');
        this.submitting = false;
      },
    });
  }

  // ✅ Custom confirm modal methods
  private showWarningConfirm(warningCount: number): Promise<boolean> {
    this.warningConfirmCount = warningCount;
    this.showWarningConfirmModal = true;
    return new Promise(resolve => {
      this.warningConfirmResolve = resolve;
    });
  }

  onWarningConfirmOk() {
    this.showWarningConfirmModal = false;
    this.warningConfirmResolve?.(true);
    this.warningConfirmResolve = null;
  }

  onWarningConfirmCancel() {
    this.showWarningConfirmModal = false;
    this.warningConfirmResolve?.(false);
    this.warningConfirmResolve = null;
  }

  private handleImportError() {
    this.loadingService.hide();
    this.totalImportErrors = 0;
    this.resultImports = {} as Partial<ImportResultMap>;
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  private formatDate(dateString?: string): string | undefined {
    if (!dateString) return undefined;
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString;
    return date.toISOString().split('T')[0];
  }

  onOpenImportMaterial(val: ImportMaterialManagementTypeOption) {
    this.importMode = val.value;
    this.importMaterialTypeSelected = val;
    this.showImportMaterial = true;
  }

  onOpenDownloadImportMaterial(option: ImportMaterialManagementTypeOption) {
    this.templateService.getTemplateMaterial(option.value).subscribe({
      next: (blob: Blob) => downloadBlob(blob, `Template_${option.value}.xlsx`),
      error: err => console.error('Error downloading template:', err),
    });
  }

  loadApprovalList(rowData: MaterialApprovalRequestDto) {
    if (rowData?.id) {
      this.service.getListApprovers(rowData.id).subscribe({
        next: approvers => {
          this.approversModalComponent.openModal(approvers);
        },
        error: () => {
          this.toast.error('Failed to load approvers.');
        },
      });
    }
  }

  checkImportOptionPermission() {
    this.importMaterialOptions = this.importMaterialOptions.filter(option =>
      this.permissionService.getGrantedPolicy(option.requiredPolicy),
    );
  }
}
