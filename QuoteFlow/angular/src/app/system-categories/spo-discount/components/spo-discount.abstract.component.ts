import { Directive, inject, OnDestroy, OnInit } from '@angular/core';
import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { filter, Subject, takeUntil } from 'rxjs';
import { SpoDiscountViewService } from '../services/spo-discount.service';
import { SPODiscountDetailViewService } from '../services/spo-discount-detail.service';
export const ChildTabDependencies = [];
export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractSpoDiscountComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SpoDiscountViewService);
  public readonly serviceDetail = inject(SPODiscountDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::SPO Discount';
  private destroy$ = new Subject<void>();

  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('SPO Discount | FA Admin');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.loadFilterOptions();
    this.service.hookToQuery();

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery();
      });
    this.service.hookToQuery();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onLoad() {
    this.service.hasKAType = this.service.filters.approval_Type === 'AP';
    this.list.get();
  }

  clearFilters() {
    this.service.clearFilters();
  }

  update(record: any) {
    this.serviceDetail.update(record);
  }
}
