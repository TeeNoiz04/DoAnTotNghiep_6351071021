import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { PagedAndSortedParamMap, ParamConfig } from '@app/shared/helpers/filter-helper';
import { GetDPOsInput } from '@proxy/dpos';

export class DpoFilterHelper extends BaseFilterHelper<GetDPOsInput> {
  private static instance: DpoFilterHelper;

  protected get paramMap(): Record<keyof GetDPOsInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      dpoNo: { param: 'dpo_no', type: 'string' },
      materialCode: { param: 'material_cod', type: 'string' },
      modelName: { param: 'model_name', type: 'string' },
      poNo: { param: 'po_no', type: 'string' },
      customerName: { param: 'cus_name', type: 'string' },
      orderDateMin: { param: 'order_date_min', type: 'string' },
      orderDateMax: { param: 'order_date_max', type: 'string' },
      buyerId: { param: 'buyer_id', type: 'string' },
      materialType: { param: 'material_type', type: 'string' },
      supplierId: { param: 'supplier_id', type: 'string' },
      specialPriceCode: { param: 'special_price_cod', type: 'string' },
      materialGroup: { param: 'material_group', type: 'string' },
      taxCode: { param: 'tax_cod', type: 'string' },
      salesOrg: { param: 'sales_org', type: 'string' },
      status: { param: 'status', type: 'string' },
      buyerTypeId: { param: 'buyer_type_id', type: 'string' },
      dpoType: { param: 'dpo_type', type: 'string' },
      dpoSubType: { param: 'dpo_sub_type', type: 'string' },
      costCenter: { param: 'cost_center', type: 'string' },
      buyerShortName: { param: 'buyer_short_name', type: 'string' },
      buyerDescription: { param: 'buyer_desc', type: 'string' },
      totalAmountMin: {
        param: 'total_amt_min',
        type: 'number',
        validation: { min: 0 },
      },
      totalAmountMax: {
        param: 'total_amt_max',
        type: 'number',
        validation: { min: 0 },
      },
      remark: { param: 'remark', type: 'string' },
      fileName: { param: 'file_name', type: 'string' },
      // Adding paged properties
      ...PagedAndSortedParamMap,
    };
  }

  protected get defaultValues(): Partial<GetDPOsInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
    };
  }

  /**
   * Get singleton instance
   */
  public static getInstance(): DpoFilterHelper {
    if (!this.instance) {
      this.instance = new DpoFilterHelper();
    }
    return this.instance;
  }

  /**
   * Static method for backward compatibility
   */
  static fromQueryParams(params: Record<string, any>): GetDPOsInput {
    return this.getInstance().fromQueryParams(params);
  }

  /**
   * Static method for backward compatibility
   */
  static toQueryParams(model: GetDPOsInput): Record<string, any> {
    return this.getInstance().toQueryParams(model);
  }

  /**
   * Custom validation for DPO filters
   */
  protected validateModel(model: GetDPOsInput): { isValid: boolean; errors: string[] } {
    const baseValidation = super.validateModel(model);
    const errors = [...baseValidation.errors];

    // Date range validation
    if (model.orderDateMin && model.orderDateMax) {
      const fromDate = new Date(model.orderDateMin);
      const toDate = new Date(model.orderDateMax);

      if (fromDate > toDate) {
        errors.push('orderDateMin must be before orderDateMax');
      }
    }

    // Amount range validation
    if (model.totalAmountMin && model.totalAmountMax) {
      if (model.totalAmountMin > model.totalAmountMax) {
        errors.push('totalAmountMin must be less than or equal to totalAmountMax');
      }
    }

    // DPO number validation
    if (model.dpoNo && model.dpoNo.length > 50) {
      errors.push('dpoNo must be 50 characters or less');
    }

    return {
      isValid: errors.length === 0,
      errors,
    };
  }
}
