import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
// TODO: Import your filter service and types
// import { YourModuleFilterService } from '../services/your-module-filter.service';
// import { GetYourModuleInput, YourModuleListDto, YourModuleDto } from '@proxy/your-module';

/**
 * Template for creating new filter components
 * Replace 'YourModule' with your actual module name
 * Replace types with your actual types
 */
@Component({
  selector: 'app-your-module-filter',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    // TODO: Add other imports as needed
  ],
  templateUrl: './your-module-filter.component.html',
  styles: [
    `
      .filter-container {
        background-color: #f8f9fa;
        padding: 1rem;
        border-radius: 0.375rem;
        margin-bottom: 1rem;
      }

      .form-label {
        font-weight: 500;
        margin-bottom: 0.5rem;
      }

      .badge {
        font-size: 0.75rem;
      }

      .spin {
        animation: spin 2s linear infinite;
      }

      @keyframes spin {
        0% {
          transform: rotate(0deg);
        }
        100% {
          transform: rotate(360deg);
        }
      }

      .alert {
        margin-top: 1rem;
      }

      .form-check {
        padding-top: 0.5rem;
      }

      .btn-group {
        gap: 0.5rem;
      }

      .is-invalid {
        border-color: #dc3545;
      }

      .invalid-feedback {
        display: block;
        font-size: 0.875em;
        color: #dc3545;
      }
    `,
  ],
})
export class YourModuleFilterComponent
  extends BaseFilterComponent<GetYourModuleInput, YourModuleListDto, YourModuleDto>
  implements OnInit
{
  // Service injection
  protected readonly filterService = inject(YourModuleFilterService);

  // Dropdown data
  categories: any[] = [];
  statusOptions: any[] = [];

  // Loading states
  isLoadingCategories = false;
  isLoadingStatuses = false;

  ngOnInit(): void {
    super.ngOnInit();
    this.loadDropdownData();
  }

  /**
   * Load dropdown data
   */
  protected loadDropdownData(): void {
    this.loadCategories();
    this.loadStatusOptions();
  }

  /**
   * Load categories dropdown
   */
  private loadCategories(): void {
    this.isLoadingCategories = true;
    // TODO: Implement category loading
    /*
    this.filterService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
        this.isLoadingCategories = false;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
        this.isLoadingCategories = false;
      }
    });
    */

    // Mock data for template
    this.categories = [
      { id: '1', name: 'Category 1' },
      { id: '2', name: 'Category 2' },
      { id: '3', name: 'Category 3' },
    ];
    this.isLoadingCategories = false;
  }

  /**
   * Load status options
   */
  private loadStatusOptions(): void {
    this.isLoadingStatuses = true;
    // TODO: Implement status loading
    /*
    this.filterService.getStatusOptions().subscribe({
      next: (statuses) => {
        this.statusOptions = statuses;
        this.isLoadingStatuses = false;
      },
      error: (error) => {
        console.error('Error loading statuses:', error);
        this.isLoadingStatuses = false;
      }
    });
    */

    // Mock data for template
    this.statusOptions = [
      { value: 'active', label: 'Active' },
      { value: 'inactive', label: 'Inactive' },
      { value: 'pending', label: 'Pending' },
    ];
    this.isLoadingStatuses = false;
  }

  /**
   * Map form values to filter object
   */
  protected mapFormToFilters(formValue: any): GetYourModuleInput {
    return {
      // TODO: Map form values to filter properties
      ...this.filters,
      filterText: formValue.filterText || '',
      code: formValue.code || '',
      name: formValue.name || '',
      status: formValue.status || '',
      categoryId: formValue.categoryId || '',
      dateFrom: formValue.dateFrom || '',
      dateTo: formValue.dateTo || '',
      minAmount: formValue.minAmount || null,
      maxAmount: formValue.maxAmount || null,
      isActive: formValue.isActive !== undefined ? formValue.isActive : true,
      statusList: formValue.statusList || [],
    };
  }

  /**
   * Handle form value changes
   */
  protected onFormChanged(formValue: any): void {
    super.onFormChanged(formValue);
    // TODO: Add custom form change handling if needed
  }

  /**
   * Before search validation
   */
  protected beforeSearch(): boolean {
    // TODO: Add custom validation before search

    // Example: Date range validation
    const dateFrom = this.getFormControlValue('dateFrom');
    const dateTo = this.getFormControlValue('dateTo');

    if (dateFrom && dateTo && new Date(dateFrom) > new Date(dateTo)) {
      // Show error or handle validation
      console.warn('Date from must be before date to');
      return false;
    }

    return super.beforeSearch();
  }

  /**
   * After search actions
   */
  protected afterSearch(): void {
    super.afterSearch();
    // TODO: Add custom post-search actions
  }

  /**
   * Before clear validation
   */
  protected beforeClear(): boolean {
    // TODO: Add custom validation before clear
    return super.beforeClear();
  }

  /**
   * After clear actions
   */
  protected afterClear(): void {
    super.afterClear();
    // TODO: Add custom post-clear actions
  }

  // TODO: Add any custom methods specific to your component

  /**
   * Example: Reset specific filter
   */
  public resetDateFilters(): void {
    this.setFormControlValue('dateFrom', '');
    this.setFormControlValue('dateTo', '');
  }

  /**
   * Example: Reset amount filters
   */
  public resetAmountFilters(): void {
    this.setFormControlValue('minAmount', null);
    this.setFormControlValue('maxAmount', null);
  }

  /**
   * Example: Set common date ranges
   */
  public setDateRange(range: 'today' | 'week' | 'month' | 'year'): void {
    const today = new Date();
    const formatDate = (date: Date) => date.toISOString().split('T')[0];

    switch (range) {
      case 'today':
        this.setFormControlValue('dateFrom', formatDate(today));
        this.setFormControlValue('dateTo', formatDate(today));
        break;
      case 'week': {
        const weekStart = new Date(today.setDate(today.getDate() - today.getDay()));
        this.setFormControlValue('dateFrom', formatDate(weekStart));
        this.setFormControlValue('dateTo', formatDate(new Date()));
        break;
      }
      case 'month': {
        const monthStart = new Date(today.getFullYear(), today.getMonth(), 1);
        this.setFormControlValue('dateFrom', formatDate(monthStart));
        this.setFormControlValue('dateTo', formatDate(new Date()));
        break;
      }
      case 'year': {
        const yearStart = new Date(today.getFullYear(), 0, 1);
        this.setFormControlValue('dateFrom', formatDate(yearStart));
        this.setFormControlValue('dateTo', formatDate(new Date()));
        break;
      }
    }
  }
}

/**
 * USAGE INSTRUCTIONS:
 *
 * 1. Copy this file to your module's components folder
 * 2. Rename the file to match your module (e.g., product-filter.component.ts)
 * 3. Replace 'YourModule' with your actual module name
 * 4. Replace all type references with your actual types
 * 5. Import your actual services and types
 * 6. Update the template with your actual form fields
 * 7. Update the component selector
 * 8. Implement dropdown data loading methods
 * 9. Customize validation and form handling
 * 10. Add any custom methods specific to your component
 * 11. Update localization keys
 * 12. Test and adjust styling as needed
 *
 * TEMPLATE CUSTOMIZATION:
 *
 * - Replace form fields with your actual filter fields
 * - Update localization keys to match your module
 * - Add/remove form controls as needed
 * - Customize validation display
 * - Adjust styling and layout
 * - Add custom buttons or actions
 *
 * FORM FIELDS GUIDELINES:
 *
 * - Use appropriate input types (text, number, date, select, checkbox)
 * - Add proper validation classes and error display
 * - Use consistent spacing and layout
 * - Include placeholders and labels
 * - Add loading states for dropdowns
 * - Handle multi-select fields properly
 *
 * STYLING GUIDELINES:
 *
 * - Use Bootstrap classes for consistency
 * - Add custom CSS only when necessary
 * - Maintain responsive design
 * - Use consistent colors and spacing
 * - Add hover and focus states
 * - Include loading animations
 *
 * EXAMPLE USAGE IN PARENT COMPONENT:
 *
 * ```html
 * <app-your-module-filter
 *   [showAdvancedFilters]="false"
 *   [enableAutoSearch]="false"
 *   [debounceTime]="300"
 *   (searched)="onFiltersApplied($event)"
 *   (cleared)="onFiltersCleared()"
 * ></app-your-module-filter>
 * ```
 */
