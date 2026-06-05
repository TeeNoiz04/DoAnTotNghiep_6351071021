import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import {
  PagedAndSortedHelper,
  PagedAndSortedParamMap,
  ParamConfig,
  QueryParamConverter,
} from './filter-helper';

/**
 * Abstract base class for filter helpers that provides standardized
 * URL query parameter handling with type safety and validation
 */
export abstract class BaseFilterHelper<T extends PagedAndSortedResultRequestDto> {
  /**
   * Parameter mapping configuration for the filter model
   * Must be implemented by concrete classes
   */
  protected abstract get paramMap(): Record<keyof T, ParamConfig>;

  /**
   * Default values for the filter model
   * Can be overridden by concrete classes
   */
  protected get defaultValues(): Partial<T> {
    return {
      maxResultCount: 50,
      skipCount: 0,
    } as Partial<T>;
  }

  /**
   * Convert query parameters to filter model with type safety and validation
   * @param params Query parameters from URL
   * @returns Typed filter model with default values
   */
  fromQueryParams(params: Record<string, any>): T {
    // Start with default values
    const model = { ...this.defaultValues } as T;

    // Handle paging and sorting first
    PagedAndSortedHelper.fromQueryParams(params, model);

    // Handle the rest of the properties with type conversion
    const result = QueryParamConverter.fromQueryParams(params, this.paramMap, model);

    // Apply post-processing if needed
    return this.postProcessFromQueryParams(result, params);
  }

  /**
   * Convert filter model to query parameters with type safety
   * @param model Filter model to convert
   * @returns Query parameters object for URL
   */
  toQueryParams(model: T): Record<string, any> {
    // Apply pre-processing if needed
    const processedModel = this.preProcessToQueryParams(model);

    // Get paged and sorted parameters
    const queryParams = PagedAndSortedHelper.toQueryParams(processedModel);

    // Add the rest of the parameters with proper type conversion
    const specificParams = QueryParamConverter.toQueryParams(processedModel, this.paramMap);

    // Merge all parameters
    const result = { ...queryParams, ...specificParams };

    // Apply post-processing if needed
    return this.postProcessToQueryParams(result, model);
  }

  /**
   * Validate filter model values
   * Can be overridden by concrete classes for custom validation
   * @param model Filter model to validate
   * @returns Validation result with any errors
   */
  protected validateModel(model: T): { isValid: boolean; errors: string[] } {
    const errors: string[] = [];

    // Basic validation
    if (model.maxResultCount && (model.maxResultCount < 1 || model.maxResultCount > 1000)) {
      errors.push('maxResultCount must be between 1 and 1000');
    }

    if (model.skipCount && model.skipCount < 0) {
      errors.push('skipCount must be non-negative');
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }

  /**
   * Post-process the model after converting from query parameters
   * Can be overridden by concrete classes for custom processing
   * @param model The converted model
   * @param params Original query parameters
   * @returns Processed model
   */
  protected postProcessFromQueryParams(model: T, params: Record<string, any>): T {
    return model;
  }

  /**
   * Pre-process the model before converting to query parameters
   * Can be overridden by concrete classes for custom processing
   * @param model The model to convert
   * @returns Processed model
   */
  protected preProcessToQueryParams(model: T): T {
    return model;
  }

  /**
   * Post-process the query parameters after conversion
   * Can be overridden by concrete classes for custom processing
   * @param params The converted query parameters
   * @param model Original model
   * @returns Processed query parameters
   */
  protected postProcessToQueryParams(params: Record<string, any>, model: T): Record<string, any> {
    return params;
  }

  /**
   * Get all parameter names used by this filter helper
   * Useful for debugging and URL cleanup
   */
  getParameterNames(): string[] {
    const standardParams = Object.values(PagedAndSortedParamMap).map(config => config.param);
    const specificParams = Object.values(this.paramMap).map(config => config.param);
    return [...standardParams, ...specificParams];
  }

  /**
   * Clear all filter parameters from query parameters object
   * @param params Query parameters object
   * @returns Clean query parameters without filter parameters
   */
  clearFilterParams(params: Record<string, any>): Record<string, any> {
    const cleanParams = { ...params };
    const filterParams = this.getParameterNames();

    filterParams.forEach(param => {
      delete cleanParams[param];
    });

    return cleanParams;
  }

  /**
   * Check if the model has any active filters (non-default values)
   * @param model Filter model to check
   * @returns True if model has active filters
   */
  hasActiveFilters(model: T): boolean {
    const defaults = this.defaultValues;

    return Object.keys(this.paramMap).some(key => {
      const modelKey = key as keyof T;
      const value = model[modelKey];
      const defaultValue = defaults[modelKey];

      // Consider empty strings as inactive filters
      if (typeof value === 'string' && value.trim() === '') {
        return false;
      }

      // For boolean values, check if they're different from default
      if (typeof value === 'boolean' && typeof defaultValue === 'boolean') {
        return value !== defaultValue;
      }

      // For other types, check if they're not null/undefined and different from default
      return value != null && value !== defaultValue;
    });
  }

  /**
   * Reset model to default values
   * @returns Model with default values
   */
  getDefaultModel(): T {
    return { ...this.defaultValues } as T;
  }

  /**
   * Merge two filter models, with the second model taking precedence
   * @param base Base model
   * @param override Override model
   * @returns Merged model
   */
  mergeModels(base: T, override: Partial<T>): T {
    return { ...base, ...override };
  }

  /**
   * Create a deep copy of the filter model
   * @param model Model to copy
   * @returns Deep copy of the model
   */
  cloneModel(model: T): T {
    return JSON.parse(JSON.stringify(model));
  }
}
