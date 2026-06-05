import { ABP, CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, InjectionToken, Input, OnInit, Output, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import {
  GetSaleOrderListDetailDPOsInput,
  SaleOrderAddedDetailDPODto,
  SaleOrderListDetailDPODto,
} from '@proxy/sale-orders';
import { finalize } from 'rxjs';

export const DETAIL_LIST = new InjectionToken<ListService>('DETAIL_LIST');

@Component({
  selector: 'app-add-from-dpo-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    CoreModule,
    ThemeSharedModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
  ],
  providers: [
    {
      provide: DETAIL_LIST,
      useClass: ListService,
    },
  ],
  templateUrl: './add-from-dpo-dialog.component.html',
})
export class AddFromDpoDialogComponent implements OnInit {
  protected readonly list = inject(DETAIL_LIST);
  private readonly service = inject(SaleOrdersManagementViewService);
  private readonly toasterService = inject(ToasterService);
  public readonly loadingService = inject(LoadingService);

  @Input() isVisible = false;
  @Input() saleOrderId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() itemsAdded = new EventEmitter<any[]>();
  @Output() itemsAddedAndDisable = new EventEmitter<any[]>();

  isBusy = false;
  totalCount = 0;
  currentPage = 1;
  pageSize = DEFAULT_PAGE_SIZE;
  items: SaleOrderListDetailDPODto[] = [];
  selectedItems: SaleOrderListDetailDPODto[] = [];
  extraFeeValidate = false;
  filteredData: SaleOrderListDetailDPODto[] = [];
  selectedAllData: SaleOrderListDetailDPODto[] = [];
  data: PagedResultDto<SaleOrderListDetailDPODto> = {
    items: [],
    totalCount: 0,
  };
  searchText: string = '';
  pagedData: SaleOrderListDetailDPODto[] = [];

  filters = {
    // maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetSaleOrderListDetailDPOsInput;

  ngOnInit(): void {
    this.hookToQuery();
  }

  hookToQuery() {
    this.loadingService.show();
    this.isBusy = true;
    const getData = (query: ABP.PageQueryParams) => {
      return this.service.proxyService.getListDetailDPO({
        // ...query,
        ...this.filters,
        buyerId: this.service.selected.buyerId,
        materialType: this.service.selected.materialType,
        vat: this.service.selected.sO_VAT,
        stockCategoryId: this.service.selected.stockCategoryId,
      });
    };

    const setData = (list: PagedResultDto<SaleOrderListDetailDPODto>) => {
      this.data = list;
      this.totalCount = list?.items?.length || 0;
    };

    this.list.hookToQuery(getData).subscribe({
      next: res => {
        setData(res);
        this.isBusy = false;
        this.setPage(1);
        this.loadingService.hide();
      },
      error: () => {
        this.toasterService.error('Failed to load items', 'Error');
        this.loadingService.hide();
      },
    });
  }

  formatPercentVat = (value, entry) => {
    return value ? value * 100 + '%' : value;
  };

  closeDialog(): void {
    this.isVisible = false;
    this.visibleChange.emit(false);
    this.resetDialog();
  }

  resetDialog(): void {
    this.selectedItems = [];
    this.items = [];
    this.totalCount = 0;
  }

  onSearch(): void {
    this.resetDialog();
    this.hookToQuery();
  }

  clearSearch(): void {
    this.resetDialog();
    this.filters = {
      // maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetSaleOrderListDetailDPOsInput;
    this.hookToQuery();
  }

  onSelectionChange($event): void {
    const newItems = $event?.data || [];

    this.selectedItems = [...this.selectedItems, ...newItems.filter(x => !this.selectedItems.includes(x))];
  }

  onSelectionCheckedFalse($event): void {
    const removeItems = $event?.data || [];

    const makeKey = (g, m, d) => `${(g || '').toLowerCase()}|${(m || '').toLowerCase()}|${(d || '').toLowerCase()}`;

    const removeSet = new Set(removeItems.map(x => makeKey(x.golfaCode, x.model, x.dpoNo)));

    this.selectedItems = this.selectedItems.filter(x => !removeSet.has(makeKey(x.golfaCode, x.model, x.dpoNo)));
  }

  onItemSelectChange(entry: any) {
    // Auto-fill quantity when row is selected - Sale orders uses qty as the quantity field
    if (entry?.checked) {
      // Sale orders DPO doesn't need auto-fill as qty is read-only display field
    } else {
      // Reset any temporary values if needed
    }
  }

  onChangeValueCellItem($event: any) {
    const { entry, column } = $event;
    $event.entry.errors = $event?.entry?.errors || {};

    // Add validation for quantity fields if they become editable
    if (column.value === 'qty') {
      const qtyNumber = NumberHelper.convertToNumber(entry?.qty);
      if (qtyNumber <= 0) {
        $event.entry.errors.qty = `Quantity must be greater than 0`;
      } else {
        delete $event.entry.errors.qty;
      }
    }

    if (column.value === 'extrafeeSO') {
      const extrafeeSONumber = NumberHelper.convertToNumber(entry?.extrafeeSO);
      const extrafeeDPONumber = NumberHelper.convertToNumber(entry?.extrafee);
      if (entry?.extrafee != null) {
        if (extrafeeSONumber > extrafeeDPONumber) {
          $event.entry.errors.extrafeeSO = `SO Extra Fee must be less than DPO Extra Fee`;
          this.extraFeeValidate = true;
        } else {
          delete $event.entry.errors.extrafeeSO;
          this.extraFeeValidate = false;
        }
      } else {
        if (extrafeeSONumber) {
          $event.entry.errors.extrafeeSO = `Cannot entered SO Extra Fee`;
          this.extraFeeValidate = true;
        } else {
          delete $event.entry.errors.extrafeeSO;
          this.extraFeeValidate = false;
        }
      }
    }
  }

  onSave(): void {
    if (this.selectedItems.length === 0) {
      this.toasterService.warn('Please select at least one item', 'Warning');
      return;
    }

    this.isBusy = true;

    const arrTemp = (this.selectedItems as any).map(item => {
      return {
        ...item,
        note: item?.note || '',
        extrafee: item?.extrafeeSO ? NumberHelper.convertToNumber(item?.extrafeeSO) : 0,
      };
    });

    const payload: SaleOrderAddedDetailDPODto[] = arrTemp.map(item => ({
      prSOId: this.saleOrderId,
      dpoDetailId: item.dpoDetail_Id,
      lockStockId: item.lockstockId,
      materialCode: item.golfaCode,
      qty: item.qty,
      price: item.unitPrice || 0,
      vat: item.vat,
      stockCategoryId: this.service?.selected?.stockCategoryId,
      extrafee: item.extrafee || 0,
      note: item.note || '',
      materialType: this.service?.selected?.materialType || '',
    }));

    this.service.proxyService
      .createDetailDPO(payload)
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: () => {
          this.toasterService.success('Items added successfully', 'Success');
          this.isVisible = false;
          this.itemsAdded.emit(this.selectedItems);
          this.itemsAddedAndDisable.emit(this.selectedItems);
          this.resetDialog();
        },
        error: error => {
          this.selectedItems = [];
          this.hookToQuery();
        },
      });
  }
  setPage(page: number) {
    this.currentPage = page;
    const start = (page - 1) * this.pageSize;
    const end = start + this.pageSize;
    const text = this.searchText?.trim().toLowerCase();
    if (!text) {
      this.pagedData = this.data.items.slice(start, end);
      this.totalCount = this.data.items.length;
    } else {
      this.pagedData = this.data.items
        .filter(
          item =>
            item.dpoNo?.toLowerCase().includes(text) ||
            item.golfaCode?.toLowerCase().includes(text) ||
            item.model?.toLowerCase().includes(text) ||
            item.note?.toLowerCase().includes(text),
        )
        .slice(start, end);
      this.totalCount = this.data.items.filter(
        item =>
          item.dpoNo?.toLowerCase().includes(text) ||
          item.golfaCode?.toLowerCase().includes(text) ||
          item.model?.toLowerCase().includes(text) ||
          item.note?.toLowerCase().includes(text),
      ).length;
    }

    this.applySearch();
  }

  onChangedPaging(event: any) {
    this.setPage(event);
  }
  applySearch() {
    const text = this.searchText?.trim().toLowerCase();

    if (!text) {
      // reset về gốc khi clear
      this.filteredData = [...this.pagedData];
      this.selectedAllData = [...this.data.items];
      return;
    }

    this.filteredData = [...this.pagedData];
    this.selectedAllData = [
      ...this.data.items.filter(
        item =>
          item.dpoNo?.toLowerCase().includes(text) ||
          item.golfaCode?.toLowerCase().includes(text) ||
          item.model?.toLowerCase().includes(text) ||
          item.note?.toLowerCase().includes(text),
      ),
    ];
  }
  onSearchChange() {
    this.setPage(1);
  }
}
