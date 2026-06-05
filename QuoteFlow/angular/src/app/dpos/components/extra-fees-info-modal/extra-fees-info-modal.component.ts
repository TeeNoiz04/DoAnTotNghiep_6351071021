import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DPOService } from '@proxy/dpos';
// import { DPODetailService } from '@proxy/dpos/dpodetails';

@Component({
  selector: 'app-extra-fees-info-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    EscCloseModalDirective,
  ],
  templateUrl: './extra-fees-info-modal.component.html',
  styleUrls: ['./extra-fees-info-modal.component.scss'],
})
export class ExtraFeesInfoModalComponent {
  private readonly dpoDetailService = inject(DPOService);

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();

  loading = false;

  // Table data
  deliveredData: any = [];
  tableLoading = false;

  get isItemFinalized(): boolean {
    return (
      this.dpoDetail?.status === RequestStatusEnum.CANCELLED || this.dpoDetail?.status === RequestStatusEnum.CLOSED
    );
  }
  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onClose(): void {
    this.closeModal();
  }
}
