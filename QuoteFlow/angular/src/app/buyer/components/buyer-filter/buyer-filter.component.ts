import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { BuyerDto, GetBuyersInput } from '@proxy/buyers';
import { LookupService } from '@proxy/general-lookups';
import { BuyerFilterService } from '../../services/buyer-filter.service';

@Component({
  selector: 'app-buyer-filter',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    NgSelectModule,
    CoreModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  templateUrl: './buyer-filter.component.html',
})
export class BuyerFilterComponent
  extends BaseFilterComponent<GetBuyersInput, BuyerDto, BuyerDto>
  implements OnInit, OnDestroy
{
  protected readonly filterService = inject(BuyerFilterService);
  protected readonly lookupService = inject(LookupService);

  buyerTypeOptions: { value: string; label: string }[] = [];
  deactiveOptions = [
    { value: true, label: 'Yes' },
    { value: false, label: 'No' },
  ];

  ngOnInit(): void {
    super.ngOnInit();
    this.loadBuyerTypes();
  }

  ngOnDestroy(): void {
    super.ngOnDestroy();
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

  private loadBuyerTypes(): void {
    this.lookupService.getBuyerTypeLookup({}).subscribe(result => {
      this.buyerTypeOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayName,
      }));
    });
  }
}
