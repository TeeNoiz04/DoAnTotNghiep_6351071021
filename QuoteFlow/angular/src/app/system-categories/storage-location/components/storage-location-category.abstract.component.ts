import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { Validators } from '@angular/forms';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { StockCategoryDto } from '@proxy/stock-categories';
import { filter, Subject, takeUntil } from 'rxjs';
import { StorageLocationDetailViewService } from '../services/storage-location-category-detail.service';
import { StorageLocationViewService } from '../services/storage-location-category.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractStorageLocationCategoryComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(StorageLocationViewService);
  public readonly serviceDetail = inject(StorageLocationDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::Storage';
  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('Storage');
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

  update(record: StockCategoryDto) {
    this.serviceDetail.update(record);

    this.serviceDetail.form.get('value')?.setValidators([Validators.required]);
    this.serviceDetail.form.get('value')?.updateValueAndValidity();
  }

  delete(record: StockCategoryDto) {
    this.service.delete(record);
  }
}
