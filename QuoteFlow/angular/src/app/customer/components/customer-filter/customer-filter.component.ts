import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { CustomerDto, GetCustomersInput } from '@proxy/customers';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { CustomerFilterService } from '../../services/customer-filter.service';

@Component({
  selector: 'app-customer-filter',
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
  templateUrl: './customer-filter.component.html',
})
export class CustomerFilterComponent
  extends BaseFilterComponent<GetCustomersInput, CustomerDto, CustomerDto>
  implements OnInit, OnDestroy
{
  protected readonly filterService = inject(CustomerFilterService);
  protected readonly lookupService = inject(LookupService);

  deactiveOptions = [
    { value: true, label: 'Yes' },
    { value: false, label: 'No' },
  ];
  customerTypeOptions: LookupDto<string>[];
  customerIndustryOptions: LookupDto<string>[];

  ngOnInit(): void {
    super.ngOnInit();
    this.loadDropdownData();
  }

  ngOnDestroy(): void {
    super.ngOnDestroy();
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
      isDeactive: formValue.isDeactive !== undefined ? formValue.isDeactive : false,
      customerShortName: formValue.customerShortName || '',
      customerType: formValue.customerType || '',
      customerIndustry: formValue.customerIndustry || '',
      fromDate: formValue.fromDate || '',
      toDate: formValue.toDate || '',
    };
  }

  protected override loadDropdownData(): void {
    this.lookupService.getCustomerTypeLookup().subscribe(result => {
      this.customerTypeOptions = result.items;
    });

    this.lookupService.getEUIndustryLookup().subscribe(result => {
      this.customerIndustryOptions = result.items;
    });
  }
}
