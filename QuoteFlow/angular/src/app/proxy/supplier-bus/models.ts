import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { SupplierDto } from '../suppliers/models';

export interface GetSupplierBUsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  supplierBUCode?: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierShortName?: string;
  supplierAddress?: string;
  sortOrderMin?: number;
  sortOrderMax?: number;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isDeactive?: boolean;
}

export interface SupplierBUCreateDto {
  supplierBUCode: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierShortName?: string;
  supplierAddress?: string;
  sortOrder: number;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isDeactive?: boolean;
}

export interface SupplierBUDto extends ExtendedAuditedEntityDto<string> {
  supplierBUCode?: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierShortName?: string;
  supplierAddress?: string;
  sortOrder: number;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isDeactive?: boolean;
  supplier: SupplierDto;
  concurrencyStamp?: string;
}

export interface SupplierBUExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  supplierBUCode?: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierShortName?: string;
  supplierAddress?: string;
  sortOrderMin?: number;
  sortOrderMax?: number;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isDeactive?: boolean;
}

export interface SupplierBUImportDto {
  no?: number;
  supplierBU?: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  sapCode?: string;
  supplierID?: string;
  supplierCode?: string;
  supplierAddress?: string;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isUpdate?: boolean;
  idUpdate?: string;
  concurrencyStamp?: string;
}

export interface SupplierBUUpdateDto {
  supplierBUCode: string;
  supplierBURemarks?: string;
  orderMethod?: string;
  poTemplate?: string;
  contact?: string;
  email?: string;
  incoTerm?: string;
  paymentTermCode?: string;
  paymentDescription?: string;
  currency?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierShortName?: string;
  supplierAddress?: string;
  sortOrder: number;
  fascmVendorCode?: string;
  fascmBuyerCode?: string;
  fascmConsigneeCode?: string;
  fascmSectionCode?: string;
  fascmPaymentTerm?: string;
  fascmFreightMethod?: string;
  fascmDeliveryTerms?: string;
  fascmPlaceOfDeliveryTerms?: string;
  fascmShippingMarkCode?: string;
  isDeactive?: boolean;
  concurrencyStamp?: string;
}
