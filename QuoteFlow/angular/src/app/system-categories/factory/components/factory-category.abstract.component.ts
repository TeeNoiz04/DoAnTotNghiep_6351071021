import { Directive, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { SupplierBUDto, SupplierBUService } from '@proxy/supplier-bus';
import { debounceTime, filter, Subject, takeUntil } from 'rxjs';
import { FactoryDetailViewService } from '../services/factory-detail.service';
import { FactoryViewService } from '../services/factory.service';
import { ImportSupplierBUModalComponent } from './import-supplier-bu-modal/import-supplier-bu-modal.component';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractFactoryCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(FactoryViewService);
  public readonly serviceDetail = inject(FactoryDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);
  public readonly toast = inject(ToasterService);
  protected readonly loadingService = inject(LoadingService);
  public readonly proxyService = inject(SupplierBUService);

  protected title = '::Supplier BU';

  private searchTextChanged$ = new Subject<string>();
  @ViewChild('importSupplierBU') importSupplierBU: ImportSupplierBUModalComponent | undefined;
  showImport = false;
  totalImportErrors: number = 0;
  showResultImport = false;
  resultImports: any = {};
  AppPermissions = AppPermissions;
  private destroy$ = new Subject<void>();
  ngOnInit() {
    this.service.filters.isDeactive = false;

    this.titleService.setTitle('Supplier BU');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        // When navigation occurs, call hookToQuery which now includes syncFromQueryParams
        this.service.hookToQuery();
      });

    // Set up search text debounce
    this.searchTextChanged$.pipe(debounceTime(500), takeUntil(this.destroy$)).subscribe(() => {
      this.onLoad();
    });

    // Initial setup
    this.service.hookToQuery();
    this.service.supplier();
    this.service.materialType();
  }
  deactiveOptions = [
    {
      value: true,
      label: 'Yes',
    },
    {
      value: false,
      label: 'No',
    },
  ];
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onLoad() {
    this.service.hookToQuery();
  }
  onSearchTextChange(text: string) {
    this.searchTextChanged$.next(text);
  }

  clearFilters() {
    this.service.clearFilter();
  }

  showForm() {
    // this.serviceDetail.showForm();
  }

  create() {
    this.serviceDetail.create();
  }

  update(record: SupplierBUDto) {
    this.serviceDetail.update(record);
  }

  delete(record: SupplierBUDto) {
    this.service.delete(record);
  }
  deactiveRow(record: SupplierBUDto) {
    this.service.deactive(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  onOpenImport() {
    this.showImport = true;
  }
  onBack() {
    setTimeout(() => {
      this.importSupplierBU?.resetForm();
    });
  }
  submitting = false;
  onSubmitImport() {
    this.submitting = true;
    const resultImport = this.resultImports;
    const requestFn = () => this.proxyService.importSupplierBU(resultImport);
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
        this.service.hookToQuery();
      },
      error: () => {
        this.submitting = false;
        this.toast.error('Failed to import Stock. Please try again.');
      },
    });
  }
  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.totalImportErrors = 0;
    this.resultImports = {};
    this.importSupplierBU?.resetForm();
  }
  onCloseImportModal(): void {
    this.showImport = false;
  }
  verifyData() {
    const file = this.importSupplierBU?.fileImport;

    if (!file || !(file instanceof File)) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.loadingService.show();
    this.totalImportErrors = 0;
    const formData = new FormData();
    formData.append('file', file);

    const handler = () => this.proxyService.validateAndParseSupplierBU(formData);

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

  isAllSelected(): boolean {
    if (this.service?.selected?.length === 0) {
      return false;
    }
    return this.service?.selected?.length === this.service?.data?.items?.length;
  }

  toggleSelectAll(event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.service.selected = isChecked ? [...this.service.data.items] : [];
  }

  deactive() {
    if (this.service.selected.length === 0) {
      // this.toast.warning('Please select at least one record to deactivate.');
      return;
    }
    this.service.deactiveMultiple();
  }
  get canDeactive(): boolean {
    return this.service.selected.length > 0;
  }
}
