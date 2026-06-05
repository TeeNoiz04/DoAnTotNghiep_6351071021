import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetPriceOffersInput } from '@proxy/price-offers';

export class PriceOfferFilterHelper extends BaseFilterHelper<GetPriceOffersInput> {
  private static instance: PriceOfferFilterHelper;

  protected get paramMap(): Record<keyof GetPriceOffersInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      priceOfferType: { param: 'price_offer_type', type: 'string' },
      materialType: { param: 'material_type', type: 'string' },
      buyerId: { param: 'buyer_id', type: 'string' },
      priceOfferCode: { param: 'price_offer_cod', type: 'string' },
      customerTaxCode: { param: 'customer_tax_cod', type: 'string' },
      customerName: { param: 'customer_name', type: 'string' },
      approvalStatus: { param: 'approval_status', type: 'string' },
      createdFrom: { param: 'created_from', type: 'string' },
      createdTo: { param: 'created_to', type: 'string' },
      relatedToMe: { param: 'related_to_me', type: 'boolean', defaultValue: false },
      projectName: { param: 'project_name', type: 'string' },
      projectResultStatus: { param: 'project_result_status', type: 'string' },
      // Adding paged properties
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetPriceOffersInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
      relatedToMe: false,
    };
  }

  /**
   * Get singleton instance
   */
  public static getInstance(): PriceOfferFilterHelper {
    if (!this.instance) {
      this.instance = new PriceOfferFilterHelper();
    }
    return this.instance;
  }

  /**
   * Static method for backward compatibility
   */
  static fromQueryParams(params: Record<string, any>): GetPriceOffersInput {
    return this.getInstance().fromQueryParams(params);
  }

  /**
   * Static method for backward compatibility
   */
  static toQueryParams(model: GetPriceOffersInput): Record<string, any> {
    return this.getInstance().toQueryParams(model);
  }

  /**
   * Post-process from query params to handle date conversions
   */
  protected postProcessFromQueryParams(
    model: GetPriceOffersInput,
    params: Record<string, any>,
  ): GetPriceOffersInput {
    // Convert date strings to proper format if needed
    if (model.createdFrom && typeof model.createdFrom === 'string') {
      // Already a string, no conversion needed
    }
    if (model.createdTo && typeof model.createdTo === 'string') {
      // Already a string, no conversion needed
    }

    return model;
  }

  /**
   * Custom validation for price offer filters
   */
  protected validateModel(model: GetPriceOffersInput): { isValid: boolean; errors: string[] } {
    const baseValidation = super.validateModel(model);
    const errors = [...baseValidation.errors];

    // Date range validation
    if (model.createdFrom && model.createdTo) {
      const fromDate = new Date(model.createdFrom);
      const toDate = new Date(model.createdTo);

      if (fromDate > toDate) {
        errors.push('createdFrom must be before createdTo');
      }
    }

    // Price offer code validation
    if (model.priceOfferCode && model.priceOfferCode.length > 50) {
      errors.push('priceOfferCode must be 50 characters or less');
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }
}
