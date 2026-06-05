import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
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
import { SaleOrderDetailDto } from '@proxy/sale-orders/sale-order-details';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

@Component({
  selector: 'app-dpo-note-info-modal',
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
  templateUrl: './dpo-note-info-modal.component.html',
  styleUrls: ['./dpo-note-info-modal.component.scss'],
})
export class DPONoteInfoModalSOComponent {
  @Input() visible: boolean = false;
  @Input() soDetail: SaleOrderDetailDto | null = null;
  @Input() soId: string = '';
  @Input() soDetailId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();

  loading = false;

  // Table data
  deliveredData: any = [];
  tableLoading = false;

  get isItemFinalized(): boolean {
    return (
      this.soDetail?.statusCode === RequestStatusEnum.CANCELLED ||
      this.soDetail?.statusCode === RequestStatusEnum.CLOSED
    );
  }

  // ngOnInit(): void {}

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
