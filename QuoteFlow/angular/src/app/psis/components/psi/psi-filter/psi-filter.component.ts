import { CoreModule } from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PSIViewService } from '@app/psis/services/psi.service';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-psi-filter',
  templateUrl: './psi-filter.component.html',
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule, TrimDirective],
})
export class PSIFilterComponent {
  public readonly service = inject(PSIViewService);

  statusOptions = [
    { value: RequestStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: RequestStatusEnum.APPROVED, label: 'Approved' },
    { value: RequestStatusEnum.REJECTED, label: 'Rejected' },
    { value: RequestStatusEnum.CANCELLED, label: 'Cancelled' },
    { value: RequestStatusEnum.OUTDATED, label: 'Outdated' },
  ];

  materialTypeOptions = [
    { displayName: 'FA', displayCode: 'FA' },
    { displayName: 'LVS', displayCode: 'LVS' },
  ];
}
