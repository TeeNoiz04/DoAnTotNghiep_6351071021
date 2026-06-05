import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetBuyersInput } from '@proxy/buyers';

export class BuyerFilterHelper extends BaseFilterHelper<GetBuyersInput> {
  private static instance: BuyerFilterHelper;

  protected get paramMap(): Record<keyof GetBuyersInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      buyerTypeId: { param: 'buyer_type_id', type: 'string' },
      buyerCode: { param: 'buyer_cod', type: 'string' },
      shortName: { param: 'short_name', type: 'string' },
      fullName: { param: 'full_name', type: 'string' },
      taxCode: { param: 'tax_cod', type: 'string' },
      address: { param: 'address', type: 'string' },
      contactPerson: { param: 'contact_person', type: 'string' },
      contactEmail: { param: 'contact_email', type: 'string' },
      contactPhoneNumber: { param: 'contact_phone_number', type: 'string' },
      paymentTermCode: { param: 'payment_term_cod', type: 'string' },
      paymentTermDescription: { param: 'payment_term_description', type: 'string' },
      creditLimitMin: { param: 'credit_limit', type: 'number' },
      creditLimitMax: { param: 'credit_limit_max', type: 'number' },
      creditExposureMin: { param: 'credit_exposure', type: 'number' },
      creditExposureMax: { param: 'credit_exposure_max', type: 'number' },
      appliedPrice: { param: 'applied_price', type: 'boolean' },
      deactive: { param: 'deactive', type: 'boolean', defaultValue: false },
      note: { param: 'note', type: 'string' },
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetBuyersInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
      deactive: false,
    };
  }

  public static getInstance(): BuyerFilterHelper {
    if (!this.instance) {
      this.instance = new BuyerFilterHelper();
    }
    return this.instance;
  }
}
