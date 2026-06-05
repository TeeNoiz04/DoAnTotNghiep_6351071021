import { ABP, AbpWindowService, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import {
  GetStockTracingDetailsInput,
  StockTracingDetailDto,
  StockTracingDetailExcelDownloadDto,
  StockTracingDetailService,
} from '@proxy/stock-tracing-details';
import { StockTracingService } from '@proxy/stock-tracings';
import { finalize, switchMap, tap } from 'rxjs/operators';
import { ImportStockTracingType } from '../components/import-stock-tracing/stock-tracing.abstract.component';
export abstract class AbstractStockTracingDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(StockTracingService);
  public readonly proxyServiceDetail = inject(StockTracingDetailService);
  public readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);

  data: PagedResultDto<StockTracingDetailDto> = {
    items: [],
    totalCount: 0,
  };

  form: FormGroup | undefined;
  isBusy = false;
  selected = {} as any;

  filters = {
    stockTracingId: undefined,
    reportType: undefined,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetStockTracingDetailsInput;

  searchForm: FormGroup | undefined;

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyServiceDetail.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<StockTracingDetailDto>) => {
      this.data = list;
    };

    this.list.hookToQuery(getData).subscribe(setData);
  }

  onPage(page: any) {
    this.filters.skipCount = page.offset * page.pageSize;
    this.filters.maxResultCount = page.pageSize;
    this.list.get();
    // this.routeStateService.saveFilters(this.filters, undefined, ['skipCount', 'maxResultCount']);
    // this.loadAllData();
  }

  getStockTracingDetails(stockTracingId: string) {
    if (!stockTracingId) return;
    this.isBusy = true;
    this.proxyService
      .get(stockTracingId)
      .pipe(
        finalize(() => (this.isBusy = false)),
        tap((response: any) => {
          const reportTypeMapping: { [key: number]: ImportStockTracingType } = {
            1: ImportStockTracingType.Delivery,
            2: ImportStockTracingType.Receipt,
            3: ImportStockTracingType.Inventory,
          };

          // Gọi method khác để update form
          this.updateForm(response.fileName, response.reportType, reportTypeMapping, response.note);
          this.selected.items = response?.details;
          // this.buildForm();
        }),
      )
      .subscribe();
  }

  private updateForm(fileName: string, reportType: any, mapping: any, note?: string): void {
    if (this.form) {
      this.form.patchValue({
        fileName: fileName,
        reportType: mapping[reportType] || null,
        note: note || null,
      });
    }
  }

  // buildForm(): void {
  //   this.form = this.fb.group({
  //     fileName: [null, []],
  //     reportType: [null, []],
  //   });
  // }

  buildSearchForm() {
    this.searchForm = this.fb.group({
      series: [''],
      reportType: [''],
      materialType: [''],
      golfaCode: [''],
      model: [''],
      fromDate: [''],
      toDate: [''],
      customer: [''],
      skuName: [''],
      packingListCode: [''],
      checkListCode: [''],
      note: [''],
    });
  }
  isImportBusy = false;
  exportStockTracingDetail() {
    const filtersExcel = this.filters as StockTracingDetailExcelDownloadDto;
    this.isImportBusy = true;

    this.proxyServiceDetail
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) => {
          return this.proxyServiceDetail.getListAsExcelFile({
            downloadToken: token,
            ...filtersExcel,
          });
        }),
        finalize(() => {
          this.isImportBusy = false; // Luôn được gọi dù thành công hay thất bại
        }),
      )
      .subscribe({
        next: result => {
          if (result) {
            this.abpWindowService.downloadBlob(result, 'StockTracingDetail.xlsx');
          }
          this.isImportBusy = false;
        },
        error: err => {
          this.isImportBusy = false;
        },
      });
  }

  buildForm(): void {
    this.form = this.fb.group({
      fileName: [null, []],
      reportType: [null, []],
      note: [null, []],
    });
  }
}
