import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferViewService } from '@app/price-offers/services/price-offer.service';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { HistoryActions } from '@app/shared/action/components/action-label.component';
import {
  ActionClickEvent,
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
  CellClickEvent,
} from '@app/shared/components/advanced-data-table';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import {
  FilterField,
  FilterPaneComponent as FiltersPaneComponent,
} from '@app/shared/components/filters-pane/filters-pane.component';
import { SmartBackButtonComponent } from '@app/shared/components/smart-back-button';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { StatePageRequest } from '@app/shared/models/common.model';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { RequestStatusEnum, StatusLabelComponent } from '@app/shared/status/components/status-label.component';
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
import { SaleOrderDetailDto } from '@proxy/sale-orders/sale-order-details';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { filter, finalize, Subscription, switchMap } from 'rxjs';
import { SaleOrderTypes } from '../sale-orders.model';
import { AddFromDpoDialogComponent } from './components/add-from-dpo-dialog/add-from-dpo-dialog.component';
import { DPONoteInfoModalSOComponent } from './components/dpo-note-info-modal/dpo-note-info-modal.component';
import { EditSaleOrderItemDialogComponent } from './components/edit-sale-order-item-dialog/edit-sale-order-item-dialog.component';
import { ExtraFeesInfoModalSOComponent } from './components/extra-fees-info-modal/extra-fees-info-modal.component';
import { SaleOrdersFormComponent } from './components/sale-orders-form/sale-orders-form.component';
import { SAPInformationFormComponent } from './components/sap-info-form/sap-info-form.component';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
@Component({
  selector: 'app-sale-orders-details',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    PageModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    MatCheckboxModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    NgSelectModule,
    StatusLabelComponent,
    ApproversModalComponent,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    SaleOrdersFormComponent,
    AddFromDpoDialogComponent,
    EditSaleOrderItemDialogComponent,
    ExpandablePanelV2Component,
    ExtraFeesInfoModalSOComponent,
    DPONoteInfoModalSOComponent,
    SAPInformationFormComponent,
    SmartBackButtonComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    AppAdvancedDataTableComponent,
    FiltersPaneComponent,
    NgbTooltip,
  ],
  providers: [
    LoadingService,
    ListService,
    SaleOrdersManagementViewService,
    PriceOfferViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './sale-orders-details.component.html',
  styleUrls: ['./sale-orders-details.component.scss'],
})
export class SaleOrdersDetailComponent implements OnInit, OnDestroy {
  protected readonly navigationHistoryService = inject(NavigationHistoryService);
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(SaleOrdersManagementViewService);
  protected readonly titleService = inject(TitleService);
  protected readonly templateService = inject(TemplateService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly loadingService = inject(LoadingService);
  title = 'Sale Order Detail';
  statePageRequest: StatePageRequest;
  isNewRequestPage = false;
  isViewOnlyMode = false;
  loading = false;
  titlePage: string;

  public actionTitle: string | null;
  public actionComment: string | null;
  public currentAction: HistoryActions | null;
  public saleOrderId: string | null = null;
  public AppPermissions = AppPermissions;
  isAddFromDpoDialogVisible = false;

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

  pageSize = DEFAULT_PAGE_SIZE;
  currentPage = 1;
  totalCount = 0;

  // Busy states for modals
  importBusy = false;
  submitBusy = false;
  actionBusy = false;
  projectResultBusy = false;
  showDPONotesModal = false;
  showExtraFeesModal = false;
  searchText = '';
  searchColumns = ['model', 'golfaCode', 'dpoDetail.model', 'note', 'dpoDetail.dpoNo'];
  private readonly tableFilterPipe = new TableFilterPipe();

  exportPriceOfferOptions = [
    { label: 'Export Price Offer', value: 'price_offer' },
    { label: 'Export All Detail', value: 'all_detail' },
  ];

  isEditSaleOrderItemDialogVisible = false;
  selectedSaleOrderDetail: SaleOrderDetailDto = null;
  pagedData: SaleOrderDetailDto[] = [];
  filteredSaleOrderListDetails: SaleOrderDetailDto[] = [];
  get requestHistoryAction(): typeof HistoryActions {
    return HistoryActions;
  }

  private subscriptions: Subscription[] = [];

  ngOnInit() {
    this.service.buildSaleOrderForm();
    this.service.buildSAPInfoForm();
    this.initializeComponent();
    this.initData();
    this.subscriptions.push(
      this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
        this.loading = true;
        this.initializeComponent();
      }),
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  get fallbackUrl(): string[] {
    return [AppRoutes.SALE_ORDERS_MANAGEMENT.BASE, AppRoutes.SALE_ORDERS_MANAGEMENT.LIST.BASE];
  }

  onDelete(): void {
    if (!this.saleOrderId) {
      this.service.toast.warn('No sale order to delete');
      return;
    }
    this.confirmationService
      .warn('Are you sure you want to delete this sale order?', 'Confirm Deletion')
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => {
          this.loading = true;
          return this.service.proxyService.delete(this.saleOrderId);
        }),
        finalize(() => (this.loading = false)),
      )
      .subscribe({
        next: () => {
          this.service.toast.success('Sale order deleted successfully', 'Success');
          this.router.navigate([AppRoutes.SALE_ORDERS_MANAGEMENT.BASE, AppRoutes.SALE_ORDERS_MANAGEMENT.LIST.BASE]);
        },
        error: error => {
          console.error('Error deleting sale order:', error);
          this.service.toast.error(error?.error?.error?.message || 'Failed to delete sale order', 'Error');
        },
      });
  }

  onSave() {
    if (this.service.saleOrderForm.invalid) {
      Object.keys(this.service.saleOrderForm.controls).forEach(key => {
        const control = this.service.saleOrderForm.get(key);
        control.markAsTouched();
      });
      this.service.toast.warn('Please fill all required fields correctly.', 'Validation Error');
      return;
    }

    if (this.service.sapInfoForm.invalid) {
      Object.keys(this.service.sapInfoForm.controls).forEach(key => {
        const control = this.service.sapInfoForm.get(key);
        control.markAsTouched();
      });
      this.service.toast.warn('Please fill all required fields correctly.', 'Validation Error');
      return;
    }

    this.isLoading = true;
    const formValue = this.service.saleOrderForm.value;
    const sapFormValue = this.service.sapInfoForm.value;

    const buyerId = formValue.buyer || this.service?.selected?.buyerId || '';
    const buyerSelected = this.service.buyerOptions.find(buyer => buyer.id === buyerId);
    const stockCategoryId = formValue.stockCategoryId || this.service?.selected?.stockCategoryId || '';
    const buyerType = formValue.buyerType || this.service?.selected?.buyerType || '';
    const buyerTypeId = this.service.buyerTypeOptions.find(type => type.displayCode === buyerType)?.id || '';
    const soNo = formValue.soNo || this.service?.selected?.soNo || '';
    const materialType = formValue.materialType || this.service?.selected?.materialType || '';

    const saleOrderData: any = {
      soNo,
      materialType,
      buyerId: buyerId,
      buyerTypeId: buyerTypeId,
      buyerCode: buyerSelected?.displayCode || '',
      buyerName: buyerSelected?.displayName || '',
      orderDate: formValue.orderDate || this.service?.selected?.orderDate,

      stockCategoryId,
      note: formValue.note,
      sosapNo: sapFormValue.sosapNo,
      sapBillingNo: sapFormValue.sapBillingNo,
      sapdoNo: sapFormValue.sapdoNo,
      sapDeliveryDate: sapFormValue.sapDeliveryDate,
      sapInvoice: sapFormValue.sapInvoice,
      sapInvoiceDate: sapFormValue.sapInvoiceDate,
      // deliveryConfirmed: formValue.deliveryConfirmed,
      buyerType,
      soType: SaleOrderTypes.DPO,
    };

    // If editing existing record
    if (this.saleOrderId) {
      if (this.service.selected?.statusCode === 'DRAFT') {
        saleOrderData.sO_VAT = formValue.sO_VAT;
        if (saleOrderData.sO_VAT === -1) {
          saleOrderData.sO_VAT = null;
        }
      } else {
        saleOrderData.sO_VAT = this.service.selected?.sO_VAT;
      }
      const saleOrderDetails = (this.service.selected?.saleOrderDetails || []).map(detail => {
        let price = detail.price || 0;
        if (typeof price === 'string') {
          price = parseFloat((price as string).replace(/,/g, ''));
        }
        return {
          ...detail,
          price,
        };
      });
      saleOrderData.saleOrderDetails = saleOrderDetails;

      this.service.proxyService
        .update(this.saleOrderId, {
          ...saleOrderData,
          concurrencyStamp: this.service.selected?.concurrencyStamp,
        })
        .pipe(finalize(() => (this.isLoading = false)))
        .subscribe({
          next: result => {
            this.service.toast.success('Sale order updated successfully', 'Success');
            this.service.selected = result;
            this.loadData();
          },
          error: error => {
            console.error('Error updating sale order:', error);
            this.service.toast.error('Failed to update sale order', 'Error');
          },
        });
    } else {
      // Creating new record
      saleOrderData.sO_VAT = formValue.sO_VAT;
      if (saleOrderData.sO_VAT === -1) {
        saleOrderData.sO_VAT = null;
      }
      this.service.proxyService
        .create(saleOrderData)
        .pipe(finalize(() => (this.isLoading = false)))
        .subscribe({
          next: result => {
            this.service.toast.success('Sale order created successfully', 'Success');
            this.router.navigate([
              AppRoutes.SALE_ORDERS_MANAGEMENT.BASE,
              result.id,
              AppRoutes.SALE_ORDERS_MANAGEMENT.DETAILS.BASE,
            ]);
          },
          error: error => {
            console.error('Error creating sale order:', error);
            this.service.toast.error('Failed to create sale order', 'Error');
          },
        });
    }
  }

  private initializeComponent(): void {
    this.saleOrderId = this.route.snapshot.paramMap.get('id');
    this.statePageRequest = this.setStatePageRequest();
    this.isNewRequestPage = this.statePageRequest === StatePageRequest.New;
    this.isViewOnlyMode = this.statePageRequest === StatePageRequest.View;

    this.initTitleAndRequestType();

    if (!this.saleOrderId) {
      // TODO: Initialize form for new request
      this.loading = false;
    } else {
      this.loading = true;
      this.loadData();
    }
  }

  private loadData(): void {
    this.service.proxyService.get(this.saleOrderId).subscribe({
      next: res => {
        this.loading = false;

        this.service.saleOrderForm.controls.soNo.disable();
        this.service.selected = res;
        this.totalCount = res.saleOrderDetails?.length || 0;
        const buyerType = this.service.selected.buyerType;
        this.service.saleOrderForm.patchValue({
          soNo: this.service.selected.soNo,
          materialType: this.service.selected.materialType,
          buyerType,
          buyer: this.service.selected.buyerId,
          buyerCode: this.service.selected.buyerCode,
          buyerName: this.service.selected.buyerName,
          orderDate: this.service.selected.orderDate,
          sO_VAT: this.service.selected.sO_VAT ?? -1,
          stockCategoryId: this.service.selected.stockCategoryId,
          note: this.service.selected.note,
        });
        this.service.sapInfoForm.patchValue({
          sosapNo: this.service.selected.sosapNo,
          sapBillingNo: this.service.selected.sapBillingNo,
          sapdoNo: this.service.selected.sapdoNo,
          sapDeliveryDate: this.service.selected.sapDeliveryDate,
          sapInvoice: this.service.selected.sapInvoice,
          sapInvoiceDate: this.service.selected.sapInvoiceDate,
        });
        if (buyerType) {
          setTimeout(() => {
            this.service.getBuyerLookupLoadData(buyerType);
          }, 200);
        }
        this.updateFormControlStates();
        this.setPage(1);
      },
      error: () => {
        this.service.toast.error('Failed to load price offer details.');
      },
    });
  }

  private updateFormControlStates(): void {
    const form = this.service.saleOrderForm;
    const sapForm = this.service.sapInfoForm;
    const hasDetails = this.service.selected?.saleOrderDetails?.length > 0;
    if (!this.isNewRequestPage && form.get('soNo')) {
      form.get('soNo').disable();
    }

    if (!this.isNewRequestPage && this.service.selected?.statusCode === 'CLOSED') {
      form.get('note').disable();
      form.get('orderDate').disable();
    }

    const keyFields = ['buyerType', 'sO_VAT', 'buyer', 'materialType', 'stockCategoryId'];
    const fullKeyFields = [
      'soNo',
      'sO_VAT',
      'orderDate',
      'buyerType',
      'buyer',
      'materialType',
      'stockCategoryId',
      'sapBillingNo',
      'note',
    ];
    const sapFullKeyField = ['sosapNo', 'sapBillingNo', 'sapdoNo', 'sapDeliveryDate', 'sapInvoice', 'sapInvoiceDate'];
    if (hasDetails && this.service.selected?.flags.canEditSAPInfo !== true) {
      fullKeyFields.forEach(field => {
        if (form.get(field)) {
          form.get(field).disable();
        }
      });

      sapFullKeyField.forEach(field => {
        if (sapForm.get(field)) {
          sapForm.get(field).disable();
        }
      });
    } else if (hasDetails) {
      keyFields.forEach(field => {
        if (form.get(field)) {
          form.get(field).disable();
        }
      });
    } else {
      keyFields.forEach(field => {
        if (form.get(field)) {
          form.get(field).enable();
        }
      });
    }
    form.markAsPristine();
    sapForm.markAsPristine();
  }

  private initData(): void {
    this.service.loadBuyerTypes();
    this.service.loadMaterialType();
    this.service.loadStockCategories();
  }

  private initTitleAndRequestType(): void {
    this.route.paramMap.subscribe(params => {
      const action = this.route.snapshot.url.slice(-1)[0].path;
      const titlePageVal = this.getTitleFromAction(action, this.title);
      this.titlePage = titlePageVal;
      this.titleService.setTitle(titlePageVal + ' | ' + this.title);
    });
  }

  private getTitleFromAction(action: string, baseTitle: string): string {
    switch (action) {
      case AppRoutes.NEW:
        return `New ${baseTitle}`;
      case AppRoutes.EDIT:
        return `Edit ${baseTitle}`;
      case AppRoutes.APPROVAL:
        return `${baseTitle} Approval`;
      case AppRoutes.VIEW:
        return `View ${baseTitle}`;
      default:
        return baseTitle;
    }
  }

  private setStatePageRequest(): StatePageRequest {
    const currentRoute = this.router.url;
    const newMode = currentRoute.includes(AppRoutes.NEW);
    const editMode = this.router.url.includes(AppRoutes.EDIT);
    const approvalMode = currentRoute.includes(AppRoutes.APPROVAL);

    if (newMode) {
      return StatePageRequest.New;
    }
    if (editMode) {
      return StatePageRequest.Edit;
    }
    if (approvalMode) {
      return StatePageRequest.Approval;
    }
    return StatePageRequest.View;
  }

  get filteredSaleOrderDetails(): SaleOrderDetailDto[] {
    let filtered = this.service?.selected?.saleOrderDetails || [];

    // Apply filter pane filters first
    if (this.currentFilters.needOrder) {
      filtered = filtered.filter(item => item.qty > 0 && item.statusCode === RequestStatusEnum.IN_PROGRESS);
    }

    // Apply status filter
    if (this.currentFilters.status) {
      filtered = filtered.filter(item => item.statusCode === this.currentFilters.status);
    }

    // Apply search text filter last
    if (this.searchText && this.searchText.trim() !== '') {
      filtered = this.tableFilterPipe.transform(filtered, this.searchText, this.searchColumns);
    }

    return filtered;
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }

  navigateBack(): void {
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }

  clearSearch(): void {
    // this.searchText = '';
    // this.setPage(1);
    this.searchText = '';
    this.onFilterChange(this.getCurrentFilters());
  }

  currentFilters: { [key: string]: any } = {};
  onFilterChange(filters: { [key: string]: any }) {
    this.currentFilters = filters;
    this.updateActiveFilterCount(filters);
  }

  onEditSaleOrderDetail(event: { entry: SaleOrderDetailDto }): void {
    this.selectedSaleOrderDetail = event.entry;
    this.isEditSaleOrderItemDialogVisible = true;
  }

  get isEditableRow(): (row: SaleOrderDetailDto) => boolean {
    return (row: SaleOrderDetailDto): boolean => {
      return this.service.canEditItem(row);
    };
  }

  // canDeleteItem giữ nguyên
  get canDeleteItem(): boolean {
    return this.service.selected?.saleOrderDetails?.length > 1 && this.service.canDelete(this.service.selected);
  }

  // Sửa lại hàm này
  onSaleOrderItemUpdated(updatedItem: SaleOrderDetailDto): void {
    if (!updatedItem || !this.service?.selected?.saleOrderDetails) {
      return;
    }

    const list = this.service.selected.saleOrderDetails;
    const index = list.findIndex(item => item.id === updatedItem.id);

    if (index !== -1) {
      const current = list[index];

      list[index] = {
        ...current,
        price: updatedItem.price ?? current.price,
        note: updatedItem.note ?? current.note,
        extrafee: updatedItem.extrafee ?? current.extrafee,
        amount: updatedItem.amount ?? current.amount,
        concurrencyStamp: updatedItem.concurrencyStamp ?? current.concurrencyStamp,
      };

      this.service.selected.saleOrderDetails = [...list];
    }
    this.updateFormControlStates();
    this.setPage(1);
  }

  onRemoveSaleOrderDetail(event: { entry: SaleOrderDetailDto }): void {
    const detailId = event.entry.id;
    this.service.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.proxyService.deleteDetail(detailId)),
      )
      .subscribe(() => {
        this.service.toast.success('Sale order item deleted successfully', 'Success');
        if (this.service?.selected?.saleOrderDetails) {
          this.service.selected.saleOrderDetails = this.service.selected.saleOrderDetails.filter(
            item => item.id !== detailId,
          );
          this.service.selected.saleOrderDetails.forEach((item, index) => {
            item.no = index + 1;
          });
          const x = this.filteredSaleOrderDetails;
          this.setPage(1);
          this.updateFormControlStates();
        }
      });
  }

  onExtraFeesInfoItemUpdated($event: any) {
    if (this.saleOrderId) {
      this.service.proxyService.get(this.saleOrderId).subscribe({
        next: res => {
          this.service.selected = res;
          const x = this.filteredSaleOrderDetails;
          this.setPage(1);
        },
        error: () => {
          this.service.toast.error('Failed to refresh sale order details.');
        },
      });
    }
  }

  onHistory(id: string): void {}

  openAddFromDPODialog(): void {
    this.isAddFromDpoDialogVisible = true;
  }

  onItemsAdded(items: SaleOrderDetailDto[]): void {
    if (this.saleOrderId) {
      this.service.proxyService.get(this.saleOrderId).subscribe({
        next: res => {
          this.service.selected = res;
          const x = this.filteredSaleOrderDetails;
          this.setPage(1);
        },
        error: () => {
          this.service.toast.error('Failed to refresh sale order details.');
        },
      });
    }
  }

  onItemsAddedAndDisable(items: SaleOrderDetailDto[]): void {
    const form = this.service.saleOrderForm;

    if (!this.isNewRequestPage && form.get('soNo')) {
      form.get('soNo').disable();
    }
    if (!this.isNewRequestPage && this.service.selected?.statusCode === 'CLOSED') {
      form.get('note').disable();
      form.get('orderDate').disable();
    }

    const keyFields = ['buyerType', 'sO_VAT', 'buyer', 'materialType', 'stockCategoryId'];

    keyFields.forEach(field => {
      if (form.get(field)) {
        form.get(field).disable();
      }
    });
    form.markAsPristine();
  }

  openViewModal(detail: any): void {
    if (detail?.action === 'extrafee') {
      this.selectedSaleOrderDetail = detail?.row;
      this.showExtraFeesModal = true;
    } else if (detail?.action === 'soNote' || detail?.action === 'dpoConfirmNotes') {
      this.selectedSaleOrderDetail = detail?.row;
      this.showDPONotesModal = true;
    }
  }

  onConfirmDelivery(): void {
    this.confirmationService
      .info('::ConfirmDeliveryMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.proxyService.confirmDeliveryById(this.saleOrderId)),
      )
      .subscribe({
        next: () => {
          this.service.toast.success('Delivery confirmed successfully', 'Success');
          this.loadData(); // Refresh the data to update the flags
        },
        error: error => {
          console.error('Error confirming delivery:', error);
          this.service.toast.error('Failed to confirm delivery', 'Error');
        },
      });
  }

  onReopen(): void {
    this.confirmationService
      .warn('::ReopenConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => {
          this.loading = true;
          return this.service.proxyService.reOpenSO(this.saleOrderId);
        }),
      )
      .subscribe({
        next: () => {
          this.service.toast.success('Sale order reopened successfully', 'Success');
          this.loadData(); // Refresh the data to update the flags
          this.loading = false;
        },
        error: error => {
          console.error('Error reopening sale order:', error);
          this.service.toast.error('Failed to reopen sale order', 'Error');
          this.loading = false;
        },
      });
  }

  setPage(page: number) {
    this.currentPage = page;
    const start = (page - 1) * this.pageSize;
    const end = start + this.pageSize;
    const text = this.searchText?.trim().toLowerCase();
    if (!text) {
      this.pagedData = this.filteredSaleOrderDetails.slice(start, end);
      this.totalCount = this.filteredSaleOrderDetails.length;
    } else {
      this.pagedData = this.filteredSaleOrderDetails
        .filter(
          item =>
            item.dpoDetail?.dpoNo.toLowerCase().includes(text) ||
            item.golfaCode?.toLowerCase().includes(text) ||
            item.dpoDetail.model?.toLowerCase().includes(text) ||
            item.note?.toLowerCase().includes(text),
        )
        .slice(start, end);
      this.totalCount = this.filteredSaleOrderDetails.filter(
        item =>
          item.dpoDetail?.dpoNo.toLowerCase().includes(text) ||
          item.golfaCode?.toLowerCase().includes(text) ||
          item.dpoDetail.model?.toLowerCase().includes(text) ||
          item.note?.toLowerCase().includes(text),
      ).length;
    }

    this.applySearch();
  }

  onChangedPaging(event: any) {
    this.setPage(event);
  }
  onSearchChange() {
    this.applySearch();
  }
  applySearch() {
    const text = this.searchText?.trim().toLowerCase();

    if (!text) {
      this.filteredSaleOrderListDetails = [...this.pagedData];
      return;
    }

    this.filteredSaleOrderListDetails = [...this.pagedData];
  }
  isFilterOpen = false;
  activeFilterCount = 0;
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
        { value: RequestStatusEnum.CLOSED, label: 'Closed' },
      ],
      value: null,
      placeholder: 'All Statuses',
      col: 'col-md-6 col-lg-3',
    },
  ];
  private getCurrentFilters(): { [key: string]: any } {
    const filters: { [key: string]: any } = {};
    this.filterFields.forEach(field => {
      filters[field.key] = field.value;
    });
    return filters;
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

  onCellClick(event: CellClickEvent): void {
    switch (event.action) {
      case 'soNote':
        this.openViewModal(event);
        break;
      case 'dpoConfirmNotes':
        this.openViewModal(event);
        break;
      case 'extrafee':
        this.openViewModal(event);
        break;
      default:
        console.error('Unknown cell action:', event.action);
    }
  }

  onActionClick(event: ActionClickEvent): void {
    switch (event.action) {
      case 'edit':
      case 'editModal':
        // Both actions open the modal without triggering inline edit mode
        this.handleEditAction(event.row);
        break;
      case 'delete':
        this.handleDeleteAction(event.row);
        break;
    }
  }

  private handleEditAction(row: any): void {
    this.onEditSaleOrderDetail({ entry: row });
  }

  private handleDeleteAction(row: any): void {
    this.onRemoveSaleOrderDetail({ entry: row });
  }
}
