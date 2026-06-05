import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetCustomersInput } from '@proxy/customers';

export class CustomerFilterHelper extends BaseFilterHelper<GetCustomersInput> {
  private static instance: CustomerFilterHelper;

  protected get paramMap(): Record<keyof GetCustomersInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      taxCode: { param: 'tax_cod', type: 'string' },
      customerName: { param: 'cust_name', type: 'string' },
      address: { param: 'addr', type: 'string' },
      phone: { param: 'phone', type: 'string' },
      province: { param: 'province', type: 'string' },
      website: { param: 'website', type: 'string' },
      isDeactive: { param: 'is_deactive', type: 'boolean?', defaultValue: false },
      customerShortName: { param: 'cust_short_name', type: 'string' },
      customerType: { param: 'cust_type', type: 'string' },
      customerIndustry: { param: 'cust_industry', type: 'string' },
      fromDate: { param: 'from_date', type: 'string' },
      toDate: { param: 'to_date', type: 'string' },
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetCustomersInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
      isDeactive: false,
    };
  }

  public static getInstance(): CustomerFilterHelper {
    if (!this.instance) {
      this.instance = new CustomerFilterHelper();
    }
    return this.instance;
  }
}
