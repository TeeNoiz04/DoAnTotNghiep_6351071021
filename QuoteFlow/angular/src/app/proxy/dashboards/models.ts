export interface ApprovalDashboardItemDto {
  title?: string;
  in_Approval: number;
  setProjectResult: number;
}

export interface DPOStatusSummaryDto {
  submitted: number;
  confirmed: number;
  lockedStock: number;
  cancelled: number;
  closed: number;
  inProgress: number;
}

export interface GICStatusSummaryDto {
  confirmed: number;
  lockedStock: number;
  cancelled: number;
  inProgress: number;
  closed: number;
}

export interface GKRStatusSummaryDto {
  submitted: number;
  confirmed: number;
  lockedStock: number;
  cancelled: number;
  closed: number;
  inProgress: number;
}

export interface SaleResultBaseDto {
  buyerCode?: string;
  materialType?: string;
  saleResult?: number;
  saleTarget?: number;
}

export interface SaleResultBuyerDto {
  buyerCode?: string;
  materialType?: string;
  saleResult?: number;
}

export interface SaleResultMaterialGroupDto {
  materialType?: string;
  material_Group?: string;
  saleResult?: number;
}

export interface SaleResultPODto {
  materialType?: string;
  material_Group?: string;
  plan?: string;
  saleResult?: number;
}
