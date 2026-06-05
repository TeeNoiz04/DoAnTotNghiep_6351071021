import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { WorkflowConfigurationDto } from '@proxy/workflow-configurations';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractMaterialStockWorkflowComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(WorkflowConfigurationViewService);
  public readonly serviceDetail = inject(WorkflowConfigurationDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);

  protected title = '::Material Stock Workflow Configuration';
  protected isActionButtonVisible: boolean | null = null;
  materialTypeOptions = [
    { label: 'Inventory Planning', value: 'MATERIAL.INVENTORY_PLANNING' },
    { label: 'Price', value: 'MATERIAL.PRICE' },
    { label: 'New Material', value: 'MATERIAL.NEW' },
  ];
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('Material Stock Workflow Configuration');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.filters.workflowType = this.materialTypeOptions?.[0]?.value;
    this.service.hookToQuery();
  }

  onLoad() {
    this.list.get();
  }

  clearFilters() {
    this.service.clearFilters();
    this.service.filters.workflowType = this.materialTypeOptions?.[0]?.value;
  }

  update(record: WorkflowConfigurationDto) {
    this.serviceDetail.update(record);
  }
}
