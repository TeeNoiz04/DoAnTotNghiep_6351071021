import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { ImportMaterialOptions } from '../import-material.type';
import { ImportMaterialViewService } from '@app/materials/services/import-material.service';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-import-material-filter',
  templateUrl: './import-material-filter.component.html',
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule, ExpandablePanelV2Component, TrimDirective],
})
export class ImportMaterialFilterComponent {
  public readonly service = inject(ImportMaterialViewService);

  statusOptions = [
    { value: RequestStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: RequestStatusEnum.APPROVED, label: 'Approved' },
    { value: RequestStatusEnum.REJECTED, label: 'Rejected' },
    { value: RequestStatusEnum.CANCELLED, label: 'Cancelled' },
  ];

  importTypeOptions = [...ImportMaterialOptions];
}
