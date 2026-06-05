import { ABP, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import {
  GetPriceOffersInput,
  PriceOfferDto,
  PriceOfferListDto,
} from '@app/proxy/price-offers/models';
import { PriceOfferService } from '@app/proxy/price-offers/price-offer.service';
import { BaseFilterHelper } from '@app/shared/helpers/base-filter-helper';
import { BaseFilterService } from '@app/shared/services/base-filter.service';
import { Observable } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { PriceOfferFilterHelper } from './price-offer-filter-helper';

@Injectable()
export class PriceOfferFilterService extends BaseFilterService<
  GetPriceOffersInput,
  PriceOfferListDto,
  PriceOfferDto
> {
  protected readonly proxyService = inject(PriceOfferService);

  // Flag to indicate if this is approval view
  private isApprovalView = false;

  protected getFilterHelper(): BaseFilterHelper<GetPriceOffersInput> {
    return PriceOfferFilterHelper.getInstance();
  }

  protected getDefaultFilters(): GetPriceOffersInput {
    return {
      maxResultCount: 50,
      skipCount: 0,
      relatedToMe: false,
      sorting: 'creationTime desc',
      filterText: null,
      priceOfferType: null,
      materialType: null,
      buyerId: null,
      priceOfferCode: null,
      customerTaxCode: null,
      customerName: null,
      approvalStatus: null,
      createdFrom: null,
      createdTo: null,
      projectName: null,
      projectResultStatus: null,
    };
  }

  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      filterText: [null],
      priceOfferType: [null],
      materialType: [null],
      buyerId: [null],
      priceOfferCode: [null],
      customerTaxCode: [null],
      customerName: [null],
      approvalStatus: [null],
      createdFrom: [null],
      createdTo: [null],
      relatedToMe: [false],
      projectName: [null],
      projectResultStatus: [null],
    };
  }

  protected getListData(
    query: ABP.PageQueryParams & GetPriceOffersInput,
  ): Observable<PagedResultDto<PriceOfferListDto>> {
    if (this.isApprovalView) {
      return this.proxyService.getMyApprovalsList(query);
    } else {
      return this.proxyService.getList(query);
    }
  }

  protected mapFormToFilters(formValue: any): GetPriceOffersInput {
    return {
      ...this.filters,
      filterText: formValue.filterText || null,
      priceOfferType: formValue.priceOfferType || null,
      materialType: formValue.materialType || null,
      buyerId: formValue.buyerId || null,
      priceOfferCode: formValue.priceOfferCode || null,
      customerTaxCode: formValue.customerTaxCode || null,
      customerName: formValue.customerName || null,
      approvalStatus: formValue.approvalStatus || null,
      createdFrom: formValue.createdFrom || null,
      createdTo: formValue.createdTo || null,
      relatedToMe: formValue.relatedToMe || false,
      projectName: formValue.projectName || null,
      projectResultStatus: formValue.projectResultStatus || null,
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

  // Set approval view mode
  setApprovalView(isApprovalView: boolean) {
    this.isApprovalView = isApprovalView;
  }

  // Initialize for approval view
  initializeForApprovalView(options: any = {}) {
    this.isApprovalView = true;
    this.initialize(options);
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
          link.download = 'PriceOffer.xlsx';
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
