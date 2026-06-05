import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';

import { MatCheckboxModule } from '@angular/material/checkbox';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';

import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { ImportBatchRequestViewService } from '../../../services/import-batch-request.service';

@Component({
  selector: 'app-import-batch-request-filter',
  templateUrl: './import-batch-request-filter.component.html',
  standalone: true,
  styleUrls: ['./import-batch-request-filter.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    CoreModule,
    MatCheckboxModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
})
export class ImportBatchRequestFilterComponent implements OnInit {
  public readonly service = inject(ImportBatchRequestViewService);
  public readonly lookupService = inject(LookupService);

  supplierOptions: LookupDto<string>[] = [];
  supplierBUOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];

  statusOptions = [{ value: RequestStatusEnum.DRAFT, label: 'Draft' }];
  status = [
    { value: 'IN_PROGRESS', label: 'In Progress' },
    { value: 'CLOSED', label: 'Closed' },
  ];

  ngOnInit(): void {}
}
