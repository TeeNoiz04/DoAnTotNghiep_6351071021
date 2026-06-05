import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import {
  PagedAndSortedHelper,
  PagedAndSortedParamMap,
  ParamConfig,
  QueryParamConverter,
} from '@app/shared/helpers/filter-helper';
import { GetSystemCategoriesInput } from '@proxy/system-categories';

export class SystemCategoryFilterHelper {
  private static paramMap: Record<keyof GetSystemCategoriesInput, ParamConfig> = {
    filterText: { param: 'filter_text', type: 'string' },
    parentId: { param: 'parent_id', type: 'string' },
    code: { param: 'code_s', type: 'string' },
    description: { param: 'description', type: 'string' },
    valueMin: { param: 'value_min', type: 'number' },
    valueMax: { param: 'value_max', type: 'number' },
    categoryType: { param: 'category_type', type: 'string' },
    note: { param: 'note', type: 'string' },
    isDeactive: { param: 'is_deactive', type: 'boolean' },
    sortOrderMin: { param: 'sort_order_min', type: 'number' },
    sortOrderMax: { param: 'sort_order_max', type: 'number' },
    // Adding paged properties
    ...PagedAndSortedParamMap,
  };

  static fromQueryParams(params: Record<string, any>): GetSystemCategoriesInput {
    // Initialize with default maxResultCount
    const model: GetSystemCategoriesInput = { maxResultCount: DEFAULT_PAGE_SIZE };

    // Handle paging and sorting first
    PagedAndSortedHelper.fromQueryParams(params, model);

    // Handle the rest of the properties with type conversion
    return QueryParamConverter.fromQueryParams(params, this.paramMap, model);
  }

  static toQueryParams(model: GetSystemCategoriesInput): Record<string, any> {
    // First get the paged and sorted parameters
    const queryParams = PagedAndSortedHelper.toQueryParams(model);

    // Then add the rest of the parameters with proper type conversion
    const specificParams = QueryParamConverter.toQueryParams(model, this.paramMap);

    // Merge all parameters
    return { ...queryParams, ...specificParams };
  }
}
