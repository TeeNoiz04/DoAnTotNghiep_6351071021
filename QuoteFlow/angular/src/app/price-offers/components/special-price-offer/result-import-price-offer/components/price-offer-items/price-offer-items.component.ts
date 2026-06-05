import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { PriceOfferImportDto } from '@proxy/price-offers';
import {
  PriceOfferDetailImportDto,
  PriceOfferUpdateLandingCostImportDto,
} from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { ImportPriceOfferType } from '../../../price-offer.types';

@Component({
  selector: 'app-price-offer-items',
  templateUrl: './price-offer-items.component.html',
  styleUrls: ['./price-offer-items.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppTableColumnGroupDirective],
})
export class PriceOfferItemsComponent implements OnChanges {
  @Input() importMode: ImportPriceOfferType | undefined;
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() resultImportItems:
    | ExcelValidationResult<PriceOfferDetailImportDto | PriceOfferUpdateLandingCostImportDto>
    | undefined;
  @Input() height: string = '450px';

  data: any[] = [];
  details: ExcelValidationResult<PriceOfferDetailImportDto> | undefined;
  summaryData: any = {};

  // Helper properties for template
  isAddMoreItemsMode = false;
  isUpdateItemPropertiesMode = false;

  ImportPriceOfferType = ImportPriceOfferType;

  ngOnChanges(changes: SimpleChanges): void {
    // Set the mode flags for template
    this.isAddMoreItemsMode = this.importMode !== ImportPriceOfferType.UpdateItemProperties;
    this.isUpdateItemPropertiesMode = this.importMode === ImportPriceOfferType.UpdateItemProperties;

    if (this.resultImport && this.resultImport.listData) {
      const singleRow = this.resultImport.singleRow;
      if (singleRow) {
        this.details = this.resultImport.listData[0].rowData?.details;
        const newListData =
          this.details?.listData.map((item, index) => ({
            ...item,
            rowData: {
              ...item.rowData,
              status: item?.hasErrors ? 'INVALID' : 'VALID',
              no: index + 1, // Add row number
            },
          })) || [];
        this.data = newListData;

        this.updateSummaryValues();
      }
    }
    if (this.resultImportItems && this.resultImportItems.listData) {
      const newListData = this.resultImportItems?.listData || [];
      this.data = newListData.map((item, index) => ({
        ...item,
        rowData: {
          ...item.rowData,
          status: item?.hasErrors ? 'INVALID' : 'VALID',
          no: index + 1, // Add row number
        },
      }));
    }
  }

  private updateSummaryValues(): void {
    // Extract summary values from the import result
    const importData = this.resultImport?.listData[0]?.rowData;
    if (importData) {
      this.summaryData = {
        'rowData.standardAmount': importData.totalStandardAmount,
        'rowData.requestedAmount': importData.totalRequestedAmount,
        'rowData.mevnOfferAmount': importData.totalMEVNOfferAmount,
        totalStandardAmount: importData.totalStandardAmount,
        totalRequestedAmount: importData.totalRequestedAmount,
        totalMEVNOfferAmount: importData.totalMEVNOfferAmount,
      };
    }
  }

  onRowClick(event: any): void {}

  formatCurrency = (value: number): string => {
    if (value === null || value === undefined) return '';
    return new Intl.NumberFormat('en-US', {
      style: 'decimal',
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
      useGrouping: true,
    }).format(value);
  };

  formatDiscountRatio = (value: unknown): string => {
    const num = parseFloat(value as string);
    return isNaN(num) ? '0%' : `${(num * 100).toFixed(0)}%`;
  };
}
