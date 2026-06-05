import { Directive, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { Confirmation, ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';

import { finalize, Observable, Subscription } from 'rxjs';

import { SpoBatchRequestDto, SpoBatchRequestService } from '@proxy/spo-batch-requests';
import { ImportBatchRequestViewService } from '../services/import-batch-request.service';
import {
  ImportBatchRequestType,
  ImportBatchRequestTypeOption,
  ImportResultMap,
} from './import-batch-request/batch-request.types';
import { ImportBatchRequestModalComponent } from './import-batch-request/import-batch-request-modal/import-batch-request-modal.component';
import { ConfirmationService } from '@abp/ng.theme.shared';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractImportBatchRequestComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(ImportBatchRequestViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly templateService = inject(TemplateService);
  public readonly toast = inject(ToasterService);
  public readonly proxyService = inject(SpoBatchRequestService);
  public readonly confirmation = inject(ConfirmationService);

  protected title = '::SPO Batch Request';

  importMode = '';

  importBatchRequestInformation = {};
  showImportBatchRequest = false;
  showResultImport = false;
  importBatchRequestTypeSelected: ImportBatchRequestTypeOption | undefined;

  resultImports: Partial<ImportResultMap> = {};

  private subscriptions: Subscription[] = [];

  @ViewChild('importBatchRequest') importBatchRequest: ImportBatchRequestModalComponent | undefined;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('SPO Batch Request Management | SPO Batch Request');
    this.service.hookToQuery();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  clearFilters() {
    this.service.clearFilters();
  }

  onSearch() {
    this.service.hookToQuery();
  }

  getDetailUrl(request: SpoBatchRequestDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.SPECIAL_PRICE_OFFERS.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS_BATCH_REQUEST.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  getDetailQueryParams(): { returnStatus: string } {
    return { returnStatus: this.service.filters.status };
  }

  onCloseImportModal(): void {
    this.showImportBatchRequest = false;
    this.importBatchRequest?.importForm.reset();
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.resultImports = {} as Partial<ImportResultMap>;
    this.importBatchRequest?.clearSelectedFile();
  }

  verifyData() {
    const file = this.importBatchRequest?.fileImport;
    const values = this.importBatchRequest?.importForm.getRawValue();
    const { note } = values || {};

    if (!file || !(file instanceof File) || !note) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.importBatchRequestInformation = { file, note };

    this.loadingService.show();
    const formData = new FormData();
    formData.append('file', file);

    const importHandlers = {
      [ImportBatchRequestType.BatchRequest]: () => this.proxyService.validateAndParseBatchRequest(formData),
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
    if (this.importBatchRequest) {
      this.importBatchRequest?.clearSelectedFile();
    }
  }

  onSubmitImport() {
    const values = this.importBatchRequest?.importForm.getRawValue();
    const resultImport = this.resultImports[this.importMode];

    const importHandlers: Partial<Record<ImportBatchRequestType, () => Observable<any>>> = {
      [ImportBatchRequestType.BatchRequest]: () => this.proxyService.importSPOBatchRequest(resultImport, values.note),
    };

    const requestFn = importHandlers[this.importMode];

    if (!requestFn) {
      this.toast.error('Invalid import type.');
      return;
    }
    this.loadingService.show();

    requestFn()
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: () => {
          this.toast.success('Stock imported successfully.');
          this.resultImports = {} as Partial<ImportResultMap>;
          this.showImportBatchRequest = false;
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

  onOpenImportBatchRequest() {
    this.importMode = 'BATCH_REQUEST';

    this.showImportBatchRequest = true;
  }

  onOpenDownloadImportBatchRequest() {
    this.templateService.getBatchRequestImportTemplate().subscribe({
      next: (blob: Blob) => this.downloadBlob(blob, `Template_BatchRequest.xlsx`),
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

  onDeleteBatchRequest(row: SpoBatchRequestDto): void {
    this.confirmation
      .warn('Are you sure you want to delete this SPO Batch Request?', 'Confirm Delete', {
        yesText: '::Yes',
        cancelText: '::No',
      })
      .subscribe(result => {
        if (result === Confirmation.Status.confirm) {
          this.loadingService.show();
          this.proxyService
            .deleteBatchRequest(row.id)
            .pipe(finalize(() => this.loadingService.hide()))
            .subscribe({
              next: () => {
                this.toast.success('SPO Batch Request deleted successfully.');
                this.service.hookToQuery();
              },
              error: err => {
                const message = err?.error?.error?.message || 'Failed to delete. Please try again.';
                this.toast.error(message);
              },
            });
        }
      });
  }
}
