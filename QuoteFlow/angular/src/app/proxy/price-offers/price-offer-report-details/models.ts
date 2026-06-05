import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetPriceOfferReportDetailsInput extends PagedAndSortedResultRequestDto {
  from?: string;
  to?: string;
  golfaCode?: string;
  modelName?: string;
  buyer?: string;
  priceOfferCode?: string;
  priceOfferName?: string;
  materialGroup?: string;
}

export interface PriceOfferReportDetailDto {
  location?: string;
  buyerType?: string;
  buyerCode?: string;
  materialType?: string;
  spoType?: string;
  priceOffer_Code?: string;
  projectName?: string;
  creationTime?: string;
  approvalStatus?: string;
  spoValidity_From?: string;
  spoValidity_To?: string;
  country?: string;
  province?: string;
  projectResultStatus?: string;
  material_Group?: string;
  golfaCode?: string;
  modelName?: string;
  qty: number;
  mevnOfferPrice?: number;
  saleOfferAmount: number;
  standardPrice: number;
  standardAmount: number;
  actualDiscountRatio?: number;
  priceOfferDetailMargin?: number;
  dpo_UsedQty?: number;
  dpo_UsedAmount?: number;
  dpo_DeliveredQty?: number;
  dpo_DeliveredAmount?: number;
  dop_BO_Qty?: number;
  dop_BO_Amount?: number;
  spo_Open_Qty?: number;
  spo_Open_Amount?: number;
}
