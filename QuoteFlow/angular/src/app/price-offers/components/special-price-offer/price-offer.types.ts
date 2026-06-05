export type ImportPriceOfferInformation = {
  file?: File | null;
  saleName: string;
  buyerId: string;
  buyerName?: string;
  locationId: string;
  locationName?: string;
  closeDate: Date | null;
  keyAccountId: string;
  keyAccountName?: string;
  keyAccountTypeId?: string;
  keyAccountTypeName?: string;
  keyAccountClassId?: string;
  keyAccountClassName?: string;
  autoGetOfferPrice?: boolean;
  note?: string;
  materialTypeId?: string;
  projectName?: string;
  buyerTypeId?: string;
  buyerTypeName?: string;
};

export enum ImportPriceOfferType {
  KeyAccountPriceOffer = 'KeyAccountPriceOffer',
  BuyerStockPriceOffer = 'BuyerStockPriceOffer',
  ProjectPriceOffer = 'ProjectPriceOffer',
  NoBuyerPriceOffer = 'NoBuyerPriceOffer',
  AddMoreItems = 'AddMoreItems',
  UpdateItemProperties = 'UpdateItemProperties',
}

export const ImportPriceOfferTypeMap: Record<ImportPriceOfferType, string> = {
  [ImportPriceOfferType.KeyAccountPriceOffer]: 'Key-Account Price Offer',
  [ImportPriceOfferType.BuyerStockPriceOffer]: 'Buyer Stock Price Offer',
  [ImportPriceOfferType.ProjectPriceOffer]: 'Project Price Offer',
  [ImportPriceOfferType.NoBuyerPriceOffer]: 'No Buyer Price Offer',
  [ImportPriceOfferType.AddMoreItems]: 'Add More Items',
  [ImportPriceOfferType.UpdateItemProperties]: 'Change Item Properties',
};

export type ImportPriceOfferTypeOption = {
  label: string;
  value: ImportPriceOfferType;
  requiredPolicy?: string;
};

export type PriceOfferSummaryData = {
  totalMEVNOfferAmount?: number;
  totalStandardAmount?: number;
  totalRequestedAmount?: number;
  totalPriceToCustomer?: number;
  totalRequestedDiscountAmount?: number;
  totalLandedCost?: number;
  totalGP?: number;
  discountRatio?: number;
};

export const PriceOfferTypes = {
  Project: 'PP',
  KeyAccount: 'AP',
  Distributor: 'DS',
  NoBuyer: 'NB',
};
