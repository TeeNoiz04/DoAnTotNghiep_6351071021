import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { CustomerImportDto } from '@proxy/customers';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Component({
  selector: 'app-customer-items',
  templateUrl: './customer-items.component.html',
  styleUrls: ['./customer-items.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppTableColumnGroupDirective],
})
export class CustomerItemsComponent implements OnChanges {
  @Input() resultImport: ExcelValidationResult<CustomerImportDto> | undefined;
  @Input() height: string = '350px';
  data: any[] = [];

  // Formatter functions for advanced data table
  formatRowNumber = (value: any, row: any): string => {
    const index = this.data.indexOf(row) + 1;
    return index.toString();
  };

  formatBoolean = (value: any): string => {
    if (value === true) return 'Yes';
    if (value === false) return 'No';
    return '';
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (this.resultImport && this.resultImport.listData) {
      // Map the data and add state field based on isUpdate
      const mappedData = this.resultImport.listData.map((item, index) => ({
        ...item,
        rowData: {
          ...item.rowData,
          state: item.rowData?.isUpdate ? 'UPDATE' : 'CREATE',
        },
      }));
      this.data = mappedData;
    }
  }

  onRowClick(event: any): void {}
}
