import { inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { filter, switchMap, finalize } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import {
  GetSupplierBUsInput,
  SupplierBUDto,
  SupplierBUService,
  SupplierBUUpdateDto,
} from '@proxy/supplier-bus';
import { LookupService } from '@proxy/general-lookups';

export abstract class AbstractFactoryViewService {
  protected readonly proxyService = inject(SupplierBUService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);
  public readonly proxyLookupService = inject(LookupService);
  private currentSubscription?: Subscription;
  supplierOptions: { value: string; code: string; label: string }[] = [];
  materialTypeOptions: { displayCode: string; displayName: string }[] = [];

  isExportToExcelBusy = false;
  filters = {} as GetSupplierBUsInput;

  data: PagedResultDto<SupplierBUDto> = {
    items: [],
    totalCount: 0,
  };
  selected: SupplierBUDto[] = [];
  clearFilter() {
    this.filters = {} as GetSupplierBUsInput;
    this.hookToQuery();
  }
  delete(record: SupplierBUDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }
  deactive(record: SupplierBUDto) {
    const messageKey = record.isDeactive
      ? '::ActiveConfirmationMessage' //
      : '::DeActiveConfirmationMessage';

    const updateParam = {
      ...record,
      isDeactive: !record.isDeactive,
    } as SupplierBUUpdateDto;

    this.confirmationService
      .warn(messageKey, '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.update(record.id, updateParam)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
      });

    const setData = (list: PagedResultDto<SupplierBUDto>) => {
      this.data = list;
    };

    if (!this.currentSubscription) {
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    } else {
      this.currentSubscription.unsubscribe();
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    }
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) =>
          this.proxyService.getListAsExcelFile({
            downloadToken: token,
            filterText: this.list.filter,
            ...this.filters,
          }),
        ),
        finalize(() => (this.isExportToExcelBusy = false)),
      )
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, 'SupplierBU.xlsx');
      });
  }
  supplier() {
    this.proxyLookupService.getSupplierLookup().subscribe(result => {
      this.supplierOptions = result.items.map(item => ({
        value: item.id,
        code: item.displayCode,
        label: item.displayName,
      }));
    });
  }
  materialType() {
    this.materialTypeOptions = [
      { displayCode: 'FA', displayName: 'FA' },
      { displayCode: 'LVS', displayName: 'LVS' },
    ];
  }

  deactiveMultiple() {
    const messageKey = this.selected[0].isDeactive
      ? '::ActiveConfirmationMessage'
      : '::DeActiveConfirmationMessage';
    this.confirmationService
      .warn(messageKey, '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => {
          const ids = this.selected.map(item => item.id) || [];

          return this.proxyService.changeDeactiveSupplierBU(ids);
        }),
      )
      .subscribe({
        next: () => {
          this.list.get();
          this.selected = [];
        },
        error: error => {
          console.error('Error in API calls:', error);
        },
      });
  }
}
