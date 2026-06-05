import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { AssetDto } from '../assets/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface AssetRequestDetailCreateDto {
  requestId: string;
  assetId: string;
  assetName: string;
  status?: string;
  note?: string;
}

export interface AssetRequestDetailDto extends ExtendedAuditedEntityDto<string> {
  requestId?: string;
  assetId?: string;
  assetName?: string;
  isDeleted: boolean;
  status?: string;
  auditNote?: string;
  auditResult?: string;
  note?: string;
  counted_Quantity?: number;
  variance?: number;
  faI_PIC?: string;
  faP_PIC?: string;
  iA_PIC?: string;
  aF_PIC?: string;
  asset: AssetDto;
  concurrencyStamp?: string;
  assetClass?: string;
  assetType?: string;
  warehouseId?: string;
  warehouseName?: string;
  salePIC?: string;
  codeMain?: string;
  codeSub?: string;
  codeMain_AF?: string;
  codeSub_AF?: string;
  numberOfComponent?: number;
  por?: string;
  pr?: string;
  giv?: string;
  materialCode?: string;
  modelName?: string;
  unit?: string;
  price?: number;
  invoicePrice?: number;
  amount?: number;
  division?: string;
  department?: string;
  section?: string;
  reg?: string;
  source?: string;
  sectionSAP?: string;
  description?: string;
  qty?: number;
  assetNote?: string;
}

export interface AssetRequestDetailUpdateDto {
  requestId: string;
  assetId?: string;
  assetName?: string;
  isDeleted: boolean;
  status?: string;
  note?: string;
  concurrencyStamp?: string;
  assetClass?: string;
  assetType?: string;
  warehouseId?: string;
  warehouseName?: string;
  salePIC?: string;
  codeMain?: string;
  codeSub?: string;
  codeMain_AF?: string;
  codeSub_AF?: string;
  numberOfComponent?: number;
  por?: string;
  pr?: string;
  giv?: string;
  materialCode?: string;
  modelName?: string;
  unit?: string;
  price?: number;
  invoicePrice?: number;
  amount?: number;
  division?: string;
  department?: string;
  section?: string;
  reg?: string;
  source?: string;
  sectionSAP?: string;
  description?: string;
  qty?: number;
  assetNote?: string;
}

export interface GetAssetRequestDetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  requestId?: string;
  assetId?: string;
  assetName?: string;
  isDeleted?: boolean;
  status?: string;
  note?: string;
}
