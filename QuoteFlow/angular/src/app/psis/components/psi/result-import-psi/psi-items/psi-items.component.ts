import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ResultImportTableComponent } from '@app/shared/components/result-import-table/result-import-table.component';
import { ImportPSIType, ImportResultMap } from '../../psi.types';
import { formatCurrency } from '@app/shared/helpers/format-helper';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components';

@Component({
  selector: 'app-psi-items',
  templateUrl: './psi-items.component.html',
  styleUrls: ['./psi-items.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ResultImportTableComponent,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
})
export class PAIItemsComponent implements OnChanges {
  @Input() importMode: ImportPSIType | undefined;
  @Input() resultImport!: ImportResultMap[keyof ImportResultMap];
  @Input() height: string = '400px';
  data: any[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (this.resultImport && this.resultImport.listData) {
      this.data = this.resultImport?.listData?.map(item => ({
        ...item,
        rowData: {
          ...item.rowData,
          status: item?.hasErrors ? 'INVALID' : 'VALID',
        },
      }));
    }
  }

  formatNumber = (value: any): string => {
    return formatCurrency(value);
  };

  formatRowIndex = (value: any, row: any): string => {
    const index = this.data.indexOf(row) + 1;

    return index.toString();
  };

  onRowClick(event: any): void {}
}
