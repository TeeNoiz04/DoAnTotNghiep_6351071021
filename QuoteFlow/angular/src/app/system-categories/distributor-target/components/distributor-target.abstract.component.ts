import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { BuyerDto } from '@proxy/buyers';
import { LookupService } from '@proxy/general-lookups';
import { SalesAssignmentDto } from '@proxy/sales-assignments';
import { LookupDto } from '@proxy/shared';
import { debounceTime, filter, Subject, takeUntil } from 'rxjs';
import { DistributorTargetDetailViewService } from '../services/distributor-target-detail.service';
import { DistributorTargetViewService } from '../services/distributor-target.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractDistributorTargetComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(DistributorTargetViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);
  public readonly serviceDetail = inject(DistributorTargetDetailViewService);
  public readonly lookupService = inject(LookupService);
  protected proxyLookupService = inject(LookupService);

  protected title = 'Buyer Target';

  private destroy$ = new Subject<void>();
  isLoading = true;
  dataBuyer: BuyerDto[] = [];
  private searchTextChanged$ = new Subject<string>();
  locationOptions: { value: string; label: string }[] = [];
  projectTypeOption: { value: string; label: string }[] = [
    { value: 'FA', label: 'FA' },
    { value: 'LVS', label: 'LVS' },
  ];
  buyerOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];
  AppPermissions = AppPermissions;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('Buyer Target');

    // --- LOGIC MỚI: CHẠY TUẦN TỰ ---

    // 1. Gọi API lấy năm trước
    this.service.generateFiscalYears().subscribe({
      next: () => {
        this.getDefault();
        this.getBuyerType();
        this.service.hookToQuery();
      },
      error: err => console.error(err),
    });

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.service.hookToQuery();
      });

    this.searchTextChanged$.pipe(debounceTime(500), takeUntil(this.destroy$)).subscribe(() => {
      this.onLoad();
    });

    // Lưu ý: Dòng this.service.hookToQuery() cũ ở cuối hàm ngOnInit
    // nên XÓA ĐI hoặc comment lại, vì ta đã chuyển nó vào trong subscribe ở trên rồi.
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchTextChange(text: string) {
    this.searchTextChanged$.next(text);
  }

  onLoad() {
    this.list.get();
    this.service.generateFiscalYears();
  }

  clearFilters() {
    this.service.clearFilters();
  }

  showForm() {
    this.serviceDetail.showForm();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.selectedMaterials = {
      fa: false,
      lvs: false,
    };
    this.serviceDetail.showForm();
  }

  update(record: SalesAssignmentDto) {
    this.serviceDetail.update(record);
  }

  delete(record: SalesAssignmentDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    // this.service.exportToExcel();
  }

  // Add a method to update title when viewing a specific buyer
  viewBuyerDetails(record: SalesAssignmentDto) {
    // Update the page title to include the buyer name
    this.titleService.setTitle(`Buyer: ${record.saleFullName}`);

    // Call your existing detail view method
    this.update(record);
  }

  getBuyer() {
    this.lookupService
      .getBuyerLookup(false)
      // .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.buyerOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });
  }
  getBuyerType() {
    this.lookupService
      .getBuyerTypeLookup({})
      // .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.buyerTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });
  }
  onBuyerTypeChange(selectedValue: any) {
    this.lookupService.getBuyerLookupByBuyerType(selectedValue.id).subscribe({
      next: result => {
        this.buyerOptions = result?.items || [];
        this.service.filters.buyerId = null;
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
  }

  getDefault() {
    this.service.filters.materialType = 'FA';

    const targetYear = new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear();

    const existsInList = this.service.fiscalYears.includes(targetYear);

    if (existsInList) {
      this.service.filters.financeYearMax = targetYear;
    } else {
      if (this.service.fiscalYears && this.service.fiscalYears.length > 0) {
        this.service.filters.financeYearMax = this.service.fiscalYears[0];
      } else {
        this.service.filters.financeYearMax = targetYear;
      }
    }
  }
}
