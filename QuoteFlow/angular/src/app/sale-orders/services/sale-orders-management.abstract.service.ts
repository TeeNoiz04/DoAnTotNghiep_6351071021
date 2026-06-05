import {
  ABP,
  AbpWindowService,
  ListService,
  PagedResultDto,
  PermissionService,
} from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { RouteStateService } from '@app/price-offers/services/route-state.service';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { LookupService } from '@proxy/general-lookups';
import { GetSaleOrdersInput, SaleOrderDto, SaleOrderService } from '@proxy/sale-orders';
import { LookupDto } from '@proxy/shared';
import { filter, finalize, map, Subscription, switchMap } from 'rxjs';
import { SaleOrderTypes } from '../components/sale-orders.model';
import { ApprovalHistoryDto } from '@proxy/approval-histories';

const _filterSessionActive = new Set<string>();

export abstract class AbstractSaleOrdersManagementViewService {
  public readonly proxyService = inject(SaleOrderService);
  public readonly list = inject(ListService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);
  public readonly toast = inject(ToasterService);
  public readonly confirmationService = inject(ConfirmationService);
  private readonly lookupService = inject(LookupService);
  public readonly routeStateService = inject(RouteStateService);
  protected readonly abpWindowService = inject(AbpWindowService);
  public readonly permissionService = inject(PermissionService);
  private currentSubscription: Subscription;
  private readonly FILTER_KEY = 'sale_orders_filters';
  isExportToExcelBusy = false;

  data: PagedResultDto<SaleOrderDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    skipCount: 0,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetSaleOrdersInput;

  filterAllData = {
    skipCount: 0,
    maxResultCount: 1000,
  } as GetSaleOrdersInput;
  selectedAllDatas: SaleOrderDto[] = [];
  searchForm: FormGroup | undefined;
  selected: SaleOrderDto | null = null;
  soCheckedList: SaleOrderDto[] | null = [];

  saleOrderForm: FormGroup | undefined;
  sapInfoForm: FormGroup | undefined;
  buyerOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];
  materialTypeOptions: any = [];
  materialGroupOptions: LookupDto<string>[] = [];
  stockCategoryOptions: LookupDto<string>[] = [];

  statusOptions = [
    {
      value: 'IN_PROGRESS',
      label: 'In Progress',
    },
    {
      value: 'CLOSED',
      label: 'Closed',
    },
  ];

  saveFiltersToStorage(): void {
    const values = this.searchForm?.getRawValue();
    if (values) {
      sessionStorage.setItem(this.FILTER_KEY, JSON.stringify(values));
      _filterSessionActive.add(this.FILTER_KEY);
    }
  }

  getSavedFilters(): any {
    if (!_filterSessionActive.has(this.FILTER_KEY)) {
      this.clearFiltersFromStorage();
      return null;
    }
    const raw = sessionStorage.getItem(this.FILTER_KEY);
    return raw ? JSON.parse(raw) : null;
  }

  clearFiltersFromStorage(): void {
    sessionStorage.removeItem(this.FILTER_KEY);
    _filterSessionActive.delete(this.FILTER_KEY);
  }

  buildSearchForm() {
    if (this.searchForm) return;

    const defaultValues = {
      orderDateMin: null,
      orderDateMax: null,
      soNo: null,
      sosapNo: null,
      statusCode: 'IN_PROGRESS',
      dpoNo: null,
      buyerId: null,
      materialCode: null,
      materialType: null,
      invoiceNo: null,
      golfaCode: null,
      buyerType: 'Distributor',
      model: null,
      soDateFrom: null,
      soDateTo: null,
      vatDateFrom: null,
      vatDateTo: null,
      materialGroup: null,
      taxCode: null,
    };

    const saved = this.getSavedFilters();
    const initialValues = saved ? { ...defaultValues, ...saved } : defaultValues;

    this.searchForm = this.fb.group({
      orderDateMin: [initialValues.orderDateMin],
      orderDateMax: [initialValues.orderDateMax],
      soNo: [initialValues.soNo],
      sosapNo: [initialValues.sosapNo],
      statusCode: [initialValues.statusCode],
      dpoNo: [initialValues.dpoNo],
      buyerId: [initialValues.buyerId],
      materialCode: [initialValues.materialCode],
      materialType: [initialValues.materialType],
      invoiceNo: [initialValues.invoiceNo],
      golfaCode: [initialValues.golfaCode],
      buyerType: [initialValues.buyerType],
      model: [initialValues.model],
      soDateFrom: [initialValues.soDateFrom],
      soDateTo: [initialValues.soDateTo],
      vatDateFrom: [initialValues.vatDateFrom],
      vatDateTo: [initialValues.vatDateTo],
      materialGroup: [initialValues.materialGroup],
      taxCode: [initialValues.taxCode],
    });

    this.filters = {
      ...this.filters,
      ...initialValues,
    };
  }

  buildSaleOrderForm() {
    this.saleOrderForm = this.fb.group({
      buyerType: [null, [Validators.required]],
      buyerTypeId: [null],
      buyer: [null, [Validators.required]],
      materialType: [null, Validators.required],
      orderDate: [null, Validators.required],
      soNo: [null],
      sO_VAT: [null, Validators.required],
      stockCategoryId: [null, Validators.required],
      note: [null],
    });
  }

  onChangeBuyerType(event: any) {
    if (event?.id) {
      this.searchForm.get('buyerId')?.setValue(null);
      this.buyerOptions = [];
      this.lookupService.getBuyerLookupByBuyerType(event.id).subscribe(result => {
        this.buyerOptions = result.items;
      });
    } else {
      this.buyerOptions = [];
    }
  }

  buildSAPInfoForm() {
    this.sapInfoForm = this.fb.group({
      sosapNo: [
        {
          value: '',
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
      sapBillingNo: [
        {
          value: '',
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
      sapdoNo: [
        {
          value: '',
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
      sapDeliveryDate: [
        {
          value: null,
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
      sapInvoice: [
        {
          value: '',
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
      sapInvoiceDate: [
        {
          value: null,
          disabled: !this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo),
        },
      ],
    });
  }

  loadBuyers(): void {
    this.lookupService.getBuyerLookup(false).subscribe({
      next: result => {
        const sortedBuyers = (result.items || [])
          .filter(x => !!x.displayCode)
          .sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.buyerOptions = sortedBuyers;
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
  }

  loadBuyerTypes(): void {
    this.lookupService.getBuyerTypeLookup({}).subscribe({
      next: result => {
        const sortedBuyers = (result.items || [])
          .filter(x => !!x.displayCode)
          .sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.buyerTypeOptions = sortedBuyers;
        this.getBuyerLookupByBuyerType(this.buyerTypeOptions[0].id);
      },
      error: error => {
        console.error('Error loading buyer types:', error);
      },
    });
  }

  getBuyerLookupByBuyerType(buyerTypeId: string): void {
    this.lookupService.getBuyerLookupByBuyerType(buyerTypeId).subscribe({
      next: result => {
        const sortedBuyers = (result.items || [])
          .filter(x => !!x.displayCode)
          .sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.buyerOptions = sortedBuyers;
      },
      error: error => {
        console.error('Error loading buyers by type:', error);
      },
    });
  }

  getBuyerLookupLoadData(buyerTypeCode: string): void {
    this.lookupService
      .getBuyerTypeLookup({ buyerTypeCode })
      .pipe(
        map(res => res.items?.[0]?.id),
        filter((buyerTypeId): buyerTypeId is string => !!buyerTypeId),
        switchMap(buyerTypeId => this.lookupService.getBuyerLookupByBuyerType(buyerTypeId)),
        map(res =>
          (res.items || [])
            .filter(x => !!x.displayCode)
            .sort((a, b) => a.displayCode!.localeCompare(b.displayCode!)),
        ),
      )
      .subscribe({
        next: buyers => {
          this.buyerOptions = buyers;
        },
        error: error => {
          console.error('Error loading buyer lookup:', error);
        },
      });
  }

  loadMaterialType() {
    this.materialTypeOptions = [
      { displayCode: 'FA', displayName: 'FA' },
      { displayCode: 'LVS', displayName: 'LVS' },
    ];
  }

  loadMaterialGroup() {
    this.lookupService
      .getMaterialGroupLookup()
      .subscribe(result => (this.materialGroupOptions = result.items));
  }

  loadStockCategories(): void {
    this.lookupService.getStockCategoryLookup().subscribe({
      next: result => {
        this.stockCategoryOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading stock categories:', error);
      },
    });
  }

  loadAllData() {
    this.filterAllData = { ...this.filters } as GetSaleOrdersInput;
    this.filterAllData.maxResultCount = 1000;
    this.proxyService.getList({ ...this.filterAllData }).subscribe({
      next: data => {
        this.selectedAllDatas = data.items;
      },
    });
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        soType: SaleOrderTypes.DPO,
      });

    const setData = (list: PagedResultDto<SaleOrderDto>) => {
      this.data = list;
    };

    if (!this.currentSubscription) {
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    } else {
      this.currentSubscription.unsubscribe();
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    }
  }

  clearFilters() {
    this.clearFiltersFromStorage();
    this.filters = {
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetSaleOrdersInput;
    this.searchForm?.reset();
    this.soCheckedList = [];
    this.buyerOptions = [];
    this.routeStateService.clearFilters();
    this.list.get();
    this.loadAllData();
  }

  onPage(page: any) {
    this.filters.skipCount = page.offset * page.pageSize;
    this.filters.maxResultCount = page.pageSize;
    this.list.get();
    this.routeStateService.saveFilters(this.filters, undefined, ['skipCount', 'maxResultCount']);
    this.loadAllData();
  }

  exportToExcel() {
    if (this.soCheckedList.length == 0) {
      this.toast.error('Please select at least one sale order to export.', 'Error');
      return;
    }
    this.isExportToExcelBusy = true;
    this.filters = this.searchForm.getRawValue();
    this.proxyService
      .getListAsExcelFile({
        ...this.filters,
        lstSO: this.soCheckedList?.map(so => so.id).join(',') || '',
        maxResultCount: 1000,
      })
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, 'SaleOrder_SAP_Data.xlsx');
      });
  }

  exportReportExcel() {
    this.filters = this.searchForm.getRawValue();
    this.proxyService
      .getListSODataAsExcelFile({
        ...this.filters,
        lstSO: this.soCheckedList?.map(so => so.id).join(',') || '',
        maxResultCount: 1000,
      })
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, 'SaleOrder_Report_Data.xlsx');
      });
  }

  get canDelete(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Delete) &&
      row?.statusCode !== 'CLOSED';
  }

  get canCreate(): boolean {
    return this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Create);
  }

  get canEdit(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Edit) &&
      row?.statusCode !== 'CLOSED';
  }

  get canEditItem(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Edit) &&
      row?.statusCode !== 'CLOSED';
  }

  get canEditSAP(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.EditSAPInfo) &&
      row?.statusCode == 'CLOSED';
  }

  get canReopen(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Reopen) &&
      row?.flags?.canReOpenSO;
  }

  get canViewHistory(): (row: any) => boolean {
    return (row: any) => this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Create);
  }

  get canConfirmDelivery(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.ConfirmDelivery) &&
      row?.flags?.canConfirmDelivery;
  }

  get canAddFromDPO(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Create) &&
      row?.statusCode !== 'CLOSED';
  }

  historySO: ApprovalHistoryDto[] = [];
  viewHistory(record: SaleOrderDto) {
    this.lookupService.getSOHistories(record.id).subscribe(history => {
      this.historySO = history;
    });
  }
}
