import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { Subscription } from 'rxjs';
import {
  GetStockManagementsListInput,
  StockManagementListDto,
  StockManagementService,
} from '@proxy/stock-managements';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RouteStateService } from '@app/price-offers/services/route-state.service';
import { formatCommaSeparatedStr } from '@app/shared/helpers/format-helper';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';

export abstract class AbstractStockManagementViewService {
  protected readonly proxyService = inject(StockManagementService);
  public readonly list = inject(ListService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  private readonly fb = inject(FormBuilder);
  public readonly routeStateService = inject(RouteStateService);
  protected readonly abpWindowService = inject(AbpWindowService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;
  selected: StockManagementListDto[] = [];
  searchForm: FormGroup | undefined;

  data: PagedResultDto<StockManagementListDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    skipCount: 0,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetStockManagementsListInput;

  hookToQuery() {
    this.loadingService.show();
    const getData = (query: ABP.PageQueryParams) => {
      return this.proxyService.getListStockManagement({
        ...query,
        ...this.filters,
        golfaCode: formatCommaSeparatedStr(this.filters.golfaCode),
        model: formatCommaSeparatedStr(this.filters.model),
      });
    };

    const setData = (list: PagedResultDto<StockManagementListDto>) => {
      this.data = list;
      this.loadingService.hide();
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

  onPage(page: any) {
    this.filters.skipCount = page.offset * page.limit;
    this.filters.maxResultCount = page.pageSize;
    this.list.get();
    this.routeStateService.saveFilters(this.filters, undefined, ['skipCount', 'maxResultCount']);
  }

  buildSearchForm() {
    this.searchForm = this.fb.group({
      supplierCode: [''],
      supplierBUCode: [''],
      materialType: [''],
      golfaCode: [''],
      model: [''],
      materialGroup: [''],
      stockCategoryId: [''],
      greaterStockQty: [''],
      greaterOnOrderStockQty: [''],
      status: [''],
    });
  }

  clearFilters() {
    this.filters = {
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetStockManagementsListInput;
    this.searchForm?.reset();
    this.routeStateService.clearFilters();
  }

  onSearchFilters() {
    const values = this.searchForm.getRawValue();
    const previousValues = this.searchForm.value;
    const filters = {
      ...this.filters,
      ...values,
      skipCount: 0,
    };
    this.filters = filters;
    this.routeStateService.saveFilters(
      filters,
      previousValues,
      ['skipCount', 'maxResultCount'],
      false,
    );
    this.hookToQuery();
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    const formValues = this.searchForm?.getRawValue() ?? {};

    const filters = {
      ...this.filters,
      ...formValues,
    } as GetStockManagementsListInput;

    filters.golfaCode = formatCommaSeparatedStr(filters.golfaCode);
    filters.model = formatCommaSeparatedStr(filters.model);

    this.proxyService.getListStockManagementExcel(filters).subscribe(
      result => {
        this.abpWindowService.downloadBlob(result, 'Export_Stock_Management.xlsx');
        this.isExportToExcelBusy = false;
      },
      error => {
        this.isExportToExcelBusy = false;
      },
    );
  }
}
