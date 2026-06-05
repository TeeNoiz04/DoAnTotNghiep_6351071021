import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { RouteStateService } from '@app/price-offers/services/route-state.service';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { Subscription } from 'rxjs';

type SaleOrdersFilterModel = {
  statusCode: string;
  orderDateMin: string;
  orderDateMax: string;
  soNo: string;
  sosapNo: string;
  dpoNo: string;
  materialCode: string;
  invoiceNo: string;
  buyerType: string;
  buyerId: string;
  materialType: string;
  model: string;
  soDateFrom: string;
  soDateTo: string;
  vatDateFrom: string;
  vatDateTo: string;
  materialGroup: string;
  taxCode: string;
  maxResultCount: number;
  skipCount: number;
};

@Component({
  selector: 'app-sale-orders-management-filter',
  templateUrl: './sale-orders-management-filter.component.html',
  standalone: true,
  styleUrls: ['./sale-orders-management-filter.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
  providers: [RouteStateService],
})
export class SaleOrdersManagementFilterComponent implements OnInit, OnDestroy {
  public readonly service = inject(SaleOrdersManagementViewService);
  public readonly lookupService = inject(LookupService);

  buyerOptions: LookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];

  private subscriptions: Subscription[] = [];

  ngOnInit(): void {
    this.service.buildSearchForm();

    const saved = this.service.getSavedFilters();
    if (saved?.buyerType) {
      this.restoreBuyersByType(saved.buyerType, saved.buyerId);
    }

    this.service.list.maxResultCount = this.service.filters.maxResultCount ?? DEFAULT_PAGE_SIZE;
    this.service.list.page = this.service.filters.skipCount ?? 0;
    this.service.hookToQuery();
    this.service.loadAllData();
  }

  private restoreBuyersByType(buyerTypeCode: string, savedBuyerId: string | null): void {
    const matchedType = this.service.buyerTypeOptions.find(opt => opt.displayCode === buyerTypeCode);

    if (matchedType?.id) {
      this.loadBuyersAndRestore(matchedType.id, savedBuyerId);
    } else {
      this.lookupService.getBuyerTypeLookup({}).subscribe({
        next: result => {
          this.service.buyerTypeOptions = result?.items || [];
          const found = this.service.buyerTypeOptions.find(opt => opt.displayCode === buyerTypeCode);
          if (found?.id) {
            this.loadBuyersAndRestore(found.id, savedBuyerId);
          }
        },
        error: err => console.error('Error loading buyer types:', err),
      });
    }
  }

  private loadBuyersAndRestore(buyerTypeId: string, savedBuyerId: string | null): void {
    const sub = this.lookupService.getBuyerLookupByBuyerType(buyerTypeId).subscribe({
      next: result => {
        this.service.buyerOptions = result?.items || [];

        if (savedBuyerId) {
          const found = this.service.buyerOptions.find(b => b.id === savedBuyerId);
          if (found) {
            this.service.searchForm.patchValue({ buyerId: savedBuyerId }, { emitEvent: false });
          }
        }
      },
      error: err => console.error('Error loading buyers:', err),
    });

    this.subscriptions.push(sub);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
