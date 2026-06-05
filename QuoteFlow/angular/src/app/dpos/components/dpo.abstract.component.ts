import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { AfterViewInit, Directive, inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DPODto } from '@app/proxy/dpos/models';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { filter, Subject, takeUntil } from 'rxjs';
import { DPOViewService } from '../services/dpo.service';

export const ChildTabDependencies = [];
export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractDPOComponent implements OnInit, OnDestroy, AfterViewInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(DPOViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly router = inject(Router);
  public readonly route = inject(ActivatedRoute);
  // Modal injection removed - using ABP modal instead
  protected title = 'Distributor Purchase Order (DPO)';
  protected isActionButtonVisible: boolean | null = null;
  protected initialized = false;
  showImportModal = false;
  private searchTextChanged$ = new Subject<string>();
  protected destroy$ = new Subject<void>();
  AppPermissions = AppPermissions;

  ngOnInit() {
    this.titleService.setTitle('DPO');
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;

    // Set default status from route data BEFORE calling hookToQuery
    const defaultStatus = this.route.snapshot.data?.['defaultStatus'];
    if (defaultStatus) {
      this.service.filters.status = defaultStatus;
    }

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        // When navigation occurs, preserve default status if no status filter exists
        const currentDefaultStatus = this.route.snapshot.data?.['defaultStatus'];
        if (currentDefaultStatus && !this.service.filters.status) {
          this.service.filters.status = currentDefaultStatus;
        }
        // Preserve title when navigation occurs due to filter changes
        this.titleService.setTitle('DPO Management');
      });

    this.checkActionButtonVisibility();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit() {
    this.initialized = true;
  }

  onLoad() {
    this.service.updateQueryParams(true);
    this.list.get();
  }

  onSearchTextChange(text: string) {
    this.service.filters.filterText = text;
    this.searchTextChanged$.next(text);
  }

  clearFilters() {
    this.service.clearFilters();
    // Restore default status if exists
    const defaultStatus = this.route.snapshot.data?.['defaultStatus'];
    if (defaultStatus) {
      this.service.filters.status = defaultStatus;
    }
  }

  delete(record: DPODto) {
    this.service.delete(record);
  }

  showHistoryModal = false;
  viewHistory(record: DPODto) {
    this.service.viewHistory(record);
    this.showHistoryModal = true;
  }

  closeHistoryDialog() {
    this.showHistoryModal = false;
  }

  exportToExcel() {
    // this.service.exportToExcel();
  }

  viewDetails(record: DPODto) {
    this.router.navigate(['/dpo', record.id, 'details']);
  }

  onCreate() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }

  onEdit(id: string) {
    this.router.navigate([id], { relativeTo: this.route });
  }

  onView(id: string) {
    this.router.navigate([id], { relativeTo: this.route });
  }

  onImport() {
    this.showImportModal = true;
  }
  onDownLoadTemplate() {
    this.service.downloadTemplate();
  }

  checkActionButtonVisibility() {
    if (this.isActionButtonVisible !== null) {
      return;
    }

    const canDelete = this.permissionService.getGrantedPolicy(AppPermissions.MovingOrders.DPOs.Delete);
    this.isActionButtonVisible = canDelete;
  }
}
