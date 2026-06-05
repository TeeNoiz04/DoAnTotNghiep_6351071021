import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { finalize, map, Observable, of, Subscription } from 'rxjs';
import {
  DataMaterialOverallStockReportDto,
  GetStockManagementsListInput,
  StockManagementService,
} from '@proxy/stock-managements';
import { FormBuilder } from '@angular/forms';
import { RouteStateService } from '@app/shared/services/route-state.service';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { ToasterService } from '@abp/ng.theme.shared';
import { DialogService } from '@app/shared/services/dialog/dialog.service';
import { InventoryReportDialogComponent } from '../components/inventory-report-dialog/inventory-report-dialog.component';
import { StockReportDialogService } from './stock-report-dialog.service';

export abstract class AbstractStockReportViewService {
  public readonly proxyService = inject(StockManagementService);
  public readonly list = inject(ListService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  private readonly fb = inject(FormBuilder);
  public readonly routeStateService = inject(RouteStateService);
  protected readonly abpWindowService = inject(AbpWindowService);
  private readonly dialogService = inject(DialogService);
  private readonly toast = inject(ToasterService);
  private readonly stockReportDialogService = inject(StockReportDialogService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;
  totals = {
    available_Stock: 0,
    keeping_Stock: 0,
    on_Order_Stock: 0,
    stockWarning: 0,
  };

  data: PagedResultDto<DataMaterialOverallStockReportDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    skipCount: 0,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetStockManagementsListInput;

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.geDataOverallStockReport().pipe(
        map(resp => {
          return {
            items: resp,
            totalCount: resp?.length,
          } as PagedResultDto<DataMaterialOverallStockReportDto>;
        }),
      );

    const setData = (list: PagedResultDto<DataMaterialOverallStockReportDto>) => {
      this.data = list;
      this.calculateTotals();
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

  calculateTotals() {
    this.totals = (this?.data?.items as any)?.reduce(
      (acc, item) => {
        acc.available_Stock += item.available_Stock ?? 0;
        acc.keeping_Stock += item.keeping_Stock ?? 0;
        acc.on_Order_Stock += item.on_Order_Stock ?? 0;
        acc.stockWarning += item.stockWarning ?? 0;
        return acc;
      },
      { available_Stock: 0, keeping_Stock: 0, on_Order_Stock: 0, stockWarning: 0 },
    );
  }

  openInventoryReportDialog(): void {
    this.stockReportDialogService.openInventoryReportDialog();
  }

  getInventoryReportGenerationFunction(): (data: any) => Observable<any> {
    return (data: any) => {
      return this.generateInventoryReport(data);
    };
  }

  private generateInventoryReport(filterData: any): Observable<any> {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return of(null);
    }

    this.isExportToExcelBusy = true;
    const payload = {
      materialCode: filterData.materialCode,
      inventoryCategory: filterData.inventoryCategory,
      materialGroup: filterData.materialGroup,
      skipCount: 0,
      maxResultCount: 1000,
    };
    return this.proxyService.getListExcelInventoryReport(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }
}
