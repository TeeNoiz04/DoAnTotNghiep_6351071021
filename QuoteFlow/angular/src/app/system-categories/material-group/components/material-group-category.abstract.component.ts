import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { MaterialGroupDto } from '@proxy/materials/material-groups';
import { filter, Subject, takeUntil } from 'rxjs';
import { MaterialGroupDetailViewService } from '../services/material-group-category-detail.service';
import { MaterialGroupViewService } from '../services/material-group-category.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractMaterialGroupCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(MaterialGroupViewService);
  public readonly serviceDetail = inject(MaterialGroupDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::MaterialGroup';

  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;

  ngOnInit() {
    this.titleService.setTitle('Material Group');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
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

  update(record: MaterialGroupDto) {
    this.serviceDetail.update(record);
  }

  delete(record: MaterialGroupDto) {
    this.service.delete(record);
  }
}
