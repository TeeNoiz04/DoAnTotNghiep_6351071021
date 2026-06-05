import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPriceOfferReportGeneralsInput extends PagedAndSortedResultRequestDto {
  from?: string;
  to?: string;
  buyer?: string;
  customerName?: string;
  priceOfferCode?: string;
  priceOfferName?: string;
  location?: string;
  status?: string;
  materialType?: string;
  orderMin?: number;
  orderMax?: number;
}

export interface PriceOfferReportGeneralDto {
  buyerType?: string;
  materialType?: string;
  spoType?: string;
  projectName?: string;
  creationTime?: string;
  approvalStatus?: string;
  projectResultStatus?: string;
  competitorBrand?: string;
  totalMEVNOfferAmount?: number;
  totalStandardAmount?: number;
  spo_DiscountRatio?: number;
  closeDate?: string;
  country?: string;
  province?: string;
  trading_Customer?: string;
  trading_TaxCode?: string;
  panelBuilder_Customer?: string;
  panelBuilder_TaxCode?: string;
  meContractor_Customer?: string;
  meContractor_TaxCode?: string;
  mainContractor_Customer?: string;
  mainContractor_TaxCode?: string;
  sioem_Customer?: string;
  sioem_TaxCode?: string;
  investorEU_Customer?: string;
  investorEU_TaxCode?: string;
}
