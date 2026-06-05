import { GetBuyersInput } from '@proxy/buyers';

export class BuyerCategoryFilterHelper {
  // private static paramMap: Record<keyof GetBuyersInput, ParamConfig> = {
  //   // filterText: { param: 'filter_text', type: 'string' },
  //   // taxCode: { param: 'code_d', type: 'string' }, // Restored 'd_cod'
  //   // shortName: { param: 'name', type: 'string' },
  //   // address: { param: 'address', type: 'string' },
  //   // contactEmail: { param: 'contact_info', type: 'string' },

  //   // note: { param: 'note', type: 'string' },
  //   // deactive: { param: 'is_deactive', type: 'boolean' },
  //   // // Adding paged properties
  //   // ...PagedAndSortedParamMap,
  // };

  static fromQueryParams(params: Record<string, any>) {
    // Initialize with default maxResultCount
    // const model: GetBuyersInput = { maxResultCount: 50 };
    // // Handle paging and sorting first
    // PagedAndSortedHelper.fromQueryParams(params, model);
    // // Handle the rest of the properties with type conversion
    // return QueryParamConverter.fromQueryParams(params, this.paramMap, model);
  }

  static toQueryParams(model: GetBuyersInput) {
    // // First get the paged and sorted parameters
    // const queryParams = PagedAndSortedHelper.toQueryParams(model);
    // // Then add the rest of the parameters with proper type conversion
    // const specificParams = QueryParamConverter.toQueryParams(model, this.paramMap);
    // // Merge all parameters
    // return { ...queryParams, ...specificParams };
  }
}
