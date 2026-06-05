import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { SupplierDto } from '@proxy/suppliers';
import { debounceTime, filter, Subject, takeUntil } from 'rxjs';
import { VendorCategoryDetailViewService } from '../service/vendor-category-detail.service';
import { VendorCategoryViewService } from '../service/vendor-category.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractVendorCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(VendorCategoryViewService);
  public readonly serviceDetail = inject(VendorCategoryDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::Supplier';
  private searchTextChanged$ = new Subject<string>();
  AppPermissions = AppPermissions;
  private destroy$ = new Subject<void>();
  deactiveOptions = [
    {
      value: true,
      label: 'Yes',
    },
    {
      value: false,
      label: 'No',
    },
  ];
  ngOnInit() {
    // this.filters = this.service.filters;
    this.service.filters.isDeactive = false;
    this.titleService.setTitle('Suppliers');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery();
      });

    // Set up search text debounce
    this.searchTextChanged$.pipe(debounceTime(500), takeUntil(this.destroy$)).subscribe(() => {
      this.onLoad();
    });
    this.service.hookToQuery();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  onLoad() {
    // this.service.updateQueryParams(true);
    this.list.get();
  }
  onSearchTextChange(text: string) {
    this.searchTextChanged$.next(text);
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

  update(record: SupplierDto) {
    this.serviceDetail.update(record);
  }

  delete(record: SupplierDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }
}
