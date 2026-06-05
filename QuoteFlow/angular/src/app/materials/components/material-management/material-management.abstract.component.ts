import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { MaterialDto } from '@proxy/materials';
import { MaterialDetailViewService } from '../../services/material-detail.service';
import { MaterialViewService } from '../../services/material.service';
import { MaterialDetailsModalComponent } from '../material-details-modal/material-details-modal.component';
import { MaterialManagementFilterComponent } from './components/material-management-filter/material-management-filter.component';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [MaterialManagementFilterComponent, MaterialDetailsModalComponent];

@Directive()
export abstract class AbstractMaterialManagementComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(MaterialViewService);
  public readonly serviceDetail = inject(MaterialDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);

  protected title = 'Material Management';

  ngOnInit() {
    this.titleService.setTitle('Material Management');

    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    //this.service.hookToQuery();
  }

  clearFilters() {
    this.service.clearFilters();
  }

  onSearch() {
    this.service.refreshList();
  }
  onExport() {
    this.service.exportToExcel();
  }

  openDetails(val: MaterialDto): void {
    this.serviceDetail.selected = val;
    this.serviceDetail.showDetails = true;
  }

  exportToExcel() {
    this.service.exportToExcel();
  }
}
