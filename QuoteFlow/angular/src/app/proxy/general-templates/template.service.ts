import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TemplateService {
  apiName = 'Default';

  getBatchRequestImportTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/batch-request',
      },
      { apiName: this.apiName, ...config },
    );

  getCargoTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/cargo-template',
      },
      { apiName: this.apiName, ...config },
    );

  getDPOTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/dpo-template',
      },
      { apiName: this.apiName, ...config },
    );

  getGICTemplate = (gicType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/gic-template',
        params: { gicType },
      },
      { apiName: this.apiName, ...config },
    );

  getGKRTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/gkr-template',
      },
      { apiName: this.apiName, ...config },
    );

  getPOTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/po-template',
      },
      { apiName: this.apiName, ...config },
    );

  getPriceOfferAddMoreItemsTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/price-offers/add-more-items-template',
      },
      { apiName: this.apiName, ...config },
    );

  getPriceOfferChangeItemPropertiesTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/price-offers/change-item-properties-template',
      },
      { apiName: this.apiName, ...config },
    );

  getPriceOfferTemplate = (templateType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/price-offers/import-template',
        params: { templateType },
      },
      { apiName: this.apiName, ...config },
    );

  getSOTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/so-template',
      },
      { apiName: this.apiName, ...config },
    );

  getSpecialInputPriceTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/special-input-price',
      },
      { apiName: this.apiName, ...config },
    );

  getStockImportPriorityTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-import-priority-template',
      },
      { apiName: this.apiName, ...config },
    );

  getStockImportTemplate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-import-template',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateMaterial = (typeTemplate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/material',
        params: { typeTemplate },
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateMaterialNewRegistration = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/material-new-registration',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateMaterialUpdateInventoryPlan = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/material-update-inventory-plan',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateMaterialUpdatePrice = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/material-update-price',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateMaterialUpdateWithoutPrice = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/material-update-without-price',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplatePSI = (typeTemplate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/psi-template',
        params: { typeTemplate },
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateStockManagement = (typeTemplate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-management-template',
        params: { typeTemplate },
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateStockTracingDelivery = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-tracing-delivery',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateStockTracingInventory = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-tracing-inventory',
      },
      { apiName: this.apiName, ...config },
    );

  getTemplateStockTracingReceipt = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/templates/stock-tracing-receipt',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
