import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { TitleService } from '@app/shared/services/title/title.service';
// import { SystemCategoryDetailViewService } from '../services/system-category-detail.service';
// import { SystemCategoryViewService } from '../services/system-category.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { BuyerDto } from '@proxy/buyers';
import { debounceTime, filter, Subject, takeUntil } from 'rxjs';
import { BuyerCategoryDetailViewService } from '../services/buyer-category-detail.service';
import { BuyerCategoryViewService } from '../services/buyer-category.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractBuyerComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(BuyerCategoryViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);
  public readonly serviceDetail = inject(BuyerCategoryDetailViewService);

  protected title = '::Buyers';

  private destroy$ = new Subject<void>();
  private searchTextChanged$ = new Subject<string>();
  AppPermissions = AppPermissions;
  ngOnInit() {
    // Listen for navigation events to refresh data when URL parameters change
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        // When navigation occurs, call hookToQuery which now includes syncFromQueryParams
        this.service.hookToQuery();
      });

    // Set up search text debounce
    this.searchTextChanged$.pipe(debounceTime(500), takeUntil(this.destroy$)).subscribe(() => {
      this.onLoad();
    });

    // Initial setup
    this.service.hookToQuery();
    // Set the page title for the Buyer component
    this.titleService.setTitle('Buyers');
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
    this.serviceDetail.showForm();
  }

  update(record: BuyerDto) {
    this.serviceDetail.update(record);
  }

  delete(record: BuyerDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  // Add a method to update title when viewing a specific buyer
  viewBuyerDetails(record: BuyerDto) {
    // Update the page title to include the buyer name
    this.titleService.setTitle(`Buyer: ${record.shortName}`);

    // Call your existing detail view method
    this.update(record);
  }
}
