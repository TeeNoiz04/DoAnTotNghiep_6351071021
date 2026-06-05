import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferFilterService } from '@app/price-offers/services/price-offer-filter.service';
import { GetPriceOffersInput, PriceOfferDto, PriceOfferListDto } from '@app/proxy/price-offers/models';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { OfferPriceStatusEnum, RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { PriceOfferTypes } from '../../price-offer.types';

@Component({
  selector: 'app-special-price-offer-filter',
  templateUrl: './special-price-offer-filter.component.html',
  standalone: true,
  styleUrls: ['./special-price-offer-filter.component.scss'],
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
export class SpecialPriceOfferFilterComponent
  extends BaseFilterComponent<GetPriceOffersInput, PriceOfferListDto, PriceOfferDto>
  implements OnInit, OnDestroy
{
  protected readonly filterService = inject(PriceOfferFilterService);
  private readonly lookupService = inject(LookupService);

  // Dropdown options
  priceOfferTypeOptions = [
    { value: PriceOfferTypes.Project, label: 'Project Price Offer' },
    { value: PriceOfferTypes.KeyAccount, label: 'Key-Account Price Offer' },
    { value: PriceOfferTypes.Distributor, label: 'Distributor Price Offer' },
    { value: PriceOfferTypes.NoBuyer, label: 'Price Offer Without Buyer' },
  ];

  materialTypeOptions: LookupDto<string>[] = [];
  buyerOptions: LookupDto<string>[] = [];

  statusOptions = [
    { value: OfferPriceStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: OfferPriceStatusEnum.APPROVED, label: 'Approved' },
    { value: OfferPriceStatusEnum.REJECTED, label: 'Rejected' },
    { value: OfferPriceStatusEnum.CANCELLED, label: 'Canceled' },
    { value: OfferPriceStatusEnum.CLOSED, label: 'Closed' },
  ];

  projectResultStatusOptions = [
    { value: RequestStatusEnum.PENDING, label: 'Pending' },
    { value: RequestStatusEnum.PRE_ORDER, label: 'Pre-Order' },
    { value: RequestStatusEnum.WON, label: 'Won' },
  ];

  ngOnInit(): void {
    super.ngOnInit();
    this.loadMaterialTypes();
    this.loadBuyers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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

  private loadBuyers(): void {
    this.lookupService.getBuyerLookup(false).subscribe({
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
