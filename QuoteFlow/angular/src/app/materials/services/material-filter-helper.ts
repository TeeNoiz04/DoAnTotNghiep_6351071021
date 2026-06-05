import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetMaterialsApprovalInput, GetMaterialsInput } from '@proxy/materials';

// Union type for all material filter inputs
export type MaterialFilterInput = GetMaterialsInput | GetMaterialsApprovalInput;

// Helper type to get all possible keys from union
type AllKeys<T> = T extends any ? keyof T : never;
type MaterialFilterKeys = AllKeys<MaterialFilterInput>;

export class MaterialFilterHelper extends BaseFilterHelper<MaterialFilterInput> {
  private static instance: MaterialFilterHelper;
  private filterType: 'management' | 'import' | 'approvals' = 'management';

  protected get paramMap(): Record<MaterialFilterKeys, ParamConfig> {
    return {
      // Common filters
      golfaCode: { param: 'golfa_cod', type: 'string' },
      model: { param: 'model', type: 'string' },

      // Material management specific filters
      sapCode: { param: 'sap_cod', type: 'string' },
      materialType: { param: 'material_type', type: 'string' },
      materialGroup: { param: 'material_group', type: 'string' },
      supplier: { param: 'supplier', type: 'string' },
      supplierBUId: { param: 'supplier_bu_id', type: 'string' },
      supplierBU: { param: 'supplier_bu', type: 'string' },
      materialStatus: { param: 'material_status', type: 'string' },
      stockQty: { param: 'stock_qty', type: 'number', validation: { min: 0 } },
      onOrderStock: { param: 'on_order_stock', type: 'number', validation: { min: 0 } },
      // isDeactive: { param: 'is_deactive', type: 'boolean', defaultValue: false },
      stockId: { param: 'stock_id', type: 'string' },

      // Approval specific filters
      importType: { param: 'import_type', type: 'string' },
      approvalStatus: { param: 'approval_status', type: 'string' },

      // Adding paged properties
      ...PagedAndSortedParamMap,
    } as Record<MaterialFilterKeys, ParamConfig>;
  }

  protected get defaultValues(): Partial<MaterialFilterInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
      materialStatus: 'Active',
    };
  }

  /**
   * Set filter type to determine which fields are relevant
   */
  setFilterType(filterType: 'management' | 'import' | 'approvals') {
    this.filterType = filterType;
  }

  /**
   * Get fields relevant to current filter type
   */
  getRelevantFields(): MaterialFilterKeys[] {
    const commonFields: MaterialFilterKeys[] = [
      'golfaCode',
      'model',
      'maxResultCount',
      'skipCount',
      'sorting',
    ];

    switch (this.filterType) {
      case 'management':
        return [
          ...commonFields,
          'sapCode',
          'materialType',
          'materialGroup',
          'supplier',
          'supplierBUId',
          'supplierBU',
          'materialStatus',
          'stockQty',
          'onOrderStock',
          // 'isDeactive',
          'stockId',
        ];
      case 'import':
      case 'approvals':
        return [...commonFields, 'importType', 'approvalStatus'];
      default:
        return commonFields;
    }
  }

  /**
   * Get default filters for specific filter type
   */
  getDefaultFiltersForType(filterType: 'management' | 'import' | 'approvals'): MaterialFilterInput {
    const baseDefaults = {
      maxResultCount: 50,
      skipCount: 0,
      golfaCode: '',
      model: '',
    };

    switch (filterType) {
      case 'management':
        return {
          ...baseDefaults,
          sapCode: '',
          materialType: '',
          materialGroup: '',
          supplier: '',
          supplierBUId: '',
          supplierBU: '',
          materialStatus: '',
          stockQty: undefined,
          onOrderStock: undefined,
          // isDeactive: false,
          stockId: '',
        } as GetMaterialsInput;
      case 'import':
      case 'approvals':
        return {
          ...baseDefaults,
          importType: '',
          approvalStatus: '',
        } as GetMaterialsApprovalInput;
      default:
        return baseDefaults as MaterialFilterInput;
    }
  }

  /**
   * Get singleton instance
   */
  public static getInstance(): MaterialFilterHelper {
    if (!this.instance) {
      this.instance = new MaterialFilterHelper();
    }
    return this.instance;
  }

  /**
   * Static method for backward compatibility
   */
  static fromQueryParams(params: Record<string, any>): MaterialFilterInput {
    return this.getInstance().fromQueryParams(params);
  }

  /**
   * Static method for backward compatibility
   */
  static toQueryParams(model: MaterialFilterInput): Record<string, any> {
    return this.getInstance().toQueryParams(model);
  }

  /**
   * Custom validation for material filters
   */
  protected validateModel(model: MaterialFilterInput): { isValid: boolean; errors: string[] } {
    const baseValidation = super.validateModel(model);
    const errors = [...baseValidation.errors];

    // Golfa code validation
    if ('golfaCode' in model && model.golfaCode && model.golfaCode.length > 50) {
      errors.push('golfaCode must be 50 characters or less');
    }

    // Model validation
    if ('model' in model && model.model && model.model.length > 100) {
      errors.push('model must be 100 characters or less');
    }

    // Material management specific validations
    if ('sapCode' in model && model.sapCode && model.sapCode.length > 50) {
      errors.push('sapCode must be 50 characters or less');
    }

    if ('stockQty' in model && model.stockQty !== undefined && model.stockQty < 0) {
      errors.push('stockQty must be 0 or greater');
    }

    if ('onOrderStock' in model && model.onOrderStock !== undefined && model.onOrderStock < 0) {
      errors.push('onOrderStock must be 0 or greater');
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }
}
