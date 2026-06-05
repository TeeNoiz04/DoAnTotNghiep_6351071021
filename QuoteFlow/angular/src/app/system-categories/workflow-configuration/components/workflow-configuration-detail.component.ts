import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { WorkflowConfigurationDetailViewService } from '../services/workflow-configuration-detail.service';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';

@Component({
  selector: 'app-workflow-configuration-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbNavModule,
    MatSlideToggleModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    NgSelectModule,
  ],
  providers: [],
  templateUrl: './workflow-configuration-detail.component.html',
  styleUrls: ['./workflow-configuration-detail.component.scss'],
})
export class WorkflowConfigurationDetailModalComponent implements OnInit {
  public readonly service = inject(WorkflowConfigurationDetailViewService);
  private readonly lookupService = inject(LookupService);

  ngOnInit(): void {
    this.loadUserList();
    const approvers = this.service.selected.workflowApprovers;
    this.service.approverSubject.next(approvers);
    this.service.initialApprovers = [...approvers];
  }

  loadUserList() {
    this.lookupService.getUserLookup().subscribe({
      next: result => {
        this.service.userOptions = result || [];
      },
      error: error => {
        console.error('Error loading suppliers:', error);
      },
    });
  }

  submitForm() {
    this.service.submitForm();
  }

  get f() {
    return this.service.form.controls;
  }
}
