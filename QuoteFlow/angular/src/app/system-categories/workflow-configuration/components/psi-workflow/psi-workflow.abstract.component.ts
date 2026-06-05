import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { WorkflowConfigurationTypes } from '@app/system-categories/system-category.model';
import { WorkflowConfigurationDto } from '@proxy/workflow-configurations';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractPSIWorkflowComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(WorkflowConfigurationViewService);
  public readonly serviceDetail = inject(WorkflowConfigurationDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);

  protected title = '::PSI Workflow Configuration';
  protected isActionButtonVisible: boolean | null = null;
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('PSI Workflow Configuration');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.filters.workflowType = WorkflowConfigurationTypes.PSI;
    this.service.hookToQuery();
  }

  update(record: WorkflowConfigurationDto) {
    this.serviceDetail.update(record);
  }
}
