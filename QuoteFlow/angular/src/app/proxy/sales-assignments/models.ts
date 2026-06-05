import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto, UserLookupCreateDto } from '../shared/models';
import type { SystemCategoryListDto } from '../system-categories/models';
import type { BuyerListDto } from '../buyers/models';

export interface GetSalesAssignmentInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  saleUserName?: string;
  materialType?: string;
  locationId?: string;
  buyerId?: string;
  buyerTypeId?: string;
}

export interface SaleReportByCustomerDto {
  customerTaxCode?: string;
  customerName?: string;
  nationality?: string;
  customerType?: string;
  industry?: string;
  typeOfBusiness?: string;
  kaType?: string;
  kaClass?: string;
  dpoNo?: string;
  distributor?: string;
  dpoDate?: string;
  spoCode?: string;
  golfaCode?: string;
  model?: string;
  materialType?: string;
  materialGroup?: string;
  dpoQty?: number;
  unitStandardPrice?: number;
  dpoUnitPrice?: number;
  dpo_Amount?: number;
  discountPercent?: number;
  invoiceNo?: string;
  invoiceDate?: string;
  invoiceQty?: number;
  amountVATInvoice?: number;
  unitLandedCost?: number;
  gpPercent?: number;
}

export interface SaleReportByCustomerR05Dto {
  customerTaxCode?: string;
  customerName?: string;
  nationality?: string;
  customerType?: string;
  industry?: string;
  typeOfBusiness?: string;
  buyer?: string;
  kaType?: string;
  kaCode?: string;
  kaClass?: string;
  materialType?: string;
  spoStandardAmount?: number;
  spoSaleOfferAmount?: number;
  discountPercent?: number;
  spoLandedCostAmount?: number;
  spogp?: number;
  dpoAmount?: number;
  soAmount?: number;
  dpogp?: number;
  sogp?: number;
}

export interface SaleReportInput extends PagedAndSortedResultRequestDto {
  fromDate?: string;
  toDate?: string;
  invoiceFromDate?: string;
  invoiceToDate?: string;
}

export interface SalesAssignmentCreateDto {
  materialType: string;
  locationId: string;
  buyerId: string;
  buyerTypeId: string;
  note?: string;
  buyerShortName?: string;
  users: UserLookupCreateDto[];
}

export interface SalesAssignmentDto extends ExtendedAuditedEntityDto<string> {
  saleUserName?: string;
  saleFullName?: string;
  materialType?: string;
  locationId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerTypeId?: string;
  note?: string;
  concurrencyStamp?: string;
  buyerType: SystemCategoryListDto;
  location: SystemCategoryListDto;
  buyer: BuyerListDto;
}

export interface SalesAssignmentUpdateDto {
  saleUserName: string;
  saleFullName?: string;
  materialType: string;
  locationId: string;
  buyerId: string;
  buyerTypeId: string;
  note?: string;
  buyerShortName?: string;
  concurrencyStamp?: string;
}
