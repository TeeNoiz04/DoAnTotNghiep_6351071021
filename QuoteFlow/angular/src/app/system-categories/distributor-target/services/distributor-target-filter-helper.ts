import {
  PagedAndSortedHelper,
  PagedAndSortedParamMap,
  ParamConfig,
  QueryParamConverter,
} from '@app/shared/helpers/filter-helper';
import { GetSalesAssignmentInput } from '@proxy/sales-assignments';

export class BuyerCategoryFilterHelper {
  private static paramMap: Record<keyof GetSalesAssignmentInput, ParamConfig> = {
    filterText: { param: 'filter_text', type: 'string' },
    buyerId: { param: 'buyer_id', type: 'string' },
    locationId: { param: 'location_id', type: 'string' },
    saleUserName: { param: 'name', type: 'string' },
    materialType: { param: 'material_type', type: 'string' },
    buyerTypeId: { param: 'buyer_type_id', type: 'string' },
    // Adding paged properties
    ...PagedAndSortedParamMap,
  };

  static fromQueryParams(params: Record<string, any>): GetSalesAssignmentInput {
    // Initialize with default maxResultCount
    const model: GetSalesAssignmentInput = { maxResultCount: 50 };

    // Handle paging and sorting first
    PagedAndSortedHelper.fromQueryParams(params, model);

    // Handle the rest of the properties with type conversion
    return QueryParamConverter.fromQueryParams(params, this.paramMap, model);
  }

  static toQueryParams(model: GetSalesAssignmentInput): Record<string, any> {
    // First get the paged and sorted parameters
    const queryParams = PagedAndSortedHelper.toQueryParams(model);

    // Then add the rest of the parameters with proper type conversion
    const specificParams = QueryParamConverter.toQueryParams(model, this.paramMap);

    // Merge all parameters
    return { ...queryParams, ...specificParams };
  }
}
