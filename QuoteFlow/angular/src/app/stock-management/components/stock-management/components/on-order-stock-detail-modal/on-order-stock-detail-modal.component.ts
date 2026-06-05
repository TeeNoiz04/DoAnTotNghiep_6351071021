import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { NgSelectModule } from '@ng-select/ng-select';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { StockManagementListDto, StockManagementService } from '@proxy/stock-managements';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-on-order-stock-detail-modal',
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
  templateUrl: './on-order-stock-detail-modal.component.html',
  styleUrls: ['./on-order-stock-detail-modal.component.scss'],
})
export class OnOrderStockDetailModalComponent implements OnInit {
  private readonly service = inject(StockManagementService);
  private readonly usernamePipe = new UsernamePipe();

  @Input() visible: boolean = false;
  @Input() materialItem: StockManagementListDto = undefined;

  @Output() visibleChange = new EventEmitter<boolean>();

  loading = false;
  onOrderStockData: any = [];
  tableLoading = false;

  ngOnInit(): void {
    if (this.visible) {
      this.loadOnOrderStockData();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  private loadOnOrderStockData(): void {
    if (!this.materialItem) {
      return;
    }
    this.tableLoading = true;
    this.service
      .getOnOrderStock(this.materialItem?.golfaCode)
      .pipe(
        tap(result => {
          this.onOrderStockData = result || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.onOrderStockData = [];
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
