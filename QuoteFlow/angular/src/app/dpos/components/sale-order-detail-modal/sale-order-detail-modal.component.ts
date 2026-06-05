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
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DPOService } from '@proxy/dpos';
import { SaleOrderListModalDPODto } from '@proxy/sale-orders';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

@Component({
  selector: 'app-sale-order-detail-modal',
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
  templateUrl: './sale-order-detail-modal.component.html',
  styleUrls: ['./sale-order-detail-modal.component.scss'],
})
export class SaleOrderDetailModalComponent implements OnInit {
  private readonly dpoDetailService = inject(DPOService);
  private readonly usernamePipe = new UsernamePipe();

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();

  loading = false;

  // Table data
  saleOrderData: SaleOrderListModalDPODto[] = [];
  tableLoading = false;

  get isItemFinalized(): boolean {
    return (
      this.dpoDetail?.status === RequestStatusEnum.CANCELLED || this.dpoDetail?.status === RequestStatusEnum.CLOSED
    );
  }

  ngOnInit(): void {
    if (this.visible) {
      this.loadLockStockData();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  private loadLockStockData(): void {
    if (!this.dpoId || !this.dpoDetailId) {
      return;
    }

    this.tableLoading = true;
    this.dpoDetailService
      .getListSaleOrderModalDPO(this.dpoDetailId)
      .pipe(
        tap(result => {
          this.saleOrderData = result || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.saleOrderData = [];
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

  formatModifiedDate = (val: string): string => {
    return val
      ? new Date(val).toLocaleDateString('en-GB') +
          ' ' +
          new Date(val).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
      : '';
  };

  formatUsername = (val: string): string => {
    return val ? this.usernamePipe.transform(val) : '';
  };

  onClose(): void {
    this.closeModal();
  }
}
