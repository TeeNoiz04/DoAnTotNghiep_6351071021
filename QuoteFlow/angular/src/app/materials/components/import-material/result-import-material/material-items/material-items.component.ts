import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ImportMaterialManagementType, ImportResultMap } from '../../import-material.type';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { ColumnGroup, MaterialItemsTableComponent } from './material-items-table/material-items-table.component';
import {
  formatCurrency,
  formatDateTime,
  formatNegative,
  formatStringDelete,
  formatNumberDelete,
  formatDateTimeDelete,
  formatCurrencyDelete,
} from '@app/shared/helpers/format-helper';

@Component({
  selector: 'app-material-items',
  templateUrl: './material-items.component.html',
  styleUrls: ['./material-items.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MaterialItemsTableComponent,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
})
export class MaterialItemsComponent implements OnChanges {
  @Input() importMode: ImportMaterialManagementType | undefined;
  @Input() resultImport!: ImportResultMap[keyof ImportResultMap];
  @Input() height: string = '400px';
  data: any[] = [];

  columnGroups: ColumnGroup[];

  setUpColumns() {
    switch (this.importMode) {
      case ImportMaterialManagementType.NewMaterial:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.materialCode', header: 'Material code', width: '180px', class: 'width_180' },
              { field: 'rowData.modelName', header: 'Model Name', width: '180px', class: 'width_180' },
              {
                field: 'rowData.registrationDate',
                header: 'Registration date	Valid from',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTimeDelete(val),
              },
              {
                field: 'rowData.validFrom',
                header: 'Valid from',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              {
                field: 'rowData.validTo',
                header: 'Valid to',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              { field: 'rowData.sapCode', header: 'SAP Code', width: '180px', class: 'width_180' },
              { field: 'rowData.spec1', header: 'Spec1', width: '180px', class: 'width_180' },
              { field: 'rowData.spec2', header: 'Spec2', width: '180px', class: 'width_180' },
              { field: 'rowData.spec3', header: 'Spec3', width: '180px', class: 'width_180' },
              { field: 'rowData.spec4', header: 'Spec4', width: '180px', class: 'width_180' },
              { field: 'rowData.descriptionEN', header: 'Description EN', width: '180px', class: 'width_180' },
              { field: 'rowData.descriptionVN', header: 'Description VN', width: '180px', class: 'width_180' },
              { field: 'rowData.materialType', header: 'Material Type', width: '180px', class: 'width_180' },
              { field: 'rowData.unit', header: 'Unit', width: '180px', class: 'width_180' },
              { field: 'rowData.materialClass', header: 'Material Class', width: '180px', class: 'width_180' },
              {
                field: 'rowData.materialSECClassification',
                header: 'Material SEC Classification',
                width: '180px',
                class: 'width_180',
              },
              { field: 'rowData.materialGroup', header: 'Material Group', width: '180px', class: 'width_180' },
              { field: 'rowData.sapGroup', header: 'SAP Mat Group', width: '180px', class: 'width_180' },
              { field: 'rowData.productHierarchy', header: 'Product Hierarchy', width: '180px', class: 'width_180' },
              {
                field: 'rowData.productHierarchy_Description',
                header: 'Product Hierachy description',
                width: '180px',
                class: 'width_180',
              },
              {
                field: 'rowData.countryOfOrigin',
                header: 'Country of Origin',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.referenceLeadTime',
                header: 'Reference Lead Time (Working Day)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.warrantyTime',
                header: 'Warranty time - month',
                width: '180px',
                class: 'width_180',
              },
              {
                field: 'rowData.inventoryCategory',
                header: 'Inventory Category',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              { field: 'rowData.cargoNote', header: 'Cargo Note', width: '180px', class: 'width_180' },
              { field: 'rowData.weight', header: 'Weight', width: '180px', class: 'width_180' },
              { field: 'rowData.size', header: 'Material Size', width: '180px', class: 'width_180' },
              { field: 'rowData.qrCode', header: 'QR Code', width: '180px', class: 'width_180' },
              {
                field: 'rowData.maxLot',
                header: 'Max lot',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.stockWarning',
                header: 'Stock Warning',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.vat',
                header: 'VAT',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNegative(val),
              },
              { field: 'rowData.hsCode', header: 'HS Code', width: '180px', class: 'width_180' },
              { field: 'rowData.supplier', header: 'Supplier', width: '180px', class: 'width_180' },
              { field: 'rowData.supplierBU', header: 'Supplier BU', width: '180px', class: 'width_180' },
              { field: 'rowData.factory', header: 'Factory', width: '180px', class: 'width_180' },
              { field: 'rowData.inputPrice', header: 'Input Price', width: '180px', class: 'width_180' },
              { field: 'rowData.inputCurrency', header: 'Input Currency', width: '180px', class: 'width_180' },
              { field: 'rowData.incoterms', header: 'INCOTERMS', width: '180px', class: 'width_180' },
              { field: 'rowData.epa', header: 'EPA', width: '180px', class: 'width_180' },
              { field: 'rowData.importDuty', header: 'Import duty', width: '180px', class: 'width_180' },
              {
                field: 'rowData.exchangeRate',
                header: 'Applied exchange rate',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.landedCost',
                header: 'Landed Cost (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.maxSaleOfferPrice',
                header: 'Max Sales offer price (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.maxManagerOfferPrice',
                header: 'Max Manager offer price (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.standardPrice',
                header: 'Standard Price',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.sellingPrice1',
                header: 'Selling Price 1',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice2',
                header: 'Selling Price 2',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice3',
                header: 'Selling Price 3',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice4',
                header: 'Selling Price 4',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice5',
                header: 'Selling Price 5',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.ApprovalPrice:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.materialCode', header: 'Material code', width: '180px', class: 'width_180' },
              { field: 'rowData.modelName', header: 'Model Name', width: '180px', class: 'width_180' },
              {
                field: 'rowData.spec1',
                header: 'Spec1',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              { field: 'rowData.materialType', header: 'Material Type', width: '180px', class: 'width_180' },
              { field: 'rowData.materialGroup', header: 'Material Group', width: '180px', class: 'width_180' },
              {
                field: 'rowData.priceValidFrom',
                header: 'Price valid from',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              {
                field: 'rowData.priceValidTo',
                header: 'Price valid to',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              { field: 'rowData.inputPrice', header: 'Input Price', width: '180px', class: 'width_180' },
              { field: 'rowData.inputCurrency', header: 'Input Currency', width: '180px', class: 'width_180' },
              { field: 'rowData.incoterms', header: 'INCOTERMS', width: '180px', class: 'width_180' },
              { field: 'rowData.epa', header: 'EPA', width: '180px', class: 'width_180' },
              {
                field: 'rowData.importDuty',
                header: 'Import duty',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNegative(val),
              },
              {
                field: 'rowData.exchangeRate',
                header: 'Applied exchange rate',
                width: '180px',
                class: 'width_180',
              },
              {
                field: 'rowData.landedCost',
                header: 'Landed Cost (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.maxSaleOfferPrice',
                header: 'Max Sales offer price (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.maxManagerOfferPrice',
                header: 'Max Manager offer price (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.standardPrice',
                header: 'Standard Price (VND)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrency(val),
              },
              {
                field: 'rowData.sellingPrice1',
                header: 'Selling Price 1',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice2',
                header: 'Selling Price 2',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice3',
                header: 'Selling Price 3',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice4',
                header: 'Selling Price 4',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
              {
                field: 'rowData.sellingPrice5',
                header: 'Selling Price 5',
                width: '180px',
                class: 'width_180',
                formatter: val => formatCurrencyDelete(val),
              },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.WithoutPrice:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.materialCode', header: 'Material code', width: '180px', class: 'width_180' },
              { field: 'rowData.modelName', header: 'Model Name', width: '180px', class: 'width_180' },
              {
                field: 'rowData.registrationDate',
                header: 'Registration date',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTimeDelete(val),
              },
              {
                field: 'rowData.validFrom',
                header: 'Valid from',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              {
                field: 'rowData.validTo',
                header: 'Valid to',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              {
                field: 'rowData.spec1',
                header: 'Spec1',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.spec2',
                header: 'Spec2',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.spec3',
                header: 'Spec3',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.spec4',
                header: 'Spec4',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.descriptionEN',
                header: 'Description EN',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.descriptionVN',
                header: 'Description VN',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.supplier',
                header: 'Supplier',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.supplierBU',
                header: 'Supplier BU',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.factory',
                header: 'Factory',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.materialType',
                header: 'Material Type',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.unit',
                header: 'Unit',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.materialGroup',
                header: 'Material Group',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.sapGroup',
                header: 'SAP Mat Group',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.productHierarchy_Description',
                header: 'Product Hierachy description',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.countryOfOrigin',
                header: 'Origin',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.referenceLeadTime',
                header: 'Reference Lead Time',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.warrantyTime',
                header: 'Warranty time',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.inventoryCategory',
                header: 'Inventory Category',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.cargoNote',
                header: 'Cargo Note',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.weight',
                header: 'Weight',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.size',
                header: 'Material Size',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.qrCode',
                header: 'QR Code',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.maxLot',
                header: 'Max lot',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.stockWarning',
                header: 'Stock Warning',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.stockQty',
                header: 'Stock Qty',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.hsCode',
                header: 'HS Code',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.Status:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.golfaCode', header: 'Material code', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model Name', width: '180px', class: 'width_180' },
              {
                field: 'rowData.acceptanceDate',
                header: 'Final DPO acceptance date',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              {
                field: 'rowData.activeDate',
                header: 'Action Date',
                width: '180px',
                class: 'width_180',
                formatter: val => formatDateTime(val),
              },
              { field: 'rowData.action', header: 'Action', width: '180px', class: 'width_180' },
              { field: 'rowData.source', header: 'Source', width: '180px', class: 'width_180' },
              { field: 'rowData.reason', header: 'Reason', width: '180px', class: 'width_180' },
              { field: 'rowData.factoryRefDoc', header: 'Factory Ref doc', width: '180px', class: 'width_180' },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.InventoryPlanning:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.golfaCode', header: 'Material Code', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model', width: '180px', class: 'width_180' },
              {
                field: 'rowData.inventoryCategory',
                header: 'Inventory Category',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.currentStockWarning',
                header: 'Current Stock Warning',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.stockWarning',
                header: 'Updated Stock Warning',
                width: '180px',
                class: 'width_180',
                formatter: (val, row) => {
                  const formattedValue = formatNumberDelete(val);
                  const current = row?.rowData?.currentStockWarning;
                  const updated = row?.rowData?.stockWarning;
                  // Highlight with primary blue bold if value differs from current
                  if (current !== updated) {
                    return `<span class="text-primary fw-bold">${formattedValue}</span>`;
                  }
                  return formattedValue;
                },
              },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.Leadtime:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.golfaCode', header: 'Material Code', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model', width: '180px', class: 'width_180' },
              {
                field: 'rowData.referenceLeadTime',
                header: 'Reference Lead Time (Working Day)',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
              {
                field: 'rowData.countryOfOrigin',
                header: 'Country of origin',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.maxlot',
                header: 'Max lot',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNumberDelete(val),
              },
            ],
          },
        ];
        break;

      case ImportMaterialManagementType.SAPCode:
        this.columnGroups = [
          {
            name: 'Material Detail',
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
              { field: 'rowData.golfaCode', header: 'Material Code', width: '180px', class: 'width_180' },
              { field: 'rowData.model', header: 'Model', width: '180px', class: 'width_180' },
              {
                field: 'rowData.sapCode',
                header: 'SAP Code',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.descriptionVN',
                header: 'Description VN',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.productHiearchy',
                header: 'Product Hierarchy',
                width: '180px',
                class: 'width_180',
                formatter: val => formatStringDelete(val),
              },
              {
                field: 'rowData.vat',
                header: 'VAT',
                width: '180px',
                class: 'width_180',
                formatter: val => formatNegative(val),
              },
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

  getRowClass = (row: any): string => {
    if (row?.warnings?.length > 0) {
      return 'row-warning';
    }
    return '';
  };
}
