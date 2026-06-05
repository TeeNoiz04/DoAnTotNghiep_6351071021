import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ImportDPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import { ImportDPODto } from '@app/proxy/dpos/models';
import { ExcelRowResult, ExcelValidationResult } from '@app/proxy/shared/excels/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';

@Component({
  selector: 'app-dpo-items',
  standalone: true,
  templateUrl: './dpo-items.component.html',
  imports: [
    CoreModule,
    CommonModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
})
export class DpoItemsComponent implements OnChanges {
  @Input() resultImport: ExcelValidationResult<ImportDPODto> | undefined;
  @Input() height: string = '450px';

  data: any[] = [];

  // Formatter functions
  formatNumber = (value: any): string => {
    if (value === null || value === undefined) return '';
    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
    }).format(value);
  };

  formatCurrency = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 0,
        }).format(value)}`
      : '';
  };

  formatRowIndex = (value: any, row: any): string => {
    return row.rowIndex?.toString() || '';
  };

  formatDate = (value: any): string => {
    if (value === null || value === undefined) return '';
    return new Date(value).toLocaleDateString();
  };

  get dpoDetailsValidation(): ExcelValidationResult<ImportDPODetailDto> | undefined {
    if (!this.resultImport?.listData?.length) return undefined;
    return this.resultImport.listData[0]?.rowData?.details;
  }

  get validItems(): ExcelRowResult<ImportDPODetailDto>[] {
    return this.dpoDetailsValidation?.listData?.filter(item => !item.hasErrors) || [];
  }

  get invalidItems(): ExcelRowResult<ImportDPODetailDto>[] {
    return this.dpoDetailsValidation?.listData?.filter(item => item.hasErrors) || [];
  }

  get hasValidItems(): boolean {
    return this.validItems.length > 0;
  }

  get hasInvalidItems(): boolean {
    return this.invalidItems.length > 0;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.dpoDetailsValidation?.listData) {
      this.data = this.dpoDetailsValidation.listData.map(item => ({
        ...item,
        rowData: {
          ...item.rowData,
          status: item?.hasErrors ? 'INVALID' : 'VALID',
        },
      }));
    }
  }

  onRowClick(event: any): void {
    // Handle row click if needed
  }
}
