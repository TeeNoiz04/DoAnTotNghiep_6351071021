import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ImportStockManagementType, ImportResultMap } from '../../import-stock.type';
import {
  ColumnGroup,
  ResultImportTableComponent,
} from '@app/shared/components/result-import-table/result-import-table.component';

@Component({
  selector: 'app-stock-items',
  templateUrl: './stock-items.component.html',
  styleUrls: ['./stock-items.component.scss'],
  standalone: true,
  imports: [CommonModule, ResultImportTableComponent],
})
export class MaterialItemsComponent implements OnChanges {
  @Input() importMode: ImportStockManagementType | undefined;
  @Input() resultImport!: ImportResultMap[keyof ImportResultMap];
  @Input() height: string = '400px';
  data: any[] = [];

  columnGroups: ColumnGroup[];

  setUpColumns() {
    switch (this.importMode) {
      case ImportStockManagementType.StockInventory:
        this.columnGroups = [
          {
            name: 'Stock Detail',
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
              { field: 'rowData.materialCode', header: 'Material code (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model Name (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.storage', header: 'Storage (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.qty', header: 'Actual Qty (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.refDoc', header: 'Ref Doc', width: '180px', class: 'width_180' },
              { field: 'rowData.remark', header: 'Remark', width: '180px', class: 'width_180' },
            ],
          },
        ];
        break;

      case ImportStockManagementType.StockTransfer:
        this.columnGroups = [
          {
            name: 'Stock Detail',
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
              { field: 'rowData.materialCode', header: 'Material code (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model Name (*)', width: '180px', class: 'width_180' },
              { field: 'rowData.storage', header: 'Source storage(*)', width: '180px', class: 'width_180' },
              {
                field: 'rowData.storageDestination',
                header: 'Destination storage(*)',
                width: '180px',
                class: 'width_180',
              },
              { field: 'rowData.qty', header: 'Qty', width: '180px', class: 'width_180' },
              { field: 'rowData.remark', header: 'Remark', width: '180px', class: 'width_180' },
            ],
          },
        ];
        break;

      default:
        this.columnGroups = [];
        break;
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.setUpColumns();
    if (this.resultImport && this.resultImport.listData) {
      this.data = this.resultImport.listData;
    }
  }

  onRowClick(event: any): void {}
}
