import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { SystemCategoryDetailViewService } from '@app/system-categories/system-category/services/system-category-detail.service';
import { SystemCategoryViewService } from '@app/system-categories/system-category/services/system-category.service';
import { filter, Subject, takeUntil } from 'rxjs';
import type { SystemCategoryDto } from '../../../proxy/system-categories/models';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractBuyerTypeCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SystemCategoryViewService);
  public readonly serviceDetail = inject(SystemCategoryDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::Buyer Type';
  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;
  ngOnInit(): void {
    this.titleService.setTitle('Buyer Type');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery(CategoryTypes.BuyerType);
      });

    // Initial setup
    this.service.hookToQuery(CategoryTypes.BuyerType);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onLoad() {
    this.service.updateQueryParams(true);
    this.list.get();
  }

  clearFilters() {
    this.service.clearFilters(CategoryTypes.BuyerType);
  }

  create() {
    this.serviceDetail.create(CategoryTypes.BuyerType);
  }

  update(record: SystemCategoryDto) {
    this.serviceDetail.update(record);
  }

  delete(record: SystemCategoryDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }
}
