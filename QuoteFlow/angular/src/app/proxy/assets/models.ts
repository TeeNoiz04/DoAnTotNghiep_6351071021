import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface AssetCreateDto {
  assetName: string;
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
  amount?: number;
  section?: string;
  status?: string;
  lendingInformation?: string;
  note?: string;
}

export interface AssetDto extends ExtendedAuditedEntityDto<string> {
  assetName?: string;
  description?: string;
  qty?: number;
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
  amount?: number;
  section?: string;
  status?: string;
  lendingInformation?: string;
  note?: string;
  invoicePrice?: number;
  source?: string;
  division?: string;
  department?: string;
  sectionSAP?: string;
  reg?: string;
  requestNo?: string;
  concurrencyStamp?: string;
}

export interface AssetImportDto {
  assetName?: string;
  description?: string;
  source?: string;
  materialCode?: string;
  modelName?: string;
  qty?: number;
  unit?: string;
  price?: number;
  invoicePrice?: number;
  locationWH?: string;
  whId?: string;
  assetType?: string;
  assetClass?: string;
  codeMain?: string;
  codeSub?: string;
  codeMain_AF?: string;
  codeSub_AF?: string;
  picEmail?: string;
  sectionSAP?: string;
  division?: string;
  department?: string;
  section?: string;
  reg?: string;
  por?: string;
  pr?: string;
  giv?: string;
  note?: string;
  id?: string;
  concurrencyStamp?: string;
}

export interface AssetUpdateDto {
  assetName: string;
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
  amount?: number;
  section?: string;
  status?: string;
  lendingInformation?: string;
  note?: string;
}

export interface GetAssetsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  assetName?: string;
  assetClass?: string;
  assetType?: string;
  warehouseId?: string;
  warehouseName?: string;
  salePIC?: string;
  codeMain?: string;
  codeSub?: string;
  codeMain_AF?: string;
  codeSub_AF?: string;
  numberOfComponentMin?: number;
  numberOfComponentMax?: number;
  por?: string;
  pr?: string;
  giv?: string;
  materialCode?: string;
  modelName?: string;
  unit?: string;
  priceMin?: number;
  priceMax?: number;
  amountMin?: number;
  amountMax?: number;
  section?: string;
  status?: string;
  lendingInformation?: string;
  note?: string;
  source?: string;
  invoicePriceMin?: number;
  invoicePriceMax?: number;
  division?: string;
  department?: string;
  sectionSAP?: string;
  reg?: string;
  requestNo?: string;
  isPendingApproval?: boolean;
}

export interface SearchAssetInput extends GetAssetsInput {
  requestId?: string;
}
