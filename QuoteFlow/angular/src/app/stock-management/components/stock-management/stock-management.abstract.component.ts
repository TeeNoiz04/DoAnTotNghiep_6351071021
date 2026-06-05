import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { StockManagementListDto, StockManagementService } from '@proxy/stock-managements';
import { finalize, of, tap } from 'rxjs';
import { StockManagementDetailViewService } from '../../services/stock-management-detail.service';
import { StockManagementViewService } from '../../services/stock-management.service';
import { StockDetailsModalComponent } from '../stock-details-modal/stock-details-modal.component';
import { StockManagementFilterComponent } from './components/stock-management-filter/stock-management-filter.component';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [StockManagementFilterComponent, StockDetailsModalComponent];

@Directive()
export abstract class AbstractStockManagementComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(StockManagementViewService);
  public readonly serviceDetail = inject(StockManagementDetailViewService);
  public readonly permissionService = inject(PermissionService);
  protected readonly proxyService = inject(StockManagementService);
  public readonly titleService = inject(TitleService);
  protected readonly lookupService = inject(LookupService);

  protected title = 'Stock Management';
  protected isLoading: boolean = false;
  protected showLockShipmentDetailModal: boolean = false;
  protected showOnOrderStockDetailModal: boolean = false;
  protected showLockStockSODetailModal: boolean = false;
  protected showStockQtyDetailModal: boolean = false;
  protected showStockLockedDetailModal: boolean = false;
  protected showStockHistoryDetailModal: boolean = false;
  protected materialItem: StockManagementListDto = null;
  protected selectedStockCode: string = '';
  protected selectedGolfaCode: string = '';
  protected stockCategoryOptions: LookupDto<string>[] = [];

  ngOnInit() {
    this.titleService.setTitle('Stock Management');

    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    // this.loadStockCategories();

    this.disableListServiceLoading();
  }

  private disableListServiceLoading(): void {
    // Override the loading observables to always return false/idle
    Object.defineProperty(this.list, 'isLoading$', {
      get: () => of(false),
      configurable: true,
    });

    Object.defineProperty(this.list, 'requestStatus$', {
      get: () => of('idle'),
      configurable: true,
    });
  }

  onSearch() {
    this.list.page = 0;
    this.service.onSearchFilters();
  }

  clearFilters() {
    this.list.page = 0;
    this.service.clearFilters();
    this.service.onSearchFilters();
  }

  openDetails(val: StockManagementListDto): void {
    this.isLoading = false;
    this.proxyService
      .get(val?.golfaCode)
      .pipe(
        finalize(() => (this.isLoading = false)),
        tap((response: any) => {
          this.serviceDetail.selected = response;
          this.serviceDetail.showDetails = true;
        }),
      )
      .subscribe();
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  onViewStockQtyDetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.showStockQtyDetailModal = true;
  }

  onViewStockLockedDetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.showStockLockedDetailModal = true;
  }

  onViewLockStockSODetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.showLockStockSODetailModal = true;
  }

  onViewOnOrderStockDetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.showOnOrderStockDetailModal = true;
  }

  onViewLockShipmentDetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.showLockShipmentDetailModal = true;
  }

  onViewStockHistoryDetail(item: StockManagementListDto) {
    this.materialItem = item;
    this.selectedGolfaCode = item.golfaCode;

    // Get the stock code from cached lookup data using stockCategoryId
    const stockCategory = this.stockCategoryOptions.find(x => x.id === item.stockCategoryId);
    this.selectedStockCode = stockCategory?.displayCode || '';
    this.showStockHistoryDetailModal = true;
  }

  // Method to toggle all rows
  toggleSelectAll(event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.service.selected = isChecked ? [...this.service.data.items] : [];
  }

  // Method to check if all rows are selected
  isAllSelected(): boolean {
    if (this.service?.selected?.length === 0) {
      return false;
    }
    return this.service?.selected?.length === this.service?.data?.items?.length;
  }
  private loadStockCategories(): void {
    this.lookupService.getStockCategoryLookup().subscribe({
      next: result => {
        this.stockCategoryOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading stock categories:', error);
      },
    });
  }
}
