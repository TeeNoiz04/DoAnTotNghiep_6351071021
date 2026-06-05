import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { formatDateTime } from '@app/shared/helpers/format-helper';
import {
  ColumnGroup,
  ResultImportTableComponent,
} from '@app/shared/components/result-import-table/result-import-table.component';

@Component({
  selector: 'app-sale-order-items',
  templateUrl: './sale-order-items.component.html',
  styleUrls: ['./sale-order-items.component.scss'],
  standalone: true,
  imports: [CommonModule, ResultImportTableComponent],
})
export class SaleOrderItemsComponent implements OnChanges {
  @Input() resultImport!: any;
  @Input() height: string = '400px';
  data: any[] = [];

  columnGroups: ColumnGroup[];

  setUpColumns() {
    this.columnGroups = [
      {
        name: 'Sale Order Detail',
        class: 'group-header item-group',
        columns: [
          {
            field: 'no',
            header: 'No',
            width: '50px',
            formatter: (val, row) => {
              const index = this.data.indexOf(row) + 1;
              return index.toString();
            },
            class: 'text-center width_50',
            textAlign: 'center',
          },
          { field: 'rowData.soNo', header: 'SO No', width: '180px', class: 'width_180' },
          { field: 'rowData.sapsoNo', header: 'SAP SO No', width: '180px', class: 'width_180' },
          { field: 'rowData.sapdoNo', header: 'SAP DO No', width: '180px', class: 'width_180' },
          { field: 'rowData.sapBillingNo', header: 'SAP Billing No', width: '180px', class: 'width_180' },
          { field: 'rowData.sapinv', header: 'SAP INV', width: '180px', class: 'width_180' },
          {
            field: 'rowData.sapinvDate',
            header: 'SAP INV date',
            width: '180px',
            class: 'width_180',
            formatter: val => formatDateTime(val),
          },
        ],
      },
    ];
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.setUpColumns();
    if (this.resultImport && this.resultImport.listData) {
      this.data = this.resultImport.listData;
    }
  }

  onRowClick(event: any): void {}
}
