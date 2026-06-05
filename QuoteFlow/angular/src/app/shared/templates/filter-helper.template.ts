import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { ParamConfig, PagedAndSortedParamMap } from '@app/shared/helpers/filter-helper';
// TODO: Import your filter input type
// import { GetYourModuleInput } from '@proxy/your-module';

/**
 * Template for creating new filter helpers
 * Replace 'YourModule' with your actual module name
 * Replace 'GetYourModuleInput' with your actual input type
 */
export class YourModuleFilterHelper extends BaseFilterHelper<GetYourModuleInput> {
  private static instance: YourModuleFilterHelper;

  protected get paramMap(): Record<keyof GetYourModuleInput, ParamConfig> {
    return {
      // TODO: Define your parameter mappings here
      // Example configurations:

      // String filters
      filterText: { param: 'filter_text', type: 'string' },
      code: { param: 'code', type: 'string' },
      name: { param: 'name', type: 'string' },

      // Number filters with validation
      minAmount: {
        param: 'min_amount',
        type: 'number',
        validation: { min: 0 },
      },
      maxAmount: {
        param: 'max_amount',
        type: 'number',
        validation: { min: 0 },
      },

      // Date filters
      dateFrom: { param: 'date_from', type: 'date', format: 'YYYY-MM-DD' },
      dateTo: { param: 'date_to', type: 'date', format: 'YYYY-MM-DD' },

      // Boolean filters
      isActive: { param: 'is_active', type: 'boolean', defaultValue: true },

      // Array filters
      statusList: { param: 'status_list', type: 'array' },

      // Required filters
      categoryId: {
        param: 'category_id',
        type: 'string',
        validation: { required: true },
      },

      // String filters with validation
      description: {
        param: 'description',
        type: 'string',
        validation: {
          maxLength: 500,
          pattern: /^[a-zA-Z0-9\s]*$/, // Only alphanumeric and spaces
        },
      },

      // Adding paged properties (always include these)
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetYourModuleInput> {
    return {
      // TODO: Define your default values
      maxResultCount: 50,
      skipCount: 0,
      isActive: true,
      // Add other default values as needed
    };
  }

  /**
   * Get singleton instance
   */
  public static getInstance(): YourModuleFilterHelper {
    if (!this.instance) {
      this.instance = new YourModuleFilterHelper();
    }
    return this.instance;
  }

  /**
   * Static method for backward compatibility
   */
  static fromQueryParams(params: Record<string, any>): GetYourModuleInput {
    return this.getInstance().fromQueryParams(params);
  }

  /**
   * Static method for backward compatibility
   */
  static toQueryParams(model: GetYourModuleInput): Record<string, any> {
    return this.getInstance().toQueryParams(model);
  }

  /**
   * Custom validation for your module filters
   * Override this method to add custom validation logic
   */
  protected validateModel(model: GetYourModuleInput): { isValid: boolean; errors: string[] } {
    const baseValidation = super.validateModel(model);
    const errors = [...baseValidation.errors];

    // TODO: Add your custom validation logic here

    // Example: Date range validation
    if (model.dateFrom && model.dateTo) {
      const fromDate = new Date(model.dateFrom);
      const toDate = new Date(model.dateTo);

      if (fromDate > toDate) {
        errors.push('dateFrom must be before dateTo');
      }
    }

    // Example: Amount range validation
    if (model.minAmount && model.maxAmount) {
      if (model.minAmount > model.maxAmount) {
        errors.push('minAmount must be less than or equal to maxAmount');
      }
    }

    // Example: Required field validation
    if (!model.categoryId) {
      errors.push('categoryId is required');
    }

    // Example: String length validation
    if (model.code && model.code.length > 20) {
      errors.push('code must be 20 characters or less');
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }

  /**
   * Post-process from query params
   * Override this method to add custom processing after converting from query params
   */
  protected postProcessFromQueryParams(
    model: GetYourModuleInput,
    params: Record<string, any>,
  ): GetYourModuleInput {
    // TODO: Add your post-processing logic here

    // Example: Convert date strings to proper format
    if (model.dateFrom && typeof model.dateFrom === 'string') {
      // Already a string, no conversion needed
    }
    if (model.dateTo && typeof model.dateTo === 'string') {
      // Already a string, no conversion needed
    }

    return model;
  }

  /**
   * Pre-process to query params
   * Override this method to add custom processing before converting to query params
   */
  protected preProcessToQueryParams(model: GetYourModuleInput): GetYourModuleInput {
    // TODO: Add your pre-processing logic here

    // Example: Trim string values
    if (model.name) {
      model.name = model.name.trim();
    }
    if (model.description) {
      model.description = model.description.trim();
    }

    return model;
  }
}

/**
 * USAGE INSTRUCTIONS:
 *
 * 1. Copy this file to your module's services folder
 * 2. Rename the file to match your module (e.g., product-filter-helper.ts)
 * 3. Replace 'YourModule' with your actual module name
 * 4. Replace 'GetYourModuleInput' with your actual input type
 * 5. Import your actual input type from the proxy
 * 6. Define your parameter mappings in the paramMap getter
 * 7. Set your default values in the defaultValues getter
 * 8. Add custom validation logic in the validateModel method
 * 9. Add custom processing logic in the post/pre-process methods if needed
 * 10. Export the helper class from your module's index.ts
 *
 * PARAMETER MAPPING GUIDELINES:
 *
 * - Use snake_case for URL parameters (e.g., 'filter_text', 'date_from')
 * - Use descriptive but concise parameter names
 * - Add validation rules for better user experience
 * - Include default values for boolean and number fields
 * - Use appropriate types: 'string', 'number', 'boolean', 'date', 'array'
 * - For dates, specify format (e.g., 'YYYY-MM-DD')
 * - Always include the PagedAndSortedParamMap spread
 *
 * VALIDATION GUIDELINES:
 *
 * - Add min/max validation for numbers
 * - Add required validation for mandatory fields
 * - Add pattern validation for formatted strings
 * - Add custom validation functions for complex rules
 * - Validate date ranges and number ranges
 * - Keep error messages user-friendly
 *
 * EXAMPLE USAGE IN SERVICE:
 *
 * ```typescript
 * export class YourModuleService extends BaseFilterService<GetYourModuleInput, YourModuleListDto> {
 *   protected getFilterHelper(): BaseFilterHelper<GetYourModuleInput> {
 *     return YourModuleFilterHelper.getInstance();
 *   }
 * }
 * ```
 */
