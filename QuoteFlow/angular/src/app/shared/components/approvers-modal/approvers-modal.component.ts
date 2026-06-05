import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, Input } from '@angular/core';
import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { DataTableComponent } from '../data-table/data-table.component';
import { HeaderTableComponent } from '../data-table/header/header.component';
import { ColumnComponent } from '../data-table/column/column.component';

@Component({
  selector: 'app-approvers-modal',
  standalone: true,
  imports: [
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
  ],
  providers: [ListService],
  templateUrl: './approvers-modal.component.html',
  styleUrl: './approvers-modal.component.scss',
})
export class ApproversModalComponent {
  @Input() approvalList: PagedResultDto<any>;
  @Input() showApprovalList = false;

  public options: NgbModalOptions = {
    size: 'lg',
    animation: true,
    centered: true,
    backdrop: true,
  };
  protected approvers = [];

  openModal(record: any[]) {
    this.showApprovalList = true;
    this.approvers = record;
  }
}
