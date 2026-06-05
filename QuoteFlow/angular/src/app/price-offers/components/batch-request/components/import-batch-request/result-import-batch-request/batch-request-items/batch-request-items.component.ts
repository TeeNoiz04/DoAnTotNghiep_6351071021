import { CommonModule, formatDate } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

import {
  ColumnGroup,
  ResultImportTableComponent,
} from '@app/shared/components/result-import-table/result-import-table.component';
import { formatDateTime } from '@app/shared/helpers/format-helper';
import { ImportBatchRequestType, ImportResultMap } from '../../batch-request.types';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components';

@Component({
  selector: 'app-batch-request-items',
  templateUrl: './batch-request-items.component.html',
  styleUrls: ['./batch-request-items.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppTableColumnGroupDirective],
})
export class BatchRequestItemsComponent implements OnChanges {
  @Input() importMode: ImportBatchRequestType | undefined;
  @Input() resultImport!: ImportResultMap[keyof ImportResultMap];
  @Input() height: string = '400px';
  data: any[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (this.resultImport && this.resultImport.listData) {
      this.data = this.resultImport.listData;
    }
  }

  onRowClick(event: any): void {}
  // Formatter functions for advanced data table
  formatRowNumber = (value: any, row: any): string => {
    const index = this.data.indexOf(row) + 1;
    return index.toString();
  };

  formatDateTime = (value: any): string => {
    return formatDateTime(value);
  };
  formatDate = (value: any): string => {
    return value ? new Date(value).toLocaleDateString('en-GB') : '';
  };
}
