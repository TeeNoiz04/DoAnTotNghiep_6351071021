import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { Validators } from '@angular/forms';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { SystemConfigurationDto } from '@proxy/system-configurations';
import { filter, Subject, takeUntil } from 'rxjs';
import { SystemConfigurationDetailViewService } from '../services/system-configuration-category-detail.service';
import { SystemConfigurationViewService } from '../services/system-configuration-category.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractSystemConfigurationCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SystemConfigurationViewService);
  public readonly serviceDetail = inject(SystemConfigurationDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::Parametters Configuration';
  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('Parametters Configuration | FA Admin');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery();
      });

    // Initial setup
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

  clearFilters() {
    this.service.clearFilters();
  }

  showForm() {
    this.serviceDetail.showForm();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.showForm();
    this.serviceDetail.form.get('value')?.setValidators([Validators.required]);
    this.serviceDetail.form.get('value')?.updateValueAndValidity();
  }

  update(record: SystemConfigurationDto) {
    this.serviceDetail.update(record);

    this.serviceDetail.form.get('value')?.setValidators([Validators.required]);
    this.serviceDetail.form.get('value')?.updateValueAndValidity();
  }
}
