import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { SaleOrderDetailDto } from './sale-order-details/models';

export interface GetSaleOrderListDetailDPOsInput {
  buyerId?: string;
  materialType?: string;
  vat?: number;
  stockCategoryId?: string;
}

export interface GetSaleOrderListDetailGICsInput extends PagedAndSortedResultRequestDto {
  gicType: string;
  gicProcess?: string;
  buyerId: string;
  materialType: string;
  vat?: number;
  stockCategoryId: string;
}

export interface GetSaleOrdersInput extends PagedAndSortedResultRequestDto {
  soNo?: string;
  sosapNo?: string;
  materialType?: string;
  dpoNo?: string;
  materialCode?: string;
  invoiceNo?: string;
  lstSO?: string;
  buyerType?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerName?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  statusCode?: string;
  model?: string;
  taxCode?: string;
  buyerTypeId?: string;
  soDateFrom?: string;
  soDateTo?: string;
  vatDateFrom?: string;
  vatDateTo?: string;
  materialGroup?: string;
  soType?: string;
  completelyClosed?: boolean;
  gicType?: string;
  gicProcess?: string;
}

export interface SODetailExtrafeeUpdateInput {
  soDetailId: string;
  extrafee: number;
  extrafeeNode: string;
}

export interface SaleOrderAddedDetailDPODto {
  prSOId?: string;
  dpoDetailId?: string;
  lockStockId?: string;
  materialCode?: string;
  qty: number;
  price: number;
  vat?: number;
  stockCategoryId?: string;
  extrafee: number;
  note?: string;
  materialType?: string;
}

export interface SaleOrderCreateDto {
  sosapNo?: string;
  materialType?: string;
  buyerType?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerName?: string;
  orderDate?: string;
  stockCategoryId?: string;
  sO_VAT?: number;
  sapdoNo?: string;
  sapBillingNo?: string;
  sapInvoice?: string;
  sapInvoiceDate?: string;
  deliveryConfirmed?: boolean;
  sapDeliveryDate?: string;
  note?: string;
  soType?: string;
  gicType?: string;
  gicProcess?: string;
  sapgicLandingCost?: number;
  gicGivNo?: string;
  gicGivDate?: string;
  completelyClosed?: boolean;
}

export interface SaleOrderDto extends ExtendedAuditedEntityDto<string> {
  soNo?: string;
  sosapNo?: string;
  materialType?: string;
  buyerId?: string;
  buyerType?: string;
  buyerCode?: string;
  buyerName?: string;
  orderDate?: string;
  statusCode?: string;
  stockCategoryId?: string;
  sO_VAT?: number;
  note?: string;
  saleOrderDetails: SaleOrderDetailDto[];
  sapdoNo?: string;
  sapBillingNo?: string;
  sapInvoice?: string;
  sapInvoiceDate?: string;
  deliveryConfirmed?: boolean;
  sapDeliveryDate?: string;
  totalAmount?: number;
  concurrencyStamp?: string;
  flags: SaleOrderFlagsDto;
  soType?: string;
  gicType?: string;
  gicProcess?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  completelyClosed?: boolean;
}

export interface SaleOrderFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  canConfirmDelivery: boolean;
  canReOpenSO: boolean;
  canEditSAPInfo: boolean;
}

export interface SaleOrderListDetailDPODto {
  dpoNo?: string;
  dpoDetail_Id?: string;
  golfaCode?: string;
  model?: string;
  qty: number;
  vat?: number;
  stockName?: string;
  unitPrice?: number;
  note?: string;
  extrafee?: number;
  extrafee_Note?: string;
  lockstockId?: string;
  confirmedNote?: string;
}

export interface SaleOrderListDetailGICDto {
  dpoNo?: string;
  dpoDetail_Id?: string;
  golfaCode?: string;
  model?: string;
  qty: number;
  vat?: number;
  stockName?: string;
  unitPrice?: number;
  note?: string;
  extrafee?: number;
  extrafee_Note?: string;
  lockstockId?: string;
  confirmedNote?: string;
}

export interface SaleOrderUpdateDto {
  sosapNo?: string;
  materialType?: string;
  buyerType?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerName?: string;
  orderDate?: string;
  stockCategoryId?: string;
  sO_VAT?: number;
  sapdoNo?: string;
  sapBillingNo?: string;
  sapInvoice?: string;
  sapInvoiceDate?: string;
  deliveryConfirmed?: boolean;
  sapDeliveryDate?: string;
  note?: string;
  concurrencyStamp?: string;
  gicType?: string;
  gicProcess?: string;
  sapgicLandingCost?: number;
  gicGivNo?: string;
  gicGivDate?: string;
  completelyClosed?: boolean;
}

export interface SaleOrderListModalDPODto {
  status?: string;
  soNo?: string;
  sosapNo?: string;
  doNo?: string;
  stockName?: string;
  quantity?: number;
  modifiedBy?: string;
  modified?: string;
}

export interface SaleOrderListModalDeliveryDto {
  soNo?: string;
  sosapNo?: string;
  invoiceNo?: string;
  stockName?: string;
  invoiceDate?: string;
  quantity?: number;
  note?: string;
  modifiedBy?: string;
  modified?: string;
}
