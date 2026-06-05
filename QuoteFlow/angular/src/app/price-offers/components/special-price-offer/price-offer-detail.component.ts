import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService, PermissionService } from '@abp/ng.core';
import { Confirmation, DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { CommonModule, DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { PriceOfferViewService } from '@app/price-offers/services/price-offer.service';
import { HistoryActions } from '@app/shared/action/components/action-label.component';
import { SelectionChangeEvent } from '@app/shared/components/advanced-data-table';
import {
  ApprovalCommentModalComponent,
  HistoryActions as HistoryActionsEnum,
} from '@app/shared/components/approval-comment/approval-comment.component';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { MetricProgressComponent, MetricRangeComponent, MetricsPanelComponent } from '@app/shared/components/metrics';
import { SmartBackButtonComponent } from '@app/shared/components/smart-back-button';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TemplateService } from '@proxy/general-templates';
import { ImportAddMoreItemsInput, SubmitProjectResultDto, WinningCustomerPerChannelDto } from '@proxy/price-offers';
import { PriceOfferCustomerDto } from '@proxy/price-offers/price-offer-customers';
import {
  PriceOfferDetailCancelDto,
  PriceOfferDetailDto,
  PriceOfferDetailImportDto,
} from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { catchError, filter, finalize, Observable, of, Subject, switchMap, takeUntil, timeout } from 'rxjs';
import type { ApplySpecialPriceRequest } from './apply-special-price-modal';
import { ApplySpecialPriceModalComponent } from './apply-special-price-modal';
import { CustomerInformationTableComponent } from './components/customer-information-table/customer-information-table.component';
import { DiscussionBoxComponent } from './components/discussion-box/discussion-box.component';
import { ImportInformationFormComponent } from './components/import-information-form/import-information-form.component';
import { ProjectInformationFormComponent } from './components/project-information-form/project-information-form.component';
import { GroupedTableComponent } from './grouped-table/grouped-table.component';
import { ImportPriceOfferComponent } from './import-price-offer/import-price-offer.component';
import { ImportPriceOfferType, PriceOfferSummaryData } from './price-offer.types';
import { ResultImportPriceOfferComponent } from './result-import-price-offer/result-import-price-offer.component';

const maxFileSizeMB: number = 50;
const OPERATION_TIMEOUT_MS = 30000; // 30 seconds timeout for operations

@Component({
  selector: 'app-price-offer-details',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    DatePipe,
    PageModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    MatCheckboxModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    NgSelectModule,
    ImportInformationFormComponent,
    ProjectInformationFormComponent,
    DiscussionBoxComponent,
    CustomerInformationTableComponent,
    GroupedTableComponent,
    ImportPriceOfferComponent,
    ResultImportPriceOfferComponent,
    HistoryModalComponent,
    StatusLabelComponent,
    ApproversModalComponent,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    ErrorDisplayComponent,
    MetricsPanelComponent,
    MetricProgressComponent,
    MetricRangeComponent,
    ApplySpecialPriceModalComponent,
    SmartBackButtonComponent,
    ApprovalCommentModalComponent,
    NgbTooltip,
  ],
  providers: [
    LoadingService,
    ListService,
    PriceOfferDetailViewService,
    PriceOfferViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './price-offer-detail.component.html',
  styleUrls: ['./price-offer-detail.component.scss'],
})
export class PriceOfferDetailComponent implements OnInit, OnDestroy {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(PriceOfferDetailViewService);
  protected readonly priceOfferService = inject(PriceOfferViewService);
  protected readonly titleService = inject(TitleService);
  protected readonly templateService = inject(TemplateService);
  protected readonly navigationHistoryService = inject(NavigationHistoryService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly cdr = inject(ChangeDetectorRef);
  protected readonly title = 'Price Offer Details';
  protected readonly permissionService = inject(PermissionService);

  private readonly tableFilterPipe = new TableFilterPipe();
  private pendingOperations = 0;
  private cleanupInProgress = false;

  @ViewChild('importPriceOffer') importPriceOffer: ImportPriceOfferComponent | undefined;
  @ViewChild('resultImportPriceOffer') resultImportPriceOffer: ResultImportPriceOfferComponent | undefined;
  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;
  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement>;

  priceOfferId: string | null;
  materialType: string | null = null;
  isDiscussionBoxOpen = false;
  showHistory = false;
  showProjectInfo = false;
  showSubmitProjectResultModal = false;
  showConfirmPreOrderResultModal = false;
  showApplySpecialPriceModal = false;
  messagesCount = 0;

  // Search and filtering properties
  searchText = '';
  showRejected = false;
  searchColumns = ['golfaCode', 'modelName', 'status'];

  public dataLoaded = false;
  public actionTitle: string | null;
  public actionComment: string | null;
  public currentAction: HistoryActions | null;

  // Project result submission properties
  public projectResult: {
    resultStatus: string | null;
    note: string;
    selectedCustomers: Map<string, string>;
  } = {
    resultStatus: null,
    note: '',
    selectedCustomers: new Map<string, string>(),
  };

  // Pre-order result confirmation properties
  public preOrderResult: {
    resultStatus: string | null;
    note: string;
  } = {
    resultStatus: null,
    note: '',
  };

  isCardCollapsed: { [key: string]: boolean } = {
    spoInformation: false,
    projectInformation: true,
    approvalInformation: true,
    customerInformation: true,
    projectResult: true,
    specialInputPrice: true,
    attachments: true,
  };

  isLoading = false;

  // Busy states for modals
  importBusy = false;
  submitBusy = false;
  actionBusy = false;
  projectResultBusy = false;
  preOrderResultBusy = false;
  isExportAllDetailBusy = false;
  isExportPriceOfferBusy = false;

  // Cancel related properties
  selectedItems: PriceOfferDetailDto[] = [];
  showCancelModal = false;
  cancelBusy = false;
  historyActions = HistoryActionsEnum;
  AppPermissions = AppPermissions;

  importPriceOfferType = ImportPriceOfferType;
  summaryData: PriceOfferSummaryData = {};

  // Subject for managing all subscriptions
  private destroy$ = new Subject<void>();

  // Cached filtered data to prevent pagination reset
  private _filteredPriceOfferDetails: PriceOfferDetailDto[] = [];
  private _lastFilterState = {
    sourceRef: null as any,
    showRejected: true,
    searchText: '',
    itemsLength: 0,
    changed: false,
  };

  get sumOfferAmount() {
    return this.service.formatCurrency(
      this.service.projectInfo?.items?.reduce((acc, curr) => acc + curr.offerAmount, 0) || 0,
    );
  }

  get sumStandardAmount() {
    return this.service.formatCurrency(
      this.service.projectInfo?.items?.reduce((acc, curr) => acc + curr.standardAmount, 0) || 0,
    );
  }

  get requestHistoryAction(): typeof HistoryActions {
    return HistoryActions;
  }

  get attachmentLength(): string {
    const count = this.service.selected?.attachments ? this.service.selected?.attachments.length : 0;
    return `${count} attachment${count !== 1 ? 's' : ''}`;
  }

  // Component fields to avoid getter performance issues
  isGPViewable: boolean = false;
  isLandedCostViewable: boolean = false;

  private updateSummaryData(): void {
    this.summaryData = {
      totalMEVNOfferAmount: this.service.selected?.totalMEVNOfferAmount,
      totalStandardAmount: this.service.selected?.totalStandardAmount,
      totalRequestedAmount: this.service.selected?.totalRequestedAmount,
      totalPriceToCustomer: this.service.selected?.totalPriceToCustomer,
      discountRatio: this.service.selected?.discountRatio,
      totalRequestedDiscountAmount: this.service.selected?.totalRequestedDiscountAmount,
      totalLandedCost: this.service.selected?.totalLandedCost,
      totalGP: this.service.selected?.totalGP,
    } as PriceOfferSummaryData;

    // Update flag fields to avoid getter performance issues
    this.isGPViewable = this.service?.selected?.flags?.isGPViewable ?? false;
    this.isLandedCostViewable = this.service?.selected?.flags?.isLandedCostViewable ?? false;

    // Force change detection after data updates to ensure flags are properly rendered
    this.cdr.detectChanges();
  }

  // Cached filtered data getter
  get filteredPriceOfferDetails() {
    const sourceData = this.service?.priceOfferDetails?.items || [];

    const currentState = {
      sourceRef: sourceData,
      showRejected: this.showRejected,
      searchText: this.searchText,
      itemsLength: sourceData.length,
      changed: false,
    };

    // recalculate if filter state has changed
    if (
      this._lastFilterState.sourceRef !== currentState.sourceRef ||
      this._lastFilterState.showRejected !== currentState.showRejected ||
      this._lastFilterState.searchText !== currentState.searchText ||
      this._lastFilterState.itemsLength !== currentState.itemsLength
    ) {
      let data = [...sourceData];

      // Filter Rejected
      if (!this.showRejected) {
        data = data.filter(item => item.status !== 'REJECTED');
      }

      // Filter Search
      if (this.searchText && this.searchText.trim() !== '') {
        data = this.tableFilterPipe.transform(data, this.searchText, this.searchColumns);
      }

      this._filteredPriceOfferDetails = data;
      this._lastFilterState = currentState;
    }

    return this._filteredPriceOfferDetails;
  }

  // Status summary getters
  get totalItemsCount(): number {
    return this.service?.priceOfferDetails?.items?.length || 0;
  }

  get filteredItemsCount(): number {
    return this.filteredPriceOfferDetails.length;
  }

  get rejectedItemsCount(): number {
    return this.service?.priceOfferDetails?.items?.filter(item => item.status === 'REJECTED').length || 0;
  }

  get approvedItemsCount(): number {
    return this.service?.priceOfferDetails?.items?.filter(item => item.status === 'APPROVED').length || 0;
  }

  get inProgressItemsCount(): number {
    return (
      this.service?.priceOfferDetails?.items?.filter(item => item.status && item.status === 'IN_PROGRESS').length || 0
    );
  }

  get cancelledItemsCount(): number {
    return (
      this.service?.priceOfferDetails?.items?.filter(item => item.status && item.status === 'CANCELLED').length || 0
    );
  }

  ngOnInit() {
    this.titleService.setTitle(this.title + ' | Special Price Offer');
    this.priceOfferId = this.route.snapshot.paramMap.get('id');

    if (this.priceOfferId) {
      this.dataLoaded = false;
      this.trackOperation(
        this.service
          .loadPriceOfferDetails(this.priceOfferId)
          .pipe(
            takeUntil(this.destroy$),
            timeout(OPERATION_TIMEOUT_MS),
            catchError(error => {
              this.service.toast.error('Failed to load price offer details. Please try again.');
              console.error('Load price offer details error:', error);
              return of(null);
            }),
          )
          .subscribe({
            next: () => {
              this.materialType = this.service?.selected?.materialType ?? null;
              this.dataLoaded = true;
              this.updateSummaryData();
              this.cdr.detectChanges();
            },
          }),
      );
    }
  }

  ngOnDestroy() {
    // Prevent additional operations during cleanup
    this.cleanupInProgress = true;

    // Close all modals
    this.closeAllModals();

    // Clean up DOM references
    if (this.fileInput?.nativeElement) {
      this.fileInput.nativeElement.value = '';
    }

    // Reset service state
    if (this.service) {
      this.service.showImportPriceOffer = false;
      this.service.showResultImportPriceOffer = false;
      this.service.openCommentRequestModal = false;
      this.service.resultImport = undefined;
      this.service.selectedFiles = [];
    }

    // Hide any loading indicators
    this.loadingService.hide();

    // Complete the destroy subject to clean up all subscriptions
    this.destroy$.next();
    this.destroy$.complete();

    // Clear any state
    this.importPriceOffer = undefined;
    this.resultImportPriceOffer = undefined;
    this.approversModalComponent = undefined;
  }

  /**
   * Closes all modals to prevent memory leaks
   */
  private closeAllModals(): void {
    this.showHistory = false;
    this.showProjectInfo = false;
    this.showSubmitProjectResultModal = false;
    this.showConfirmPreOrderResultModal = false;
    this.showApplySpecialPriceModal = false;

    if (this.service) {
      this.service.showImportPriceOffer = false;
      this.service.showResultImportPriceOffer = false;
      this.service.openCommentRequestModal = false;
    }

    if (this.approversModalComponent) {
      // this.approversModalComponent.visible = false;
    }
  }

  /**
   * Tracks an operation and ensures it doesn't prevent navigation
   */
  private trackOperation<T>(subscription: T): T {
    this.pendingOperations++;

    if (subscription && typeof (subscription as any).add === 'function') {
      (subscription as any).add(() => {
        this.pendingOperations--;
      });
    }

    return subscription;
  }

  onOpenImportPriceOffer(importMode: ImportPriceOfferType) {
    if (this.cleanupInProgress) return;

    this.service.importMode = importMode || ImportPriceOfferType.AddMoreItems;
    this.service.showImportPriceOffer = !this.service.showImportPriceOffer;
  }

  verifyData() {
    if (this.cleanupInProgress) return;

    const file = this.importPriceOffer?.fileImport;
    const comment = this.importPriceOffer?.importForm.get('commentAddMoreItems')?.value || '';
    console.log('Import comment:', comment);

    const importMode = this.service.importMode;

    if (importMode === ImportPriceOfferType.ProjectPriceOffer) {
      const isValidForm = this.importPriceOffer.importForm.valid;
      if (!isValidForm) {
        this.importPriceOffer.importForm.markAllAsTouched();
        this.service.toast.error('Please fill in all required fields before submitting.');
        return;
      }
    }
    const getPriceAuto = this.importPriceOffer?.importForm?.get('autoGetOfferPrice')?.value ?? false;
    if (file && file instanceof File) {
      const formData = new FormData();
      formData.append('file', file);
      this.importBusy = true;

      // Handle different import modes
      let validationObservable;
      if (importMode === ImportPriceOfferType.UpdateItemProperties) {
        validationObservable = this.service.proxyService.validateAndParseUpdateLandingCost(this.priceOfferId, formData);
      } else {
        validationObservable = this.service.proxyService.validateAndParseAddMoreItemDetail(
          this.priceOfferId,
          formData,
          getPriceAuto,
        );
      }

      this.trackOperation(
        validationObservable
          .pipe(
            takeUntil(this.destroy$),
            timeout(OPERATION_TIMEOUT_MS),
            finalize(() => {
              this.importBusy = false;
              this.clearFile();
            }),
            catchError(error => {
              this.handleImportError(error, 'validation');
              return of(null);
            }),
          )
          .subscribe({
            next: response => {
              if (response) {
                this.service.resultImport = response;
                this.service.showResultImportPriceOffer = true;
              }
            },
          }),
      );
    } else {
      this.service.resultImport = undefined;
      this.service.toast.error('Please select a file and fill in all required fields before submitting.');
    }
  }

  onSubmitImport() {
    if (this.cleanupInProgress) return;

    this.submitBusy = true;
    const importMode = this.service.importMode;
    let submitObservable;

    if (importMode === ImportPriceOfferType.UpdateItemProperties) {
      submitObservable = this.service.proxyService.importUpdateLandingCost(
        this.priceOfferId,
        this.service.resultImport,
      );
    } else {
      // check if this.service.resultImport is type of PriceOfferDetailImportDto, if not don't do
      if (
        !this.service.resultImport ||
        !(this.service.resultImport as ExcelValidationResult<PriceOfferDetailImportDto>)
      ) {
        this.submitBusy = false;
        return;
      }
      const comment = this.importPriceOffer?.importForm.get('commentAddMoreItems')?.value || '';
      const result = this.service.resultImport as ExcelValidationResult<PriceOfferDetailImportDto>;
      const payload: ImportAddMoreItemsInput = {
        validationResult: result,
        comment: comment,
      };
      submitObservable = this.service.proxyService.importAddMoreItemDetail(this.priceOfferId, payload);
    }

    this.trackOperation(
      submitObservable
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          finalize(() => {
            this.submitBusy = false;
            this.clearFile();
          }),
          catchError(error => {
            this.handleImportError(error, importMode);
            return of(null);
          }),
        )
        .subscribe({
          next: () => {
            const successMessage =
              importMode === ImportPriceOfferType.UpdateItemProperties
                ? 'Item properties updated successfully.'
                : 'Add More Item Detail imported successfully.';
            this.service.toast.success(successMessage);
            this.service.resultImport = undefined;
            this.service.showImportPriceOffer = false;
            this.service.showResultImportPriceOffer = false;

            if (!this.cleanupInProgress && this.priceOfferId) {
              this.trackOperation(
                this.service.loadPriceOfferDetails(this.priceOfferId).pipe(takeUntil(this.destroy$)).subscribe(),
              );
            }
          },
        }),
    );
  }

  onBackFromResult() {
    // Clear the result import data
    this.service.resultImport = undefined;
    this.service.showResultImportPriceOffer = false;
    this.clearFile();
  }

  onActionConfirmed() {
    if (this.cleanupInProgress) return;

    if (this.currentAction) {
      this.actionBusy = true;

      this.trackOperation(
        this.service
          .performAction(this.currentAction, this.actionComment)
          .pipe(
            takeUntil(this.destroy$),
            timeout(OPERATION_TIMEOUT_MS),
            finalize(() => {
              this.actionBusy = false;
            }),
            catchError(error => {
              this.service.toast.error(`Failed to ${this.currentAction} the request. Please try again.`);
              console.error('Action error:', error);
              return of(null);
            }),
          )
          .subscribe({
            next: result => {
              if (!result) return; // skip on failure

              this.service.toast.success(`Request ${this.currentAction} successfully.`);
              this.actionComment = null;
              this.currentAction = null;
              this.service.openCommentRequestModal = false;

              if (!this.cleanupInProgress) {
                if (this.currentAction !== HistoryActions.Cancelled) {
                  this.navigateBack();
                } else if (this.priceOfferId) {
                  this.trackOperation(
                    this.service.loadPriceOfferDetails(this.priceOfferId).pipe(takeUntil(this.destroy$)).subscribe(),
                  );
                }
              }
            },
          }),
      );
    } else {
      this.service.toast.error('Please provide a comment for the action.');
    }
  }

  toggleCardCollapse(cardId: string): void {
    if (this.cleanupInProgress) return;

    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }

  toggleDiscussionBox(): void {
    if (this.cleanupInProgress) return;

    this.isDiscussionBoxOpen = !this.isDiscussionBoxOpen;
  }

  viewHistory(): void {
    if (this.cleanupInProgress) return;

    this.service.approveHistory();
    this.service.approvalHistories = this.service.selected?.approvalHistories || [];
    this.showHistory = true;
  }

  closeHistoryDialog(): void {
    this.showHistory = false;
  }

  onShowApprovers(): void {
    if (this.cleanupInProgress) return;

    if (this.service.selected?.id) {
      this.trackOperation(
        this.priceOfferService
          .getListApprovers(this.service.selected.id)
          .pipe(
            takeUntil(this.destroy$),
            timeout(OPERATION_TIMEOUT_MS),
            catchError(error => {
              this.service.toast.error('Failed to load approvers.');
              console.error('Load approvers error:', error);
              return of(null);
            }),
          )
          .subscribe({
            next: approvers => {
              if (approvers && this.approversModalComponent) {
                this.approversModalComponent.openModal(approvers);
              }
            },
          }),
      );
    }
  }

  showSummaryInformation(): void {
    if (this.cleanupInProgress) return;

    this.service.showSummaryInformation();
    this.showProjectInfo = true;
  }

  closeProjectInfoDialog(): void {
    this.showProjectInfo = false;
  }

  navigateBack(): void {
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }

  // Fallback URL for smart back button
  get fallbackUrl(): string[] {
    return [AppRoutes.SPECIAL_PRICE_OFFERS.BASE, AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE];
  }

  // TrackBy function that incorporates visibility flags to force table re-render when columns change
  getTableTrackBy(): (index: number, item: any) => string {
    const flagsKey = `${this.isGPViewable}-${this.isLandedCostViewable}`;
    return (index: number, item: any) => {
      return `${item?.id || index}-${flagsKey}`;
    };
  }

  save(): void {
    if (this.cleanupInProgress) return;

    this.service.submitForm();
  }

  formatCurrency(value: number): string {
    return this.service.formatCurrency(value);
  }

  onActionStart(requestHistoryAction: HistoryActions) {
    if (this.cleanupInProgress) return;

    switch (requestHistoryAction) {
      case HistoryActions.Approved:
        this.actionTitle = 'Approve Request';
        this.service.requiredComment = false;
        break;
      case HistoryActions.Rejected:
        this.actionTitle = 'Reject Request';
        this.service.requiredComment = true;
        break;
      case HistoryActions.Closed:
        this.actionTitle = 'Close Request';
        this.service.requiredComment = true;
        break;
      case HistoryActions.Cancelled:
        this.actionTitle = 'Cancel Request';
        this.service.requiredComment = true;
        break;
    }

    this.currentAction = requestHistoryAction;
    this.service.openCommentRequestModal = true;
  }
  get isSubmitDisabled(): boolean {
    if (this.service.requiredComment) {
      return !this.actionComment || this.actionComment.trim() === '';
    }
    return false;
  }
  onCloseCommentRequestModal() {
    this.actionComment = null;
    this.service.onCloseCommentRequestModal();
  }

  // Project Result Modal Methods
  onOpenSubmitProjectResultModal() {
    if (this.cleanupInProgress) return;

    this.showSubmitProjectResultModal = true;
    this.resetProjectResult();
    this.autoSelectSingleCustomersPerChannel();
  }

  onCloseSubmitProjectResultModal() {
    this.showSubmitProjectResultModal = false;
    this.resetProjectResult();
  }

  private resetProjectResult() {
    this.projectResult = {
      resultStatus: null,
      note: '',
      selectedCustomers: new Map<string, string>(),
    };
  }

  private autoSelectSingleCustomersPerChannel() {
    if (!this.service?.customers?.items?.length) return;

    // Group customers by sale channel
    const customersByChannel = this.groupCustomersByChannel();

    // Auto-select customers where there's only one per channel
    customersByChannel.forEach((customers, channel) => {
      if (customers.length === 1) {
        this.projectResult.selectedCustomers.set(channel, customers[0].customerId);
      }
    });
  }

  private groupCustomersByChannel(): Map<string, PriceOfferCustomerDto[]> {
    const customersByChannel = new Map<string, PriceOfferCustomerDto[]>();

    this.service?.customers?.items?.forEach(customer => {
      if (!customersByChannel.has(customer.saleChannel)) {
        customersByChannel.set(customer.saleChannel, []);
      }
      customersByChannel.get(customer.saleChannel)!.push(customer);
    });

    return customersByChannel;
  }

  get sortedCustomers(): PriceOfferCustomerDto[] {
    if (!this.service?.customers?.items?.length) {
      return [];
    }

    return [...this.service.customers.items].sort((a, b) => {
      // First sort by sale channel, then by customer name
      if (a.saleChannel !== b.saleChannel) {
        return a.saleChannel.localeCompare(b.saleChannel);
      }
      return a.customerName.localeCompare(b.customerName);
    });
  }

  isCustomerSelected(customer: PriceOfferCustomerDto): boolean {
    return this.projectResult.selectedCustomers.get(customer.saleChannel) === customer.customerId;
  }

  isCustomerDisabled(customer: PriceOfferCustomerDto): boolean {
    // Disable checkbox if this is the only customer in the channel (auto-selected)
    const customersByChannel = this.groupCustomersByChannel();
    const customersInChannel = customersByChannel.get(customer.saleChannel) || [];
    return customersInChannel.length === 1;
  }

  onCustomerSelectionChange(customer: PriceOfferCustomerDto, event: Event) {
    if (this.cleanupInProgress) return;

    const checkbox = event.target as HTMLInputElement;

    if (checkbox.checked) {
      // Select this customer for the channel (unselect any other customer from the same channel)
      this.projectResult.selectedCustomers.set(customer.saleChannel, customer.customerId);
    } else {
      // Unselect this customer
      if (this.projectResult.selectedCustomers.get(customer.saleChannel) === customer.customerId) {
        this.projectResult.selectedCustomers.delete(customer.saleChannel);
      }
    }
  }

  onProjectResultChange() {
    if (this.cleanupInProgress) return;

    // Auto-select single customers when Win or PreOrder is selected
    if (this.projectResult.resultStatus === 'WON' || this.projectResult.resultStatus === 'PRE_ORDER') {
      this.autoSelectSingleCustomersPerChannel();
    } else {
      // Clear selections when Lost is selected
      this.projectResult.selectedCustomers.clear();
    }
  }

  getCustomerSelectionErrors(): string[] {
    const errors: string[] = [];

    if (this.projectResult.resultStatus === 'WON' || this.projectResult.resultStatus === 'PRE_ORDER') {
      // Get all unique channels from customers
      const channels = new Set(this.service?.customers?.items?.map(c => c.saleChannel) || []);

      // Check if all channels have a selection
      for (const channel of channels) {
        if (!this.projectResult.selectedCustomers.has(channel)) {
          errors.push(`Please select a customer for channel: ${channel}`);
        }
      }
    }

    return errors;
  }

  isSubmitProjectResultValid(): boolean {
    // Check if project result is selected
    if (!this.projectResult.resultStatus) {
      return false;
    }

    // Check if note is provided
    if (!this.projectResult.note?.trim()) {
      return false;
    }

    // If Win or PreOrder is selected, check customer selections
    if (this.projectResult.resultStatus === 'WON' || this.projectResult.resultStatus === 'PRE_ORDER') {
      return this.getCustomerSelectionErrors().length === 0;
    }

    return true;
  }

  onSubmitProjectResult() {
    if (this.cleanupInProgress) return;

    if (!this.isSubmitProjectResultValid()) {
      this.service.toast.error('Please fill in all required fields correctly.');
      return;
    }

    const winningCustomers: WinningCustomerPerChannelDto[] = [];

    if (this.projectResult.resultStatus === 'WON' || this.projectResult.resultStatus === 'PRE_ORDER') {
      // Convert selected customers to the required format
      this.projectResult.selectedCustomers.forEach((customerId, channelId) => {
        winningCustomers.push({
          channelId: channelId,
          customerId: customerId,
        });
      });
    }

    const submitData: SubmitProjectResultDto = {
      resultStatus: this.projectResult.resultStatus,
      winningCustomers: winningCustomers,
      note: this.projectResult.note.trim(),
    };

    this.projectResultBusy = true;

    this.trackOperation(
      this.service.proxyService
        .submitProjectResult(this.priceOfferId, submitData)
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          finalize(() => {
            this.projectResultBusy = false;
          }),
          catchError(error => {
            this.service.toast.error('Failed to submit project result. Please try again.');
            console.error('Error submitting project result:', error);
            return of(null);
          }),
        )
        .subscribe({
          next: () => {
            this.service.toast.success('Project result submitted successfully.');
            this.onCloseSubmitProjectResultModal();
            if (!this.cleanupInProgress) {
              this.navigateBack();
            }
          },
        }),
    );
  }

  // Pre-order Result Modal Methods
  onOpenConfirmPreOrderResultModal() {
    if (this.cleanupInProgress) return;

    this.showConfirmPreOrderResultModal = true;
    this.resetPreOrderResult();
  }

  onCloseConfirmPreOrderResultModal() {
    this.showConfirmPreOrderResultModal = false;
    this.resetPreOrderResult();
  }

  private resetPreOrderResult() {
    this.preOrderResult = {
      resultStatus: null,
      note: '',
    };
  }

  isConfirmPreOrderResultValid(): boolean {
    return !!(this.preOrderResult.resultStatus && this.preOrderResult.note?.trim());
  }

  onConfirmPreOrderResult() {
    if (this.cleanupInProgress) return;

    if (!this.isConfirmPreOrderResultValid()) {
      this.service.toast.error('Please fill in all required fields.');
      return;
    }

    this.preOrderResultBusy = true;

    this.trackOperation(
      this.service.proxyService
        .confirmPreOrderStatus(this.priceOfferId!, {
          resultStatus: this.preOrderResult.resultStatus!,
          note: this.preOrderResult.note,
        })
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          finalize(() => {
            this.preOrderResultBusy = false;
          }),
          catchError(error => {
            this.service.toast.error('Failed to confirm pre-order result. Please try again.');
            console.error('Error confirming pre-order result:', error);
            return of(null);
          }),
        )
        .subscribe({
          next: () => {
            this.service.toast.success('Pre-order result confirmed successfully!');
            this.onCloseConfirmPreOrderResultModal();
            if (!this.cleanupInProgress) {
              this.navigateBack();
            }
          },
        }),
    );
  }

  onItemHistoryClick(item: PriceOfferDetailDto) {
    if (this.cleanupInProgress) return;

    // For now, we'll show the same approval histories as the main price offer
    // In a real implementation, you might want to fetch item-specific history
    this.service.approveHistory();
    this.service.approvalHistories = item?.approvalHistories || [];
    this.showHistory = true;
  }

  hasImportActions(): boolean {
    const flags = this.service?.selected?.flags;
    return !!(flags?.canAddMoreItems || flags?.isDetailPropertiesChangeable);
  }

  hasExportActions(): boolean {
    return this.permissionService.getGrantedPolicy(AppPermissions.PriceOffers.ExportAllDetails);
  }

  hasDownloadTemplateActions(): boolean {
    return (
      this.permissionService.getGrantedPolicy(AppPermissions.PriceOffers.Uploads.AddMoreItems) ||
      this.permissionService.getGrantedPolicy(AppPermissions.PriceOffers.Uploads.ChangeItemProperties)
    );
  }

  hasApprovalActions(): boolean {
    const flags = this.service?.selected?.flags;
    return !!(
      flags?.isApprovable ||
      flags?.isRejectable ||
      flags?.isCancellable ||
      flags?.isClosable ||
      flags?.isProjectResultSubmittable ||
      flags?.isPreOrderResultConfirmable ||
      flags?.isSpecialInputPriceApplicable
    );
  }

  // Apply Special Price Modal Methods
  onOpenApplySpecialPriceModal() {
    if (this.cleanupInProgress) return;

    this.showApplySpecialPriceModal = true;
  }

  onApplySpecialPriceConfirmed(data: ApplySpecialPriceRequest) {
    if (this.cleanupInProgress) return;

    // Handle the apply special price confirmation from the modal
    // Force reload ALL price offer data: main data, details, histories, and summary
    if (this.priceOfferId) {
      this.dataLoaded = false;

      // Reset cached filter state to force recalculation
      this._filteredPriceOfferDetails = [];
      this._lastFilterState = { sourceRef: null, showRejected: true, searchText: '', itemsLength: 0, changed: false };

      this.trackOperation(
        this.service
          .loadPriceOfferDetails(this.priceOfferId)
          .pipe(
            takeUntil(this.destroy$),
            timeout(OPERATION_TIMEOUT_MS),
            // loadPriceOfferDetails loads main price offer (stored in service.selected)
            // Then wait for offer items to be fully loaded before updating UI
            switchMap(() => this.service.loadOfferItems(this.priceOfferId)),
            catchError(error => {
              this.service.toast.error('Failed to refresh price offer data.');
              console.error('Refresh price offer details error:', error);
              return of(null);
            }),
          )
          .subscribe({
            next: () => {
              this.dataLoaded = true;
              // Update approval histories for the main price offer history button
              this.service.approvalHistories = this.service.selected?.approvalHistories || [];
              // Update summary data (totals, metrics, flags)
              this.updateSummaryData();
              this.cdr.detectChanges();
            },
          }),
      );
    }
  }

  // Helper methods for special input price display
  getSpecialInputPriceDisplayName(): string {
    // For now, we'll show the account number as the display name
    // In a real scenario, you might want to fetch the full special input price details
    // or store more information in the PriceOfferDto
    return this.service.selected?.specialInputPriceAccountName || '';
  }

  getSpecialInputPriceAssignerInfo(): string {
    const assigner = this.service.selected;
    if (!assigner) return '';

    const fullName = assigner.specialInputPriceAssignerFullName;
    const username = assigner.specialInputPriceAssignerUsername;

    if (fullName && username) {
      return `${fullName} (${username})`;
    } else if (fullName) {
      return fullName;
    } else if (username) {
      return username;
    }

    return '';
  }

  deleteAttachment($event: any): void {
    if (this.cleanupInProgress) return;

    if ($event?.entry) {
      this.trackOperation(
        this.service.confirmationService
          .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
          .pipe(
            takeUntil(this.destroy$),
            filter(status => status === Confirmation.Status.confirm),
          )
          .subscribe(status => {
            if (status === Confirmation.Status.confirm) {
              this.trackOperation(
                this.service.uploadProxyService
                  .deleteFile($event.entry.id)
                  .pipe(takeUntil(this.destroy$))
                  .subscribe({
                    next: () => {
                      if (!this.cleanupInProgress && this.priceOfferId) {
                        this.trackOperation(this.service.loadPriceOfferDetails(this.priceOfferId).subscribe());
                      }
                    },
                    error: error => {
                      console.error('Error deleting attachment:', error);
                    },
                  }),
              );
            }
          }),
      );
    }
  }

  downloadFile(id: string, fileNameDB: string) {
    if (this.cleanupInProgress) return;

    this.trackOperation(
      this.service.uploadProxyService
        .downloadFile(id)
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          catchError(error => {
            this.service.toast.error('Failed to fetch file content (Not found file in file path');
            return of(null);
          }),
        )
        .subscribe({
          next: res => {
            if (res) {
              this.service.abpWindowService.downloadBlob(res, fileNameDB);
            }
          },
        }),
    );
  }

  onFileSelected(event: Event): void {
    if (this.cleanupInProgress) return;

    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const files = Array.from(input.files);

      const invalidFiles: string[] = [];

      const filesWithoutExtension = files.filter(file => !file.name.includes('.'));
      if (filesWithoutExtension.length > 0) {
        const filesWithoutExtensionNames = filesWithoutExtension.map(file => file.name).join(', ');
        invalidFiles.push(
          `The following files do not have an extension and cannot be uploaded: ${filesWithoutExtensionNames}`,
        );
      }

      const oversizedFiles = files.filter(file => file.size > maxFileSizeMB * 1024 * 1024);
      if (oversizedFiles.length > 0) {
        const oversizedFileNames = oversizedFiles.map(file => file.name).join(', ');
        invalidFiles.push(
          `The following files are too large to upload: ${oversizedFileNames}. The maximum size allowed is ${maxFileSizeMB}MB.`,
        );
      }

      if (invalidFiles.length > 0) {
        this.service.toast.error(invalidFiles.join('\n'), 'Error');
      }

      const validFiles = files.filter(file => file.name.includes('.') && file.size <= maxFileSizeMB * 1024 * 1024);

      if (validFiles.length > 0) {
        this.service.selectedFiles = validFiles;
        this.service.uploadFiles();
      }

      // Always clear the input to prevent memory leaks
      input.value = '';
    }
  }

  onDownloadTemplate(templateType: 'add-more-items' | 'change-item-properties') {
    if (this.cleanupInProgress) return;

    let templateObservable: Observable<Blob>;
    let fileName: string;

    switch (templateType) {
      case 'add-more-items':
        templateObservable = this.templateService.getPriceOfferAddMoreItemsTemplate();
        fileName = 'Template_PriceOffer_AddMoreItems.xlsx';
        break;
      case 'change-item-properties':
        templateObservable = this.templateService.getPriceOfferChangeItemPropertiesTemplate();
        fileName = 'Template_PriceOffer_ChangeItemProperties.xlsx';
        break;
      default:
        this.service.toast.error('Invalid template type');
        return;
    }

    this.trackOperation(
      templateObservable
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          catchError(error => {
            this.service.toast.error(error.message || 'Failed to download template');
            return of(null);
          }),
        )
        .subscribe({
          next: (response: Blob) => {
            if (response) {
              const url = window.URL.createObjectURL(response);
              const a = document.createElement('a');
              a.href = url;
              a.download = fileName;
              a.click();
              window.URL.revokeObjectURL(url);
            }
          },
        }),
    );
  }

  getDiscountRatioMax(): number | string {
    //potype = two left digits of price offer code
    const selected = this.service?.selected;
    const poType = selected?.priceOfferCode?.substring(0, 2) || '';
    if (poType === 'DS' && selected?.materialType === 'LVS' && selected?.discountRatioConfigured === 0) {
      if (selected?.totalMEVNOfferAmount < 500000000)
        // 500M as example
        return 0;
      else {
        return 'TBC';
      }
    }
    return selected.discountRatioConfigured * 100; // 10M as example
  }

  getOfferBudgetMax(): number {
    const baseAmount = this.service?.selected?.initialTotalMEVNOfferAmount || 0;
    const allowancePercent = this.service.spoAdjustmentAllowancePercent || 0;
    const diff = baseAmount * (allowancePercent / 100);
    return baseAmount + diff;
  }

  getOfferBudgetMin(): number {
    const baseAmount = this.service?.selected?.initialTotalMEVNOfferAmount || 0;
    const allowancePercent = this.service.spoAdjustmentAllowancePercent || 0;
    const diff = baseAmount * (allowancePercent / 100);
    return baseAmount - diff;
  }

  // Formatters for metric components
  currencyFormatter = (value: number): string => {
    return value.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
  };

  percentFormatter = (value: number): string => {
    return value.toFixed(2);
  };

  getDpoUsedLabel(): string {
    const percentage = this.service?.selected?.totalDpoUsedPercentage ?? 0;
    return `DPO Used - ${(percentage * 100).toFixed(1)}%`;
  }

  private clearFile() {
    if (this.importPriceOffer) {
      this.importPriceOffer.clearSelectedFile();
    }
  }

  exportPriceOffer() {
    if (this.cleanupInProgress) return;

    this.isExportPriceOfferBusy = true;
    this.service.loadingService.show();

    this.trackOperation(
      this.service.proxyService
        .getDownloadToken()
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          switchMap(({ token }) => {
            const params = {
              downloadToken: token,
              priceOfferId: this.priceOfferId,
            };
            return this.service.proxyService.getListAsExcelFile(params);
          }),
          finalize(() => {
            this.isExportPriceOfferBusy = false;
            this.service.loadingService.hide();
          }),
          catchError(error => {
            console.error('Export failed:', error);
            this.service.toast.error('Failed to export price offer. Please try again.');
            return of(null);
          }),
        )
        .subscribe({
          next: (response: Blob) => {
            if (response) {
              const link = document.createElement('a');
              link.href = window.URL.createObjectURL(response);
              link.download = `PriceOffer_${new Date().toISOString().slice(0, 10)}.xlsx`;
              link.click();
              window.URL.revokeObjectURL(link.href);
            }
          },
        }),
    );
  }

  exportAllDetail() {
    if (this.cleanupInProgress) return;

    this.isExportAllDetailBusy = true;
    this.service.loadingService.show();
    this.service.toast.info('Exporting all details...');

    this.trackOperation(
      this.service.proxyService
        .getDownloadToken()
        .pipe(
          takeUntil(this.destroy$),
          timeout(OPERATION_TIMEOUT_MS),
          switchMap(({ token }) => {
            return this.service.proxyService.getListDetailsAsExcelFile(this.priceOfferId, token);
          }),
          finalize(() => {
            this.isExportAllDetailBusy = false;
            this.service.loadingService.hide();
          }),
          catchError(error => {
            console.error('Export All Detail failed:', error);
            this.service.toast.error('Failed to export all detail. Please try again.');
            return of(null);
          }),
        )
        .subscribe({
          next: (response: Blob) => {
            if (response) {
              this.service.toast.success('Export all detail completed successfully.');
              const link = document.createElement('a');
              link.href = window.URL.createObjectURL(response);
              link.download = `PriceOfferDetails_${this.service.selected.priceOfferCode}.xlsx`;
              link.click();
              window.URL.revokeObjectURL(link.href);
            }
          },
        }),
    );
  }

  private handleImportError(error: any, context: string | ImportPriceOfferType): void {
    // Check if this is the specific DPO usage limit error
    if (error?.error?.code === 'QuoteFlow:10703017') {
      const details = error.error.details;
      if (details) {
        const message = `Cannot add more items beyond the configured limit when DPO has been used.

Base Amount: ${this.formatCurrency(details.baseAmount)}
Allowance: ${details.allowancePercent}%
Upper Limit: ${this.formatCurrency(details.upperLimit)}
Lower Limit: ${this.formatCurrency(details.lowerLimit)}
Current Amount: ${this.formatCurrency(details.currentAmount)}
Proposed Amount: ${this.formatCurrency(details.proposedAmount)}
Impact: ${this.formatCurrency(details.impact)}`;

        this.service.toast.error(message, 'DPO Usage Limit Exceeded');
      } else {
        this.service.toast.error('Cannot add more items beyond the configured limit when DPO has been used.');
      }
    } else {
      // Default error handling
      let errorMessage: string;
      if (context === 'validation') {
        errorMessage = 'Failed to validate file. Please check the file format and try again.';
      } else if (context === ImportPriceOfferType.UpdateItemProperties) {
        errorMessage = 'Failed to update item properties. Please try again.';
      } else {
        errorMessage = 'Failed to import price offer. Please try again.';
      }
      this.service.toast.error(errorMessage);
    }
  }

  // Cancel related methods
  get canCancelItem(): boolean {
    // Use the canCancelItem flag from the flagging service
    const flags = this.service?.selected?.flags;
    if (flags?.canCancelItem !== undefined) {
      return flags.canCancelItem;
    }

    // Fallback logic: Check if it's AP (Key Account) type and status allows cancellation
    const priceOfferCode = this.service?.selected?.priceOfferCode;
    const isAPType = priceOfferCode && priceOfferCode.toUpperCase().startsWith('AP');
    const approvalStatus = this.service?.selected?.approvalStatus;

    return isAPType && approvalStatus === 'APPROVED';
  }

  onSelectionChange(event: SelectionChangeEvent) {
    this.selectedItems = event.selectedItems as PriceOfferDetailDto[];
  }

  onCancel(): void {
    if (!this.selectedItems?.length) {
      this.service.toast.warn('Please select items to cancel');
      return;
    }
    this.showCancelModal = true;
  }

  onCancelModalResult(result: { action: HistoryActionsEnum; comment: string; dateTime?: string | null }): void {
    if (!result || !result.comment) {
      this.showCancelModal = false;
      return;
    }

    if (!this.selectedItems?.length) {
      this.service.toast.warn('No items selected for cancellation');
      this.showCancelModal = false;
      return;
    }

    this.cancelBusy = true;

    const input: PriceOfferDetailCancelDto = {
      priceOfferDetailIds: this.selectedItems.map(item => item.id),
      note: result.comment || 'Cancelled by user',
    };

    this.service.proxyService
      .cancelPriceOfferDetails(this.priceOfferId!, input)
      .pipe(
        finalize(() => {
          this.cancelBusy = false;
          this.showCancelModal = false;
        }),
      )
      .subscribe({
        next: cancelledDetails => {
          this.service.toast.success(`Successfully cancelled ${cancelledDetails.length} item(s)`);

          // Clear selection
          this.selectedItems = [];
          // Refresh data
          if (this.priceOfferId) {
            this.service.loadOfferItems(this.priceOfferId).subscribe({
              next: result => {
                this.updateSummaryData();
                this.cdr.detectChanges();
                this._filteredPriceOfferDetails = result.items;
              },
            });
            // this.dataLoaded = false;
            // this.trackOperation(
            //   this.service
            //     .loadPriceOfferDetails(this.priceOfferId)
            //     .pipe(
            //       takeUntil(this.destroy$),
            //       timeout(OPERATION_TIMEOUT_MS),
            //       catchError(error => {
            //         this.service.toast.error('Failed to load price offer details. Please try again.');
            //         console.error('Load price offer details error:', error);
            //         return of(null);
            //       }),
            //     )
            //     .subscribe({
            //       next: () => {
            //         this.dataLoaded = true;
            //         this.updateSummaryData();
            //         this.cdr.detectChanges();
            //       },
            //     }),
            // );
          }
        },
        error: error => {
          console.error('Error cancelling price offer details:', error);

          // Extract error message from response
          let errorMessage = 'Failed to cancel price offer details';
          if (error?.error?.error?.details) {
            errorMessage = error.error.error.details;
          } else if (error?.error?.error?.message) {
            errorMessage = error.error.error.message;
          }

          this.service.toast.error(errorMessage);
        },
      });
  }

  onCloseCancelModal(): void {
    this.showCancelModal = false;
    this.cancelBusy = false;
  }

  getMessagesCount(unreadCount: number) {
    this.messagesCount = unreadCount;
  }

  showHistoryAddMoreItems = false;
  Math = Math;

  addMoreItemsSearchText: string = '';
  filteredAddMoreItemsCount: number = 0;
  addMoreItemsPageSize = 20;
  addMoreItemsCurrentPage = 1;
  addMoreItemsTotalCount = 0;

  get pagedAddMoreItemHistories() {
    const searchLower = this.addMoreItemsSearchText?.toLowerCase().trim() || '';
    let filtered = this.service.addMoreItemHistories || [];

    if (searchLower) {
      filtered = filtered.filter(
        item =>
          item.materialCode?.toLowerCase().includes(searchLower) || item.model?.toLowerCase().includes(searchLower),
      );
    }

    this.filteredAddMoreItemsCount = filtered.length;

    const startIndex = (this.addMoreItemsCurrentPage - 1) * this.addMoreItemsPageSize;
    const endIndex = startIndex + this.addMoreItemsPageSize;
    return filtered.slice(startIndex, endIndex);
  }

  onAddMoreItemsPageChange(page: number) {
    this.addMoreItemsCurrentPage = page;
    this.cdr.detectChanges();
  }

  onAddMoreItemsSearch(): void {
    this.addMoreItemsCurrentPage = 1;
    this.cdr.detectChanges();
  }

  clearAddMoreItemsSearch(): void {
    this.addMoreItemsSearchText = '';
    this.addMoreItemsCurrentPage = 1;
    this.cdr.detectChanges();
  }

  // In component
  onSubmittedMoreItems(history: any) {
    this.showHistoryAddMoreItems = true;
    this.service.addMoreItemHistories = [];
    this.addMoreItemsSearchText = '';

    this.service.loadAddMoreItemHistories(history.id).subscribe(result => {
      setTimeout(() => {
        this.addMoreItemsTotalCount = this.service.addMoreItemHistories?.length || 0;
        this.filteredAddMoreItemsCount = this.addMoreItemsTotalCount;
        this.addMoreItemsCurrentPage = 1;
        this.cdr.detectChanges();
      }, 0);
    });
  }
}
