import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ValidationResult } from '@app/shared/helpers/filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { Observable, Subject, takeUntil } from 'rxjs';
/**
 * Abstract base component for filter components with standardized behavior
 */
@Component({
  template: '', // Must be implemented by concrete components
})
export abstract class BaseFilterComponent<
    TFilter extends { maxResultCount: number; skipCount?: number; sorting?: string },
    TListItem,
    TDetailItem = TListItem,
  >
  implements OnInit, OnDestroy
{
  protected readonly destroy$ = new Subject<void>();

  @Input() showAdvancedFilters: boolean = false;
  @Input() enableAutoSearch: boolean = false;
  @Input() debounceTime: number = 300;
  @Input() showClearButton: boolean = true;
  @Input() showSearchButton: boolean = true;
  @Input() disabled: boolean = false;

  @Output() filtersChanged = new EventEmitter<TFilter>();
  @Output() searched = new EventEmitter<TFilter>();
  @Output() cleared = new EventEmitter<void>();
  @Output() advancedToggled = new EventEmitter<boolean>();

  // Service injection - must be provided by concrete components
  protected abstract get filterService(): BaseFilterService<TFilter, TListItem, TDetailItem>;

  // Form reference
  get searchForm(): FormGroup | undefined {
    return this.filterService.searchForm;
  }

  // Observable properties
  get isLoading$(): Observable<boolean> {
    return this.filterService.isLoading$;
  }

  get hasActiveFilters$(): Observable<boolean> {
    return this.filterService.hasActiveFilters$;
  }

  get validationErrors$(): Observable<ValidationResult[]> {
    return this.filterService.validationErrors$;
  }

  // Current filters
  get filters(): TFilter {
    return this.filterService.filters;
  }

  ngOnInit(): void {
    this.initializeComponent();
    this.setupFormSubscriptions();
    this.loadInitialData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Initialize component with service setup
   */
  protected initializeComponent(): void {
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: this.enableAutoSearch,
      debounceTime: this.debounceTime,
    });
  }

  /**
   * Setup form subscriptions for reactive behavior
   */
  protected setupFormSubscriptions(): void {
    if (this.searchForm) {
      // Subscribe to form changes
      this.searchForm.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(formValue => {
        this.onFormChanged(formValue);
      });

      // Subscribe to form status changes
      this.searchForm.statusChanges.pipe(takeUntil(this.destroy$)).subscribe(status => {
        this.onFormStatusChanged(status);
      });
    }
  }

  /**
   * Load initial data (dropdowns, lookups, etc.)
   * Override in concrete components
   */
  protected loadInitialData(): void {
    // Default implementation - override as needed
  }

  /**
   * Handle form value changes
   */
  protected onFormChanged(formValue: any): void {
    const filters = this.mapFormToFilters(formValue);
    this.filtersChanged.emit(filters);
  }

  /**
   * Handle form status changes
   */
  protected onFormStatusChanged(status: string): void {
    // Default implementation - override as needed
  }

  /**
   * Execute search with current form values
   */

  public onSearch(): void {
    if (this.disabled) return;

    this.filterService.filters.skipCount = 0;
    this.filterService.list.page = 0;
    this.filterService.search();
    this.searched.emit(this.filters);
  }

  /**
   * Update pagination and execute search
   * Called by ngx-datatable page change events
   */
  public updatePageThenSearch(page: { offset: number; pageSize: number }): void {
    if (this.disabled) return;

    // Update filter pagination properties

    this.filterService.filters.skipCount = page.offset * page.pageSize;
    this.filterService.filters.maxResultCount = page.pageSize;

    // Execute search with updated pagination
    this.filterService.search();
    this.searched.emit(this.filters);
  }

  /**
   * Clear all filters
   */
  public onClear(): void {
    if (this.disabled) return;

    this.filterService.clearFilters();
    this.cleared.emit();
  }

  /**
   * Toggle advanced filters display
   */
  public onToggleAdvanced(): void {
    this.showAdvancedFilters = !this.showAdvancedFilters;
    this.advancedToggled.emit(this.showAdvancedFilters);
  }

  /**
   * Reset specific form control
   */
  public resetFormControl(controlName: string): void {
    if (this.searchForm && this.searchForm.get(controlName)) {
      this.searchForm.get(controlName)?.reset();
    }
  }

  /**
   * Enable/disable specific form control
   */
  public setFormControlEnabled(controlName: string, enabled: boolean): void {
    if (this.searchForm && this.searchForm.get(controlName)) {
      const control = this.searchForm.get(controlName);
      if (enabled) {
        control?.enable();
      } else {
        control?.disable();
      }
    }
  }

  /**
   * Get form control value
   */
  public getFormControlValue(controlName: string): any {
    return this.searchForm?.get(controlName)?.value;
  }

  /**
   * Set form control value
   */
  public setFormControlValue(controlName: string, value: any): void {
    if (this.searchForm && this.searchForm.get(controlName)) {
      this.searchForm.get(controlName)?.setValue(value);
    }
  }

  /**
   * Check if form control has error
   */
  public hasFormControlError(controlName: string): boolean {
    const control = this.searchForm?.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  /**
   * Get form control error message
   */
  public getFormControlError(controlName: string): string | null {
    const control = this.searchForm?.get(controlName);
    if (control && control.errors) {
      const errors = control.errors;

      if (errors['required']) return 'This field is required';
      if (errors['min']) return `Value must be at least ${errors['min'].min}`;
      if (errors['max']) return `Value must be at most ${errors['max'].max}`;
      if (errors['pattern']) return 'Invalid format';
      if (errors['email']) return 'Invalid email address';
      if (errors['minlength']) return `Must be at least ${errors['minlength'].requiredLength} characters`;
      if (errors['maxlength']) return `Must be at most ${errors['maxlength'].requiredLength} characters`;

      // Return first error key as fallback
      return Object.keys(errors)[0];
    }

    return null;
  }

  /**
   * Check if any filter is active
   */
  public hasActiveFilters(): boolean {
    return this.filterService.hasActiveFilters();
  }

  /**
   * Get current validation errors
   */
  public getValidationErrors(): ValidationResult[] {
    return this.filterService.getValidationResults();
  }

  /**
   * Check if component is in loading state
   */
  public isLoading(): boolean {
    return this.filterService.isLoading$.value;
  }

  /**
   * Abstract method to map form values to filter object
   * Must be implemented by concrete components
   */
  protected abstract mapFormToFilters(formValue: any): TFilter;

  /**
   * Optional method to handle dropdown data loading
   * Override in concrete components that need dropdown data
   */
  protected loadDropdownData(): void {
    // Default implementation - override as needed
  }

  /**
   * Optional method to handle form initialization
   * Override in concrete components for custom form setup
   */
  protected initializeForm(): void {
    // Default implementation - override as needed
  }

  /**
   * Optional method to handle custom validation
   * Override in concrete components for custom validation logic
   */
  protected validateForm(): boolean {
    return this.searchForm ? this.searchForm.valid : true;
  }

  /**
   * Optional method to handle pre-search operations
   * Override in concrete components for pre-search logic
   */
  protected beforeSearch(): boolean {
    return true; // Return false to prevent search
  }

  /**
   * Optional method to handle post-search operations
   * Override in concrete components for post-search logic
   */
  protected afterSearch(): void {
    // Default implementation - override as needed
  }

  /**
   * Optional method to handle pre-clear operations
   * Override in concrete components for pre-clear logic
   */
  protected beforeClear(): boolean {
    return true; // Return false to prevent clear
  }

  /**
   * Optional method to handle post-clear operations
   * Override in concrete components for post-clear logic
   */
  protected afterClear(): void {
    // Default implementation - override as needed
  }

  /**
   * Utility method to format date for display
   */
  protected formatDate(date: Date | string, format: string = 'yyyy-MM-dd'): string {
    if (!date) return '';

    const dateObj = typeof date === 'string' ? new Date(date) : date;
    if (isNaN(dateObj.getTime())) return '';

    const year = dateObj.getFullYear();
    const month = String(dateObj.getMonth() + 1).padStart(2, '0');
    const day = String(dateObj.getDate()).padStart(2, '0');

    switch (format) {
      case 'yyyy-MM-dd':
        return `${year}-${month}-${day}`;
      case 'dd/MM/yyyy':
        return `${day}/${month}/${year}`;
      case 'MM/dd/yyyy':
        return `${month}/${day}/${year}`;
      default:
        return `${year}-${month}-${day}`;
    }
  }

  /**
   * Utility method to parse date from string
   */
  protected parseDate(dateString: string): Date | null {
    if (!dateString) return null;

    const date = new Date(dateString);
    return isNaN(date.getTime()) ? null : date;
  }
}
