import { ABP, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import { DPOService } from '@app/proxy/dpos';
import { DPODto, DPOExportDataInputDto, GetDPOsInput } from '@app/proxy/dpos/models';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { Observable } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { DpoFilterHelper } from './dpo-filter-helper';

@Injectable()
export class DpoFilterService extends BaseFilterService<GetDPOsInput, DPODto, DPODto> {
  protected readonly proxyService = inject(DPOService);

  protected getFilterHelper(): BaseFilterHelper<GetDPOsInput> {
    return DpoFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetDPOsInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      dpoNo: '',
      materialCode: '',
      modelName: '',
      poNo: '',
      customerName: '',
      orderDateMin: '',
      orderDateMax: '',
      buyerId: null,
      buyerTypeId: null,
      materialType: null,
      supplierId: null,
      specialPriceCode: '',
      materialGroup: null,
      taxCode: '',
      salesOrg: '',
      remark: '',
      status: null,
      dpoType: 'DPO',
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      dpoNo: [''],
      materialCode: [''],
      model: [''],
      poNo: [''],
      customerName: [''],
      orderFromDate: [''],
      orderToDate: [''],
      buyerId: [null],
      buyerTypeId: [null],
      materialType: [null],
      supplierId: [null],
      specialPriceCode: [''],
      materialGroup: [null],
      taxCode: [''],
      salesOrg: [''],
      spoCode: [''], // remark
      status: [null],
    };
  }

  protected getListData(
    query: ABP.PageQueryParams & GetDPOsInput,
  ): Observable<PagedResultDto<DPODto>> {
    return this.proxyService.getList(query);
  }

  protected mapFormToFilters(formValue: any): GetDPOsInput {
    return {
      ...this.filters,
      dpoNo: formValue.dpoNo || '',
      materialCode: formValue.materialCode || '',
      modelName: formValue.model || '',
      poNo: formValue.poNo || '',
      customerName: formValue.customerName || '',
      orderDateMin: formValue.orderFromDate || '',
      orderDateMax: formValue.orderToDate || '',
      buyerTypeId: formValue.buyerTypeId || '',
      buyerId: formValue.buyerId || null,
      materialType: formValue.materialType || null,
      supplierId: formValue.supplierId || null,
      specialPriceCode: formValue.specialPriceCode || '',
      materialGroup: formValue.materialGroup || null,
      taxCode: formValue.taxCode || '',
      salesOrg: formValue.salesOrg || '',
      remark: formValue.spoCode || '',
      status: formValue.status || null,
    };
  }

  protected override mapFiltersToForm(filters: GetDPOsInput): any {
    return {
      dpoNo: filters.dpoNo || '',
      materialCode: filters.materialCode || '',
      model: filters.modelName || '',
      poNo: filters.poNo || '',
      customerName: filters.customerName || '',
      orderFromDate: filters.orderDateMin || '',
      orderToDate: filters.orderDateMax || '',
      buyerTypeId: filters.buyerTypeId || null,
      buyerId: filters.buyerId || null,
      materialType: filters.materialType || null,
      supplierId: filters.supplierId || null,
      specialPriceCode: filters.specialPriceCode || '',
      materialGroup: filters.materialGroup || null,
      taxCode: filters.taxCode || '',
      salesOrg: filters.salesOrg || '',
      spoCode: filters.remark || '',
      status: filters.status || null,
    };
  }

  protected getExportData(filters: GetDPOsInput): Observable<Blob> {
    return this.proxyService.getDownloadToken().pipe(
      switchMap(({ token }) => {
        const params = {
          downloadToken: token,
          ...filters,
        };
        return this.proxyService.getListAsExcelFile(params);
      }),
    );
  }

  protected deleteItem(item: DPODto): Observable<any> {
    return this.proxyService.delete(item.id);
  }

  // Export functionality
  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.loadingService.show();

    // Sync current form values to filters before export
    if (this.searchForm) {
      const formValue = this.searchForm.getRawValue();
      this.filters = this.mapFormToFilters(formValue);
    }

    const filters: DPOExportDataInputDto = {
      dpoNo: this.filters.dpoNo,
      status: this.filters.status,
      golfaCode: this.filters.materialCode,
      model: this.filters.modelName,
      poNo: this.filters.poNo,
      customerName: this.filters.customerName,
      fromDate: this.filters.orderDateMin,
      toDate: this.filters.orderDateMax,
      buyerTypeId: this.filters.buyerTypeId,
      buyerId: this.filters.buyerId,
      materialType: this.filters.materialType,
      supplierCode: this.filters.supplierId,
      spoCode: this.filters.specialPriceCode,
      taxCode: this.filters.taxCode,
      materialGroup: this.filters.materialGroup,
    };

    return this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) => {
          const params = {
            downloadToken: token,
            ...filters,
          };
          //   return of(null);
          return this.proxyService.exportDataAsExcel(params);
        }),
        finalize(() => {
          this.isExportToExcelBusy = false;
          this.loadingService.hide();
        }),
      )
      .subscribe({
        next: (response: Blob) => {
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(response);
          link.download = `DPO_${new Date().toISOString().slice(0, 10)}.xlsx`;
          link.click();
          window.URL.revokeObjectURL(link.href);
        },
        error: error => {
          console.error('Download failed:', error);
        },
      });
  }
}
