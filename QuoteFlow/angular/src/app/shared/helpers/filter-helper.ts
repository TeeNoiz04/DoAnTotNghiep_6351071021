import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { DEFAULT_PAGE_SIZE } from '../constants';

export class PagedAndSortedHelper {
  static fromQueryParams<T extends PagedAndSortedResultRequestDto>(
    params: Record<string, any>,
    model: T,
    keys = {
      skip: 'skip_count',
      //   take: 'max',
      sort: 'sort_by',
    },
  ): T {
    if (params[keys.skip]) model.skipCount = +params[keys.skip];
    // if (params[keys.take]) model.maxResultCount = +params[keys.take];
    else model.maxResultCount = DEFAULT_PAGE_SIZE;
    if (params[keys.sort]) model.sorting = params[keys.sort];
    return model;
  }

  static toQueryParams<T extends PagedAndSortedResultRequestDto>(
    model: T,
    keys = {
      skip: 'skip_count',
      //   take: 'max',
      sort: 'sort_by',
    },
  ): Record<string, any> {
    const queryParams: Record<string, any> = {};

    if (model.skipCount != null) queryParams[keys.skip] = model.skipCount;
    // if (model.maxResultCount != null) queryParams[keys.take] = model.maxResultCount;
    if (model.sorting) queryParams[keys.sort] = model.sorting;

    return queryParams;
  }
}

// Define the parameter configuration type
export interface ParamConfig {
  param: string;
  type:
    | 'boolean'
    | 'number'
    | 'date'
    | 'string'
    | 'array'
    | 'boolean?'
    | 'number?'
    | 'date?'
    | 'string?'
    | 'array?';
  format?: string;
  validation?: {
    required?: boolean;
    min?: number;
    max?: number;
    pattern?: RegExp;
    custom?: (value: any) => boolean;
  };
  defaultValue?: any;
}

export const PagedAndSortedParamMap: Record<keyof PagedAndSortedResultRequestDto, ParamConfig> = {
  maxResultCount: {
    param: 'max',
    type: 'number',
    validation: { min: 1, max: 1000 },
    defaultValue: 50,
  },
  skipCount: {
    param: 'skip_count',
    type: 'number',
    validation: { min: 0 },
    defaultValue: 0,
  },
  sorting: { param: 'sort_by', type: 'string' },
};

export interface ValidationResult {
  isValid: boolean;
  errors: string[];
  warnings: string[];
}

export class QueryParamConverter {
  /**
   * Converts a query parameter value to the appropriate type for the filter model
   * @param value The value from the query parameter
   * @param config Parameter configuration including type and validation
   * @param paramName Parameter name for error reporting
   * @returns The converted value or default value if conversion fails
   */
  static toModelValue(value: any, config: ParamConfig, paramName?: string): any {
    if (value === undefined || value === null) {
      return config.defaultValue !== undefined ? config.defaultValue : value;
    }

    try {
      let convertedValue: any;

      switch (config.type) {
        case 'boolean':
          convertedValue = value === true || value === 'true' || value === '1' || value === 1;
          break;
        case 'boolean?':
          // Handle nullable booleans - convert 'false' and false values properly
          if (value === 'false' || value === false || value === '0' || value === 0) {
            convertedValue = false;
          } else if (value === 'true' || value === true || value === '1' || value === 1) {
            convertedValue = true;
          } else {
            convertedValue = null;
          }
          break;
        case 'number':
          convertedValue = Number(value);
          if (isNaN(convertedValue)) {
            console.warn(`Invalid number value for parameter '${paramName}': ${value}`);
            return config.defaultValue !== undefined ? config.defaultValue : null;
          }
          break;
        case 'number?':
          if (value === '' || value === null || value === undefined) {
            convertedValue = null;
          } else {
            convertedValue = Number(value);
            if (isNaN(convertedValue)) {
              console.warn(`Invalid number value for parameter '${paramName}': ${value}`);
              convertedValue = null;
            }
          }
          break;
        case 'date':
          if (typeof value === 'string') {
            convertedValue = new Date(value);
            if (isNaN(convertedValue.getTime())) {
              console.warn(`Invalid date value for parameter '${paramName}': ${value}`);
              return config.defaultValue !== undefined ? config.defaultValue : null;
            }
          } else {
            convertedValue = new Date(value);
          }
          break;
        case 'date?':
          if (value === '' || value === null || value === undefined) {
            convertedValue = null;
          } else if (typeof value === 'string') {
            convertedValue = new Date(value);
            if (isNaN(convertedValue.getTime())) {
              console.warn(`Invalid date value for parameter '${paramName}': ${value}`);
              convertedValue = null;
            }
          } else {
            convertedValue = new Date(value);
            if (isNaN(convertedValue.getTime())) {
              convertedValue = null;
            }
          }
          break;
        case 'array':
          if (Array.isArray(value)) {
            convertedValue = value;
          } else if (typeof value === 'string') {
            convertedValue = value
              .split(',')
              .map(item => item.trim())
              .filter(item => item);
          } else {
            convertedValue = [value];
          }
          break;
        case 'string':
        default:
          convertedValue = String(value);
          break;
        case 'string?':
          if (value === '' || value === null || value === undefined) {
            convertedValue = null;
          } else {
            convertedValue = String(value);
          }
          break;
      }

      // Validate the converted value
      const validation = this.validateValue(convertedValue, config, paramName);
      if (!validation.isValid) {
        console.warn(`Validation failed for parameter '${paramName}':`, validation.errors);
        return config.defaultValue !== undefined ? config.defaultValue : null;
      }

      return convertedValue;
    } catch (error) {
      console.error(`Error converting parameter '${paramName}':`, error);
      return config.defaultValue !== undefined ? config.defaultValue : null;
    }
  }

  /**
   * Validate a value against parameter configuration
   * @param value The value to validate
   * @param config Parameter configuration
   * @param paramName Parameter name for error reporting
   * @returns Validation result
   */
  static validateValue(value: any, config: ParamConfig, paramName?: string): ValidationResult {
    const errors: string[] = [];
    const warnings: string[] = [];

    if (!config.validation) {
      return { isValid: true, errors, warnings };
    }

    const validation = config.validation;

    // Required validation
    if (validation.required && (value === null || value === undefined || value === '')) {
      errors.push(`Parameter '${paramName}' is required`);
    }

    // Number-specific validations
    if (config.type === 'number' && typeof value === 'number') {
      if (validation.min !== undefined && value < validation.min) {
        errors.push(`Parameter '${paramName}' must be at least ${validation.min}`);
      }
      if (validation.max !== undefined && value > validation.max) {
        errors.push(`Parameter '${paramName}' must be at most ${validation.max}`);
      }
    }

    // String-specific validations
    if (config.type === 'string' && typeof value === 'string') {
      if (validation.min !== undefined && value.length < validation.min) {
        errors.push(`Parameter '${paramName}' must be at least ${validation.min} characters`);
      }
      if (validation.max !== undefined && value.length > validation.max) {
        errors.push(`Parameter '${paramName}' must be at most ${validation.max} characters`);
      }
      if (validation.pattern && !validation.pattern.test(value)) {
        errors.push(`Parameter '${paramName}' does not match required pattern`);
      }
    }

    // Array-specific validations
    if (config.type === 'array' && Array.isArray(value)) {
      if (validation.min !== undefined && value.length < validation.min) {
        errors.push(`Parameter '${paramName}' must have at least ${validation.min} items`);
      }
      if (validation.max !== undefined && value.length > validation.max) {
        errors.push(`Parameter '${paramName}' must have at most ${validation.max} items`);
      }
    }

    // Custom validation
    if (validation.custom && typeof validation.custom === 'function') {
      try {
        if (!validation.custom(value)) {
          errors.push(`Parameter '${paramName}' failed custom validation`);
        }
      } catch (error) {
        errors.push(`Parameter '${paramName}' custom validation error: ${error.message}`);
      }
    }

    return {
      isValid: errors.length === 0,
      errors,
      warnings,
    };
  }

  /**
   * Converts a model value to a suitable query parameter value
   * @param value The value from the model
   * @param config Parameter configuration including type and format
   * @param paramName Parameter name for error reporting
   * @returns The converted value suitable for URL query parameters
   */
  static toQueryParamValue(
    value: any,
    config: ParamConfig,
    paramName?: string,
  ): string | number | boolean | null {
    if (value === undefined || value === null) {
      return null;
    }

    try {
      switch (config.type) {
        case 'boolean':
          return value === true;
        case 'boolean?':
          // Handle nullable booleans - preserve false, true, and null values
          if (value === true) return true;
          if (value === false) return false;
          return null;
        case 'number': {
          const numValue = Number(value);
          return isNaN(numValue) ? null : numValue;
        }
        case 'number?': {
          if (value === null || value === undefined) return null;
          const numValue = Number(value);
          return isNaN(numValue) ? null : numValue;
        }
        case 'date':
          if (value instanceof Date) {
            if (config.format) {
              // Use custom format if provided
              return this.formatDate(value, config.format);
            }
            return value.toISOString();
          } else if (typeof value === 'string') {
            // Try to parse and reformat
            const dateValue = new Date(value);
            if (!isNaN(dateValue.getTime())) {
              return config.format
                ? this.formatDate(dateValue, config.format)
                : dateValue.toISOString();
            }
          }
          return String(value);
        case 'date?':
          if (value === null || value === undefined) return null;
          if (value instanceof Date) {
            if (config.format) {
              return this.formatDate(value, config.format);
            }
            return value.toISOString();
          } else if (typeof value === 'string') {
            const dateValue = new Date(value);
            if (!isNaN(dateValue.getTime())) {
              return config.format
                ? this.formatDate(dateValue, config.format)
                : dateValue.toISOString();
            }
          }
          return null;
        case 'array':
          if (Array.isArray(value)) {
            return value.join(',');
          }
          return String(value);
        case 'string':
        default:
          return String(value);
        case 'string?':
          if (value === null || value === undefined) return null;
          return String(value);
      }
    } catch (error) {
      console.error(`Error converting parameter '${paramName}' to query param value:`, error);
      return null;
    }
  }

  /**
   * Format date according to specified format
   * @param date Date to format
   * @param format Format string (e.g., 'YYYY-MM-DD')
   * @returns Formatted date string
   */
  private static formatDate(date: Date, format: string): string {
    // Basic date formatting - can be expanded with more sophisticated formatting
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    switch (format) {
      case 'YYYY-MM-DD':
        return `${year}-${month}-${day}`;
      case 'MM/DD/YYYY':
        return `${month}/${day}/${year}`;
      case 'DD/MM/YYYY':
        return `${day}/${month}/${year}`;
      default:
        return date.toISOString();
    }
  }

  /**
   * Maps query parameters to a filter model with type conversions and validation
   * @param params The query parameters object
   * @param paramMap Mapping of model property names to query param names and their types
   * @param model The target model object to populate (optional, will create a new object if not provided)
   * @returns The populated model with validation results
   */
  static fromQueryParams<T>(
    params: Record<string, any>,
    paramMap: Record<keyof T, ParamConfig>,
    model: T = {} as T,
  ): T {
    Object.entries(paramMap).forEach(([modelKey, config]) => {
      const typedConfig = config as ParamConfig;
      const paramValue = params[typedConfig.param];

      if (paramValue !== undefined) {
        (model as any)[modelKey] = this.toModelValue(paramValue, typedConfig, modelKey);
      } else if (typedConfig.defaultValue !== undefined) {
        (model as any)[modelKey] = typedConfig.defaultValue;
      }
    });

    return model;
  }

  /**
   * Maps a filter model to query parameters with type conversions and validation
   * @param model The filter model
   * @param paramMap Mapping of model property names to query param names and their types
   * @returns Query parameters object
   */
  static toQueryParams<T>(model: T, paramMap: Record<keyof T, ParamConfig>): Record<string, any> {
    const queryParams: Record<string, any> = {};

    Object.entries(paramMap).forEach(([modelKey, config]) => {
      const typedConfig = config as ParamConfig;
      if (typedConfig.param === 'max' || typedConfig.param === 'sort_by') {
        // bypass
        return;
      }

      const value = (model as any)[modelKey];

      if (value !== undefined && value !== null) {
        const paramValue = this.toQueryParamValue(value, typedConfig, modelKey);

        // Only add the parameter if it's not null after conversion
        if (paramValue !== null && paramValue !== undefined) {
          // For empty strings, skip them as well
          if (typeof paramValue !== 'string' || paramValue.trim() !== '') {
            // For boolean values, only add if they're true (to keep URLs clean)
            // For nullable booleans (boolean?), include false values but skip null
            if (typedConfig.type === 'boolean' && paramValue === false) {
              return;
            }
            if (typedConfig.type === 'boolean?' && paramValue === null) {
              return;
            }

            queryParams[typedConfig.param] = paramValue;
          }
        }
      }
    });

    return queryParams;
  }

  /**
   * Get validation results for a complete model
   * @param model The filter model to validate
   * @param paramMap Parameter mapping configuration
   * @returns Array of validation results
   */
  static validateModel<T>(model: T, paramMap: Record<keyof T, ParamConfig>): ValidationResult[] {
    const results: ValidationResult[] = [];

    Object.entries(paramMap).forEach(([modelKey, config]) => {
      const typedConfig = config as ParamConfig;
      const value = (model as any)[modelKey];

      const validation = this.validateValue(value, typedConfig, modelKey);
      if (!validation.isValid || validation.warnings.length > 0) {
        results.push(validation);
      }
    });

    return results;
  }

  /**
   * Clean query parameters by removing filter-related parameters
   * @param params Query parameters object
   * @param paramMap Parameter mapping configuration
   * @returns Clean query parameters
   */
  static cleanQueryParams<T>(
    params: Record<string, any>,
    paramMap: Record<keyof T, ParamConfig>,
  ): Record<string, any> {
    const cleanParams = { ...params };

    Object.values(paramMap).forEach((config: ParamConfig) => {
      delete cleanParams[config.param];
    });

    return cleanParams;
  }
}
