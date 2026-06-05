import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { SupplierBUDto } from '../supplier-bus/models';
import type { SupplierDto } from '../suppliers/models';

export interface GetSpecialInputPricesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  accountNo?: string;
  accountName?: string;
  materials: string[];
  models: string[];
  projectName?: string;
  validFromMin?: string;
  validFromMax?: string;
  validToMin?: string;
  validToMax?: string;
  status?: string;
  note?: string;
}

export interface SpecialInputPriceCreateDto {
  accountNo: string;
  accountName: string;
  projectName?: string;
  materialType?: string;
  supplierId?: string;
  supplierBUId?: string;
  currency?: string;
  validFrom?: string;
  validTo?: string;
  status: string;
  note?: string;
}

export interface SpecialInputPriceDetailImportDto {
  no?: number;
  accountNo?: string;
  materialCode?: string;
  modelName?: string;
  spec?: string;
  limitQty?: number;
  specialInputPrice?: number;
  landedCost?: number;
}

export interface SpecialInputPriceDto extends ExtendedAuditedEntityDto<string> {
  accountNo?: string;
  accountName?: string;
  projectName?: string;
  materialType?: string;
  supplierId?: string;
  supplierBUId?: string;
  currency?: string;
  validFrom?: string;
  validTo?: string;
  status?: string;
  note?: string;
  totalAmount: number;
  supplierBU: SupplierBUDto;
  supplier: SupplierDto;
  concurrencyStamp?: string;
}

export interface SpecialInputPriceUpdateDto {
  accountNo: string;
  accountName: string;
  projectName?: string;
  materialType?: string;
  supplierId?: string;
  supplierBUId?: string;
  currency?: string;
  validFrom?: string;
  validTo?: string;
  status: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface SpecialInputPriceLookupDto<TKey> {
  id: TKey;
  accountName?: string;
  accountNo?: string;
  status?: string;
  materialType?: string;
}
