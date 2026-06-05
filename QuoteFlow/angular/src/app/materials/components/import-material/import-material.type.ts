import { TemplateRef } from '@angular/core';
import { AppPermissions } from '@app/app.permissions';
import {
  MaterialNewRegistrationImportDto,
  MaterialUpdateInventoryPlanImportDto,
  MaterialUpdatePriceImportDto,
  MaterialUpdateWithoutPriceImportDto,
} from '@proxy/materials';
import { MaterialFactoryUpdateExcelDto } from '@proxy/materials/material-import/material-factory';
import { MaterialSAPUpdateExcelDto } from '@proxy/materials/material-import/material-sap';
import { MaterialStatusUpdateExcelDto } from '@proxy/materials/material-import/material-status';
import { ExcelValidationResult } from '@proxy/shared/excels';

export enum ImportMaterialManagementType {
  NewMaterial = 'MATERIAL.NEW', //M1U
  ApprovalPrice = 'MATERIAL.PRICE', //M2U
  WithoutPrice = 'MATERIAL.WITHOUT_PRICE', //M3U
  Status = 'MATERIAL.STATUS', //M4U
  InventoryPlanning = 'MATERIAL.INVENTORY_PLANNING', //M5U
  Leadtime = 'MATERIAL.LEADTIME', //M6U
  SAPCode = 'MATERIAL.SAP_CODE', //M7U
  Masterdata = 'MATERIAL.M8U',
}

export type ImportMaterialInformation = {
  file?: File | null;
  fromDate?: Date | null;
  toDate?: Date | null;
  note?: string;
};

export type ImportMaterialManagementTypeOption = {
  label: string;
  value: ImportMaterialManagementType;
  requiredPolicy?: string;
};

export interface TableColumn {
  prop: string;
  name: string;
  minWidth?: number;
  sortable?: boolean;
  format?: string;
  frozenLeft?: boolean;
  cellTemplate?: TemplateRef<any>;
}

export type ImportResultMap = {
  [ImportMaterialManagementType.NewMaterial]: ExcelValidationResult<MaterialNewRegistrationImportDto>;
  [ImportMaterialManagementType.ApprovalPrice]: ExcelValidationResult<MaterialUpdatePriceImportDto>;
  [ImportMaterialManagementType.WithoutPrice]: ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>;
  [ImportMaterialManagementType.Status]: ExcelValidationResult<MaterialStatusUpdateExcelDto>;
  [ImportMaterialManagementType.InventoryPlanning]: ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>;
  [ImportMaterialManagementType.Leadtime]: ExcelValidationResult<MaterialFactoryUpdateExcelDto>;
  [ImportMaterialManagementType.SAPCode]: ExcelValidationResult<MaterialSAPUpdateExcelDto>;
};

export const ImportMaterialOptions: ImportMaterialManagementTypeOption[] = [
  {
    label: 'New Material',
    value: ImportMaterialManagementType.NewMaterial,
    requiredPolicy: `${AppPermissions.Materials.Uploads.NewMaterial}`,
  },
  {
    label: 'Approval Price',
    value: ImportMaterialManagementType.ApprovalPrice,
    requiredPolicy: `${AppPermissions.Materials.Uploads.UpdatePrice}`,
  },
  {
    label: 'Without Price',
    value: ImportMaterialManagementType.WithoutPrice,
    requiredPolicy: `${AppPermissions.Materials.Uploads.UpdateMaterialWithoutPrice}`,
  },
  {
    label: 'Material Status',
    value: ImportMaterialManagementType.Status,
    requiredPolicy: `${AppPermissions.Materials.Uploads.MaterialStatus}`,
  },
  {
    label: 'Inventory Planning',
    value: ImportMaterialManagementType.InventoryPlanning,
    requiredPolicy: `${AppPermissions.Materials.Uploads.InventoryPlanning}`,
  },
  {
    label: 'Leadtime',
    value: ImportMaterialManagementType.Leadtime,
    requiredPolicy: `${AppPermissions.Materials.Uploads.Leadtime}`,
  },
  {
    label: 'SAP Code',
    value: ImportMaterialManagementType.SAPCode,
    requiredPolicy: `${AppPermissions.Materials.Uploads.SapCode}`,
  },
];

export const ImportMaterialColumns: { [key in ImportMaterialManagementType]: TableColumn[] } = {
  [ImportMaterialManagementType.NewMaterial]: [
    { prop: 'golfaCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    { prop: 'registrationDate', name: 'Registration date', format: 'date' },
    { prop: 'validFrom', name: 'Valid from', format: 'date' },
    { prop: 'validTo', name: 'Valid to', format: 'date' },
    { prop: 'sap_Code', name: 'SAP Code' },
    { prop: 'spec1', name: 'Spec1' },
    { prop: 'spec2', name: 'Spec2' },
    { prop: 'spec3', name: 'Spec3' },
    { prop: 'spec4', name: 'Spec4' },
    { prop: 'description_EN', name: 'Description EN' },
    { prop: 'description_VN', name: 'Description VN' },
    { prop: 'materialType', name: 'Material Type' },
    { prop: 'unit', name: 'Unit' },
    { prop: 'materialClass', name: 'Material Class' },
    { prop: 'material_SEC_Classification', name: 'Material SEC Classification', minWidth: 220 },
    { prop: 'material_Group', name: 'Material Group' },
    { prop: 'sapMatGroup', name: 'SAP Mat Group' },
    { prop: 'product_Hierarchy', name: 'Product Hierarchy' },
    { prop: 'productHierarchyDescription', name: 'Product Hierachy description', minWidth: 220 },
    { prop: 'countryOfOrigin', name: 'Country of Origin' },
    { prop: 'referenceLeadTime', name: 'Reference Lead Time (Working Day)', minWidth: 260 },
    { prop: 'warrantyTime', name: 'Warranty time - month', minWidth: 180 },
    { prop: 'inventoryCategory', name: 'Inventory Category', minWidth: 160 },
    { prop: 'cargoNote', name: 'Cargo Note', minWidth: 160 },
    { prop: 'weight', name: 'Weight', minWidth: 160 },
    { prop: 'materialSize', name: 'Material Size', minWidth: 160 },
    { prop: 'qrCode', name: 'QR Code', minWidth: 160 },
    { prop: 'maxlot', name: 'Max lot' },
    { prop: 'stockWarning', name: 'Stock Warning' },
    { prop: 'vat', name: 'VAT' },
    { prop: 'hS_Code', name: 'HS Code' },
    { prop: 'supplierCode', name: 'Supplier' },
    { prop: 'supplierBUCode', name: 'Supplier BU' },
    { prop: 'factory_Text', name: 'Factory' },
    { prop: 'input_Price', name: 'Input Price' },
    { prop: 'inputCurrency', name: 'Input Currency' },
    { prop: 'incoterms', name: 'INCOTERMS' },
    { prop: 'epa', name: 'EPA' },
    { prop: 'importDuty', name: 'Import duty' },
    { prop: 'appliedExchangeRate', name: 'Applied exchange rate', format: 'number', minWidth: 180 },
    { prop: 'landedCost', name: 'Landed Cost (VND)', format: 'number', minWidth: 180 },
    {
      prop: 'maxSalesOfferPrice',
      name: 'Max Sales offer price (VND)',
      format: 'number',
      minWidth: 200,
    },
    {
      prop: 'maxMangerOfferPrice',
      name: 'Max Manager offer price (VND)',
      format: 'number',
      minWidth: 220,
    },
    { prop: 'standard_Price', name: 'Standard Price', format: 'number', minWidth: 180 },
    { prop: 'sellingPrice1', name: 'Selling Price 1', format: 'number' },
    { prop: 'sellingPrice2', name: 'Selling Price 2', format: 'number' },
    { prop: 'sellingPrice3', name: 'Selling Price 3', format: 'number' },
    { prop: 'sellingPrice4', name: 'Selling Price 4', format: 'number' },
    { prop: 'sellingPrice5', name: 'Selling Price 5', format: 'number' },
  ],
  [ImportMaterialManagementType.ApprovalPrice]: [
    { prop: 'golfaCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    { prop: 'spec1', name: 'Spec1' },
    { prop: 'materialType', name: 'Material Type' },
    { prop: 'material_Group', name: 'Material Group' },
    { prop: 'validFrom', name: 'Price valid from', format: 'date', minWidth: 150 },
    { prop: 'validTo', name: 'Price valid to', format: 'date', minWidth: 120 },
    { prop: 'input_Price', name: 'Input Price' },
    { prop: 'inputCurrency', name: 'Input Currency', minWidth: 150 },
    { prop: 'incoterms', name: 'INCOTERMS' },
    { prop: 'epa', name: 'EPA', format: 'boolean' },
    { prop: 'importDuty', name: 'Import duty' },
    { prop: 'appliedExchangeRate', name: 'Applied exchange rate', format: 'number', minWidth: 180 },
    { prop: 'landedCost', name: 'Landed Cost (VND)', format: 'number', minWidth: 180 },
    {
      prop: 'maxSalesOfferPrice',
      name: 'Max Sales offer price (VND)',
      format: 'number',
      minWidth: 200,
    },
    {
      prop: 'maxMangerOfferPrice',
      name: 'Max Manager offer price (VND)',
      format: 'number',
      minWidth: 220,
    },
    { prop: 'standard_Price', name: 'Standard Price (VND)', format: 'number', minWidth: 180 },
    { prop: 'sellingPrice1', name: 'Selling Price 1', format: 'number' },
    { prop: 'sellingPrice2', name: 'Selling Price 2', format: 'number' },
    { prop: 'sellingPrice3', name: 'Selling Price 3', format: 'number' },
    { prop: 'sellingPrice4', name: 'Selling Price 4', format: 'number' },
    { prop: 'sellingPrice5', name: 'Selling Price 5', format: 'number' },
  ],
  [ImportMaterialManagementType.WithoutPrice]: [
    { prop: 'golfaCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    { prop: 'registrationDate', name: 'Registration date', format: 'date' },
    { prop: 'validFrom', name: 'Valid from', format: 'date' },
    { prop: 'validTo', name: 'Valid to', format: 'date' },
    { prop: 'spec1', name: 'Spec1' },
    { prop: 'spec2', name: 'Spec2' },
    { prop: 'spec3', name: 'Spec3' },
    { prop: 'spec4', name: 'Spec4' },
    { prop: 'description_EN', name: 'Description EN' },
    { prop: 'description_VN', name: 'Description VN' },
    { prop: 'supplierCode', name: 'Supplier' },
    { prop: 'supplierBUCode', name: 'Supplier BU' },
    { prop: 'factory_Text', name: 'Factory' },
    { prop: 'materialType', name: 'Material Type' },
    { prop: 'unit', name: 'Unit' },
    { prop: 'material_Group', name: 'Material Group' },
    { prop: 'sapMatGroup', name: 'SAP Mat Group' },
    { prop: 'productHierarchyDescription', name: 'Product Hierachy description', minWidth: 220 },
    { prop: 'countryOfOrigin', name: 'Origin' },
    { prop: 'referenceLeadTime', name: 'Reference Lead Time', minWidth: 180 },
    { prop: 'warrantyTime', name: 'Warranty time' },
    { prop: 'inventoryCategory', name: 'Inventory Category' },
    { prop: 'rowData.cargoNote', name: 'Cargo Note' },
    { prop: 'rowData.weight', name: 'Weight' },
    { prop: 'rowData.size', name: 'Material Size' },
    { prop: 'rowData.qrCode', name: 'QR Code' },

    { prop: 'maxlot', name: 'Max lot' },
    { prop: 'stockWarning', name: 'Stock Warning' },
    { prop: 'hS_Code', name: 'HS Code' },
  ],
  [ImportMaterialManagementType.Status]: [
    { prop: 'golfaCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    {
      prop: 'finalDPOAcceptanceDate',
      name: 'Final DPO acceptance date',
      format: 'date',
      minWidth: 220,
    },
    { prop: 'actionDate', name: 'Action Date', format: 'date' },
    { prop: 'materialStatus', name: 'Action' },
    { prop: 'source', name: 'Source' },
    { prop: 'reason', name: 'Reason' },
    { prop: 'factoryRefDoc', name: 'Factory Ref doc' },
  ],
  [ImportMaterialManagementType.InventoryPlanning]: [
    { prop: 'golfaCode', name: 'Material Code', frozenLeft: true },
    { prop: 'model', name: 'Model', frozenLeft: true },
    { prop: 'inventoryCategory', name: 'Inventory Category' },
    { prop: 'currentStockWarning', name: 'Current Stock Warning' },
    { prop: 'updatedStockWarning', name: 'Updated Stock Warning' },
  ],
  [ImportMaterialManagementType.Leadtime]: [
    { prop: 'golfaCode', name: 'Material Code', frozenLeft: true },
    { prop: 'model', name: 'Model', frozenLeft: true },
    { prop: 'referenceLeadTime', name: 'Reference Lead Time (Working Day)', minWidth: 180 },
    { prop: 'countryOfOrigin', name: 'Country of origin', minWidth: 180 },
    { prop: 'maxlot', name: 'Max lot' },
  ],
  [ImportMaterialManagementType.SAPCode]: [
    { prop: 'golfaCode', name: 'Material Code', frozenLeft: true },
    { prop: 'model', name: 'Model', frozenLeft: true },
    { prop: 'sap_Code', name: 'SAP Code' },
    { prop: 'description_VN', name: 'Description VN' },
    { prop: 'product_Hierarchy', name: 'Product Hierarchy' },
    { prop: 'vat', name: 'VAT' },
  ],
  [ImportMaterialManagementType.Masterdata]: [],
};
