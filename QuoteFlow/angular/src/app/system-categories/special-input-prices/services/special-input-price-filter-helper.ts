import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetSpecialInputPricesInput } from '@proxy/special-input-prices';

export class SpecialInputPriceFilterHelper extends BaseFilterHelper<GetSpecialInputPricesInput> {
  private static instance: SpecialInputPriceFilterHelper;

  protected get paramMap(): Record<keyof GetSpecialInputPricesInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      accountNo: { param: 'account_no', type: 'string' },
      accountName: { param: 'account_name', type: 'string' },
      materials: { param: 'materials', type: 'string' },
      models: { param: 'models', type: 'string' },
      // supplierId: { param: 'supplier_id', type: 'string' },
      projectName: { param: 'project_name', type: 'string' },
      validFromMin: { param: 'valid_from_min', type: 'date', format: 'YYYY-MM-DD' },
      validFromMax: { param: 'valid_from_max', type: 'date', format: 'YYYY-MM-DD' },
      validToMin: { param: 'valid_to_min', type: 'date', format: 'YYYY-MM-DD' },
      validToMax: { param: 'valid_to_max', type: 'date', format: 'YYYY-MM-DD' },
      status: { param: 'status', type: 'string' },
      note: { param: 'note', type: 'string' },
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetSpecialInputPricesInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
    };
  }

  /**
   * Get singleton instance
   */
  public static getInstance(): SpecialInputPriceFilterHelper {
    if (!this.instance) {
      this.instance = new SpecialInputPriceFilterHelper();
    }
    return this.instance;
  }

  /**
   * Static method for backward compatibility
   */
  static fromQueryParams(params: Record<string, any>): GetSpecialInputPricesInput {
    return this.getInstance().fromQueryParams(params);
  }

  /**
   * Static method for backward compatibility
   */
  static toQueryParams(model: GetSpecialInputPricesInput): Record<string, any> {
    return this.getInstance().toQueryParams(model);
  }

  /**
   * Custom validation for Special Input Price filters
   */
  protected validateModel(model: GetSpecialInputPricesInput): {
    isValid: boolean;
    errors: string[];
  } {
    const baseValidation = super.validateModel(model);
    const errors = [...baseValidation.errors];

    // Valid From date range validation
    if (model.validFromMin && model.validFromMax) {
      const fromDate = new Date(model.validFromMin);
      const toDate = new Date(model.validFromMax);

      if (fromDate > toDate) {
        errors.push('validFromMin must be before validFromMax');
      }
    }

    // Valid To date range validation
    if (model.validToMin && model.validToMax) {
      const fromDate = new Date(model.validToMin);
      const toDate = new Date(model.validToMax);

      if (fromDate > toDate) {
        errors.push('validToMin must be before validToMax');
      }
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }
}
