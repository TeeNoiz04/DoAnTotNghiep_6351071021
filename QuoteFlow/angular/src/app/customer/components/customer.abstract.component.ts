import { Directive, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { AppPermissions } from '@app/app.permissions';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { CustomerDto, CustomerService } from '@proxy/customers';
import { Subject } from 'rxjs';
import { CustomerDetailViewService } from '../services/customer-detail.service';
import { CustomerFilterService } from '../services/customer-filter.service';
import { CustomerViewService } from '../services/customer.service';
import { ImportCustomerModalComponent } from './import-customer-modal/import-customer-modal.component';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractCustomerComponent implements OnInit, OnDestroy {
  public readonly track = inject(TrackByService);
  public readonly service = inject(CustomerViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly serviceDetail = inject(CustomerDetailViewService);
  protected readonly filterService = inject(CustomerFilterService);
  public readonly toast = inject(ToasterService);
  protected readonly loadingService = inject(LoadingService);
  public readonly proxyService = inject(CustomerService);

  protected title = '::Customer';

  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;
  ngOnInit() {
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });
    this.filterService.hookToQuery();
    this.titleService.setTitle('Customer Management');
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.showForm();
  }

  update(record: CustomerDto) {
    this.serviceDetail.update(record);
  }

  delete(record: CustomerDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.filterService.exportToExcel();
  }

  showImport = false;
  showResultImport = false;
  resultImports: any = {};
  totalImportErrors: number = 0;
  @ViewChild('importCustomer') importCustomer: ImportCustomerModalComponent | undefined;
  onCloseImportModal(): void {
    this.showImport = false;
  }

  importExcel() {
    this.showImport = true;
  }
  verifyData() {
    const file = this.importCustomer?.fileImport;

    if (!file || !(file instanceof File)) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.loadingService.show();
    this.totalImportErrors = 0;
    const formData = new FormData();
    formData.append('file', file);

    const handler = () => this.proxyService.validateAndParseCustomer(formData);

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
        this.totalImportErrors = result?.listData?.reduce((acc, item) => acc + item.errors.length, 0);
        this.loadingService.hide();
      },
      error: this.handleImportError.bind(this),
    });
  }
  private handleImportError() {
    this.loadingService.hide();
    this.totalImportErrors = 0;
    this.resultImports = {};
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.totalImportErrors = 0;
    this.resultImports = {};
    this.importCustomer?.resetForm();
  }
  onBack() {
    setTimeout(() => {
      this.importCustomer?.resetForm();
    });
  }

  public submitting = false;
  onSubmitImport() {
    this.submitting = true;
    const resultImport = this.resultImports;
    const requestFn = () => this.proxyService.importCustomer(resultImport);
    if (!requestFn) {
      this.toast.error('Invalid import type.');
      this.submitting = false;
      return;
    }
    requestFn().subscribe({
      next: () => {
        this.toast.success('Stock imported successfully.');
        this.resultImports = {};
        this.showImport = false;
        this.showResultImport = false;
        this.submitting = false;
        this.filterService.hookToQuery();
      },
      error: () => {
        this.submitting = false;
        this.toast.error('Failed to import Stock. Please try again.');
      },
    });
  }
}
