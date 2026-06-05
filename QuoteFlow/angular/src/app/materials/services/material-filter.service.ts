import { ABP, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { GetMaterialsApprovalInput, GetMaterialsInput, MaterialDto } from '@proxy/materials';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests/models';
import { MaterialService } from '@proxy/materials/material.service';
import { Observable } from 'rxjs';
import { MaterialFilterHelper, MaterialFilterInput } from './material-filter-helper';

// Union type for all material DTOs
export type MaterialDto_Union = MaterialDto | MaterialApprovalRequestDto;

@Injectable()
export class MaterialFilterService extends BaseFilterService<
  MaterialFilterInput,
  MaterialDto_Union,
  MaterialDto_Union
> {
  protected readonly proxyService = inject(MaterialService);
  private filterType: 'management' | 'import' | 'approvals' = 'management';

  protected getFilterHelper(): BaseFilterHelper<MaterialFilterInput> {
    return MaterialFilterHelper.getInstance();
  }

  protected getDefaultFilters(): MaterialFilterInput {
    return MaterialFilterHelper.getInstance().getDefaultFiltersForType(this.filterType);
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    const baseControls = {
      golfaCodes: [''],
      models: [''],
    };

    switch (this.filterType) {
      case 'management':
        return {
          ...baseControls,
          sapCode: [''],
          materialType: [null],
          materialGroup: [null],
          supplier: [''],
          supplierBUId: [null],
          supplierBU: [''],
          materialStatus: [null],
          stockQty: [null],
          onOrderStock: [null],
          // isDeactive: [false],
          stockId: [''],
        };
      case 'import':
      case 'approvals':
        return {
          ...baseControls,
          importType: [null],
          approvalStatus: [null],
        };
      default:
        return baseControls;
    }
  }

  protected getListData(
    query: ABP.PageQueryParams & MaterialFilterInput,
  ): Observable<PagedResultDto<MaterialDto_Union>> {
    switch (this.filterType) {
      case 'management':
        return this.proxyService.getList(query as GetMaterialsInput) as Observable<
          PagedResultDto<MaterialDto_Union>
        >;
      case 'import':
        return this.proxyService.getListApproval(query as GetMaterialsApprovalInput) as Observable<
          PagedResultDto<MaterialDto_Union>
        >;
      case 'approvals':
        return this.proxyService.getListMyApproval(
          query as GetMaterialsApprovalInput,
        ) as Observable<PagedResultDto<MaterialDto_Union>>;
      default:
        return this.proxyService.getList(query as GetMaterialsInput) as Observable<
          PagedResultDto<MaterialDto_Union>
        >;
    }
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
          // isDeactive: formValue.isDeactive || false,
          stockId: formValue.stockId || '',
        } as GetMaterialsInput;
      case 'import':
      case 'approvals':
        return {
          ...baseFilters,
          importType: formValue.importType || '',
          approvalStatus: formValue.approvalStatus || '',
        } as GetMaterialsApprovalInput;
      default:
        return baseFilters;
    }
  }

  protected getExportData(filters: MaterialFilterInput): Observable<Blob> {
    const params = {
      filterText: this.list.filter,
      ...filters,
    };

    switch (this.filterType) {
      case 'management':
        return this.proxyService.getListAsExcelFile(params);
      //   case 'import':
      //     return this.proxyService.getListApprovalAsExcelFile(params);
      //   case 'approvals':
      // return this.proxyService.getListMyApprovalAsExcelFile(params);
      default:
        return this.proxyService.getListAsExcelFile(params);
    }
  }

  protected deleteItem(item: MaterialDto_Union): Observable<any> {
    return this.proxyService.delete(item.id);
  }

  /**
   * Set filter type and update helper
   */
  setFilterType(filterType: 'management' | 'import' | 'approvals') {
    this.filterType = filterType;
    MaterialFilterHelper.getInstance().setFilterType(filterType);
  }

  /**
   * Initialize for specific filter type
   */
  initializeForType(filterType: 'management' | 'import' | 'approvals', options: any = {}) {
    this.setFilterType(filterType);
    this.initialize(options);
  }

  /**
   * Get current filter type
   */
  getFilterType(): 'management' | 'import' | 'approvals' {
    return this.filterType;
  }

  /**
   * Check if field is relevant for current filter type
   */
  isFieldRelevant(fieldName: string): boolean {
    const relevantFields = MaterialFilterHelper.getInstance().getRelevantFields();
    return relevantFields.includes(fieldName as any);
  }

  // Export functionality
  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.loadingService.show();

    const currentFormValue = this.searchForm?.value ?? {};
    const currentFilters = this.mapFormToFilters(currentFormValue);

    const params = {
      filterText: this.list.filter,
      ...currentFilters,
    };

    let subscription: Observable<Blob>;
    switch (this.filterType) {
      case 'management':
        subscription = this.proxyService.getListAsExcelFile(params);
        break;
      // case 'import':
      //   subscription = this.proxyService.getListApprovalAsExcelFile(params);
      // case 'approvals':
      //   subscription = this.proxyService.getListMyApprovalAsExcelFile(params);
      default:
        subscription = this.proxyService.getListAsExcelFile(params);
        break;
    }

    return subscription.subscribe({
      next: (response: Blob) => {
        this.isExportToExcelBusy = false;
        this.loadingService.hide();

        const today = new Date();
        const yy = today.getFullYear().toString().slice(-2);
        const mm = String(today.getMonth() + 1).padStart(2, '0');
        const dd = String(today.getDate()).padStart(2, '0');
        const hh = String(today.getHours()).padStart(2, '0');
        const mi = String(today.getMinutes()).padStart(2, '0');
        const ss = String(today.getSeconds()).padStart(2, '0');

        const dateStr = `${yy}${mm}${dd}_${hh}${mi}${ss}`;

        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(response);
        link.download = `Materials_${dateStr}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(link.href);
      },
      error: error => {
        console.error('Download failed:', error);
        this.isExportToExcelBusy = false;
        this.loadingService.hide();
      },
    });
  }

  // Delete functionality
  delete(record: MaterialDto_Union) {
    return this.proxyService.delete(record.id);
  }
}
