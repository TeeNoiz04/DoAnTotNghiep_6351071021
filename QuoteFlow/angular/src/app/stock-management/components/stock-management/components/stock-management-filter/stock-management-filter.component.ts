import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { StockManagementViewService } from '@app/stock-management/services/stock-management.service';
import { NgbModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-stock-management-filter',
  templateUrl: './stock-management-filter.component.html',
  standalone: true,
  styleUrls: ['./stock-management-filter.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    NgbTooltipModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  providers: [LookupService],
})
export class StockManagementFilterComponent implements OnInit, OnDestroy {
  protected readonly service = inject(StockManagementViewService);
  private readonly lookupService = inject(LookupService);

  private subscriptions: Subscription[] = [];
  private previousFormValues = {};
  private isInitialLoad = true;

  // Dropdown options
  supplierOptions: LookupDto<string>[] = [];
  supplierBUOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];
  materialTypeOptions: any = [];
  stockCategoryOptions: LookupDto<string>[] = [];

  getDefaultFilters(): any {
    return {
      supplierCode: null,
      supplierBUCode: null,
      materialType: null,
      golfaCode: null,
      model: null,
      materialGroup: null,
      stockCategoryId: null, // Will be set after loading stock categories
      greaterStockQty: null,
      greaterOnOrderStockQty: null,
      status: null,
    };
  }

  statusOptions = [
    {
      value: 'Active',
      label: 'Active',
    },
    {
      value: 'Deactive',
      label: 'Deactive',
    },
    {
      value: 'Discontinue',
      label: 'Discontinue',
    },
  ];

  deactiveOptions = [
    {
      value: true,
      label: 'Yes',
    },
    {
      value: false,
      label: 'No',
    },
  ];

  isCollapsed = true;

  ngOnInit(): void {
    this.service.buildSearchForm();

    // Load lookups first, then initialize filters
    this.loadLookups();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadLookups(): void {
    // Load stock categories FIRST
    this.lookupService.getStockCategoryLookup().subscribe({
      next: result => {
        this.stockCategoryOptions = result.items || [];

        // Load damaged stock categories and merge
        this.lookupService.getDamagedStockCategoryLookup().subscribe({
          next: damagedResult => {
            this.stockCategoryOptions = [...(this.stockCategoryOptions || []), ...(damagedResult.items || [])];

            // After stock categories are loaded, initialize filters
            this.initializeFiltersAndForm();
          },
          error: error => {
            console.error('Error loading damaged stock categories:', error);
            // Still initialize even if damaged categories fail
            this.initializeFiltersAndForm();
          },
        });
      },
      error: error => {
        console.error('Error loading stock categories:', error);
        // Initialize anyway with null stockCategoryId
        this.initializeFiltersAndForm();
      },
    });

    // Load other lookups in parallel
    this.lookupService.getSupplierLookup().subscribe({
      next: result => {
        this.supplierOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading suppliers:', error);
      },
    });

    this.lookupService.getMaterialGroupLookup().subscribe({
      next: result => {
        this.materialGroupOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading material groups:', error);
      },
    });

    this.materialTypeOptions = [
      { displayName: 'FA', displayCode: 'FA' },
      { displayName: 'LVS', displayCode: 'LVS' },
    ];
  }

  private initializeFiltersAndForm(): void {
    // Load saved filters from route state
    const savedFilters = this.service.routeStateService.getFilters<any>(this.getDefaultFilters());

    // If no saved filters, set default stockCategoryId
    if (!this.service.routeStateService.hasSavedFilters() && this.stockCategoryOptions.length > 0) {
      // Option 1: Use first item in the list
      // savedFilters.stockCategoryId = this.stockCategoryOptions[0]?.id;

      // Option 2: Find specific category by code (like 'HCM')
      const hcmCode = 'HCM';
      const hcmCategory = this.stockCategoryOptions.find(cat => cat.displayCode === hcmCode);
      if (hcmCategory) {
        savedFilters.stockCategoryId = hcmCategory.id;
      } else if (this.stockCategoryOptions.length > 0) {
        // Fallback to first item if HCM not found
        savedFilters.stockCategoryId = this.stockCategoryOptions[0].id;
      }
    }

    // Apply saved/default filters
    this.service.filters = {
      ...this.service.filters,
      ...savedFilters,
    };
    this.service.searchForm.patchValue(savedFilters, { emitEvent: false });
    this.previousFormValues = { ...this.service.searchForm.value };

    // Subscribe to form changes
    const formSub = this.service.searchForm.valueChanges.subscribe(values => {
      const filters = {
        ...values,
        maxResultCount: this.service.filters.maxResultCount,
        skipCount: this.service.filters.skipCount,
      };

      this.service.routeStateService.saveFilters(
        filters,
        this.previousFormValues,
        ['skipCount', 'maxResultCount'],
        false,
      );
      this.previousFormValues = { ...values };
    });

    this.subscriptions.push(formSub);

    // Set page and maxResultCount
    this.service.list.maxResultCount = this.service.filters.maxResultCount;
    this.service.list.page = this.service.filters.skipCount;

    // Execute query if there are saved filters
    if (Object.keys(savedFilters).length > 0) {
      this.service.hookToQuery();
    }
  }

  getSupplierBUBySupplierLookup(id: string): void {
    this.lookupService.getSupplierBUBySupplierLookup(id).subscribe({
      next: result => {
        this.supplierBUOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading vendors:', error);
      },
    });
  }

  onSupplierChange(event: LookupDto<string>): void {
    this.getSupplierBUBySupplierLookup(event.id);
    this.service.searchForm.get('supplierBUCode')?.setValue(null);
  }
}
