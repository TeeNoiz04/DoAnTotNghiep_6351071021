CREATE TABLE `AbpUsers` (
  `Id` guid PRIMARY KEY,
  `UserName` varchar(255) UNIQUE NOT NULL,
  `Name` varchar(255),
  `Surname` varchar(255),
  `Email` varchar(255),
  `IsActive` bool NOT NULL,
  `TenantId` guid
);

CREATE TABLE `AbpRoles` (
  `Id` guid PRIMARY KEY,
  `Name` varchar(255) NOT NULL,
  `TenantId` guid
);

CREATE TABLE `AbpUserRoles` (
  `UserId` guid NOT NULL,
  `RoleId` guid NOT NULL,
  PRIMARY KEY (`UserId`, `RoleId`)
);

CREATE TABLE `AppWorkflow_Configuration` (
  `Id` guid PRIMARY KEY,
  `WF_Type` varchar(255) NOT NULL COMMENT 'PriceOffer | DPO ',
  `WF_Level` int NOT NULL,
  `WF_Role` varchar(255) NOT NULL,
  `Condition` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppWorkflow_Approver` (
  `Id` guid PRIMARY KEY,
  `WFId` guid NOT NULL,
  `Approver` varchar(255) NOT NULL,
  `Note` varchar(255)
);

CREATE TABLE `AppApprovalRoute` (
  `Id` guid PRIMARY KEY,
  `EntityType` varchar(255) NOT NULL COMMENT 'discriminator: ApprovalRoute | PriceOffer | MaterialApprovalRequest ',
  `InstanceId` guid,
  `StepSequence` int NOT NULL,
  `Approver` varchar(255),
  `ApproverRoleCode` varchar(255) NOT NULL,
  `ApproverRoleName` varchar(255) NOT NULL,
  `IsApproved` bool NOT NULL,
  `ApprovalDate` datetime,
  `Notes` varchar(255),
  `PriceOfferId` guid,
  `MaterialApprovalRequestId` guid
);

CREATE TABLE `AppApprovalHistories` (
  `Id` guid PRIMARY KEY,
  `EntityType` varchar(255) NOT NULL COMMENT 'discriminator: ApprovalHistory | PriceOffer | PriceOfferDetail | DPO | DPODetail | SO | MaterialApprovalRequest  ',
  `Action` varchar(255) NOT NULL COMMENT 'Submitted | Approved | Rejected | Cancelled',
  `ActionDate` datetime NOT NULL,
  `ApproverUsername` varchar(255),
  `ApproverFullName` varchar(255),
  `ApproverRoleCode` varchar(255),
  `ApproverRoleName` varchar(255),
  `IsLastApprovalInCurrentWorkflow` bool NOT NULL,
  `Note` varchar(255),
  `ImportGuid` guid,
  `PriceOfferId` guid,
  `PriceOfferDetailId` guid,
  `DPOId` guid,
  `DPODetailId` guid,
  `SOId` guid,
  `MaterialApprovalRequestId` guid
);

CREATE TABLE `AppAttachments` (
  `Id` guid PRIMARY KEY,
  `AttachName` varchar(255) NOT NULL COMMENT 'discriminator:  PriceOffer',
  `RequestPart` varchar(255),
  `AttachCode` varchar(255),
  `FileName` varchar(255) NOT NULL,
  `FileNameDB` varchar(255) NOT NULL,
  `FilePath` varchar(255),
  `OfflineAttachment` bool NOT NULL,
  `Description` varchar(255),
  `PriceOfferId` guid
);

CREATE TABLE `AppSystemCategories` (
  `Id` guid PRIMARY KEY,
  `ParentId` guid,
  `Code` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Value` decimal,
  `CategoryType` varchar(255) NOT NULL COMMENT 'Location | BuyerType | Material_Group | Material_Type | Currency | Project_Type | EU_Industry | Customer_Type...',
  `SortOrder` int,
  `IsDeactive` bool,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialGroups` (
  `Id` guid PRIMARY KEY,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Parent` guid,
  `MaterialType` varchar(255),
  `SortOrder` int NOT NULL,
  `MaterialGroupPSI` varchar(255),
  `IsDeActive` bool NOT NULL,
  `Note` varchar(255)
);

CREATE TABLE `AppBuyer` (
  `Id` guid PRIMARY KEY,
  `BuyerTypeId` guid NOT NULL,
  `BuyerTypeCode` varchar(255) NOT NULL,
  `BuyerCode` varchar(255) NOT NULL,
  `ShortName` varchar(255),
  `FullName` varchar(255),
  `TaxCode` varchar(255),
  `Address` varchar(255),
  `ContactPerson` varchar(255),
  `ContactEmail` varchar(255),
  `ContactPhoneNumber` varchar(255),
  `PaymentTermCode` varchar(255),
  `PaymentTermDescription` varchar(255),
  `Deactive` bool,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialGroupBuyers` (
  `Id` guid PRIMARY KEY,
  `MaterialGroupId` guid,
  `BuyerId` guid NOT NULL
);

CREATE TABLE `AppSaleTeam` (
  `Id` guid PRIMARY KEY,
  `SaleUserName` varchar(255) NOT NULL,
  `SaleFullName` varchar(255),
  `MaterialType` varchar(255) NOT NULL,
  `LocationId` guid NOT NULL,
  `BuyerId` guid NOT NULL,
  `BuyerShortName` varchar(255),
  `BuyerTypeId` guid NOT NULL,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterials` (
  `Id` guid PRIMARY KEY,
  `GolfaCode` varchar(255) UNIQUE NOT NULL COMMENT 'IX_GolfaCode',
  `Model` varchar(255) NOT NULL,
  `SAP_Code` varchar(255),
  `Spec1` varchar(255),
  `Description_EN` varchar(255),
  `Description_VN` varchar(255),
  `MaterialType` varchar(255),
  `Material_Group` varchar(255) COMMENT 'denorm',
  `SAPMatGroup` varchar(255),
  `Product_Hierarchy` varchar(255),
  `ProductHierarchyDescription` varchar(255),
  `CountryOfOrigin` varchar(255),
  `Unit` varchar(255),
  `VAT` decimal,
  `Factory_Text` varchar(255),
  `Input_Price` decimal,
  `InputCurrency` varchar(255),
  `INCOTERMS` varchar(255),
  `ImportDuty` decimal,
  `AppliedExchangeRate` decimal,
  `LandedCost` decimal,
  `Standard_Price` decimal NOT NULL,
  `MaxSalesOfferPrice` decimal,
  `MaxMangerOfferPrice` decimal,
  `SellingPrice1` decimal,
  `MaterialStatus` varchar(255) NOT NULL,
  `MaterialClass` varchar(255),
  `Material_SEC_Classification` varchar(255),
  `EPA` bool NOT NULL,
  `ReferenceLeadTime` int,
  `Maxlot` int,
  `StockWarning` int,
  `ValidFrom` datetime,
  `ValidTo` datetime,
  `RegistrationDate` datetime,
  `Weight` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialApprovalRequest` (
  `Id` guid PRIMARY KEY,
  `RequestNo` varchar(255) NOT NULL,
  `ImportType` varchar(255) NOT NULL,
  `FileName` varchar(255),
  `Status` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialApprovalRequestDetail` (
  `Id` guid PRIMARY KEY,
  `MaterialApprovalId` guid NOT NULL,
  `GolfaCode` varchar(255) NOT NULL,
  `Model` varchar(255) NOT NULL,
  `Spec1` varchar(255),
  `Description_EN` varchar(255),
  `Description_VN` varchar(255),
  `MaterialType` varchar(255),
  `Material_Group` varchar(255),
  `Unit` varchar(255),
  `HS_Code` varchar(255),
  `Input_Price` decimal,
  `InputCurrency` varchar(255),
  `INCOTERMS` varchar(255),
  `EPA` bool,
  `ImportDuty` decimal,
  `AppliedExchangeRate` decimal,
  `LandedCost` decimal,
  `Standard_Price` decimal,
  `SellingPrice1` decimal,
  `MaxSalesOfferPrice` decimal,
  `MaxMangerOfferPrice` decimal,
  `MaterialStatus` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialHistory` (
  `Id` guid PRIMARY KEY,
  `MaterialId` guid NOT NULL,
  `MaterialApprovalRequestId` guid,
  `Action` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppStockCategory` (
  `Id` guid PRIMARY KEY,
  `StockCode` varchar(255) NOT NULL,
  `StockName` varchar(255) NOT NULL,
  `SAPCode` varchar(255),
  `MainStock` bool,
  `FOC` bool,
  `DamagedStock` bool,
  `IsDeactive` bool,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialStock` (
  `Id` guid PRIMARY KEY,
  `MaterialId` guid NOT NULL,
  `StockCategoryId` guid NOT NULL,
  `GolfaCode` varchar(255) NOT NULL COMMENT 'denorm',
  `Model` varchar(255) NOT NULL COMMENT 'denorm',
  `Qty` int,
  `Locked` int,
  `LockStockKeeping` int,
  `LockStockSO` int,
  `Available_Qty` int,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialStockLockShipment` (
  `Id` guid PRIMARY KEY,
  `MaterialStockId` guid NOT NULL,
  `Qty` int,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialStockLockStock` (
  `Id` guid PRIMARY KEY,
  `MaterialStockId` guid NOT NULL,
  `Qty` int
);

CREATE TABLE `AppCustomer` (
  `Id` guid PRIMARY KEY,
  `TaxCode` varchar(255) NOT NULL,
  `CustomerName` varchar(255),
  `CustomerShortName` varchar(255),
  `CustomerType` varchar(255),
  `CustomerIndustry` varchar(255),
  `Country` varchar(255),
  `Province` varchar(255),
  `Address` varchar(255),
  `Website` varchar(255),
  `Phone` varchar(255),
  `Note` varchar(255),
  `IsDeactive` bool
);

CREATE TABLE `AppPriceOffer` (
  `Id` guid PRIMARY KEY,
  `PriceOffer_Code` varchar(255) UNIQUE NOT NULL COMMENT 'IX_PriceOffer_Code',
  `MaterialType` varchar(255) NOT NULL,
  `ApprovalStatus` varchar(255) COMMENT 'Draft | Verifying | InProgress | Approved | Rejected | Cancelled | Closed',
  `BuyerId` guid,
  `BuyerCode` varchar(255) COMMENT 'denorm',
  `BuyerTypeId` guid,
  `BuyerTypeDescription` varchar(255) COMMENT 'denorm',
  `LocationId` guid,
  `LocationDescription` varchar(255) COMMENT 'denorm',
  `Location_Old` varchar(255),
  `ProjectName` varchar(255),
  `ProjectTypeDescription` varchar(255) COMMENT 'denorm',
  `EUIndustryId` guid,
  `EUIndustryDescription` varchar(255) COMMENT 'denorm',
  `Application` varchar(255),
  `Country` varchar(255),
  `DetailedAddress` varchar(255),
  `CompetitorBrand` varchar(255),
  `POPlannedDate` datetime,
  `DeliveryDate` datetime,
  `CloseDate` datetime,
  `UpcomingPotentialProjects` varchar(255),
  `OtherPJInformation` varchar(255),
  `FileName` varchar(255),
  `TotalMEVNOfferAmount` decimal NOT NULL,
  `InitialTotalMEVNOfferAmount` decimal,
  `TotalStandardAmount` decimal NOT NULL,
  `TotalRequestedAmount` decimal,
  `TotalPriceToCustomer` decimal NOT NULL,
  `SPO_DiscountRatio` decimal,
  `SPO_DiscountRatio_CFG` decimal,
  `HasDPOUsed` bool,
  `ProjectResultStatus` varchar(255) COMMENT 'Won | Lost | Pending | PreOrder',
  `Note` varchar(255)
);

CREATE TABLE `AppPriceOffer_Customer` (
  `Id` guid PRIMARY KEY,
  `PriceOfferId` guid NOT NULL,
  `CustomerId` guid NOT NULL,
  `SaleChannel` varchar(255) NOT NULL,
  `SaleChannelNumber` int NOT NULL,
  `CustomerTaxCode` varchar(255) COMMENT 'denorm',
  `CustomerName` varchar(255) COMMENT 'denorm',
  `CustomerAddress` varchar(255) COMMENT 'denorm',
  `CustomerNationality` varchar(255) COMMENT 'denorm',
  `CustomerType` varchar(255) COMMENT 'denorm',
  `CustomerIndustry` varchar(255) COMMENT 'denorm',
  `Note` varchar(255)
);

CREATE TABLE `AppPriceOfferDetail` (
  `Id` guid PRIMARY KEY,
  `PriceOfferId` guid NOT NULL,
  `RowNo` int NOT NULL,
  `GolfaCode` varchar(255) NOT NULL COMMENT 'IX_PriceOfferDetail_GolfaCode',
  `ModelName` varchar(255) NOT NULL,
  `SpecialSpec1` varchar(255),
  `Qty` decimal NOT NULL,
  `StandardPrice` decimal NOT NULL,
  `StandardAmount` decimal NOT NULL,
  `BuyerPrice` decimal,
  `RequestedAmount` decimal,
  `RequestedDiscountRatio` decimal,
  `MEVNOfferPrice` decimal NOT NULL,
  `PriceToCustomer` decimal,
  `LandingCost` decimal,
  `InputPrice` decimal,
  `InputCurrency` varchar(255),
  `MaxSalesOfferPrice` decimal,
  `MaxMangerOfferPrice` decimal,
  `ManagerMargin` decimal,
  `PriceOfferDetailMargin` decimal,
  `ActualDiscountRatio` decimal,
  `DpoUsed` decimal,
  `Status` varchar(255),
  `ImportGuid` guid NOT NULL,
  `Note` varchar(255)
);

CREATE TABLE `AppDPO` (
  `Id` guid PRIMARY KEY,
  `DPONo` varchar(255),
  `DPOType` varchar(255) COMMENT 'DPO',
  `MaterialType` varchar(255),
  `Status` varchar(255),
  `BuyerId` guid,
  `BuyerShortName` varchar(255) COMMENT 'denorm',
  `BuyerTypeId` guid,
  `BuyerTypeDescription` varchar(255) COMMENT 'denorm',
  `OrderDate` datetime,
  `ExpirationDate` datetime,
  `TotalAmount` decimal NOT NULL,
  `TotalAmountIncludeExtraFee` decimal,
  `Remark` varchar(255),
  `FileName` varchar(255),
  `ReferenceDoc` varchar(255),
  `ReferenceDocDate` datetime,
  `CurrentApprovalStepSequence` int,
  `CurrentApproverRoleCode` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppDPODetail` (
  `Id` guid PRIMARY KEY,
  `DPOId` guid NOT NULL,
  `RowNo` int,
  `GolfaCode` varchar(255) NOT NULL,
  `Model` varchar(255),
  `Spec1` varchar(255),
  `Qty` int,
  `UnitPrice` decimal,
  `LandedCost` decimal,
  `Amount` decimal,
  `AmountIncludeExtraFee` decimal,
  `RequestedETA` datetime,
  `SPOCode` varchar(255),
  `CustomerId` guid,
  `CustomerTaxCode` varchar(255) COMMENT 'denorm',
  `CustomerName` varchar(255) COMMENT 'denorm',
  `CustomerType` varchar(255) COMMENT 'denorm',
  `CustomerIndustry` varchar(255) COMMENT 'denorm',
  `LockStock` int,
  `LockStockSO` int,
  `LockShipment` int,
  `Delivered` int,
  `NeedDelivery` int,
  `OrderReason` int,
  `AccountNo` varchar(255),
  `Extrafee` decimal,
  `Extrafee_Used_InSO` decimal,
  `Extrafee_Available` decimal,
  `Extrafee_Note` varchar(255),
  `DamagedProduct` varchar(255),
  `ProductSerialNo` varchar(255),
  `MEVNSellingInvoiceNo` varchar(255),
  `ConfirmNoted` varchar(255),
  `Status` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppSaleOrders` (
  `Id` guid PRIMARY KEY,
  `SONo` varchar(255),
  `MaterialType` varchar(255),
  `BuyerId` guid,
  `OrderDate` datetime,
  `StatusCode` varchar(255),
  `StockCategoryId` guid,
  `SO_VAT` decimal,
  `IsDeleted` bool
);

CREATE TABLE `AppSaleOrderDetails` (
  `Id` guid PRIMARY KEY,
  `SaleOrderId` guid NOT NULL,
  `DPODetailId` guid,
  `GolfaCode` varchar(255),
  `Qty` int,
  `Price` decimal,
  `Amount` decimal,
  `VAT` decimal,
  `StatusCode` varchar(255),
  `IsDeleted` bool
);

CREATE TABLE `AppStockImports` (
  `Id` guid PRIMARY KEY,
  `FileName` varchar(255),
  `Status` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppStockImportDetails` (
  `Id` guid PRIMARY KEY,
  `StockImportId` guid NOT NULL,
  `GolfaCode` varchar(255),
  `Qty` int
);

CREATE TABLE `AppMaterialStockUploads` (
  `Id` guid PRIMARY KEY,
  `Note` varchar(255)
);

CREATE TABLE `AppMaterialStockUploadDetail` (
  `Id` guid PRIMARY KEY,
  `RequestId` guid NOT NULL,
  `MaterialCode` varchar(255) NOT NULL,
  `Model` varchar(255),
  `Storage` varchar(255),
  `StorageDestination` varchar(255),
  `Qty` int,
  `RefDoc` varchar(255),
  `Remark` varchar(255),
  `StorageSrc_Id` guid,
  `StorageDesc_Id` guid
);

CREATE TABLE `AppMessages` (
  `Id` guid PRIMARY KEY,
  `PriceOfferId` guid,
  `Content` varchar(255)
);

CREATE TABLE `AppAddMoreItemHistories` (
  `Id` guid PRIMARY KEY,
  `MaterialCode` varchar(255),
  `Model` varchar(255),
  `Spec1` varchar(255),
  `Spec2` varchar(255),
  `Qty` int,
  `StandardPriceToDist` decimal,
  `StandardAmount` decimal,
  `DistRequestedPrice` decimal,
  `RequestedAmount` decimal,
  `RequestedDiscount` decimal,
  `PriceToCustomer` decimal,
  `PriceOffer` decimal,
  `CometiorBrand` varchar(255),
  `CompetiorModel` varchar(255),
  `CompetiorPrice` decimal,
  `ImportGuid` guid
);

CREATE TABLE `AppHistoryTrackings` (
  `Id` guid PRIMARY KEY,
  `Note` varchar(255)
);

CREATE TABLE `AppStockTracings` (
  `Id` guid PRIMARY KEY,
  `FileName` varchar(255),
  `ReportType` varchar(255),
  `FromDate` datetime,
  `ToDate` datetime,
  `Note` varchar(255)
);

CREATE TABLE `AppStockTracingDetails` (
  `Id` guid PRIMARY KEY,
  `StockTracingId` guid NOT NULL,
  `ReportType` varchar(255),
  `RowNo` int,
  `GolfaCode` varchar(255),
  `SKUCode` varchar(255),
  `SKUName` varchar(255),
  `Stock` varchar(255),
  `BU` varchar(255),
  `Customer` varchar(255),
  `Category` varchar(255),
  `Unit` varchar(255),
  `Quality` varchar(255),
  `Warranty` varchar(255),
  `Series` varchar(255),
  `Location` varchar(255),
  `Note` varchar(255)
);

CREATE TABLE `AppDistributorTargets` (
  `Id` guid PRIMARY KEY,
  `BuyerId` guid,
  `FY` int,
  `Note` varchar(255)
);

CREATE TABLE `AppCfgDiscountRatios` (
  `Id` guid PRIMARY KEY,
  `MaterialType` varchar(255),
  `DiscountRatio` decimal,
  `Note` varchar(255)
);

CREATE TABLE `AppSystemConfigurations` (
  `Id` guid PRIMARY KEY,
  `Key` varchar(255) NOT NULL,
  `Value` varchar(255),
  `Note` varchar(255)
);

ALTER TABLE `AbpUserRoles` ADD FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`);

ALTER TABLE `AbpUserRoles` ADD FOREIGN KEY (`RoleId`) REFERENCES `AbpRoles` (`Id`);

ALTER TABLE `AppWorkflow_Approver` ADD FOREIGN KEY (`WFId`) REFERENCES `AppWorkflow_Configuration` (`Id`);

ALTER TABLE `AppApprovalRoute` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppApprovalRoute` ADD FOREIGN KEY (`MaterialApprovalRequestId`) REFERENCES `AppMaterialApprovalRequest` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`PriceOfferDetailId`) REFERENCES `AppPriceOfferDetail` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`DPOId`) REFERENCES `AppDPO` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`DPODetailId`) REFERENCES `AppDPODetail` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`SOId`) REFERENCES `AppSaleOrders` (`Id`);

ALTER TABLE `AppApprovalHistories` ADD FOREIGN KEY (`MaterialApprovalRequestId`) REFERENCES `AppMaterialApprovalRequest` (`Id`);

ALTER TABLE `AppAttachments` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppSystemCategories` ADD FOREIGN KEY (`ParentId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppMaterialGroups` ADD FOREIGN KEY (`Parent`) REFERENCES `AppMaterialGroups` (`Id`);

ALTER TABLE `AppBuyer` ADD FOREIGN KEY (`BuyerTypeId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppMaterialGroupBuyers` ADD FOREIGN KEY (`MaterialGroupId`) REFERENCES `AppMaterialGroups` (`Id`);

ALTER TABLE `AppMaterialGroupBuyers` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);

ALTER TABLE `AppSaleTeam` ADD FOREIGN KEY (`LocationId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppSaleTeam` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);

ALTER TABLE `AppSaleTeam` ADD FOREIGN KEY (`BuyerTypeId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppMaterialApprovalRequestDetail` ADD FOREIGN KEY (`MaterialApprovalId`) REFERENCES `AppMaterialApprovalRequest` (`Id`);

ALTER TABLE `AppMaterialHistory` ADD FOREIGN KEY (`MaterialId`) REFERENCES `AppMaterials` (`Id`);

ALTER TABLE `AppMaterialHistory` ADD FOREIGN KEY (`MaterialApprovalRequestId`) REFERENCES `AppMaterialApprovalRequest` (`Id`);

ALTER TABLE `AppMaterialStock` ADD FOREIGN KEY (`MaterialId`) REFERENCES `AppMaterials` (`Id`);

ALTER TABLE `AppMaterialStock` ADD FOREIGN KEY (`StockCategoryId`) REFERENCES `AppStockCategory` (`Id`);

ALTER TABLE `AppMaterialStockLockShipment` ADD FOREIGN KEY (`MaterialStockId`) REFERENCES `AppMaterialStock` (`Id`);

ALTER TABLE `AppMaterialStockLockStock` ADD FOREIGN KEY (`MaterialStockId`) REFERENCES `AppMaterialStock` (`Id`);

ALTER TABLE `AppPriceOffer` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);

ALTER TABLE `AppPriceOffer` ADD FOREIGN KEY (`BuyerTypeId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppPriceOffer` ADD FOREIGN KEY (`LocationId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppPriceOffer` ADD FOREIGN KEY (`EUIndustryId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppPriceOffer_Customer` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppPriceOffer_Customer` ADD FOREIGN KEY (`CustomerId`) REFERENCES `AppCustomer` (`Id`);

ALTER TABLE `AppPriceOfferDetail` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppDPO` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);

ALTER TABLE `AppDPO` ADD FOREIGN KEY (`BuyerTypeId`) REFERENCES `AppSystemCategories` (`Id`);

ALTER TABLE `AppDPODetail` ADD FOREIGN KEY (`DPOId`) REFERENCES `AppDPO` (`Id`);

ALTER TABLE `AppDPODetail` ADD FOREIGN KEY (`CustomerId`) REFERENCES `AppCustomer` (`Id`);

ALTER TABLE `AppSaleOrders` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);

ALTER TABLE `AppSaleOrders` ADD FOREIGN KEY (`StockCategoryId`) REFERENCES `AppStockCategory` (`Id`);

ALTER TABLE `AppSaleOrderDetails` ADD FOREIGN KEY (`SaleOrderId`) REFERENCES `AppSaleOrders` (`Id`);

ALTER TABLE `AppSaleOrderDetails` ADD FOREIGN KEY (`DPODetailId`) REFERENCES `AppDPODetail` (`Id`);

ALTER TABLE `AppStockImportDetails` ADD FOREIGN KEY (`StockImportId`) REFERENCES `AppStockImports` (`Id`);

ALTER TABLE `AppMaterialStockUploadDetail` ADD FOREIGN KEY (`RequestId`) REFERENCES `AppMaterialStockUploads` (`Id`);

ALTER TABLE `AppMessages` ADD FOREIGN KEY (`PriceOfferId`) REFERENCES `AppPriceOffer` (`Id`);

ALTER TABLE `AppStockTracingDetails` ADD FOREIGN KEY (`StockTracingId`) REFERENCES `AppStockTracings` (`Id`);

ALTER TABLE `AppDistributorTargets` ADD FOREIGN KEY (`BuyerId`) REFERENCES `AppBuyer` (`Id`);
