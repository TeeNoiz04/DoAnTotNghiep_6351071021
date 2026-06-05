import type {
  ApprovalDashboardItemDto,
  DPOStatusSummaryDto,
  GICStatusSummaryDto,
  GKRStatusSummaryDto,
  SaleResultBaseDto,
  SaleResultBuyerDto,
  SaleResultMaterialGroupDto,
  SaleResultPODto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetDPOsInput } from '../dpos/models';
import type { GetGICsInput } from '../gics/models';
import type { GetGKRsInput } from '../gkrs/models';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  apiName = 'Default';

  getApprovalDashboard = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApprovalDashboardItemDto[]>(
      {
        method: 'GET',
        url: '/api/app/dashboard/approval-dashboard',
      },
      { apiName: this.apiName, ...config },
    );

  getDPOStatusSummary = (input: GetDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPOStatusSummaryDto>(
      {
        method: 'GET',
        url: '/api/app/dashboard/dpo-status-summary',
        params: {
          filterText: input.filterText,
          dpoNo: input.dpoNo,
          materialCode: input.materialCode,
          modelName: input.modelName,
          poNo: input.poNo,
          customerName: input.customerName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          buyerId: input.buyerId,
          materialType: input.materialType,
          supplierId: input.supplierId,
          specialPriceCode: input.specialPriceCode,
          materialGroup: input.materialGroup,
          taxCode: input.taxCode,
          salesOrg: input.salesOrg,
          dpoType: input.dpoType,
          dpoSubType: input.dpoSubType,
          costCenter: input.costCenter,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerShortName: input.buyerShortName,
          buyerDescription: input.buyerDescription,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          fileName: input.fileName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getGICStatusSummary = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICStatusSummaryDto>(
      {
        method: 'GET',
        url: '/api/app/dashboard/gic-status-summary',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getGKRStatusSummary = (input: GetGKRsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRStatusSummaryDto>(
      {
        method: 'GET',
        url: '/api/app/dashboard/gkr-status-summary',
        params: {
          gkrNo: input.gkrNo,
          materialType: input.materialType,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          linkedDPONo: input.linkedDPONo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          costCenter: input.costCenter,
          materialGroup: input.materialGroup,
          materialCode: input.materialCode,
          modelName: input.modelName,
          specialPriceCode: input.specialPriceCode,
          customerName: input.customerName,
          customerTaxCode: input.customerTaxCode,
          poNo: input.poNo,
          supplierCode: input.supplierCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  poResult = (fy: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleResultPODto[]>(
      {
        method: 'GET',
        url: '/api/app/dashboard/po-result',
        params: { fy },
      },
      { apiName: this.apiName, ...config },
    );

  saleResultBase = (fy: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleResultBaseDto[]>(
      {
        method: 'GET',
        url: '/api/app/dashboard/sale-result-base',
        params: { fy },
      },
      { apiName: this.apiName, ...config },
    );

  saleResultByBuyer = (fy: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleResultBuyerDto[]>(
      {
        method: 'GET',
        url: '/api/app/dashboard/sale-result-by-buyer',
        params: { fy },
      },
      { apiName: this.apiName, ...config },
    );

  saleResultByMaterialGroup = (fy: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleResultMaterialGroupDto[]>(
      {
        method: 'GET',
        url: '/api/app/dashboard/sale-result-by-material-group',
        params: { fy },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
