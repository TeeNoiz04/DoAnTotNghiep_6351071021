import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { PSIViewService } from '@app/psis/services/psi.service';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-my-approvals-filter',
  templateUrl: './my-approvals-filter.component.html',
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule, TrimDirective],
})
export class MyApprovalsFilterComponent {
  public readonly service = inject(PSIViewService);

  statusOptions = [
    { value: RequestStatusEnum.IN_PROGRESS, label: 'In Progress' },
    { value: RequestStatusEnum.APPROVED, label: 'Approved' },
    { value: RequestStatusEnum.REJECTED, label: 'Rejected' },
  ];
}
