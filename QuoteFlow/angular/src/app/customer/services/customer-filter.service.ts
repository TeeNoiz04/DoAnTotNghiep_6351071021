import { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { CustomerDto, CustomerService, GetCustomersInput } from '@proxy/customers';
import { Observable, switchMap } from 'rxjs';
import { CustomerFilterHelper } from './customer-filter-helper';

@Injectable({
  providedIn: 'root',
})
export class CustomerFilterService extends BaseFilterService<
  GetCustomersInput,
  CustomerDto,
  CustomerDto
> {
  protected readonly proxyService = inject(CustomerService);

  protected getFilterHelper(): BaseFilterHelper<GetCustomersInput> {
    return CustomerFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetCustomersInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      isDeactive: false,
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      filterText: [''],
      taxCode: [''],
      customerName: [''],
      address: [''],
      phone: [''],
      province: [''],
      website: [''],
      isDeactive: [false],
      customerShortName: [''],
      customerType: [''],
      customerIndustry: [''],
      fromDate: [''],
      toDate: [''],
    };
  }

  protected getListData(query: any): Observable<PagedResultDto<CustomerDto>> {
    return this.proxyService.getList(query);
  }

  protected mapFormToFilters(formValue: any): GetCustomersInput {
    return {
      ...this.filters,
      filterText: formValue.filterText || '',
      taxCode: formValue.taxCode || '',
      customerName: formValue.customerName || '',
      address: formValue.address || '',
      phone: formValue.phone || '',
      province: formValue.province || '',
      website: formValue.website || '',
      customerType: formValue.customerType || '',
      customerIndustry: formValue.customerIndustry || '',
      fromDate: formValue.fromDate || '',
      toDate: formValue.toDate || '',
      isDeactive: formValue.isDeactive !== undefined ? formValue.isDeactive : false,
      customerShortName: formValue.customerShortName || '',
    };
  }

  exportToExcel(): void {
    this.isExportToExcelBusy = true;
    this.proxyService.getDownloadToken().subscribe(({ token }) => {
      this.proxyService
        .getListAsExcelFile({
          downloadToken: token,
          filterText: this.list.filter,
          ...this.filters,
        })
        .subscribe({
          next: result => {
            this.abpWindowService.downloadBlob(result, 'Customers.xlsx');
            this.isExportToExcelBusy = false;
          },
          error: () => {
            this.isExportToExcelBusy = false;
          },
        });
    });
  }
  protected getExportData(filters: GetCustomersInput): Observable<Blob> {
    throw new Error('Method not implemented.');
  }
  protected deleteItem(item: CustomerDto): Observable<any> {
    throw new Error('Method not implemented.');
  }
}
