import { ABP, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import { SpecialInputPriceService } from '@app/proxy/special-input-prices';
import {
  GetSpecialInputPricesInput,
  SpecialInputPriceDto,
} from '@app/proxy/special-input-prices/models';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { Observable } from 'rxjs';
import { SpecialInputPriceFilterHelper } from './special-input-price-filter-helper';

@Injectable()
export class SpecialInputPriceFilterService extends BaseFilterService<
  GetSpecialInputPricesInput,
  SpecialInputPriceDto,
  SpecialInputPriceDto
> {
  protected readonly proxyService = inject(SpecialInputPriceService);

  protected getFilterHelper(): BaseFilterHelper<GetSpecialInputPricesInput> {
    return SpecialInputPriceFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetSpecialInputPricesInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      filterText: '',
      accountNo: '',
      accountName: '',
      materials: [],
      models: [],
      projectName: '',
      validFromMin: '',
      validFromMax: '',
      validToMin: '',
      validToMax: '',
      status: '',
      note: '',
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      accountNo: [''],
      accountName: [''],
      status: [''],
      materialCodes: [''],
      modelNames: [''],
      supplier: [null],
      validDate: [''],
    };
  }

  protected getListData(
    query: ABP.PageQueryParams & GetSpecialInputPricesInput,
  ): Observable<PagedResultDto<SpecialInputPriceDto>> {
    return this.proxyService.getList(query);
  }

  protected mapFormToFilters(formValue: any): GetSpecialInputPricesInput {
    return {
      ...this.filters,
      accountNo: formValue.accountNo || '',
      accountName: formValue.accountName || '',
      status: formValue.status || '',
      materials: formValue.materialCodes || '',
      models: formValue.modelNames || '',
      // Map supplier and validDate to appropriate fields when available
      validFromMin: formValue.validDate || '',
      validFromMax: formValue.validDate || '',
    };
  }

  protected deleteItem(item: SpecialInputPriceDto): Observable<any> {
    return this.proxyService.delete(item.id);
  }

  protected getExportData(filters: GetSpecialInputPricesInput): Observable<Blob> {
    return this.proxyService.getInputPriceAsExcel(filters);
  }

  /**
   * Map filters back to form values for form synchronization
   */
  protected mapFiltersToForm(filters: GetSpecialInputPricesInput): any {
    return {
      accountNo: filters.accountNo || '',
      accountName: filters.accountName || '',
      materials: filters.materials || '',
      models: filters.models || '',
      supplier: null, // Not implemented yet
      validDate: filters.validFromMin || '',
    };
  }
}
