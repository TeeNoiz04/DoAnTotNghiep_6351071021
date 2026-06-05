import type { EntityDto } from '@abp/ng.core';

export interface ExtendedAuditedEntityDto<TKey> extends EntityDto<TKey> {
  creatorId?: string;
  creatorUsername?: string;
  creatorName?: string;
  creationTime?: string;
  lastModifierId?: string;
  lastModifierUsername?: string;
  lastModifierName?: string;
  lastModificationTime?: string;
}

export interface ExtendedFullAuditedEntityDto<TKey> extends ExtendedAuditedEntityDto<TKey> {
  deleterId?: string;
  deleterUsername?: string;
  deleterName?: string;
  deletionTime?: string;
  isDeleted: boolean;
  forceDelete: boolean;
}

export interface NoteMetadataDto {
  note?: string;
  concurrencyStamp: string;
}

export interface UserLookupCreateDto {
  id?: string;
  fullName?: string;
  email?: string;
  phoneNumber?: string;
  userName?: string;
}

export interface AccountCodeLookupDto {
  accountNo?: string;
  accountName?: string;
  inputPrice: number;
  landedCost: number;
  materialCode?: string;
  status?: string;
}

export interface ActionDto {
  action: string;
  comment?: string;
  concurrencyStamp: string;
}

export interface DownloadTokenResultDto {
  token?: string;
}

export interface GicTypeLookupDto<TKey> {
  id: TKey;
  displayCode?: string;
  displayName?: string;
  hasProcess: boolean;
  processes: LookupDto<TKey>[];
}

export interface KeyAccountLookupDto<TKey> extends LookupDto<TKey> {
  keyAccountClassId: TKey;
  keyAccountTypeId: TKey;
  keyAccountClassName?: string;
  keyAccountTypeName?: string;
}

export interface LookupDto<TKey> {
  id: TKey;
  displayCode?: string;
  displayName?: string;
}

export interface StockCategoryLookupDto<TKey> extends LookupDto<TKey> {
  availableQuantity: number;
}

export interface SupplierBULookupDto<TKey> extends LookupDto<TKey> {
  materialType?: string;
  currency?: string;
}

export interface SupplierPOLookupDto {
  supplierCode?: string;
  supplierId?: string;
  supplierBUCode?: string;
  supplierBUId?: string;
}

export interface UserLookupDto {
  id?: string;
  fullName?: string;
  email?: string;
  phoneNumber?: string;
  userName?: string;
}
