import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, inject, Input, Output } from '@angular/core';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
  SelectionChangeEvent,
} from '@app/shared/components/advanced-data-table';
import { PriceOfferDetailDto } from '@proxy/price-offers/price-offer-details';
import { PriceOfferSummaryData } from '../price-offer.types';

@Component({
  selector: 'app-grouped-table',
  templateUrl: './grouped-table.component.html',
  styleUrls: ['./grouped-table.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppTableColumnGroupDirective],
})
export class GroupedTableComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  @Input() data: PriceOfferDetailDto[] = [];
  @Input() height: string = '650px';
  @Input() isGPViewable: boolean = false;
  @Input() isLandedCostViewable: boolean = false;
  @Input() summaryData: PriceOfferSummaryData = {};
  @Input() canCancelItem: boolean = false;
  @Input() selectedItems: PriceOfferDetailDto[] = [];
  @Input() trackBy?: (index: number, item: any) => any;
  @Output() historyClick = new EventEmitter<PriceOfferDetailDto>();
  @Output() selectionChange = new EventEmitter<SelectionChangeEvent>();

  // Unique key that changes when visibility flags change, forcing table re-render
  get tableKey(): string {
    return `table-${this.isGPViewable}-${this.isLandedCostViewable}`;
  }

  // TrackBy function for ngFor to force recreation when key changes
  trackByKey = (index: number, key: string): string => key;
  formatCurrency = (value: number): string => {
    if (!value && value !== 0) return '';
    return new Intl.NumberFormat('en-US', {
      style: 'decimal',
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
      useGrouping: true,
    }).format(value);
  };

  formatCurrencyWithBlank = (value: number): string => {
    if (value === null || value === undefined || value === 0) return '-';
    return this.formatCurrency(value);
  };

  onHistoryClick(row: PriceOfferDetailDto) {
    this.historyClick.emit(row);
  }

  onSelectionChange(event: SelectionChangeEvent) {
    this.selectionChange.emit(event);
  }

  canSelectRow = (row: PriceOfferDetailDto): boolean => {
    // Only allow selection for non-cancelled items if cancel is enabled
    return this.canCancelItem && row.status !== 'CANCELLED';
  };

  formatDiscountRatioD1 = (value: unknown): string => {
    const num = parseFloat(value as string);
    return isNaN(num) ? '0%' : `${(num * 100).toFixed(1)}%`;
  };

  formatDpoUsed = (value: unknown): string => {
    return (value as string) || '0';
  };

  formatPriceToCustomerAmount = (value: unknown, row: any): string => {
    const priceToCustomer = row?.priceToCustomer || 0;
    const qty = row?.qty || 0;
    const amount = priceToCustomer * qty;
    return this.formatCurrency(amount);
  };
}
