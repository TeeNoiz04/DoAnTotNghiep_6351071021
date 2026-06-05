import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { PriceOfferDetailDto, PriceOfferDetailImportDto } from './price-offer-details/models';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { BuyerDto, BuyerListDto } from '../buyers/models';
import type { KeyAccountListDto } from '../key-accounts/models';
import type {
  PriceOfferCustomerDto,
  PriceOfferCustomerImportDto,
} from './price-offer-customers/models';
import type { ApprovalHistoryDto } from '../approval-histories/models';
import type { AttachmentDto } from '../attachments/models';
import type { SystemCategoryListDto } from '../system-categories/models';

export interface AssignSpecialInputPriceDto {
  specialInputPriceId?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface ConfirmPreOrderStatusDto {
  resultStatus: string;
  note: string;
}

export interface GetPriceOffersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  priceOfferType?: string;
  materialType?: string;
  buyerId?: string;
  priceOfferCode?: string;
  customerTaxCode?: string;
  customerName?: string;
  approvalStatus?: string;
  createdFrom?: string;
  createdTo?: string;
  relatedToMe: boolean;
  projectName?: string;
  projectResultStatus?: string;
}

export interface ImportAddMoreItemsInput {
  validationResult: ExcelValidationResult<PriceOfferDetailImportDto>;
  comment?: string;
}

export interface PriceOfferAPImportInput {
  getPriceAutomatically: boolean;
  buyerId: string;
  buyerTypeId: string;
  locationId: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
  salePIC?: string;
  materialType?: string;
  projectName?: string;
  note?: string;
}

export interface PriceOfferCreateDto {
  priceOfferCode: string;
  buyerId: string;
  buyerTypeId: string;
  materialType: string;
  locationId?: string;
  locationOld?: string;
  projectName?: string;
  projectTypeId?: string;
  euIndustryId?: string;
  application?: string;
  country?: string;
  province?: string;
  detailedAddress?: string;
  competitorBrand?: string;
  priceGapWithCompetitor?: string;
  decisionRight?: string;
  poPlannedDate?: string;
  deliveryDate?: string;
  upcomingPotentialProjects?: string;
  otherPJInformation?: string;
  fileName?: string;
  note?: string;
  closeDate?: string;
  totalMEVNOfferAmount?: number;
  accountNo?: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
  projectTypeDescription?: string;
  euIndustryDescription?: string;
  keyAccountClassDescription?: string;
  keyAccountTypeDescription?: string;
  locationDescription?: string;
}

export interface PriceOfferDSImportInput {
  buyerId: string;
  buyerTypeId: string;
  locationId: string;
  salePIC?: string;
  materialType?: string;
  projectName?: string;
  note?: string;
}

export interface PriceOfferDto extends ExtendedAuditedEntityDto<string> {
  priceOfferCode?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerTypeId?: string;
  buyerTypeDescription?: string;
  materialType?: string;
  locationId?: string;
  locationOld?: string;
  projectName?: string;
  projectTypeId?: string;
  euIndustryId?: string;
  application?: string;
  country?: string;
  province?: string;
  detailedAddress?: string;
  competitorBrand?: string;
  priceGapWithCompetitor?: string;
  decisionRight?: string;
  poPlannedDate?: string;
  deliveryDate?: string;
  upcomingPotentialProjects?: string;
  otherPJInformation?: string;
  fileName?: string;
  note?: string;
  closeDate?: string;
  totalMEVNOfferAmount?: number;
  approvalStatus?: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: string;
  currentApproverRoleName?: string;
  projectResultStatus?: string;
  projectResultNote?: string;
  projectResultSubmittedAt?: string;
  projectResultSubmitterId?: string;
  projectResultSubmitterUsername?: string;
  projectResultSubmitterFullName?: string;
  projectTypeDescription?: string;
  euIndustryDescription?: string;
  keyAccountClassDescription?: string;
  keyAccountTypeDescription?: string;
  locationDescription?: string;
  totalUsableOfferAmount?: number;
  totalDpoUsedPercentage?: number;
  totalDpoUsedAmount?: number;
  totalStandardAmount: number;
  totalRequestedAmount: number;
  totalPriceToCustomer: number;
  totalRequestedDiscountAmount: number;
  totalLandedCost: number;
  totalGP: number;
  discountRatio?: number;
  discountRatioConfigured?: number;
  totalMarginIssues?: number;
  accountNo?: string;
  specialInputPriceId?: string;
  specialInputPriceAssignmentNote?: string;
  specialInputPriceAccountName?: string;
  specialInputPriceAssignedTime?: string;
  specialInputPriceAssignerId?: string;
  specialInputPriceAssignerUsername?: string;
  specialInputPriceAssignerFullName?: string;
  initialTotalMEVNOfferAmount: number;
  hasDPOUsed: boolean;
  concurrencyStamp?: string;
  buyer: BuyerDto;
  keyAccount: KeyAccountListDto;
  customers: PriceOfferCustomerDto[];
  details: PriceOfferDetailDto[];
  approvalHistories: ApprovalHistoryDto[];
  attachments: AttachmentDto[];
  flags: PriceOfferFlagsDto;
}

export interface PriceOfferExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  priceOfferCode?: string;
  buyerId?: string;
  buyerTypeId?: string;
  materialType?: string;
  locationId?: string;
  locationOld?: string;
  projectName?: string;
  projectTypeId?: string;
  euIndustryId?: string;
  application?: string;
  country?: string;
  province?: string;
  detailedAddress?: string;
  competitorBrand?: string;
  priceGapWithCompetitor?: string;
  decisionRight?: string;
  poPlannedDateMin?: string;
  poPlannedDateMax?: string;
  deliveryDateMin?: string;
  deliveryDateMax?: string;
  upcomingPotentialProjects?: string;
  otherPJInformation?: string;
  fileName?: string;
  note?: string;
  closeDateMin?: string;
  closeDateMax?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  approvalStatus?: string;
  projectResultStatus?: string;
  accountNo?: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
}

export interface PriceOfferFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  isApprovable: boolean;
  isRejectable: boolean;
  isCancellable: boolean;
  isClosable: boolean;
  isProjectResultSubmittable: boolean;
  isPreOrderResultConfirmable: boolean;
  isGPViewable: boolean;
  isLandedCostViewable: boolean;
  isDetailPropertiesChangeable: boolean;
  canAddMoreItems: boolean;
  isSpecialInputPriceApplicable: boolean;
  isSpecialInputPriceViewable: boolean;
  isDetailsPropertiesTemplateDownloadable: boolean;
  canCancelItem: boolean;
}

export interface PriceOfferImportDto {
  priceOfferCode?: string;
  fileName?: string;
  totalMEVNOfferAmount?: number;
  totalPriceToCustomer?: number;
  totalRequestedAmount?: number;
  totalStandardAmount?: number;
  discountRatio?: number;
  discountRatioConfigured?: number;
  buyerId?: string;
  buyerTypeId?: string;
  locationId?: string;
  accountNo?: string;
  note?: string;
  closeDate?: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
  materialType?: string;
  projectName?: string;
  projectTypeId?: string;
  euIndustryId?: string;
  application?: string;
  country?: string;
  province?: string;
  detailedAddress?: string;
  competitorBrand?: string;
  priceGapWithCompetitor?: string;
  decisionRight?: string;
  poPlannedDate?: string;
  deliveryDate?: string;
  upcomingPotentialProjects?: string;
  otherPJInformation?: string;
  projectTypeDescription?: string;
  euIndustryDescription?: string;
  details: ExcelValidationResult<PriceOfferDetailImportDto>;
  customers: ExcelValidationResult<PriceOfferCustomerImportDto>;
}

export interface PriceOfferImportInput {
  buyerId: string;
  buyerTypeId: string;
  locationId: string;
  salePIC?: string;
  closeDate: string;
  materialType?: string;
  projectName?: string;
  note: string;
}

export interface PriceOfferListDto extends ExtendedAuditedEntityDto<string> {
  approvalStatus?: string;
  projectResultStatus?: string;
  priceOfferCode?: string;
  buyerId?: string;
  buyerTypeId?: string;
  projectName?: string;
  totalMEVNOfferAmount?: number;
  totalDpoUsedAmount?: number;
  totalStandardAmount?: number;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: string;
  currentApproverRoleName?: string;
  flags: PriceOfferFlagsDto;
}

export interface PriceOfferNBImportInput {
  locationId: string;
  salePIC?: string;
  closeDate: string;
  materialType?: string;
  projectName?: string;
  note: string;
}

export interface PriceOfferUpdateDto {
  priceOfferCode: string;
  buyerId: string;
  materialType: string;
  locationId?: string;
  locationOld?: string;
  projectName?: string;
  projectTypeId?: string;
  euIndustryId?: string;
  application?: string;
  country?: string;
  province?: string;
  detailedAddress?: string;
  competitorBrand?: string;
  priceGapWithCompetitor?: string;
  decisionRight?: string;
  poPlannedDate?: string;
  deliveryDate?: string;
  upcomingPotentialProjects?: string;
  otherPJInformation?: string;
  fileName?: string;
  note?: string;
  closeDate?: string;
  totalMEVNOfferAmount?: number;
  accountNo?: string;
  keyAccountId?: string;
  keyAccountTypeId?: string;
  keyAccountClassId?: string;
  projectTypeDescription?: string;
  euIndustryDescription?: string;
  keyAccountClassDescription?: string;
  keyAccountTypeDescription?: string;
  locationDescription?: string;
  concurrencyStamp?: string;
}

export interface PriceOfferWithNavigationListDto extends PriceOfferListDto {
  approvalHistories: ApprovalHistoryDto[];
  buyer: BuyerListDto;
  buyerType: SystemCategoryListDto;
}

export interface SubmitProjectResultDto {
  resultStatus: string;
  winningCustomers: WinningCustomerPerChannelDto[];
  note: string;
}

export interface WinningCustomerPerChannelDto {
  channelId: string;
  customerId: string;
}
