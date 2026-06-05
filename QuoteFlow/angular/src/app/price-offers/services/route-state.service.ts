import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RouteStateService {
  constructor(
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  saveFilters<T extends object>(
    filters: T,
    previousValues?: Partial<T>,
    preserveParams: string[] = [],
    explicitlyIncludeEmpty: boolean = false,
  ): void {
    const currentParams = this.route.snapshot.queryParams;
    const hasDeletedValues =
      previousValues &&
      Object.entries(previousValues).some(
        ([key, prevValue]) =>
          prevValue !== null &&
          prevValue !== undefined &&
          prevValue !== '' &&
          (filters[key as keyof T] === null ||
            filters[key as keyof T] === undefined ||
            filters[key as keyof T] === ''),
      );
    const shouldReplace = hasDeletedValues;
    const clearedKeys = Object.keys(currentParams).filter(
      key =>
        key in filters &&
        (filters[key as keyof T] === null ||
          filters[key as keyof T] === undefined ||
          filters[key as keyof T] === ''),
    );

    const queryParams = Object.entries(filters)
      .filter(
        ([key, value]) =>
          preserveParams.includes(key) ||
          (value !== null && value !== undefined && value !== '') ||
          (explicitlyIncludeEmpty && (value === null || value === '')) ||
          clearedKeys.includes(key),
      )
      .reduce(
        (obj, [key, value]) => {
          if (value === null || value === undefined || value === '') {
            obj[key] = '';
          } else if (typeof value === 'boolean') {
            obj[key] = value.toString();
          } else if (value instanceof Date) {
            obj[key] = value.toISOString();
          } else {
            obj[key] = value;
          }
          return obj;
        },
        {} as Record<string, any>,
      );

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: shouldReplace ? undefined : 'merge',
      replaceUrl: true,
    });
  }

  getFilters<T extends object>(defaults: T): T {
    // 1. Get query parameters from the active route snapshot
    const params: Params = this.route.snapshot.queryParams;

    // 2. Determine if a search has been performed
    // Check if the query parameter object is entirely empty
    const isFirstLoad = Object.keys(params).length === 0;

    // 3. Initialize the result object
    let result: any;

    if (isFirstLoad) {
      // SCENARIO A: True First Load (No Params)
      // Use all default values.
      result = { ...defaults };
    } else {
      // SCENARIO B: Search Performed (One or more params exist)
      // Initialize with an empty object.
      // We will explicitly set properties to 'undefined' if they are missing
      // from the URL but present in defaults. This prevents defaults from overriding.
      result = {};
    }

    // 4. Iterate over the expected keys (from the defaults object)

    Object.keys(defaults).forEach(key => {
      const keyOfT = key as keyof T;
      const paramValue = params[key];

      if (isFirstLoad) {
        // Defaults were already copied in step 3. No action needed.
        return;
      }

      // --- Logic for SCENARIO B (Search Performed) ---

      // Check if the parameter is explicitly present in the URL (even if it's an empty string for a cleared filter, e.g., ?status=)
      if (paramValue !== undefined) {
        const defaultValue = defaults[keyOfT];

        // Perform type conversion based on the type of the default value
        if (typeof defaultValue === 'number') {
          const numValue = Number(paramValue);
          result[keyOfT] = (!isNaN(numValue) ? numValue : undefined) as any;
        } else if (typeof defaultValue === 'boolean') {
          result[keyOfT] = (paramValue === 'true') as any;
        } else {
          // This covers strings, and correctly assigns "" for a cleared filter (?status=)
          result[keyOfT] = paramValue as any;
        }
      } else {
        // The key is defined in 'defaults' but is *missing* from the URL.
        // If we've searched before (isFirstLoad is false), this missing key
        // must represent a cleared or unselected filter.
        // We set it to undefined/null instead of the default value.
        result[keyOfT] = undefined as any; // Or null, depending on your preferred 'cleared' value
      }
    });

    return result;
  }

  clearFilters(): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {},
      replaceUrl: true,
    });
  }

  hasSavedFilters(): boolean {
    const params: Params = this.route.snapshot.queryParams;
    return Object.keys(params).length > 0;
  }
}
