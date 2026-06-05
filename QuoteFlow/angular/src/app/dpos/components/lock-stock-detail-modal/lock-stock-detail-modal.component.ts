import { PermissionDirective, PermissionService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppPermissions } from '@app/app.permissions';
import { DPODto, DPOService } from '@app/proxy/dpos';
// import { DPODetailService } from '@app/proxy/dpos/dpodetails/dpodetail.service';
import { DPODetailDto, DPODetailLockStockDto, DPODetailUpdateLockStockDto } from '@app/proxy/dpos/dpodetails/models';
import { LookupService } from '@app/proxy/general-lookups';
import { MaterialStockLockStockDto } from '@app/proxy/materials/material-stocks/material-stock-lock-stocks/models';
import {
  ActionClickEvent,
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
  InputChangeEvent,
} from '@app/shared/components/advanced-data-table';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { StockCategoryLookupDto } from '@proxy/shared';
import { catchError, EMPTY, finalize, map, of, tap } from 'rxjs';

export interface LockStockDetailResult {
  confirmed?: boolean;
  isDataChanged: boolean;
  stockCategoryId?: string;
  lockQty?: number;
  note?: string;
}

@Component({
  selector: 'app-lock-stock-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    ReactiveFormsModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    InputNumberComponent,
    PermissionDirective,
    EscCloseModalDirective,
  ],
  templateUrl: './lock-stock-detail-modal.component.html',
  styleUrls: ['./lock-stock-detail-modal.component.scss'],
})
export class LockStockDetailModalComponent implements OnInit, OnChanges {
  private readonly lookupService = inject(LookupService);
  private readonly dpoService = inject(DPOService);
  // private readonly dpoDetailService = inject(DPODetailService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly permissionService = inject(PermissionService);
  private readonly usernamePipe = new UsernamePipe();
  private readonly toast = inject(ToasterService);
  protected readonly fb = inject(FormBuilder);

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';
  @Input() dpo: DPODto | null = null;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<LockStockDetailResult>();

  AppPermissions = AppPermissions;
  stockCategories: StockCategoryLookupDto<string>[] = [];
  lockQty: number = 0;
  note: string = '';
  loading = false;
  addLoading = false;
  canLockStock: boolean = false;

  isDataChanged: boolean = false;

  // Table data
  lockStockData: MaterialStockLockStockDto[] = [];
  currentLockData: MaterialStockLockStockDto[] = [];
  releasedLockData: MaterialStockLockStockDto[] = [];
  tableLoading = false;

  // Row operation tracking
  private currentOriginalRow: MaterialStockLockStockDto | null = null;

  public form: FormGroup | undefined;

  @ViewChild('advancedTable') advancedTable: AppAdvancedDataTableComponent | undefined;

  ngOnInit(): void {
    this.initializeForm();

    if (this.visible) {
      this.loadStockCategories();
      this.loadLockStockData();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['dpo'].currentValue) {
      this.canLockStock = changes['dpo'].currentValue?.flags?.canLockStock;
    }
  }

  private async loadStockCategories(): Promise<void> {
    this.loading = true;
    const materialCode = this.dpoDetail?.golfaCode || '';

    this.lookupService
      .getStockCategoryLookupWithAvailableAmount(materialCode, false)
      .pipe(
        tap(() => (this.loading = true)),
        map(result => result?.items || []),
        catchError(error => {
          console.error('Error loading stock categories:', error);
          return of([]);
        }),
        finalize(() => (this.loading = false)),
      )
      .subscribe(stockCategories => {
        this.stockCategories = stockCategories;

        if (this.stockCategories?.length > 0) {
          const firstStock = this.stockCategories[0];
          this.form?.get('selectedStockCategoryId')?.setValue(firstStock.id);
          // Set initial stock available value
          this.form?.get('stockAvailable')?.setValue(firstStock.availableQuantity || 0);
        }
      });
  }

  reloadStockCategories(): void {
    this.loadStockCategories();
  }

  private initializeForm(): void {
    this.form = this.fb.group({
      selectedStockCategoryId: [null, [Validators.required]],
      stockAvailable: [{ value: 0, disabled: true }, []],
      needOrder: [{ value: this.dpoDetail.needDelivery || 0, disabled: true }, []],
      lockQty: [null, [Validators.required]],
      note: [null, []],
    });

    // Subscribe to stock category changes to update stock available
    this.form.get('selectedStockCategoryId')?.valueChanges.subscribe(stockCategoryId => {
      if (stockCategoryId) {
        const selectedStock = this.stockCategories.find(category => category.id === stockCategoryId);
        const availableQuantity = selectedStock?.availableQuantity || 0;
        this.form.get('stockAvailable')?.setValue(availableQuantity);
      } else {
        this.form.get('stockAvailable')?.setValue(0);
      }
    });
  }

  get maxQuantity(): number {
    return this.dpoDetail ? this.dpoDetail.qty - (this.dpoDetail.lockStock || 0) : 0;
  }

  get isItemFinalized(): boolean {
    return (
      this.dpoDetail?.status === RequestStatusEnum.CANCELLED || this.dpoDetail?.status === RequestStatusEnum.CLOSED
    );
  }

  get ctrls() {
    return this.form?.controls || {};
  }

  get maxAvailableAddQuantity(): number {
    const selectedStockCategoryId = this.form?.get('selectedStockCategoryId')?.value;
    const selectedStockAvailableQty =
      this.stockCategories.find(category => category.id === selectedStockCategoryId)?.availableQuantity || 0;
    const needOrder = this.form?.get('needOrder')?.value || 0;
    return Math.min(needOrder, selectedStockAvailableQty);
  }

  get exceedMaxAddQuantity(): boolean {
    const currentQtyStr = this.form?.get('lockQty')?.value;
    if (!currentQtyStr) {
      return true;
    }
    const currentQty = NumberHelper.convertToNumber(currentQtyStr);
    return currentQty > this.maxAvailableAddQuantity;
  }

  closeModal(): void {
    const result: LockStockDetailResult = {
      isDataChanged: this.isDataChanged,
    };

    this.visible = false;
    this.modalResult.emit(result);
    this.visibleChange.emit(false);
  }

  onQuantityChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    const value = parseInt(target.value, 10);
    this.lockQty = isNaN(value) ? 0 : Math.max(0, Math.min(value, this.maxQuantity));
  }

  private loadLockStockData(): void {
    if (!this.dpoId || !this.dpoDetailId) {
      return;
    }

    this.tableLoading = true;
    this.dpoService
      .getLockStocks(this.dpoId, this.dpoDetailId)
      .pipe(
        tap(result => {
          this.lockStockData = result?.items || [];
          // Filter data based on Released field (0 = current lock, others = released)
          this.currentLockData = this.lockStockData.filter(item => item.releasedLock === 0);
          this.releasedLockData = this.lockStockData.filter(item => item.releasedLock !== 0);
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.lockStockData = [];
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

  onActionClick(event: ActionClickEvent): void {
    switch (event.action) {
      case 'edit':
        // Edit is handled automatically by the table component
        break;
      case 'save':
        this.currentOriginalRow = event.originalRow || null;
        this.handleSaveAction(event.row);
        break;
      case 'cancel':
        // Cancel is handled automatically by the table component
        this.clearCurrentOperation();
        break;
      case 'delete':
        this.handleDeleteAction(event.row);
        break;
    }
  }

  onInputChange(event: InputChangeEvent): void {
    // Update the row data with the new value
    const fieldPath = event.field.split('.');
    let target = event.row;

    // Navigate to the nested property if needed
    for (let i = 0; i < fieldPath.length - 1; i++) {
      if (!target[fieldPath[i]]) {
        target[fieldPath[i]] = {};
      }
      target = target[fieldPath[i]];
    }

    // Set the final value
    target[fieldPath[fieldPath.length - 1]] = event.value;
  }

  private currentActionErrorHandler: (() => void) | null = null;

  onActionError(event: { row: any; resetErrorState: () => void }): void {
    // Store the reset function to call it when an error occurs
    this.currentActionErrorHandler = event.resetErrorState;
  }

  private handleSaveAction(row: MaterialStockLockStockDto): void {
    if (!this.currentOriginalRow) {
      console.error('Original row data not found for:', row);
      this.clearCurrentOperation();
      return;
    }

    if (row.qty <= 0) {
      this.toast.warn('Quantity must be greater than 0', 'Validation Error');
      return;
    }
    const updateDto: DPODetailUpdateLockStockDto = {
      stockCategoryId: row.stockCategory?.id || '',
      oldQty: this.currentOriginalRow.qty || 0,
      newQty: row.qty || 0,
      note: row.note || '',
    };

    this.dpoService
      .updateLockStockDetail(this.dpoDetailId, updateDto)
      .pipe(
        tap(() => {
          this.toast.success('Lock stock updated successfully.');
          this.clearCurrentOperation();
          this.loadLockStockData();
          this.reloadStockCategories();
          this.refreshNeedDeliveryValue();
          this.isDataChanged = true;
        }),
        catchError(error => {
          console.error('Error updating lock stock:', error);
          this.toast.error('Failed to update lock stock.');
          // this.clearCurrentOperation();
          if (this.currentActionErrorHandler) {
            this.currentActionErrorHandler();
          }
          return EMPTY;
        }),
      )
      .subscribe();
  }

  private handleDeleteAction(row: MaterialStockLockStockDto): void {
    const stockName = row.stockCategory?.stockName || 'Unknown';
    const confirmation = this.confirmationService.info(
      `Are you sure you want to delete the lock stock for "${stockName}"?`,
      'Delete Lock Stock',
    );

    confirmation.subscribe(result => {
      if (result === Confirmation.Status.confirm) {
        // TODO: Implement delete API call
        this.dpoService.deleteLockStock(this.dpoDetailId, row.id).subscribe(() => {
          this.toast.success('Lock stock deleted successfully.');
          this.loadLockStockData();
          this.reloadStockCategories();
          this.refreshNeedDeliveryValue();
          this.isDataChanged = true;
        });
      }
    });
  }

  onAddLockStock(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        control.markAsTouched();
      });
      this.toast.warn('Please correct the form errors before saving', 'Validation Error');
      return;
    }

    const valueForm = this.form.getRawValue();

    this.addLoading = true;

    const lockStockDto: DPODetailLockStockDto = {
      golfaCode: this.dpoDetail.golfaCode,
      stockCategoryId: valueForm.selectedStockCategoryId,
      lockQty: valueForm?.lockQty ? NumberHelper?.convertToNumber(valueForm?.lockQty) : 0,
      note: valueForm.note || '',
    };

    this.dpoService
      .lockStock(this.dpoDetailId, lockStockDto)
      .pipe(
        tap(() => {
          // Reset form after successful addition
          this.toast.success('Lock stock successfully.');
          this.resetFormAfterAdd();
          // Reload the lock stock data to reflect the new addition
          this.loadLockStockData();
          this.refreshNeedDeliveryValue();
          this.reloadStockCategories();
          this.isDataChanged = true;
        }),
        catchError(error => {
          console.error('Error adding lock stock:', error);
          return EMPTY;
        }),
        finalize(() => {
          this.addLoading = false;
        }),
      )
      .subscribe();
  }

  private clearCurrentOperation(): void {
    this.currentOriginalRow = null;
  }

  private resetFormAfterAdd(): void {
    // Clear quantity and note fields
    this.form?.get('lockQty')?.setValue(null);
    this.form?.get('note')?.setValue(null);

    // Reset to first stock if available
    if (this.stockCategories?.length > 0) {
      const firstStock = this.stockCategories[0];
      this.form?.get('selectedStockCategoryId')?.setValue(firstStock.id);
      this.form?.get('stockAvailable')?.setValue(firstStock.availableQuantity || 0);
    }

    // Mark form as untouched to remove validation messages
    this.form?.get('lockQty')?.markAsUntouched();
    this.form?.get('note')?.markAsUntouched();
  }

  private refreshNeedDeliveryValue(): void {
    if (!this.dpoId) {
      return;
    }

    this.dpoService
      .get(this.dpoId)
      .pipe(
        tap(dpo => {
          if (dpo && dpo.details && this.dpoDetailId) {
            const updatedDetail = dpo.details.find(detail => detail.id === this.dpoDetailId);
            if (updatedDetail && this.form) {
              this.form.get('needOrder')?.setValue(updatedDetail.needDelivery || 0);
              // Update the dpoDetail reference as well
              this.dpoDetail = { ...this.dpoDetail, ...updatedDetail };
            }
          }
        }),
        catchError(error => {
          console.error('Error refreshing DPO detail data:', error);
          return EMPTY;
        }),
      )
      .subscribe();
  }
}
