import { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { BuyerDto, BuyerService, GetBuyersInput } from '@proxy/buyers';
import { Observable } from 'rxjs';
import { BuyerFilterHelper } from './buyer-filter-helper';

@Injectable({
  providedIn: 'root',
})
export class BuyerFilterService extends BaseFilterService<GetBuyersInput, BuyerDto, BuyerDto> {
  protected readonly proxyService = inject(BuyerService);

  protected getFilterHelper(): BaseFilterHelper<GetBuyersInput> {
    return BuyerFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetBuyersInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      deactive: false,
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      buyerTypeId: [''],
      buyerCode: [''],
      shortName: [''],
      fullName: [''],
      taxCode: [''],
      address: [''],
      contactPerson: [''],
      contactEmail: [''],
      contactPhoneNumber: [''],
      paymentTermCode: [''],
      paymentTermDescription: [''],
      creditLimitMin: [null],
      creditLimitMax: [null],
      creditExposureMin: [null],
      creditExposureMax: [null],
      appliedPrice: [null],
      deactive: [false],
      note: [''],
    };
  }

  protected getListData(query: any): Observable<PagedResultDto<BuyerDto>> {
    return this.proxyService.getList(query);
  }

  protected mapFormToFilters(formValue: any): GetBuyersInput {
    return {
      ...this.filters,
      buyerTypeId: formValue.buyerTypeId || '',
      buyerCode: formValue.buyerCode || '',
      shortName: formValue.shortName || '',
      fullName: formValue.fullName || '',
      taxCode: formValue.taxCode || '',
      address: formValue.address || '',
      contactPerson: formValue.contactPerson || '',
      contactEmail: formValue.contactEmail || '',
      contactPhoneNumber: formValue.contactPhoneNumber || '',
      paymentTermCode: formValue.paymentTermCode || '',
      paymentTermDescription: formValue.paymentTermDescription || '',
      creditLimitMin: formValue.creditLimitMin || null,
      creditLimitMax: formValue.creditLimitMax || null,
      creditExposureMin: formValue.creditExposureMin || null,
      creditExposureMax: formValue.creditExposureMax || null,
      appliedPrice: formValue.appliedPrice,
      deactive: formValue.deactive !== undefined ? formValue.deactive : false,
      note: formValue.note || '',
    };
  }

  exportToExcel(): void {
    this.isExportToExcelBusy = true;
    this.proxyService.getListAsExcelFile(this.filters).subscribe({
      next: result => {
        this.abpWindowService.downloadBlob(result, 'Buyers.xlsx');
        this.isExportToExcelBusy = false;
      },
      error: () => {
        this.isExportToExcelBusy = false;
      },
    });
  }
  protected getExportData(filters: GetBuyersInput): Observable<Blob> {
    throw new Error('Method not implemented.');
  }
  protected deleteItem(item: BuyerDto): Observable<any> {
    throw new Error('Method not implemented.');
  }
}
