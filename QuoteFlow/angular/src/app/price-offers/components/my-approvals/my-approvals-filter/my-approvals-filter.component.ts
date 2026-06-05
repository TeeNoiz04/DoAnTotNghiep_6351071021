import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferMyApprovalsFilterService } from '@app/price-offers/services/price-offer-my-approvals-filter.service';
import { GetPriceOffersInput, PriceOfferDto, PriceOfferWithNavigationListDto } from '@app/proxy/price-offers/models';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { OfferPriceStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { PriceOfferTypes } from '../../special-price-offer/price-offer.types';

@Component({
  selector: 'app-my-approvals-filter',
  templateUrl: './my-approvals-filter.component.html',
  standalone: true,
  styleUrls: ['./my-approvals-filter.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  providers: [LookupService],
})
export class MyApprovalsFilterComponent
  extends BaseFilterComponent<GetPriceOffersInput, PriceOfferWithNavigationListDto, PriceOfferDto>
  implements OnInit, OnDestroy
{
  protected readonly filterService = inject(PriceOfferMyApprovalsFilterService);
  private readonly lookupService = inject(LookupService);

  // Dropdown options
  priceOfferTypeOptions = [
    { value: PriceOfferTypes.Project, label: 'Project Price Offer' },
    { value: PriceOfferTypes.KeyAccount, label: 'Key-Account Price Offer' },
    { value: PriceOfferTypes.Distributor, label: 'Distributor Price Offer' },
    { value: PriceOfferTypes.NoBuyer, label: 'Price Offer Without Buyer' },
  ];

  buyerOptions: LookupDto<string>[] = [];

  statusOptions = [
    { value: OfferPriceStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: OfferPriceStatusEnum.APPROVED, label: 'Approved' },
    { value: OfferPriceStatusEnum.REJECTED, label: 'Rejected' },
    { value: OfferPriceStatusEnum.CANCELLED, label: 'Canceled' },
    { value: OfferPriceStatusEnum.CLOSED, label: 'Closed' },
  ];

  ngOnInit(): void {
    super.ngOnInit();
    this.loadBuyers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadBuyers(): void {
    this.lookupService.getBuyerLookup(true).subscribe({
      next: result => {
        const sortedBuyers = (result.items || []).sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        const nooBuyerOption: LookupDto<string> = {
          id: '00000000-0000-0000-0000-000000000000',
          displayCode: 'No Buyer',
          displayName: 'No Buyer',
        };
        this.buyerOptions = [nooBuyerOption, ...sortedBuyers];
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
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
}
