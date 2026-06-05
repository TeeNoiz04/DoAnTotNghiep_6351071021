import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { ReportType } from '@proxy/stock-tracings';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import type { GetStockTracingsInput, StockTracingDto } from '../../../proxy/stock-tracings/models';
import { StockTracingService } from '../../../proxy/stock-tracings/stock-tracing.service';

export abstract class AbstractStockTracingViewService {
  protected readonly proxyService = inject(StockTracingService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);

  isExportToExcelBusy = false;

  data: PagedResultDto<StockTracingDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    fileName: undefined,
    reportType: ReportType.None,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetStockTracingsInput;

  delete(record: StockTracingDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  // hookToQuery() {
  //   const getData = (query: ABP.PageQueryParams) => {
  //     // Explicitly set pagination parameters
  //     this.filters.skipCount = query.skipCount || 0;
  //     this.filters.maxResultCount = query.maxResultCount || DEFAULT_PAGE_SIZE;

  //     return this.proxyService.getList({
  //       ...this.filters,
  //       filterText: query.filter || '',
  //     });
  //   };

  //   const setData = (list: PagedResultDto<StockTracingDto>) => {
  //     this.data = list;
  //   };

  //   return this.list.hookToQuery(getData).pipe(tap(setData));
  // }
  hookToQuery() {
    this.filters.maxResultCount = DEFAULT_PAGE_SIZE;
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<StockTracingDto>) => {
      this.data = list;
    };

    return this.list.hookToQuery(getData).pipe(tap(setData));
  }
  clearFilters() {
    this.filters = {
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetStockTracingsInput;
    this.hookToQuery();
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
        this.abpWindowService.downloadBlob(result, 'StockTracing.xlsx');
      });
  }
}
