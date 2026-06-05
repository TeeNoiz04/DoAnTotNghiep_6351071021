import { Directive, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { StockTracingDetailDto } from '@proxy/stock-tracing-details';
import {
  StockTracingDeliveryImportDto,
  StockTracingDto,
  StockTracingInventoryImportDto,
  StockTracingReceiptImportDto,
  StockTracingService,
} from '@proxy/stock-tracings';
import { Observable, Subscription } from 'rxjs';
import { StockTracingDetailViewService } from '../../services/stock-tracing-detail.service';
import { StockTracingViewService } from '../../services/stock-tracing.service';
import { ImportStockComponent } from './import-stock/import-stock.component';
import { ImportStockTracingInformation } from './stock-tracing.types';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

export enum ImportStockTracingType {
  Delivery = 'Delivery',
  Receipt = 'Receipt',
  Inventory = 'Inventory',
}

export type ImportStockTracingTypeOption = {
  label: string;
  value: ImportStockTracingType;
};

@Directive()
export abstract class AbstractStockTracingComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  protected readonly router = inject(Router);
  public readonly track = inject(TrackByService);
  public readonly service = inject(StockTracingViewService);
  public readonly serviceDetail = inject(StockTracingDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly templateService = inject(TemplateService);
  public readonly titleService = inject(TitleService);
  protected readonly loadingService = inject(LoadingService);
  public readonly toast = inject(ToasterService);
  proxyService = inject(StockTracingService);

  protected title = '::Stock Data Upload';

  stockTracingSelected: StockTracingDto | undefined;
  stockTracingDetailSelected: StockTracingDetailDto[] = [];

  showImportStock = false;
  showResultImportStockTracing = false;
  resultImportDelivery: ExcelValidationResult<StockTracingDeliveryImportDto> | undefined;
  resultImportInventory: ExcelValidationResult<StockTracingInventoryImportDto> | undefined;
  resultImportReceipt: ExcelValidationResult<StockTracingReceiptImportDto> | undefined;
  stockTracingInformation: ImportStockTracingInformation | undefined;
  importStockTracingTypeSelected: ImportStockTracingTypeOption | undefined;
  importStockOptions: ImportStockTracingTypeOption[] = [
    {
      label: 'Delivery Report',
      value: ImportStockTracingType.Delivery,
    },
    {
      label: 'Receipt Report',
      value: ImportStockTracingType.Receipt,
    },
    {
      label: 'Inventory Report',
      value: ImportStockTracingType.Inventory,
    },
  ];

  private subscriptions: Subscription[] = [];

  importMode: ImportStockTracingType | undefined;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('Import Data | Stock Tracing');
    this.service.hookToQuery().subscribe();
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

  onFilterChange(filters: any) {
    this.service.filters = { ...filters };
    this.list.get();
  }

  getDetailUrl(request: any): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.STOCK_TRACING.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.STOCK_TRACING.DETAILS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  onDelete(record: StockTracingDto) {
    this.service.delete(record);
  }

  onOpenImportStockTracing(val: ImportStockTracingTypeOption) {
    this.importMode = val.value;
    this.importStockTracingTypeSelected = val;
    this.showImportStock = true;
  }

  onOpenDownloadStockTracing(option: ImportStockTracingTypeOption) {
    let request$: Observable<Blob>;

    switch (option.value) {
      case ImportStockTracingType.Delivery:
        request$ = this.templateService.getTemplateStockTracingDelivery();
        break;
      case ImportStockTracingType.Receipt:
        request$ = this.templateService.getTemplateStockTracingReceipt();
        break;
      case ImportStockTracingType.Inventory:
        request$ = this.templateService.getTemplateStockTracingInventory();
        break;
      default:
        console.error('Invalid import type');
        return;
    }

    request$.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Template_${option.value}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: err => {
        console.error('Error downloading template:', err);
      },
    });
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  @ViewChild('importStockTracing') importStockTracing: ImportStockComponent | undefined;
  onCloseImportResultModal() {
    this.importStockTracing?.resetForm();
  }
}
