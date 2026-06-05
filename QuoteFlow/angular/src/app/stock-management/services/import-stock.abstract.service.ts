import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { Subscription } from 'rxjs';
import {
  StockManagementUploadDto,
  StockManagementService,
  GetStockManagementApprovalsInput,
} from '@proxy/stock-managements';
import { GetMaterialStocksInput } from '@proxy/materials/material-stocks';

export abstract class AbstractImportStockViewService {
  protected readonly proxyService = inject(StockManagementService);
  protected readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;

  data: PagedResultDto<StockManagementUploadDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    isDeactive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetStockManagementApprovalsInput;

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getListUpload({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<StockManagementUploadDto>) => {
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

  clearFilters() {
    this.filters = {
      // isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetMaterialStocksInput;
    this.list.get();
  }

  exportToExcel() {
    // Implement if needed - for future enhancement
  }
}
