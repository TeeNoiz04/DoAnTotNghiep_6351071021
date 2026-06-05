import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  ColumnGroup,
  ResultImportTableComponent,
} from '@app/shared/components/result-import-table/result-import-table.component';

@Component({
  selector: 'app-supplier-bu-items',
  templateUrl: './supplier-bu-items.component.html',
  styleUrls: ['./supplier-bu-items.component.scss'],
  standalone: true,
  imports: [CommonModule, ResultImportTableComponent],
})
export class SupplierBUItemsComponent implements OnChanges {
  @Input() resultImport!: any;
  @Input() height: string = '400px';
  data: any[] = [];

  columnGroups: ColumnGroup[];

  setUpColumns() {
    this.columnGroups = [
      {
        name: 'Allocation Detail',
        class: 'group-header item-group',
        columns: [
          {
            field: 'rowData.no',
            header: 'No',
            width: '50px',
            formatter: (val, row) => {
              const index = this.data.indexOf(row) + 1;
              return index.toString();
            },
            class: 'text-center width_50',
            textAlign: 'center',
          },
          { field: 'rowData.supplierBU', header: 'Supplier BU', width: '180px', class: 'width_180' },
          // { field: 'rowData.isUpdate', header: 'Update', width: '180px', class: 'width_180' },
          { field: 'rowData.supplierBURemarks', header: 'Supplier BU Remarks', width: '200px', class: 'width_200' },
          { field: 'rowData.orderMethod', header: 'Order Method', width: '150px', class: 'width_150' },
          { field: 'rowData.poTemplate', header: 'PO Template', width: '180px', class: 'width_180' },
          { field: 'rowData.contact', header: 'Contact', width: '180px', class: 'width_180' },
          { field: 'rowData.email', header: 'Email', width: '220px', class: 'width_220' },
          { field: 'rowData.incoTerm', header: 'Incoterm', width: '120px', class: 'width_120' },
          { field: 'rowData.paymentTermCode', header: 'Payment Term Code', width: '180px', class: 'width_180' },
          { field: 'rowData.paymentDescription', header: 'Payment Description', width: '300px', class: 'width_300' },
          { field: 'rowData.currency', header: 'Currency', width: '100px', class: 'width_100', textAlign: 'center' },
          { field: 'rowData.materialType', header: 'Material Type', width: '150px', class: 'width_150' },
          { field: 'rowData.sapCode', header: 'Supplier Code', width: '150px', class: 'width_150' },
          // { field: 'rowData.supplierID', header: 'Supplier ID', width: '300px', class: 'width_300' },
          { field: 'rowData.supplierCode', header: 'Supplier Short Name', width: '200px', class: 'width_200' },
          { field: 'rowData.supplierAddress', header: 'Supplier Address', width: '400px', class: 'width_400' },
          { field: 'rowData.fascmVendorCode', header: 'FASCM Vendor Code', width: '200px', class: 'width_200' },
          { field: 'rowData.fascmBuyerCode', header: 'FASCM Buyer Code', width: '200px', class: 'width_200' },
          { field: 'rowData.fascmConsigneeCode', header: 'FASCM Consignee Code', width: '220px', class: 'width_220' },
          { field: 'rowData.fascmSectionCode', header: 'FASCM Section Code', width: '200px', class: 'width_200' },
          { field: 'rowData.fascmPaymentTerm', header: 'FASCM Payment Term', width: '220px', class: 'width_220' },
          { field: 'rowData.fascmFreightMethod', header: 'FASCM Freight Method', width: '220px', class: 'width_220' },
          { field: 'rowData.fascmDeliveryTerms', header: 'FASCM Delivery Terms', width: '220px', class: 'width_220' },
          {
            field: 'rowData.fascmPlaceOfDeliveryTerms',
            header: 'FASCM Place of Delivery Terms',
            width: '280px',
            class: 'width_280',
          },
          {
            field: 'rowData.fascmShippingMarkCode',
            header: 'FASCM Shipping Mark Code',
            width: '250px',
            class: 'width_250',
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
