import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { DPODto, GetDPOsInput } from '@app/proxy/dpos/models';
import { LookupService } from '@app/proxy/general-lookups';
import { LookupDto } from '@app/proxy/shared/models';
import { BaseFilterComponent } from '@app/shared/components/base-filter.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { takeUntil } from 'rxjs';
import { DpoFilterService } from '../../services/dpo-filter.service';

@Component({
  selector: 'app-dpo-filter',
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
  templateUrl: './dpo-filter.component.html',
  styleUrls: ['./dpo-filter.component.scss'],
})
export class DpoFilterComponent extends BaseFilterComponent<GetDPOsInput, DPODto, DPODto> implements OnInit, OnDestroy {
  protected readonly filterService = inject(DpoFilterService);
  private readonly lookupService = inject(LookupService);

  // Filter dropdown options
  buyerOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];
  supplierTypeOptions: LookupDto<string>[] = [];
  materialGroupTypeOptions: LookupDto<string>[] = [];

  ngOnInit() {
    super.ngOnInit();
    this.loadFilterOptions();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected mapFormToFilters(formValue: any): GetDPOsInput {
    return {
      ...this.filters,
      // filterText: formValue.materialCode || '',
      dpoNo: formValue.dpoNo || '',
      materialCode: formValue.materialCode || '',
      modelName: formValue.model || '',
      poNo: formValue.poNo || '',
      customerName: formValue.customerName || '',
      orderDateMin: formValue.orderFromDate || '',
      orderDateMax: formValue.orderToDate || '',
      buyerId: formValue.buyerId || null,
      materialType: formValue.materialType || null,
      supplierId: formValue.supplierId || null,
      specialPriceCode: formValue.specialPriceCode || '',
      materialGroup: formValue.materialGroup || null,
      taxCode: formValue.taxCode || '',
      salesOrg: formValue.salesOrg || '',
      // remark: formValue.spoCode || '',
    };
  }

  private loadFilterOptions() {
    // Load material types
    this.lookupService
      .getMaterialTypeLookup()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.materialTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading material types:', error);
        },
      });

    // Load buyers
    this.lookupService
      .getBuyerLookup(false)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.buyerOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });

    this.lookupService
      .getSupplierLookup()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.supplierTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading material types:', error);
        },
      });
    this.lookupService
      .getMaterialGroupLookup()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.materialGroupTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading material types:', error);
        },
      });
    this.lookupService.getBuyerTypeLookup({}).subscribe({
      next: result => {
        this.buyerTypeOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading buyer types:', error);
      },
    });
  }

  override onSearch(): void {
    // call base class search method
    super.onSearch();

    //
  }

  onChangeBuyerType(event: any) {
    if (event?.id) {
      this.searchForm.get('buyerId')?.setValue(null);
      this.buyerOptions = [];
      this.lookupService.getBuyerLookupByBuyerType(event.id).subscribe(result => {
        this.buyerOptions = result.items;
      });
    } else {
      this.lookupService.getBuyerLookup(false).subscribe(result => {
        this.buyerOptions = result.items;
      });
    }
  }
}
