import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import {
  ParamConfig,
  QueryParamConverter,
  ValidationResult,
} from '@app/shared/helpers/filter-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
import {
  BehaviorSubject,
  Observable,
  Subscription,
  debounceTime,
  distinctUntilChanged,
  filter,
  finalize,
  switchMap,
} from 'rxjs';

/**
 * Abstract base service for filter-based list views with Smart Back Button integration
 */
export abstract class BaseFilterService<
  TFilter extends { maxResultCount: number; skipCount?: number; sorting?: string },
  TListItem,
  TDetailItem = TListItem,
> {
  protected readonly confirmationService = inject(ConfirmationService);
  public readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly fb = inject(FormBuilder);
  protected readonly loadingService = inject(LoadingService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly navigationHistoryService = inject(NavigationHistoryService);

  // Data and state management
  protected currentSubscription?: Subscription;
  protected searchFormSubscription?: Subscription;
  public isExportToExcelBusy = false;
  public searchForm: FormGroup | undefined;
  public isLoading$ = new BehaviorSubject<boolean>(false);
  public hasActiveFilters$ = new BehaviorSubject<boolean>(false);
  public validationErrors$ = new BehaviorSubject<ValidationResult[]>([]);

  public data: PagedResultDto<TListItem> = {
    items: [],
    totalCount: 0,
  };

  public filters: TFilter = this.getDefaultFilters();

  /**
   * Abstract methods to be implemented by concrete services
   */
  protected abstract getFilterHelper(): BaseFilterHelper<TFilter>;
  protected abstract getDefaultFilters(): TFilter;
  protected abstract buildSearchFormControls(): { [key: string]: any };
  protected abstract getListData(
    query: ABP.PageQueryParams & TFilter,
  ): Observable<PagedResultDto<TListItem>>;
  protected abstract getExportData(filters: TFilter): Observable<Blob>;
  protected abstract deleteItem(item: TDetailItem): Observable<any>;

  /**
   * Optional methods for specialized list views
   */
  protected getApprovalListData?(
    query: ABP.PageQueryParams & TFilter,
  ): Observable<PagedResultDto<TListItem>>;

  /**
   * Initialize the service with form building and query parameter sync
   */
  public initialize(
    options: {
      buildForm?: boolean;
      syncFromQuery?: boolean;
      autoSearch?: boolean;
      debounceTime?: number;
      restoreFromHistory?: boolean;
    } = {},
  ): void {
    const {
      buildForm = true,
      syncFromQuery = true,
      autoSearch = false,
      debounceTime = 300,
      restoreFromHistory = false,
    } = options;

    if (buildForm) {
      this.buildSearchForm();
    }

    // Try to restore from history first, then sync from query params
    let restored = false;
    if (restoreFromHistory) {
      restored = this.restoreFilterStateFromHistory();
    }

    if (syncFromQuery && !restored) {
      this.syncFromQueryParams();
    }

    if (autoSearch && this.searchForm) {
      this.setupAutoSearch(debounceTime);
    }

    this.updateActiveFiltersState();
  }

  /**
   * Build the search form using the abstract method
   */
  protected buildSearchForm(): void {
    const controls = this.buildSearchFormControls();
    this.searchForm = this.fb.group(controls);
  }

  /**
   * Setup automatic search on form changes with debouncing
   */
  protected setupAutoSearch(debounceTimeMs: number = 300): void {
    if (!this.searchForm) return;

    this.searchFormSubscription?.unsubscribe();
    this.searchFormSubscription = this.searchForm.valueChanges
      .pipe(
        debounceTime(debounceTimeMs),
        distinctUntilChanged((prev, curr) => JSON.stringify(prev) === JSON.stringify(curr)),
      )
      .subscribe(() => {
        this.search();
      });
  }

  /**
   * Hook to query parameters and setup list data fetching
   */
  public hookToQuery(isApprovalView: boolean = false): void {
    this.syncFromQueryParams();

    const getData = (query: ABP.PageQueryParams) => {
      const fullQuery = { ...query, ...this.filters };

      if (isApprovalView && this.getApprovalListData) {
        return this.getApprovalListData(fullQuery);
      }

      return this.getListData(fullQuery);
    };

    const setData = (list: PagedResultDto<TListItem>) => {
      this.data = list;
      this.isLoading$.next(false);
    };

    // Clean up existing subscription
    this.currentSubscription?.unsubscribe();

    // Setup new subscription
    this.currentSubscription = this.list.hookToQuery(getData).subscribe({
      next: res => {
        setData(res);
      },
      error: () => {
        this.loadingService.hide();
        this.isLoading$.next(false);
      },
    });
  }

  /**
   * Hook to query parameters without syncing from URL (used during search)
   */
  public hookToQueryWithCurrentFilters(isApprovalView: boolean = false): void {
    const getData = (query: ABP.PageQueryParams) => {
      const fullQuery = { ...query, ...this.filters };

      if (isApprovalView && this.getApprovalListData) {
        return this.getApprovalListData(fullQuery);
      }

      return this.getListData(fullQuery);
    };

    const setData = (list: PagedResultDto<TListItem>) => {
      this.data = list;
      this.isLoading$.next(false);
    };

    // Clean up existing subscription
    this.currentSubscription?.unsubscribe();

    // Setup new subscription
    this.currentSubscription = this.list.hookToQuery(getData).subscribe({
      next: res => {
        setData(res);
      },
      error: () => {
        this.loadingService.hide();
        this.isLoading$.next(false);
      },
    });
  }

  /**
   * Sync filters from URL query parameters
   */
  public syncFromQueryParams(): void {
    const queryParams = this.route.snapshot.queryParams;
    const helper = this.getFilterHelper();

    // Check if we have any query parameters (excluding just navigation params)
    const hasFilterParams = Object.keys(queryParams).length > 0;

    if (hasFilterParams) {
      // User has searched/filtered before - use query params as-is to preserve null/empty values
      this.filters = helper.fromQueryParams(queryParams);
    } else {
      // Fresh page load - use complete default filters
      this.filters = this.getDefaultFilters();
    }

    this.list.maxResultCount = this.filters.maxResultCount || 50;
    this.list.page = Math.floor((this.filters.skipCount || 0) / this.list.maxResultCount);
    this.updateActiveFiltersState();

    // Validate the filters
    this.validateFilters();

    // Sync form if it exists
    if (this.searchForm) {
      this.syncFormWithFilters();
    }
  }

  /**
   * Update URL query parameters based on current filters
   */
  public updateQueryParams(replaceUrl = false): void {
    const helper = this.getFilterHelper();
    const queryParams = helper.toQueryParams(this.filters);

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: replaceUrl ? undefined : 'merge',
    });
  }

  /**
   * Search with current form values
   */
  public search(): void {
    if (this.searchForm) {
      this.isLoading$.next(true);

      const formValue = this.searchForm.getRawValue();
      this.filters = this.mapFormToFilters(formValue);
      this.validateFilters();
      this.updateActiveFiltersState();
      this.updateQueryParams(true);
      this.hookToQueryWithCurrentFilters();
    }
  }

  /**
   * Clear all filters and reset form
   */
  public clearFilters(): void {
    this.filters = this.getDefaultFilters();
    this.searchForm?.reset();
    this.syncFormWithFilters();
    this.updateActiveFiltersState();
    this.updateQueryParams(true);
    this.list.page = 0;
    this.list.get();
  }

  /**
   * Delete an item with confirmation
   */
  public delete(item: TDetailItem): void {
    if (!this.deleteItem) {
      console.warn('Delete functionality not implemented for this service');
      return;
    }

    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.deleteItem!(item)),
      )
      .subscribe(() => {
        this.list.get();
      });
  }

  /**
   * Export data to Excel
   */
  public exportToExcel(): void {
    if (!this.getExportData) {
      console.warn('Export functionality not implemented for this service');
      return;
    }

    this.isExportToExcelBusy = true;
    this.getExportData(this.filters)
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, this.getExportFileName());
      });
  }

  /**
   * Navigate to detail page with smart back button integration
   */
  public navigateToDetail(id: string, relativePath: string = 'details'): void {
    // Store current filter state in navigation history
    this.storeFilterStateInHistory();

    this.router.navigate([relativePath, id], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }

  /**
   * Store current filter state in navigation history
   */
  private storeFilterStateInHistory(): void {
    const currentUrl = this.router.url;
    const filterParams = this.getCurrentFilterParams();

    // Store both URL and filter state
    try {
      const historyData = {
        url: currentUrl,
        filterState: this.filters,
        filterParams: filterParams,
        timestamp: Date.now(),
      };

      sessionStorage.setItem('filter-state-history', JSON.stringify(historyData));
    } catch (error) {
      console.warn('Failed to store filter state in session storage:', error);
    }
  }

  /**
   * Restore filter state from navigation history
   */
  public restoreFilterStateFromHistory(): boolean {
    try {
      const storedData = sessionStorage.getItem('filter-state-history');
      if (storedData) {
        const historyData = JSON.parse(storedData);

        // Check if the stored data is recent (within last hour)
        const timeDiff = Date.now() - (historyData.timestamp || 0);
        if (timeDiff < 3600000) {
          // 1 hour
          this.filters = historyData.filterState || this.getDefaultFilters();
          this.updateActiveFiltersState();

          if (this.searchForm) {
            this.syncFormWithFilters();
          }

          return true;
        }
      }
    } catch (error) {
      console.warn('Failed to restore filter state from session storage:', error);
    }

    return false;
  }

  /**
   * Clear stored filter state from history
   */
  public clearStoredFilterState(): void {
    try {
      sessionStorage.removeItem('filter-state-history');
    } catch (error) {
      console.warn('Failed to clear filter state from session storage:', error);
    }
  }

  /**
   * Smart back navigation with filter state preservation
   */
  public smartBack(fallbackUrl: string | string[]): void {
    this.navigationHistoryService.smartBack(fallbackUrl);
  }

  /**
   * Get current filter state as query parameters
   */
  public getCurrentFilterParams(): Record<string, any> {
    const helper = this.getFilterHelper();
    return helper.toQueryParams(this.filters);
  }

  /**
   * Check if filters have active values
   */
  public hasActiveFilters(): boolean {
    const helper = this.getFilterHelper();
    return helper.hasActiveFilters(this.filters);
  }

  /**
   * Get validation results for current filters
   */
  public getValidationResults(): ValidationResult[] {
    return this.validationErrors$.value;
  }

  /**
   * Cleanup subscriptions
   */
  public dispose(): void {
    this.currentSubscription?.unsubscribe();
    this.searchFormSubscription?.unsubscribe();
  }

  /**
   * Abstract method to map form values to filter object
   * Must be implemented by concrete services
   */
  protected abstract mapFormToFilters(formValue: any): TFilter;

  /**
   * Optional method to sync form with current filters
   * Can be overridden by concrete services
   */
  public syncFormWithFilters(): void {
    if (!this.searchForm) return;

    // Default implementation - can be overridden
    const formValue = this.mapFiltersToForm(this.filters);
    this.searchForm.patchValue(formValue, { emitEvent: false });
  }

  /**
   * Optional method to map filters to form values
   * Can be overridden by concrete services
   */
  protected mapFiltersToForm(filters: TFilter): any {
    // Default implementation - return filters as-is
    return filters;
  }

  /**
   * Get export file name
   * Can be overridden by concrete services
   */
  protected getExportFileName(): string {
    return 'export.xlsx';
  }

  /**
   * Validate current filters
   */
  protected validateFilters(): void {
    const helper = this.getFilterHelper();
    const paramMap = (helper as any).paramMap as Record<keyof TFilter, ParamConfig>;

    if (paramMap) {
      const validationResults = QueryParamConverter.validateModel(this.filters, paramMap);
      this.validationErrors$.next(validationResults);
    }
  }

  /**
   * Update active filters state
   */
  protected updateActiveFiltersState(): void {
    this.hasActiveFilters$.next(this.hasActiveFilters());
  }

  /**
   * Get filter parameter names for cleanup
   */
  protected getFilterParameterNames(): string[] {
    const helper = this.getFilterHelper();
    return helper.getParameterNames();
  }

  /**
   * Clean query parameters from filter parameters
   */
  protected cleanQueryParams(params: Record<string, any>): Record<string, any> {
    const helper = this.getFilterHelper();
    return helper.clearFilterParams(params);
  }
}
