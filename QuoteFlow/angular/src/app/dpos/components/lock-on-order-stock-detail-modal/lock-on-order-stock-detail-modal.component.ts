import { PermissionDirective, PermissionService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DPODto, DPOLockOnOrderStockDto, DPOLockShipmentItemUpdateDto, DPOService } from '@app/proxy/dpos';
import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import {
  ActionClickEvent,
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
  CellClickEvent,
} from '@app/shared/components/advanced-data-table';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { ETAETDDetailModalComponent } from '../eta-etd-detail-modal/eta-etd-detail-modal.component';

export interface LockOnOrderStockResult {
  isDataChanged: boolean;
}

@Component({
  selector: 'app-lock-on-order-stock-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    ETAETDDetailModalComponent,
    PermissionDirective,
  ],
  templateUrl: './lock-on-order-stock-detail-modal.component.html',
  styleUrls: ['./lock-on-order-stock-detail-modal.component.scss'],
})
export class LockOnOrderStockDetailModalComponent implements OnInit, OnChanges {
  private readonly permissionService = inject(PermissionService);
  private readonly dpoService = inject(DPOService);

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';
  @Input() dpo: DPODto | null = null;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<LockOnOrderStockResult>();

  loading = false;
  tableLoading = false;
  lockOnOrderStockData: DPOLockOnOrderStockDto[] = [];
  currentOriginalRow: DPOLockOnOrderStockDto | null = null;
  isDataChanged: boolean = false;
  canLockOnOrderStock: boolean = false;

  showETAETDDetailModal: boolean = false;
  poDetail: DPOLockOnOrderStockDto;

  get isItemFinalized(): boolean {
    return (
      this.dpoDetail?.status === RequestStatusEnum.CANCELLED ||
      this.dpoDetail?.status === RequestStatusEnum.CLOSED ||
      this.dpo?.status === RequestStatusEnum.CONFIRMED
    );
  }

  private currentActionErrorHandler: (() => void) | null = null;

  onActionError(event: { row: any; resetErrorState: () => void }): void {
    // Store the reset function to call it when an error occurs
    this.currentActionErrorHandler = event.resetErrorState;
  }

  ngOnInit(): void {
    if (this.visible) {
      this.loadLockOnOrderStockData();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['visible'] && this.visible && !changes['visible'].firstChange) {
      this.loadLockOnOrderStockData();
    }

    if (changes['dpo'].currentValue) {
      this.canLockOnOrderStock = changes['dpo'].currentValue?.flags?.canLockOnOrder;
    }
  }

  onCellClick(event: CellClickEvent): void {
    switch (event.field) {
      case 'eta_etd':
        this.onClickETAETDCell(event.row);
        break;
      default:
    }
  }

  onClickETAETDCell(row: DPOLockOnOrderStockDto) {
    this.showETAETDDetailModal = true;
    this.poDetail = row;
  }

  closeModal(): void {
    const result: LockOnOrderStockResult = {
      isDataChanged: this.isDataChanged,
    };

    this.visible = false;
    this.modalResult.emit(result);
    this.visibleChange.emit(false);
  }

  private loadLockOnOrderStockData(): void {
    if (!this.dpoId || !this.dpoDetailId) {
      return;
    }

    this.tableLoading = true;
    this.dpoService
      .getListLockOnOrderStock(this.dpoId, this.dpoDetailId)
      .pipe(
        tap(result => {
          this.lockOnOrderStockData = result.items.filter(item => item.qtyLocked != 0) || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.lockOnOrderStockData = [];
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

  // use UserNamePipe
  formatUsername = (val: string): string => {
    const usernamePipe = new UsernamePipe();

    return usernamePipe.transform(val);
  };

  onActionClick(event: ActionClickEvent): void {
    switch (event.action) {
      case 'edit':
        // Edit is handled automatically by the table component
        break;
      case 'save':
        this.handleSaveAction(event.row);
        break;
      case 'cancel':
        // Cancel is handled automatically by the table component
        break;
      case 'delete':
        this.handleDeleteAction(event.row);
        break;
    }
  }

  private handleSaveAction(row: DPOLockOnOrderStockDto): void {
    this.loading = true;
    const input: DPOLockShipmentItemUpdateDto = {
      golfaCode: this.dpoDetail.golfaCode || '',
      qty: row.qtyLocked || 0,
      note: row.note || '',
    };
    this.dpoService
      .updateLockShipment(this.dpoDetail.id, row.poDetailId, input)
      .pipe(
        tap(() => {
          this.loadLockOnOrderStockData();
          this.isDataChanged = true;
        }),
        catchError(error => {
          console.error('Error saving lock on order stock:', error);
          if (this.currentActionErrorHandler) {
            this.currentActionErrorHandler();
          }
          return EMPTY;
        }),
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe();
  }

  private handleDeleteAction(row: DPOLockOnOrderStockDto): void {
    this.loading = true;
    this.dpoService
      .deleteLockOnOrderStock(this.dpoDetailId, row.poDetailId)
      .pipe(
        tap(() => {
          this.loadLockOnOrderStockData();
          this.isDataChanged = true;
        }),
        catchError(error => {
          console.error('Error deleting lock on order stock:', error);
          return EMPTY;
        }),
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe();
  }
}
