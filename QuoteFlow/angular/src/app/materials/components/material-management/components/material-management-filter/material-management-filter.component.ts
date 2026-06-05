import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialFilterInput } from '@app/materials/services/material-filter-helper';
import { MaterialDto_Union, MaterialFilterService } from '@app/materials/services/material-filter.service';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import {
  ImportMaterialOptions,
  ImportStockManagementTypeOption,
} from '@app/stock-management/components/import-stock/import-stock.type';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';

@Component({
  selector: 'app-material-management-filter',
  templateUrl: './material-management-filter.component.html',
  standalone: true,
  styleUrls: ['./material-management-filter.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    NgbModule,
    NgSelectModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  providers: [LookupService],
})
export class MaterialManagementFilterComponent
  extends BaseFilterComponent<MaterialFilterInput, MaterialDto_Union, MaterialDto_Union>
  implements OnInit, OnDestroy
{
  @Input() filterType: 'management' | 'import' | 'approvals' = 'management';

  protected readonly filterService = inject(MaterialFilterService);
  private readonly lookupService = inject(LookupService);

  // Dropdown options
  materialTypeOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];
  supplierOptions: LookupDto<string>[] = [];
  supplierBUOptions: LookupDto<string>[] = [];
  materialStatusOptions: LookupDto<string>[] = [];
  importTypeOptions: ImportStockManagementTypeOption[] = [...ImportMaterialOptions];
  approvalStatusOptions: LookupDto<string>[] = [
    { id: '', displayCode: RequestStatusEnum.IN_PROGRESS, displayName: 'In Progress' },
    { id: '', displayCode: RequestStatusEnum.APPROVED, displayName: 'Approved' },
    { id: '', displayCode: RequestStatusEnum.REJECTED, displayName: 'Rejected' },
  ];

  ngOnInit(): void {
    super.ngOnInit();

    // Set filter type on service
    this.filterService.setFilterType(this.filterType);

    // Load dropdown options based on filter type
    this.loadDropdownOptions();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadDropdownOptions(): void {
    this.loadMaterialTypes();

    if (this.filterType === 'management') {
      this.loadMaterialGroups();
      this.loadSuppliers();
      this.loadMaterialStatuses();
    }
  }

  private loadMaterialTypes(): void {
    this.lookupService.getMaterialTypeLookup().subscribe({
      next: result => {
        this.materialTypeOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading material types:', error);
      },
    });
  }

  private loadMaterialGroups(): void {
    this.lookupService.getMaterialGroupLookup().subscribe({
      next: result => {
        this.materialGroupOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading material groups:', error);
      },
    });
  }

  private loadSuppliers(): void {
    this.lookupService.getSupplierLookup().subscribe({
      next: result => {
        this.supplierOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading suppliers:', error);
      },
    });
  }

  private loadMaterialStatuses(): void {
    this.materialStatusOptions = [
      { id: '', displayCode: 'Active', displayName: 'Active' },
      { id: '', displayCode: 'Deactive', displayName: 'Deactive' },
      { id: '', displayCode: 'Discontinue', displayName: 'Discontinue' },
    ];
  }
  /**
   * Check if field should be shown for current filter type
   */
  isFieldVisible(fieldName: string): boolean {
    return this.filterService.isFieldRelevant(fieldName);
  }

  protected mapFormToFilters(formValue: any): MaterialFilterInput {
    const baseFilters = {
      ...this.filters,
      golfaCodes: formValue.golfaCodes || '',
      models: formValue.models || '',
    };

    switch (this.filterType) {
      case 'management':
        return {
          ...baseFilters,
          sapCode: formValue.sapCode || '',
          materialType: formValue.materialType || '',
          materialGroup: formValue.materialGroup || '',
          supplier: formValue.supplier || '',
          supplierBUId: formValue.supplierBUId || '',
          supplierBU: formValue.supplierBU || '',
          materialStatus: formValue.materialStatus || '',
          stockQty: formValue.stockQty || undefined,
          onOrderStock: formValue.onOrderStock || undefined,
          isDeactive: formValue.isDeactive || false,
          stockId: formValue.stockId || '',
        };
      case 'import':
      case 'approvals':
        return {
          ...baseFilters,
          importType: formValue.importType || '',
          approvalStatus: formValue.approvalStatus || '',
        };
      default:
        return baseFilters;
    }
  }
  onSupplierChange(event: LookupDto<string>): void {
    this.getSupplierBUBySupplierLookup(event.id);
    this.searchForm.get('supplierBUId')?.setValue(null);
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
}
