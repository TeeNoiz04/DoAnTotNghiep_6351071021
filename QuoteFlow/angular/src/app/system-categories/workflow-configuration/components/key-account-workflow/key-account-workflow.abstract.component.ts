import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { WorkflowConfigurationTypes } from '@app/system-categories/system-category.model';
import { LookupService } from '@proxy/general-lookups';
import { WorkflowConfigurationDto } from '@proxy/workflow-configurations';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractKeyAccountWorkflowComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(WorkflowConfigurationViewService);
  public readonly serviceDetail = inject(WorkflowConfigurationDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly serviceLookup = inject(LookupService);
  protected title = '::Key Account Workflow Configuration';

  priceOfferOptions = [
    { label: 'Key-Account Price Offer', value: 'AP' },
    { label: 'Distributor Stock Price Offer', value: 'DS' },
    { label: 'Project Price Offer', value: 'PP' },
  ];
  levelOptions: { label: string; value: number }[] = [];
  conditionOptions: { label: string; value: string }[] = [];
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('Key Account Workflow Configuration');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.filters.workflowType = WorkflowConfigurationTypes.KeyAccount;
    this.service.hookToQuery();
    this.getLevel(WorkflowConfigurationTypes.KeyAccount);
    this.getCondition(WorkflowConfigurationTypes.KeyAccount);
  }

  update(record: WorkflowConfigurationDto) {
    this.serviceDetail.update(record);
  }
  onLoad() {
    this.list.get();
  }

  clearFilters() {
    this.service.clearFilters();
    this.service.filters.workflowType = this.priceOfferOptions?.[0]?.value;
  }
  // onChangePriceOffer(event: any) {
  //   this.levelOptions = [];
  //   this.conditionOptions = [];
  //   this.service.filters.workflowLevel = null;
  //   this.service.filters.condition = null;
  //   this.getLevel(event?.value);
  //   this.getCondition(event?.value);
  // }
  getLevel(type: string) {
    this.serviceLookup.getLevelLookupWorkflow(type).subscribe(res => {
      this.levelOptions = res.items.map(item => ({
        label: 'Level ' + item,
        value: item,
      }));
    });
  }
  getCondition(type: string) {
    this.serviceLookup.getConditionLookupWorkflow(type).subscribe(res => {
      this.conditionOptions = res.items.map(item => ({
        label: item,
        value: item,
      }));
    });
  }
}
