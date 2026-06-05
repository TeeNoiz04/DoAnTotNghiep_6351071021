import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { ApprovalHistoryDto } from '../../approval-histories/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface PriceOfferDetailDto extends ExtendedAuditedEntityDto<string> {
  rowNo: number;
  priceOfferId?: string;
  golfaCode?: string;
  modelName?: string;
  specialSpec1?: string;
  specialSpec2?: string;
  dpoUsed?: number;
  qty: number;
  standardPrice: number;
  standardAmount: number;
  buyerPrice?: number;
  requestedAmount?: number;
  requestedDiscountRatio?: number;
  priceToCustomer?: number;
  priceToCustomerAmount: number;
  mevnOfferPrice: number;
  competitorBrand?: string;
  competitorModel?: string;
  competitorPrice?: number;
  landingCost?: number;
  landingCostAmount?: number;
  inputPrice?: number;
  inputCurrency?: string;
  managerMargin?: number;
  priceOfferDetailMargin?: number;
  accountCode?: string;
  note?: string;
  importGuid?: string;
  status?: string;
  maxSalesOfferPrice?: number;
  maxMangerOfferPrice?: number;
  actualDiscountRatio?: number;
  mevnOfferAmount?: number;
  approvalHistories: ApprovalHistoryDto[];
}

export interface PriceOfferDetailImportDto {
  rowNo: number;
  golfaCode?: string;
  modelName?: string;
  specialSpec1?: string;
  specialSpec2?: string;
  qty?: number;
  standardPrice?: number;
  standardAmount?: number;
  buyerPrice?: number;
  requestedAmount?: number;
  requestedDiscountRatio?: number;
  priceToCustomer?: number;
  mevnOfferPrice?: number;
  mevnOfferAmount?: number;
  competitorBrand?: string;
  competitorModel?: string;
  competitorPrice?: number;
}

export interface GetPriceOfferDetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  golfaCode?: string;
  modelName?: string;
  specialSpec1?: string;
  specialSpec2?: string;
  dpoUsedMin?: number;
  dpoUsedMax?: number;
  qtyMin?: number;
  qtyMax?: number;
  standardPriceMin?: number;
  standardPriceMax?: number;
  standardAmountMin?: number;
  standardAmountMax?: number;
  buyerPriceMin?: number;
  buyerPriceMax?: number;
  requestedAmountMin?: number;
  requestedAmountMax?: number;
  requestedDiscountRatioMin?: number;
  requestedDiscountRatioMax?: number;
  priceToCustomerMin?: number;
  priceToCustomerMax?: number;
  mevnOfferPriceMin?: number;
  mevnOfferPriceMax?: number;
  competitorBrand?: string;
  competitorModel?: string;
  competitorPriceMin?: number;
  competitorPriceMax?: number;
  landingCostMin?: number;
  landingCostMax?: number;
  inputPriceMin?: number;
  inputPriceMax?: number;
  inputCurrency?: string;
  managerMarginMin?: number;
  managerMarginMax?: number;
  priceOfferDetailMarginMin?: number;
  priceOfferDetailMarginMax?: number;
  accountCode?: string;
  note?: string;
  importGuid?: string;
}

export interface PriceOfferDetailCancelDto {
  priceOfferDetailIds: string[];
  note?: string;
}

export interface PriceOfferUpdateLandingCostImportDto {
  golfaCode?: string;
  modelName?: string;
  saleOfferPrice?: number;
  qty?: number;
  landingCost?: number;
  newSaleOfferPrice?: number;
  materialId?: string;
  priceOfferDetailId?: string;
  priceOfferId?: string;
}
