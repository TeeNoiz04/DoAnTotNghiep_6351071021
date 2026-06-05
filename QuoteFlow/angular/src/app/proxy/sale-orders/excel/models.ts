export interface SaleOrderExcelDto {
  soNo?: string;
  sapsoNo?: string;
  sapdoNo?: string;
  sapBillingNo?: string;
  sapinv?: string;
  sapinvDate?: string;
}

export interface SaleOrderGICFOCExcelDto {
  materialCode?: string;
  modelName?: string;
  soType?: string;
  soNo?: string;
  sapsoNo?: string;
  gicsapLandingCost?: number;
  soQty?: number;
  gicAmountSAPLandingCost?: number;
  gicporNo?: string;
  gicprNo?: string;
  gicGivNo?: string;
  invoiceNo?: string;
  invoiceDate?: string;
  note?: string;
  gicNo?: string;
  billingNo?: string;
  doNo?: string;
  changeNote?: string;
}

export interface SaleOrderGICInternalUseChangeExcelDto {
  materialCode?: string;
  modelName?: string;
  soType?: string;
  soNo?: string;
  gicsapLandingCost?: number;
  gicAmountSAPLandingCost?: number;
  gicporNo?: string;
  gicprNo?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  gicSalesPIC?: string;
  gicLocation?: string;
  gicReservationNo?: string;
  note?: string;
  gicNo?: string;
}

export interface SaleOrderGICInternalUseExcelDto {
  materialCode?: string;
  modelName?: string;
  soType?: string;
  soNo?: string;
  gicsapLandingCost?: number;
  gicAmountSAPLandingCost?: number;
  gicporNo?: string;
  gicprNo?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  gicSalesPIC?: string;
  gicLocation?: string;
  gicReservationNo?: string;
  gicAssetClass?: string;
  gicMainAssetCode?: string;
  gicSubAssetCode?: string;
  gicAssetName?: string;
  note?: string;
  gicNo?: string;
  gicProcess?: string;
}

export interface SaleOrderGICWarrantyExcelDto {
  soType?: string;
  soNo?: string;
  sapsoNo?: string;
  materialCode?: string;
  modelName?: string;
  invoiceNo?: string;
  invoiceDate?: string;
  gicsapLandingCost?: number;
  soQty?: number;
  gicAmountSAPLandingCost?: number;
  gicporNo?: string;
  gicprNo?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  note?: string;
  gicNo?: string;
  doNo?: string;
  billingNo?: string;
}

export interface SaleOrderGICWriteOffExcelDto {
  soType?: string;
  gicwoNo?: string;
  gicwoDate?: string;
  costCenter?: string;
  materialType?: string;
  no?: string;
  sapCode?: string;
  materialCode?: string;
  modelName?: string;
  spec1?: string;
  soNo?: string;
  soQty?: number;
  note?: string;
  sapLandingCost?: number;
  amountInSAPLandingCost?: number;
  porNo?: string;
  prNo?: string;
  givNo?: string;
  givDate?: string;
  disposed?: string;
}
