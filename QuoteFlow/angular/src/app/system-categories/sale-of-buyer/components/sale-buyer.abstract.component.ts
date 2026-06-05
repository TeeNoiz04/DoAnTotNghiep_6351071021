import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { BuyerDto } from '@proxy/buyers';
import { LookupService } from '@proxy/general-lookups';
import { SalesAssignmentDto } from '@proxy/sales-assignments';
import { debounceTime, filter, Subject, takeUntil } from 'rxjs';
import { SaleBuyerDetailViewService } from '../services/sale-buyer-detail.service';
import { SaleBuyerViewService } from '../services/sale-buyer.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractSaleBuyerCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SaleBuyerViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);
  public readonly serviceDetail = inject(SaleBuyerDetailViewService);
  protected proxyLookupService = inject(LookupService);

  protected title = 'Sales Team';

  private destroy$ = new Subject<void>();
  isLoading = true;
  dataBuyer: BuyerDto[] = [];
  private searchTextChanged$ = new Subject<string>();
  locationOptions: { value: string; label: string }[] = [];
  buyerOptions: { value: string; label: string }[] = [];
  materialOptions = [
    { value: 'FA', label: 'FA' },
    { value: 'LVS', label: 'LVS' },
  ];
  AppPermissions = AppPermissions;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.getLocation();
    this.getBuyer();
    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery();
      });

    this.searchTextChanged$.pipe(debounceTime(500), takeUntil(this.destroy$)).subscribe(() => {
      this.onLoad();
    });

    this.service.hookToQuery();
    this.titleService.setTitle('Sales Team | FA Admin');
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchTextChange(text: string) {
    this.searchTextChanged$.next(text);
  }

  onLoad() {
    // Use the new updateQueryParams method from service
    this.service.updateQueryParams(true);
    this.list.get();
  }

  clearFilters() {
    this.service.clearFilters();
  }

  showForm() {
    this.serviceDetail.showForm();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.selectedMaterials = {
      fa: false,
      lvs: false,
    };
    this.serviceDetail.showForm();
  }

  update(record: SalesAssignmentDto) {
    this.serviceDetail.update(record);
  }

  delete(record: SalesAssignmentDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    // this.service.exportToExcel();
  }

  // Add a method to update title when viewing a specific buyer
  viewBuyerDetails(record: SalesAssignmentDto) {
    // Update the page title to include the buyer name
    this.titleService.setTitle(`Buyer: ${record.saleFullName}`);

    // Call your existing detail view method
    this.update(record);
  }
  getLocation() {
    this.proxyLookupService.getLocationLookup().subscribe(result => {
      this.locationOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayName,
      }));
    });
  }
  getBuyer() {
    this.proxyLookupService.getBuyerLookup(false).subscribe(result => {
      this.buyerOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayCode,
      }));
    });
  }
}
