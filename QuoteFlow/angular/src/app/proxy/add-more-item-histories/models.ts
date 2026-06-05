import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface AddMoreItemHistoryDto extends ExtendedAuditedEntityDto<string> {
  importGuid?: string;
  materialCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qty?: number;
  standardPriceToDist?: number;
  standardAmount?: number;
  distRequestedPrice?: number;
  requestedAmount?: number;
  requestedDiscount?: number;
  priceToCustomer?: number;
  priceOffer?: number;
  cometiorBrand?: string;
  competiorModel?: string;
  competiorPrice?: number;
  concurrencyStamp?: string;
}
