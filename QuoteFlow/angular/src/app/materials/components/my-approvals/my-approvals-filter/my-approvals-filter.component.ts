import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import {
  ImportMaterialManagementType,
  ImportMaterialManagementTypeOption,
  ImportMaterialOptions,
} from '../../import-material/import-material.type';
import { MaterialViewService } from '@app/materials/services/material.service';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { AppPermissions } from '@app/app.permissions';

@Component({
  selector: 'app-my-approvals-filter',
  templateUrl: './my-approvals-filter.component.html',
  standalone: true,
  styleUrls: ['./my-approvals-filter.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
})
export class MyApprovalsFilterComponent {
  public readonly service = inject(MaterialViewService);

  statusOptions = [
    { value: RequestStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: RequestStatusEnum.APPROVED, label: 'Approved' },
    { value: RequestStatusEnum.REJECTED, label: 'Rejected' },
  ];

  importTypeOptions: ImportMaterialManagementTypeOption[] = [
    {
      label: 'New Material',
      value: ImportMaterialManagementType.NewMaterial,
      requiredPolicy: `${AppPermissions.Materials.Uploads.NewMaterial}`,
    },
    {
      label: 'Approval Price',
      value: ImportMaterialManagementType.ApprovalPrice,
      requiredPolicy: `${AppPermissions.Materials.Uploads.UpdatePrice}`,
    },
    {
      label: 'Inventory Planning',
      value: ImportMaterialManagementType.InventoryPlanning,
      requiredPolicy: `${AppPermissions.Materials.Uploads.InventoryPlanning}`,
    },
  ];
}
