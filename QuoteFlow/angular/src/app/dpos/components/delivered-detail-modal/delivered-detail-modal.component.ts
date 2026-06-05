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
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DPOService } from '@proxy/dpos';
// import { DPODetailService } from '@proxy/dpos/dpodetails';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

@Component({
  selector: 'app-delivered-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
  templateUrl: './delivered-detail-modal.component.html',
  styleUrls: ['./delivered-detail-modal.component.scss'],
})
export class DeliveredDetailModalComponent implements OnInit {
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

  ngOnInit(): void {
    if (this.visible) {
      this.loadDeliveryData();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  private loadDeliveryData(): void {
    if (!this.dpoId || !this.dpoDetailId) {
      return;
    }

    this.tableLoading = true;
    this.dpoDetailService
      .getListSaleOrderModalDelivery(this.dpoDetailId)
      .pipe(
        tap(result => {
          this.deliveredData = result || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.deliveredData = [];
          return EMPTY;
        }),
        finalize(() => {
          this.tableLoading = false;
        }),
      )
      .subscribe();
  }

  // Formatters for table columns
  formatQuantity = (val: number): string => {
    return val ? new Intl.NumberFormat('en-US').format(val) : '0';
  };

  formatDate = (val: string): string => {
    return val ? new Date(val).toLocaleDateString('en-GB') : '';
  };

  onClose(): void {
    this.closeModal();
  }
}
