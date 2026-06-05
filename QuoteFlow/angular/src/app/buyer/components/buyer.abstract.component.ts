import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { PermissionService, TrackByService } from '@abp/ng.core';

import { AppPermissions } from '@app/app.permissions';
import { TitleService } from '@app/shared/services/title/title.service';
import { BuyerDto } from '@proxy/buyers';
import { Subject } from 'rxjs';
import { BuyerDetailViewService } from '../services/buyer-detail.service';
import { BuyerFilterService } from '../services/buyer-filter.service';
import { BuyerViewService } from '../services/buyer.service';
import { MaterialGroupBuyerDetailViewService } from '../services/material-group-buyer-detail.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractBuyerComponent implements OnInit, OnDestroy {
  public readonly track = inject(TrackByService);
  public readonly service = inject(BuyerViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly serviceDetail = inject(BuyerDetailViewService);
  protected readonly filterService = inject(BuyerFilterService);
  public readonly serviceMaterialDetail = inject(MaterialGroupBuyerDetailViewService);

  protected title = 'Buyer Management';
  protected isActionButtonVisible: boolean | null = null;

  private destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;
  ngOnInit() {
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });
    this.filterService.hookToQuery();
    this.titleService.setTitle('Buyer Management');
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.showForm();
  }

  update(record: BuyerDto) {
    this.serviceDetail.update(record);
  }

  updateMaterialGroup(record: BuyerDto) {
    this.serviceMaterialDetail.update(record);
  }

  delete(record: BuyerDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.filterService.exportToExcel();
  }

  showImportCustomer = false;
  onOpenImportPSITracing() {
    this.showImportCustomer = true;
  }
  onOpenDownloadPSITracing() {
    // TODO
  }
}
