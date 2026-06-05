import type { ExtendedAuditedEntityDto } from '../../shared/models';

export interface SpecialInputPriceDetailDto extends ExtendedAuditedEntityDto<string> {
  specialInputPriceId?: string;
  materialCode?: string;
  standard?: number;
  model?: string;
  spec1?: string;
  limitQty?: number;
  inputPrice: number;
  landedCost: number;
  used: number;
  note?: string;
  concurrencyStamp?: string;
}
