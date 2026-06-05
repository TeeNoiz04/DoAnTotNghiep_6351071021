import { PermissionDirective } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  HostListener,
  inject,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DPODto, DPOListPOsDto, DPOLockShipmentItemDto, DPOService } from '@app/proxy/dpos';
// import { DPODetailService } from '@app/proxy/dpos/dpodetails/dpodetail.service';
import { AppPermissions } from '@app/app.permissions';
import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import {
  KeyboardNavigationConfig,
  KeyboardNavigationHintComponent,
  KeyboardNavigationHintService,
} from '@app/shared/components/keyboard-navigation-hint';
import { ModalNavigationService } from '@app/shared/services/modal-navigation.service';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { DPO_MODAL_NAVIGATION_CONFIG, DpoModalType } from '../../configs/dpo-modal-navigation.config';

@Component({
  selector: 'app-lock-on-order-stock-available-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    PermissionDirective,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    KeyboardNavigationHintComponent,
  ],
  templateUrl: './lock-on-order-stock-available-detail-modal.component.html',
  styleUrls: ['./lock-on-order-stock-available-detail-modal.component.scss'],
})
export class LockOnOrderStockAvailableDetailModalComponent implements OnInit, OnChanges {
  private readonly dpoService = inject(DPOService);
  // private readonly dpoDetailService = inject(DPODetailService);
  private readonly toasterService = inject(ToasterService);
  private readonly navigationService = inject(ModalNavigationService);
  private readonly MODULE_TYPE = 'dpo';

  // Keyboard navigation hint configuration with custom Enter key for Lock action
  readonly keyboardHintConfig: KeyboardNavigationConfig = {
    ...KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL,
    keys: [
      ...KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL.keys,
      { key: 'Enter', icon: 'bi-lock', description: 'Lock', action: 'submit' },
    ],
  };

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';
  @Input() allDpoDetails: DPODetailDto[] = [];
  @Input() currentDetailIndex: number = 0;
  @Input() dpo: DPODto | null = null;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<any>();
  @Output() nextDetailRequested = new EventEmitter<{ nextIndex: number; nextDetailId: string }>();
  @Output() previousDetailRequested = new EventEmitter<{ nextIndex: number; nextDetailId: string }>();
  @Output() dpoDetailUpdated = new EventEmitter<DPODetailDto>();

  protected AppPermissions = AppPermissions;
  isBusy = false;
  loading = false;
  lockOnOrderStockData: DPOListPOsDto[] = [];
  tableLoading = false;
  poDetail: DPOListPOsDto;
  nextDetailLoading = false;

  constructor() {
    // Register DPO modal navigation configuration
    this.navigationService.registerConfig(this.MODULE_TYPE, DPO_MODAL_NAVIGATION_CONFIG);
  }

  get isItemFinalized(): boolean {
    return (
      this.dpoDetail?.status === RequestStatusEnum.CANCELLED || this.dpoDetail?.status === RequestStatusEnum.CLOSED
    );
  }

  get hasNextDetail(): boolean {
    return this.navigationService.hasNextEligibleDetail(
      this.MODULE_TYPE,
      this.allDpoDetails,
      this.currentDetailIndex,
      DpoModalType.LockOnOrderStockAvailable,
    );
  }

  get hasPreviousDetail(): boolean {
    return this.navigationService.hasPreviousEligibleDetail(
      this.MODULE_TYPE,
      this.allDpoDetails,
      this.currentDetailIndex,
      DpoModalType.LockOnOrderStockAvailable,
    );
  }

  private getNextEligibleDetailIndex(): number {
    return this.navigationService.getNextEligibleDetailIndex(
      this.MODULE_TYPE,
      this.allDpoDetails,
      this.currentDetailIndex,
      DpoModalType.LockOnOrderStockAvailable,
    );
  }

  private getPreviousEligibleDetailIndex(): number {
    return this.navigationService.getPreviousEligibleDetailIndex(
      this.MODULE_TYPE,
      this.allDpoDetails,
      this.currentDetailIndex,
      DpoModalType.LockOnOrderStockAvailable,
    );
  }

  ngOnInit(): void {
    if (this.visible) {
      this.loadLockOnOrderStockData();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reload data when dpoDetail or dpoDetailId changes (navigation between details)
    if ((changes['dpoDetail'] || changes['dpoDetailId']) && this.visible) {
      this.loadLockOnOrderStockData();
      // Reset loading state when navigation completes
      this.nextDetailLoading = false;
    }

    // Also reload when modal becomes visible
    if (changes['visible'] && this.visible) {
      this.loadLockOnOrderStockData();
    }
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onNextDetail(): void {
    const nextIndex = this.getNextEligibleDetailIndex();
    if (nextIndex === -1) {
      return;
    }

    const nextDetail = this.allDpoDetails[nextIndex];
    if (!nextDetail?.id) {
      return;
    }

    this.nextDetailLoading = true;

    // Emit event to parent to handle the navigation
    this.nextDetailRequested.emit({
      nextIndex: nextIndex,
      nextDetailId: nextDetail.id,
    });
  }

  onPreviousDetail(): void {
    const previousIndex = this.getPreviousEligibleDetailIndex();
    if (previousIndex === -1) {
      return;
    }

    const previousDetail = this.allDpoDetails[previousIndex];
    if (!previousDetail?.id) {
      return;
    }

    this.nextDetailLoading = true;

    // Emit event to parent to handle the navigation
    this.previousDetailRequested.emit({
      nextIndex: previousIndex,
      nextDetailId: previousDetail.id,
    });
  }

  private loadLockOnOrderStockData(): void {
    if (!this.dpoId || !this.dpoDetailId) {
      return;
    }

    this.tableLoading = true;
    this.dpoService
      .getListAvailablePOs(this.dpoId, this.dpoDetailId, this.dpoDetail?.golfaCode)
      .pipe(
        tap(result => {
          this.lockOnOrderStockData = result.items || [];
          // Auto-fill locked quantities top-down after data is loaded

          if (!this.isItemFinalized) {
            this.autoFillLockedQuantities();
          }
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

  /**
   * Auto-fill locked quantities top-down based on need order amount
   * Fill first PO up to its available qty, then move to next PO, etc.
   */
  private autoFillLockedQuantities(): void {
    if (!this.dpoDetail?.needDelivery || !this.lockOnOrderStockData?.length) {
      return;
    }

    let remainingNeedOrder = this.dpoDetail.needDelivery;

    // Reset all locked values first
    this.lockOnOrderStockData.forEach(item => {
      (item as any).locked = 0;
    });

    // Fill top-down until need order is satisfied
    for (const item of this.lockOnOrderStockData) {
      if (remainingNeedOrder <= 0) {
        break;
      }

      const availableQty = item.qtyAvailable || 0;
      const lockAmount = Math.min(remainingNeedOrder, availableQty);

      (item as any).locked = lockAmount;
      remainingNeedOrder -= lockAmount;
    }
  }

  /**
   * Refresh DPO detail data and reload available POs after successful lock operation
   */
  private refreshDataAfterLock(): void {
    if (!this.dpoDetailId) {
      return;
    }

    // Get updated DPO detail to refresh needDelivery and other data
    this.dpoService
      .getDPODetail(this.dpoDetailId)
      .pipe(
        tap(updatedDetail => {
          // Update the current dpoDetail with fresh data
          this.dpoDetail = updatedDetail;
          // Emit updated detail to parent component
          this.dpoDetailUpdated.emit(updatedDetail);
        }),
        catchError(error => {
          console.error('Error refreshing DPO detail:', error);
          return EMPTY;
        }),
      )
      .subscribe(() => {
        // After getting updated detail data, reload the available POs list
        this.loadLockOnOrderStockData();
      });
  }

  onSubmit(): void {
    // Find items where qty > qtyAvailable
    const errors = (this.lockOnOrderStockData as any).filter(item => item.locked > item.qtyAvailable);

    if (errors?.length > 0) {
      this.toasterService.error('Some items have Locked greater than Available Qty', 'Error');

      return;
    }

    this.isBusy = true;

    const items = (this.lockOnOrderStockData as any).map(item => {
      return {
        poDetailId: item?.poDetailId,
        golfaCode: item?.golfaCode,
        qty: item?.locked,
        note: item?.note,
      } as DPOLockShipmentItemDto;
    });

    // Get all objects that have a defined (non-null/undefined) qty
    const resultArr = items.filter(item => item.qty);

    this.dpoService
      .lockShipment(this.dpoDetailId, { items: resultArr })
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: result => {
          this.toasterService.success('Lock successfully', 'Success');
          this.modalResult.emit(result);
          // Refresh DPO detail data and reload available POs after successful lock
          this.refreshDataAfterLock();
        },
        error: error => {
          console.error('Error updating lock stock:', error);
          this.toasterService.error('Failed to update lock stock', 'Error');
        },
      });
  }

  onRowClick(event: any): void {
    // Handle row click if needed
    event.hasErrors = event.locked > event.qtyAvailable;
    event.errors = [`Locked must equal or less than ${event.qtyAvailable}`];
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

  /**
   * Generic keyboard navigation handler for modal navigation
   * Can be easily extracted to a shared service/directive later
   */
  @HostListener('document:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    // Only handle arrow keys when modal is visible and not busy
    if (!this.visible || this.isBusy || this.nextDetailLoading) {
      return;
    }

    // Check if user is focused on an input/textarea/select element
    const activeElement = document.activeElement;
    const isInputFocused =
      activeElement?.tagName === 'INPUT' ||
      activeElement?.tagName === 'TEXTAREA' ||
      activeElement?.tagName === 'SELECT' ||
      activeElement?.hasAttribute('contenteditable');

    // Don't handle navigation if user is typing in an input
    if (isInputFocused) {
      return;
    }

    // Handle navigation keys
    switch (event.key) {
      case 'ArrowLeft':
        event.preventDefault();
        this.handleNavigationKey('previous');
        break;
      case 'ArrowRight':
        event.preventDefault();
        this.handleNavigationKey('next');
        break;
      // Optional: Add more navigation keys
      case 'PageUp':
        event.preventDefault();
        this.handleNavigationKey('previous');
        break;
      case 'PageDown':
        event.preventDefault();
        this.handleNavigationKey('next');
        break;
      case 'Enter':
        event.preventDefault();
        this.handleEnterKey();
        break;
    }
  }

  /**
   * Generic navigation handler that can be customized per modal
   * Can be easily extracted to a shared service later
   */
  private handleNavigationKey(direction: 'next' | 'previous'): void {
    if (direction === 'next' && this.hasNextDetail) {
      this.onNextDetail();
    } else if (direction === 'previous' && this.hasPreviousDetail) {
      this.onPreviousDetail();
    } else {
      // later
    }
  }

  /**
   * Handle Enter key press to submit/lock when button is not disabled
   */
  private handleEnterKey(): void {
    // Check if Lock button should be enabled (same conditions as in template)
    const isLockButtonDisabled = this.isBusy || this.isItemFinalized || !this.dpo?.flags?.canLockOnOrder;

    if (!isLockButtonDisabled) {
      this.onSubmit();
    }
  }
}
