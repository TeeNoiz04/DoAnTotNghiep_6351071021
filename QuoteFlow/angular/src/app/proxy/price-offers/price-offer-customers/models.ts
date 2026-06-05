import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface PriceOfferCustomerDto extends ExtendedAuditedEntityDto<string> {
  priceOfferId?: string;
  saleChannel?: string;
  saleChannelNumber: number;
  customerId?: string;
  customerTaxCode?: string;
  customerName?: string;
  customerAddress?: string;
  customerNationality?: string;
  customerType?: string;
  customerIndustry?: string;
  note?: string;
  hasKeyAccount: boolean;
  concurrencyStamp?: string;
}

export interface PriceOfferCustomerImportDto {
  saleChannel?: string;
  customerTaxCode?: string;
  customerName?: string;
  customerAddress?: string;
  customerNationality?: string;
  customerNationalityId?: string;
  customerType?: string;
  customerIndustry?: string;
  hasKeyAccount: boolean;
}

export interface GetPriceOfferCustomersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  priceOfferId?: string;
  saleChannel?: string;
  customerId?: string;
  customerTaxCode?: string;
  customerName?: string;
  customerAddress?: string;
  customerNationality?: string;
  customerType?: string;
  note?: string;
}
