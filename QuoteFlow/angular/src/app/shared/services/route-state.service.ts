import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

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
    const params = this.route.snapshot.queryParams;
    const result = { ...defaults };

    Object.keys(defaults).forEach(key => {
      if (params[key] !== undefined) {
        const defaultValue = defaults[key as keyof T];

        if (typeof defaultValue === 'number') {
          result[key as keyof T] = Number(params[key]) as any;
        } else if (typeof defaultValue === 'boolean') {
          result[key as keyof T] = (params[key] === 'true') as any;
        } else {
          result[key as keyof T] = params[key] as any;
        }
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
}
