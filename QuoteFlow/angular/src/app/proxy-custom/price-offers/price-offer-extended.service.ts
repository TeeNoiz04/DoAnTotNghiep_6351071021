import { Rest, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import {
  ImportAddMoreItemsInput,
  PriceOfferAPImportInput,
  PriceOfferDSImportInput,
  PriceOfferImportDto,
  PriceOfferImportInput,
  PriceOfferNBImportInput,
  PriceOfferService,
} from '@proxy/price-offers';
import { PriceOfferDetailDto } from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Injectable({
  providedIn: 'root',
})
export class PriceOfferExtendedService extends PriceOfferService {
  constructor(private myRestService: RestService) {
    super(myRestService);
  }

  validateAndParsePPManual(
    file: File,
    input: PriceOfferImportInput,
    config?: Partial<Rest.Config>,
  ) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('buyerId', input.buyerId);
    formData.append('buyerTypeId', input.buyerTypeId);
    formData.append('locationId', input.locationId);
    formData.append('materialType', input.materialType);
    formData.append('projectName', input.projectName);
    formData.append('salePIC', input.salePIC);
    formData.append('closeDate', input.closeDate);
    formData.append('note', input.note ?? '');

    return this.myRestService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/pp',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  validateAndParseAPManual(
    file: File,
    input: PriceOfferAPImportInput,
    config?: Partial<Rest.Config>,
  ) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('getPriceAutomatically', input.getPriceAutomatically.toString());
    formData.append('buyerId', input.buyerId);
    formData.append('buyerTypeId', input.buyerTypeId);
    formData.append('locationId', input.locationId);
    formData.append('materialType', input.materialType);
    formData.append('projectName', input.projectName);
    formData.append('keyAccountId', input.keyAccountId);
    formData.append('keyAccountClassId', input.keyAccountClassId);
    formData.append('keyAccountTypeId', input.keyAccountTypeId);
    formData.append('salePIC', input.salePIC);
    formData.append('note', input.note ?? '');

    return this.myRestService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/ap',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  validateAndParseDSManual(
    file: File,
    input: PriceOfferDSImportInput,
    config?: Partial<Rest.Config>,
  ) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('buyerId', input.buyerId);
    formData.append('buyerTypeId', input.buyerTypeId);
    formData.append('locationId', input.locationId);
    formData.append('materialType', input.materialType);
    formData.append('projectName', input.projectName);
    formData.append('salePIC', input.salePIC);
    formData.append('note', input.note ?? '');

    return this.myRestService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/ds',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  validateAndParseNBManual(
    file: File,
    input: PriceOfferNBImportInput,
    config?: Partial<Rest.Config>,
  ) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('locationId', input.locationId);
    formData.append('materialType', input.materialType);
    formData.append('projectName', input.projectName);
    formData.append('salePIC', input.salePIC);
    formData.append('closeDate', input.closeDate);
    formData.append('note', input.note ?? '');

    return this.myRestService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/nb',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  importAddMoreItemDetailWithFormData = (
    priceOfferId: string,
    input: ImportAddMoreItemsInput,
    config?: Partial<Rest.Config>,
  ) => {
    const formData = new FormData();
    formData.append('validationResult', JSON.stringify(input.validationResult));

    // Append comment
    if (input.comment) {
      formData.append('comment', input.comment);
    }

    return this.myRestService.request<any, PriceOfferDetailDto[]>(
      {
        method: 'POST',
        url: `/api/app/price-offers/import/${priceOfferId}/add-more-items`,
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  };
}
