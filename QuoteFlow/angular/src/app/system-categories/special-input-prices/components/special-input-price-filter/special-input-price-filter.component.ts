import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { GetSpecialInputPricesInput, SpecialInputPriceDto } from '@app/proxy/special-input-prices/models';
import { LookupService } from '@app/proxy/general-lookups';
import { LookupDto } from '@app/proxy/shared/models';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { takeUntil } from 'rxjs';
import { SpecialInputPriceFilterService } from '../../services/special-input-price-filter.service';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-special-input-price-filter',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    NgbModule,
    NgSelectModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  providers: [LookupService],
  templateUrl: './special-input-price-filter.component.html',
  styleUrls: ['./special-input-price-filter.component.scss'],
})
export class SpecialInputPriceFilterComponent
  extends BaseFilterComponent<GetSpecialInputPricesInput, SpecialInputPriceDto, SpecialInputPriceDto>
  implements OnInit, OnDestroy
{
  protected readonly filterService = inject(SpecialInputPriceFilterService);
  private readonly lookupService = inject(LookupService);

  // Filter dropdown options
  supplierOptions: LookupDto<string>[] = [];
  statusOptions = [
    { value: 'Valid', label: 'Valid' },
    { value: 'Closed', label: 'Closed' },
  ];

  ngOnInit() {
    super.ngOnInit();
    this.loadFilterOptions();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected mapFormToFilters(formValue: any): GetSpecialInputPricesInput {
    return {
      ...this.filters,
      accountNo: formValue.accountNo || '',
      accountName: formValue.accountName || '',
      materials: formValue.materialCodes || '',
      models: formValue.modelNames || '',
      validFromMin: formValue.validDate || '',
      validFromMax: formValue.validDate || '',
      status: formValue.status || '',
    };
  }

  private loadFilterOptions() {
    // Load suppliers
    this.lookupService
      .getSupplierLookup()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.supplierOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading suppliers:', error);
        },
      });
  }
  public onSearch(): void {
    super.onSearch();
    this.filterService.filters.skipCount = 0;
  }
}
