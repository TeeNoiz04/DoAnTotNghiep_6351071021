import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { PriceOfferImportDto } from '@proxy/price-offers';
import { PriceOfferDetailImportDto } from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { ImportStockTracingType } from '../../stock-tracing.types';

@Component({
  selector: 'app-stock-tracing-items',
  templateUrl: './stock-tracing-items.component.html',
  styleUrls: ['./stock-tracing-items.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppTableColumnGroupDirective],
})
export class StockTracingItemsComponent implements OnChanges {
  @Input() importMode: ImportStockTracingType | undefined;
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() resultImportItems: ExcelValidationResult<PriceOfferDetailImportDto> | undefined;
  @Input() height: string = '450px';

  data: any[] = [];
  details: ExcelValidationResult<PriceOfferDetailImportDto> | undefined;

  // Formatter functions for advanced data table
  formatRowNumber = (value: any, row: any): string => {
    const index = this.data.indexOf(row) + 1;
    return index.toString();
  };

  formatCurrency = (value: number): string => {
    if (value === null || value === undefined) return '';
    return new Intl.NumberFormat('en-US', {
      style: 'decimal',
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
      useGrouping: true,
    }).format(value);
  };

  formatDateTime = (value: string | Date): string => {
    if (!value) return '';

    const date = new Date(value);

    const pad = (n: number) => n.toString().padStart(2, '0');

    const day = pad(date.getDate());
    const month = pad(date.getMonth() + 1);
    const year = date.getFullYear();

    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());
    const seconds = pad(date.getSeconds());

    return `${day}/${month}/${year} - ${hours}:${minutes}:${seconds}`;
  };

  formatDate = (value: string | Date): string => {
    if (!value) return '';

    const date = new Date(value);

    const pad = (n: number) => n.toString().padStart(2, '0');

    const day = pad(date.getDate());
    const month = pad(date.getMonth() + 1);
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
  };

  // Helper methods to check import mode
  get isDeliveryMode(): boolean {
    return this.importMode === ImportStockTracingType.Delivery;
  }

  get isInventoryMode(): boolean {
    return this.importMode === ImportStockTracingType.Inventory;
  }

  get isReceiptMode(): boolean {
    return this.importMode === ImportStockTracingType.Receipt;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.resultImport && this.resultImport.listData) {
      this.data = this.resultImport.listData;
    }
  }

  onRowClick(event: any): void {}
}
