import { ABP, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import {
  GetPriceOffersInput,
  PriceOfferDto,
  PriceOfferWithNavigationListDto,
} from '@app/proxy/price-offers/models';
import { PriceOfferService } from '@app/proxy/price-offers/price-offer.service';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { Observable } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { PriceOfferFilterHelper } from './price-offer-filter-helper';

@Injectable()
export class PriceOfferMyApprovalsFilterService extends BaseFilterService<
  GetPriceOffersInput,
  PriceOfferWithNavigationListDto,
  PriceOfferDto
> {
  protected readonly proxyService = inject(PriceOfferService);

  protected getFilterHelper(): BaseFilterHelper<GetPriceOffersInput> {
    return PriceOfferFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetPriceOffersInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      relatedToMe: false,
      filterText: '',
      priceOfferType: '',
      materialType: '',
      buyerId: '',
      priceOfferCode: '',
      customerTaxCode: '',
      customerName: '',
      approvalStatus: '',
      createdFrom: '',
      createdTo: '',
      projectName: '',
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      filterText: [''],
      priceOfferType: [null],
      materialType: [null],
      buyerId: [null],
      priceOfferCode: [''],
      customerTaxCode: [''],
      customerName: [''],
      approvalStatus: [null],
      createdFrom: [''],
      createdTo: [''],
      relatedToMe: [false],
      projectName: [''],
    };
  }

  protected getListData(
    query: ABP.PageQueryParams & GetPriceOffersInput,
  ): Observable<PagedResultDto<PriceOfferWithNavigationListDto>> {
    return this.proxyService.getMyApprovalsList(query);
  }

  protected mapFormToFilters(formValue: any): GetPriceOffersInput {
    return {
      ...this.filters,
      filterText: formValue.filterText || '',
      priceOfferType: formValue.priceOfferType || '',
      materialType: formValue.materialType || '',
      buyerId: formValue.buyerId || '',
      priceOfferCode: formValue.priceOfferCode || '',
      customerTaxCode: formValue.customerTaxCode || '',
      customerName: formValue.customerName || '',
      approvalStatus: formValue.approvalStatus || '',
      createdFrom: formValue.createdFrom || '',
      createdTo: formValue.createdTo || '',
      relatedToMe: formValue.relatedToMe || false,
      projectName: formValue.projectName || '',
    };
  }

  protected getExportData(filters: GetPriceOffersInput): Observable<Blob> {
    return this.proxyService.getDownloadToken().pipe(
      switchMap(({ token }) => {
        const params = {
          downloadToken: token,
          filterText: this.list.filter,
          ...filters,
        };
        return this.proxyService.getListAsExcelFile(params);
      }),
    );
  }

  protected deleteItem(item: PriceOfferDto): Observable<any> {
    return this.proxyService.delete(item.id);
  }

  // Export functionality
  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.loadingService.show();

    return this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) => {
          const params = {
            downloadToken: token,
            filterText: this.list.filter,
            ...this.filters,
          };
          return this.proxyService.getListAsExcelFile(params);
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
          link.download = 'PriceOfferMyApprovals.xlsx';
          link.click();
          window.URL.revokeObjectURL(link.href);
        },
        error: error => {
          console.error('Download failed:', error);
        },
      });
  }

  // Get list of approvers
  getListApprovers(priceOfferId: string) {
    return this.proxyService.getListApprovers(priceOfferId);
  }

  // Delete functionality
  delete(record: PriceOfferDto) {
    return this.proxyService.delete(record.id);
  }
}
