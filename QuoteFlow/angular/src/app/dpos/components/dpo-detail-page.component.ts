import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService, TrackByService } from '@abp/ng.core';
import { Confirmation, ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { DpoGkrAllocationDto, DPOLockShipmentAutoDto, DPOService } from '@app/proxy/dpos';
import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import { NoteMetadataDto } from '@app/proxy/shared/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
  SelectionChangeEvent,
} from '@app/shared/components/advanced-data-table';
import { CellClickEvent } from '@app/shared/components/advanced-data-table/advanced-data-table.component';
import {
  ApprovalCommentModalComponent,
  HistoryActions,
} from '@app/shared/components/approval-comment/approval-comment.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import {
  FilterField,
  FilterPaneComponent as FiltersPaneComponent,
} from '@app/shared/components/filters-pane/filters-pane.component';
import { SmartBackButtonComponent } from '@app/shared/components/smart-back-button';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { RequestStatusEnum, StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { filter, switchMap } from 'rxjs';
import { DPODetailViewService } from '../services/dpo-detail.service';
import { DeliveredDetailModalComponent } from './delivered-detail-modal/delivered-detail-modal.component';
import { DpoDiscussionBoxComponent } from './discussion-box/dpo-discussion-box.component';
import { EditDPOItemDialogComponent } from './edit-dpo-item-dialog/edit-dpo-item-dialog.component';
import { ExtraFeeModalComponent } from './extra-fee-modal/extra-fee-modal.component';
import { ExtraFeesInfoModalComponent } from './extra-fees-info-modal/extra-fees-info-modal.component';
import { GkrAllocationTableComponent } from './gkr-allocation-table/gkr-allocation-table.component';
import { LockOnOrderStockAvailableDetailModalComponent } from './lock-on-order-stock-available-detail-modal/lock-on-order-stock-available-detail-modal.component';
import {
  LockOnOrderStockDetailModalComponent,
  LockOnOrderStockResult,
} from './lock-on-order-stock-detail-modal/lock-on-order-stock-detail-modal.component';
import {
  LockStockDetailModalComponent,
  LockStockDetailResult,
} from './lock-stock-detail-modal/lock-stock-detail-modal.component';
import { LockStockModalComponent } from './lock-stock-modal/lock-stock-modal.component';
import { SaleOrderDetailModalComponent } from './sale-order-detail-modal/sale-order-detail-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { LockOnOrderStockModalComponent } from './lock-on-order-stock-modal/lock-on-order-stock-modal.component';

@Component({
  selector: 'app-dpo-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CoreModule,
    ThemeSharedModule,
    PageModule,
    CommercialUiModule,
    NgbModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    LockStockModalComponent,
    LockStockDetailModalComponent,
    ExtraFeeModalComponent,
    SmartBackButtonComponent,
    StatusLabelComponent,
    ExpandablePanelV2Component,
    SaleOrderDetailModalComponent,
    LockOnOrderStockDetailModalComponent,
    LockOnOrderStockAvailableDetailModalComponent,
    DeliveredDetailModalComponent,
    ApprovalCommentModalComponent,
    ExtraFeesInfoModalComponent,
    DpoDiscussionBoxComponent,
    FiltersPaneComponent,
    EditDPOItemDialogComponent,
    GkrAllocationTableComponent,
    HistoryModalComponent,
    NgbTooltip,
    LockOnOrderStockModalComponent,
  ],
  providers: [ListService, DPODetailViewService],
  templateUrl: './dpo-detail-page.component.html',
  styleUrls: ['./dpo-detail-page.component.scss'],
})
export class DPODetailComponent implements OnInit {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly dpoService = inject(DPOService);
  // protected readonly dpoDetailService = inject(DPODetailService);
  protected readonly list = inject(ListService);
  protected readonly track = inject(TrackByService);
  protected readonly titleService = inject(TitleService);
  protected readonly navigationHistoryService = inject(NavigationHistoryService);
  public readonly service = inject(DPODetailViewService);
  protected readonly toasterService = inject(ToasterService);

  @ViewChild('lockStockModal') lockStockModal!: LockStockModalComponent;
  @ViewChild('lockOnOrderStockModal') lockOnOrderStockModal!: LockOnOrderStockModalComponent;

  private readonly tableFilterPipe = new TableFilterPipe();

  AppPermissions = AppPermissions;
  historyActions = HistoryActions;
  requestStatusEnum = RequestStatusEnum;
  searchText = '';
  gkrSearchText = '';
  isFilterOpen = false;
  activeFilterCount = 0;
  currentFilters: { [key: string]: any } = {};
  filterFields: FilterField[] = [
    {
      key: 'needOrder',
      label: 'Need Order',
      type: 'checkbox',
      value: false,
      col: 'col-md-4 col-lg-2',
      icon: 'bi bi-exclamation-circle',
      checkedIcon: 'bi bi-exclamation-circle-fill',
    },
    {
      key: 'status',
      label: 'Status',
      type: 'select',
      options: [
        { value: RequestStatusEnum.IN_PROGRESS, label: 'In Progress' },
        { value: RequestStatusEnum.CANCELLED, label: 'Cancelled' },
        { value: RequestStatusEnum.CLOSED, label: 'Closed' },
      ],
      value: null,
      placeholder: 'All Statuses',
      col: 'col-md-6 col-lg-3',
    },
  ];

  hasHandledModalResult = false;
  // Modal properties
  showLockStockModal = false;
  showLockOnOrderStockAvailableDetailModal = false;
  showExtraFeeModal = false;
  showLockStockDetailModal = false;
  showSaleOrderDetailModal = false;
  showLockOnOrderStockDetailModal = false;
  showDeliveredDetailModal = false;
  showExtraFeesInfoModal = false;
  showLockOnOrderStockAutoModal = false;
  showGkrAllocationModal = false;
  lockOnOrderStockAutoLoading = false;
  isEditDPOItemDialogVisible = false;
  selectedDpoDetail: DPODetailDto | null = null;

  // Approve/Reject modal properties
  showApproveModal = false;
  showRejectModal = false;
  approveRejectLoading = false;

  // Discussion drawer properties
  showDiscussionBox = false;

  currentDetailIndex: number = 0;
  messagesCount = 0;

  selectedGkrId: string | null = null;
  gkrAllocationNote: string = '';

  // Table configuration - no longer needed for advanced data table

  // Search configuration
  searchColumns = ['golfaCode', 'model', 'unitPrice', 'amount', 'extraFeeNote'];
  // Filtered data getter
  get filteredDpoDetails(): DPODetailDto[] {
    let filteredData = this.service.dpoDetails;

    // Apply filter pane filters first
    if (this.currentFilters.needOrder) {
      filteredData = filteredData.filter(
        item => item.needDelivery > 0 && item.status === RequestStatusEnum.IN_PROGRESS,
      );
    }

    // Add other filter pane filters
    if (this.currentFilters.status) {
      filteredData = filteredData.filter(item => item.status === this.currentFilters.status);
    }

    // Apply search text filter if provided
    if (this.searchText && this.searchText.trim() !== '') {
      filteredData = this.tableFilterPipe.transform(filteredData, this.searchText, this.searchColumns);
    }

    return filteredData;
  }

  gkrSearchColumns = [
    'materialType',
    'dpoNo',
    'buyerId',
    'buyerTypeId',
    'buyerTypeDescription',
    'buyerShortName',
    'orderDate',
    'expirationDate',
    'totalAmount',
    'remark',
    'linkedNote',
  ];
  get gkrAllocations(): DpoGkrAllocationDto[] {
    const filteredData = this.service.gkrAllocations;
    if (this.gkrSearchText && this.gkrSearchText.trim() !== '') {
      return this.tableFilterPipe.transform(filteredData, this.gkrSearchText, this.gkrSearchColumns);
    }
    return filteredData;
  }

  get actionsTitle(): string {
    const title = {
      [HistoryActions.ConfirmLockOnOrder]: 'Confirm Lock On Order',
      [HistoryActions.ConfirmLockStock]: 'Confirm Lock Stock',
      [HistoryActions.Cancelled]: 'Cancel Item',
      [HistoryActions.ConfirmNote]: 'Confirm Note',
    };
    return title[this?.service?.currentAction];
  }
  get requireComment(): boolean {
    if (this.actionsTitle === 'Confirm Lock On Order' || this.actionsTitle === 'Confirm Lock Stock') {
      return false;
    }
    return true;
  }

  get completedLockOnOrder(): boolean {
    return (
      this.service.dpo?.status === RequestStatusEnum.CLOSED ||
      this.service.dpo?.status === RequestStatusEnum.IN_PROGRESS ||
      this.service.dpo?.status === RequestStatusEnum.CANCELLED
    );
  }

  get completedLockStock(): boolean {
    return (
      this.service.dpo.status !== RequestStatusEnum.SUBMITTED && this.service.dpo.status !== RequestStatusEnum.CONFIRMED
    );
  }

  get columnLockShipment(): boolean {
    return this.service.dpo.status !== RequestStatusEnum.SUBMITTED;
  }

  // Cell class function for accessing row data
  getTextClassForNeedOrder = (value: any, row: DPODetailDto): string => {
    return row.needDelivery === 0 ? '' : 'text-danger fw-bold';
  };

  // Card collapse state - kept for future use but not currently used
  isCardCollapsed: { [key: string]: boolean } = {
    dpoInformation: false,
    orderItems: false,
  };

  ngOnInit(): void {
    this.titleService.setTitle('DPO Detail | DPO Management');

    const dpoId = this.route.snapshot.paramMap.get('id');
    if (dpoId) {
      this.service.loadDPO(dpoId);
      this.service.loadDPODetails(dpoId);
      this.service.loadGKRAllocations(dpoId);
    }
  }

  // Formatter functions for advanced data table
  formatNumber = (value: any): string => {
    return value ? NumberHelper.convertToFormattedNumber(value, 0) : '0';
  };

  formatCurrency = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 0,
        }).format(value)}`
      : '';
  };

  formatLockStock = (value: any): string => {
    return value ? new Intl.NumberFormat('en-US').format(value) : '0';
  };

  formatDate = (value: any): string => {
    return value ? new Date(value).toLocaleDateString('en-GB') : '';
  };

  formatDateTime = (value: any): string => {
    return value
      ? new Date(value).toLocaleDateString('en-GB') +
          ' ' +
          new Date(value).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
      : '';
  };

  formatUsername = (value: any): string => {
    return value ? new UsernamePipe().transform(value) : '';
  };

  canSelectRow = (row: DPODetailDto): boolean => {
    // Hide selection checkbox for cancelled items
    return row.status !== RequestStatusEnum.CANCELLED && row.status !== RequestStatusEnum.CLOSED;
  };

  onRowClick(event: any): void {
    // Handle row click - check if it's a lock stock column click
    if (event.field === 'lockStock') {
      this.onLockStockDetailClick(event.row);
    }
  }

  onCellClick(event: CellClickEvent): void {
    switch (event.action) {
      case 'lockStock':
        this.onLockStockDetailClick(event.row);
        break;
      case 'saleOrder':
        this.onSaleOrderDetailClick(event.row);
        break;
      case 'lockShipment':
        this.onLockOnOrderStockDetailClick(event.row);
        break;
      case 'delivered':
        this.onDeliveredDetailClick(event.row);
        break;
      case 'onOrderStockAvailable':
        this.onLockOnOrderStockAvalilableStock(event.row);
        break;
      case 'extrafee':
        this.onExtraFeesInfoModalResult(event.row);
        break;
      default:
        console.error('Unknown cell action:', event.action);
    }
  }

  navigateBack(): void {
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }

  onLockStockDetailClick(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.hasHandledModalResult = false;
    this.showLockStockDetailModal = true;
  }

  onSaleOrderDetailClick(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.showSaleOrderDetailModal = true;
  }

  onLockOnOrderStockDetailClick(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.hasHandledModalResult = false;
    this.showLockOnOrderStockDetailModal = true;
  }

  onDeliveredDetailClick(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.showDeliveredDetailModal = true;
  }

  onLockOnOrderStockAvalilableStock(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.currentDetailIndex = this.service.dpoDetails.findIndex(d => d.id === dpoDetail.id);
    this.showLockOnOrderStockAvailableDetailModal = true;
  }

  toggleCollapse(section: string): void {
    this.isCardCollapsed[section] = !this.isCardCollapsed[section];
  }

  onViewHistory(): void {
    // TODO: Implement view history logic
    this.viewHistory(this.service.dpo.id);
  }

  confirmAction(action: HistoryActions) {
    this.service.performAction(action);
  }

  onCancel(action: HistoryActions): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.service.performAction(action);
  }

  onConfirmNote(action: HistoryActions): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.service.performAction(action);
  }

  onLockStock(): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.showLockStockModal = true;
  }

  onAutoLockStock(): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.showLockStockModal = true;
  }

  onAutoLockOnOrderStock(): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.showLockOnOrderStockAutoModal = true;
  }

  onExtraFee(): void {
    if (this?.service?.selectedItems.length === 0) {
      return;
    }
    this.showExtraFeeModal = true;
  }
  onLockOnOrderStockModalResult(result: any): void {
    // Reload DPO details to reflect changes
    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
    }
    // Clear selection after successful lock
    this.service.selectedItems = [];
    this.showLockOnOrderStockAutoModal = false;
  }
  onLockStockModalResult(result: any): void {
    // Reload DPO details to reflect changes
    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
    }
    // Clear selection after successful lock
    this.service.selectedItems = [];
    this.showLockStockModal = false;
  }

  onLockStockDetailModalResult(result: LockStockDetailResult): void {
    if (!result?.isDataChanged) return;
    // Handle only once
    if (this.hasHandledModalResult) return;
    this.hasHandledModalResult = true;

    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
    }

    this.service.selectedItems = [];
    this.selectedDpoDetail = null;

    this.showLockStockDetailModal = false;
    result.isDataChanged = false;
  }

  onLockOnOrderStockDetailModalResult(result: LockOnOrderStockResult): void {
    if (!result?.isDataChanged) return;
    // Handle only once
    if (this.hasHandledModalResult) return;
    this.hasHandledModalResult = true;

    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
    }

    this.service.selectedItems = [];
    this.selectedDpoDetail = null;

    this.showLockOnOrderStockDetailModal = false;
    result.isDataChanged = false;
  }

  onLockStockAvailableDetailModalResult($vent: any) {
    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
    }

    // this.showLockOnOrderStockAvailableDetailModal = false;
    // this.service.selectedItems = [];
    // this.selectedDpoDetail = null;
  }

  onNextDetailRequested(event: { nextIndex: number; nextDetailId: string }): void {
    if (!this.service.dpo?.id) {
      return;
    }

    // First reload the detail to get the latest data
    this.dpoService.getDPODetail(event.nextDetailId).subscribe({
      next: nextDetail => {
        // Update the current detail and index
        this.selectedDpoDetail = nextDetail;
        this.currentDetailIndex = event.nextIndex;

        // The modal will automatically reload its data through ngOnChanges
        // since the dpoDetail input has changed
      },
      error: error => {
        console.error('Error loading next detail:', error);
        this.toasterService.error('Failed to load next detail', 'Error');
      },
    });
  }

  onPreviousDetailRequested(event: { nextIndex: number; nextDetailId: string }): void {
    if (!this.service.dpo?.id) {
      return;
    }

    // First reload the detail to get the latest data
    this.dpoService.getDPODetail(event.nextDetailId).subscribe({
      next: previousDetail => {
        // Update the current detail and index
        this.selectedDpoDetail = previousDetail;
        this.currentDetailIndex = event.nextIndex;

        // The modal will automatically reload its data through ngOnChanges
        // since the dpoDetail input has changed
      },
      error: error => {
        console.error('Error loading previous detail:', error);
        this.toasterService.error('Failed to load previous detail', 'Error');
      },
    });
  }

  onExtraFeeModalResult(result: any): void {
    // Reload DPO details to reflect changes
    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
      this.service.loadDPO(this.service.dpo.id);
    }
    // Clear selection after successful operation
    this.service.selectedItems = [];
    this.showExtraFeeModal = false;
  }

  onExtraFeesInfoModalResult(dpoDetail: DPODetailDto): void {
    this.selectedDpoDetail = dpoDetail;
    this.showExtraFeesInfoModal = true;
  }

  clearSearch(): void {
    this.searchText = '';
    this.onFilterChange(this.getCurrentFilters());
  }

  clearGkrSearch(): void {
    this.gkrSearchText = '';
  }

  onSelectionChange(event: SelectionChangeEvent) {
    this.service.selectedItems = event.selectedItems as DPODetailDto[];
  }

  onLockOnOrderStockAutoModalResult(result: { action: any; comment: string }): void {
    if (!this.service?.selectedItems?.length) {
      this.showLockOnOrderStockAutoModal = false;
      return;
    }

    this.lockOnOrderStockAutoLoading = true;

    const input: DPOLockShipmentAutoDto = {
      dpoDetailIds: this.service.selectedItems.map(item => item.id),
      note: result.comment || '',
    };

    this.dpoService.lockShipmentAuto(input).subscribe({
      next: () => {
        this.toasterService.success('Lock on order stock completed successfully', 'Success');

        // Reload DPO details to reflect changes
        if (this.service.dpo?.id) {
          this.service.loadDPODetails(this.service.dpo.id);
        }

        // Clear selection after successful operation
        this.service.selectedItems = [];
        this.showLockOnOrderStockAutoModal = false;
      },
      error: error => {
        console.error('Error locking on order stock auto:', error);
        this.toasterService.error('Failed to lock on order stock', 'Error');
      },
      complete: () => {
        this.lockOnOrderStockAutoLoading = false;
      },
    });
  }

  onCloseLockOnOrderStockAutoModal(): void {
    this.showLockOnOrderStockAutoModal = false;
    this.lockOnOrderStockAutoLoading = false;
  }

  // Approve/Reject methods
  onApprove(): void {
    this.showApproveModal = true;
  }

  onReject(): void {
    this.showRejectModal = true;
  }

  onDelete(): void {
    this.service.confirmationService
      .warn('::DeleteConfirmationDPOMessage', '::AreYouSure', {
        messageLocalizationParams: [this.service.dpo?.dpoNo],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.dpoService.delete(this.service.dpo.id)),
      )
      .subscribe({
        next: () => {
          // smart back
          this.toasterService.success('DPO deleted successfully', 'Success');
          this.navigationHistoryService.smartBack(this.fallbackUrl);
        },
      });
  }

  onApproveModalResult(result: { action: HistoryActions; comment: string }): void {
    if (!this.service.dpo?.id) {
      console.error('No DPO ID available for confirm action');
      return;
    }

    this.approveRejectLoading = true;

    const input: NoteMetadataDto = {
      note: result.comment,
      concurrencyStamp: this.service.dpo.concurrencyStamp || '',
    };

    this.dpoService.approve(this.service.dpo.id, input).subscribe({
      next: () => {
        this.toasterService.success('DPO confirmed successfully', 'Success');
        this.showApproveModal = false;
        this.approveRejectLoading = false;

        // Navigate back to list
        this.navigationHistoryService.smartBack(this.fallbackUrl);
      },
      error: error => {
        console.error('Error approving DPO:', error);
        this.toasterService.error('Failed to confirm DPO', 'Error');
        this.approveRejectLoading = false;
      },
    });
  }

  onRejectModalResult(result: { action: HistoryActions; comment: string }): void {
    if (!this.service.dpo?.id) {
      console.error('No DPO ID available for reject action');
      return;
    }

    this.approveRejectLoading = true;

    const input: NoteMetadataDto = {
      note: result.comment,
      concurrencyStamp: this.service.dpo.concurrencyStamp || '',
    };

    this.dpoService.reject(this.service.dpo.id, input).subscribe({
      next: () => {
        this.toasterService.success('DPO rejected successfully', 'Success');
        this.showRejectModal = false;
        this.approveRejectLoading = false;

        // Navigate back to list
        this.navigationHistoryService.smartBack(this.fallbackUrl);
      },
      error: error => {
        console.error('Error rejecting DPO:', error);
        this.toasterService.error('Failed to reject DPO', 'Error');
        this.approveRejectLoading = false;
      },
    });
  }

  onCloseApproveModal(): void {
    this.showApproveModal = false;
    this.approveRejectLoading = false;
  }

  onCloseRejectModal(): void {
    this.showRejectModal = false;
    this.approveRejectLoading = false;
  }

  // Discussion drawer methods
  toggleDiscussion(): void {
    this.showDiscussionBox = !this.showDiscussionBox;
  }

  onCloseDiscussion(): void {
    this.showDiscussionBox = false;
  }

  // Fallback URL for smart back button
  get fallbackUrl(): string[] {
    return [AppRoutes.DPO.BASE, AppRoutes.DPO.LIST.BASE];
  }

  getMessagesCount(unreadCount: number) {
    this.messagesCount = unreadCount;
  }

  onFilterChange(filters: { [key: string]: any }) {
    this.currentFilters = filters;
    this.updateActiveFilterCount(filters);
  }

  updateActiveFilterCount(filters: { [key: string]: any }) {
    let count = 0;

    if (filters.needOrder) count++;
    if (filters.status) count++;
    if (filters.categories && filters.categories.length > 0) count++;
    if (filters.priceRange && (filters.priceRange.min !== null || filters.priceRange.max !== null)) count++;
    if (filters.dateRange && (filters.dateRange.from || filters.dateRange.to)) count++;
    if (filters.priority) count++;

    this.activeFilterCount = count;
  }

  private getCurrentFilters(): { [key: string]: any } {
    const filters: { [key: string]: any } = {};
    this.filterFields.forEach(field => {
      filters[field.key] = field.value;
    });
    return filters;
  }
  onDPOItemUpdated() {
    if (this.service.dpo?.id) {
      this.service.loadDPODetails(this.service.dpo.id);
      this.service.loadDPO(this.service.dpo.id);
      this.selectedDpoDetail = null;
    }
  }
  onActionClick(event: any) {
    const { action, row } = event;

    if (action === 'editModal') {
      this.selectedDpoDetail = row;
      this.isEditDPOItemDialogVisible = true;
    }
  }
  isEditableRow = (row: any): boolean => {
    return !!row && row.status !== 'CANCELLED' && row.status !== 'CLOSED' && row.status !== 'REJECTED';
  };

  materialTooltipMessage = (value: any, row: any) => {
    return `Material Code "${value}" has status "${row.materialStatus}".`;
  };

  materialTooltipCondition = (value: any, row: any) => {
    return row.materialStatus !== 'Active' && row.needDelivery > 0;
  };
  onAddGkrAllocation() {
    this.showGkrAllocationModal = true;
    this.service.loadAvailableGKRAllocations();
  }
  onGkrSelectionChange(event: DpoGkrAllocationDto | null) {
    this.selectedGkrId = event?.id;
  }

  onAllocateGkr() {
    // Handle allocate GKR action
    this.service.allocateGKRToDPO(this.selectedGkrId, this.gkrAllocationNote).subscribe({
      next: () => {
        this.toasterService.success('GKR allocated successfully', 'Success');
        // Reload GKR allocations to reflect changes
        if (this.service.dpo?.id) {
          this.service.loadGKRAllocations(this.service.dpo.id);
        }
        this.resetGkrAllocationModalInfo();
      },
      error: error => {
        console.error('Error allocating GKR:', error);
        this.toasterService.error('Failed to allocate GKR', 'Error');
      },
    });
  }

  private resetGkrAllocationModalInfo() {
    this.showGkrAllocationModal = false;
    this.selectedGkrId = null;
    this.gkrAllocationNote = '';
  }
  showHistoryModal = false;
  viewHistory(id: string) {
    this.service.viewHistory(id);
    this.showHistoryModal = true;
  }
  closeHistoryDialog() {
    this.showHistoryModal = false;
  }
}
