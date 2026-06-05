import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests';
import { Subscription } from 'rxjs';
import {
  MaterialService,
  GetMaterialsApprovalInput,
  GetMaterialsInput,
  MaterialDto,
} from '@proxy/materials';

export abstract class AbstractMaterialViewService {
  protected readonly proxyService = inject(MaterialService);
  protected readonly list = inject(ListService);
  protected readonly listApproval = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;

  filters = {
    materialStatus: 'Active',
    // isDeactive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetMaterialsInput;
  data: PagedResultDto<MaterialDto> = {
    items: [],
    totalCount: 0,
  };

  filtersApproval = {
    approvalStatus: '',
    isDeactive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetMaterialsApprovalInput;
  dataApproval: PagedResultDto<MaterialApprovalRequestDto> = {
    items: [],
    totalCount: 0,
  };

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<MaterialDto>) => {
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

  hookToQueryApproval() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getListMyApproval({
        ...query,
        ...this.filtersApproval,
      });

    const setData = (list: PagedResultDto<MaterialApprovalRequestDto>) => {
      this.dataApproval = list;
    };

    if (!this.currentSubscription) {
      this.currentSubscription = this.listApproval.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    } else {
      this.currentSubscription.unsubscribe();
      this.currentSubscription = this.listApproval.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    }
  }

  refreshList() {
    this.list.get();
  }

  clearFilters() {
    this.filters = {
      materialStatus: 'Active',
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetMaterialsInput;
    this.list.get();
  }

  refreshApprovalList() {
    this.listApproval.get();
  }

  clearFiltersApproval() {
    this.filtersApproval = {
      approvalStatus: '',
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetMaterialsApprovalInput;
    this.listApproval.get();
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.proxyService.getListAsExcelFile(this.filters).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'MaterialMasterData.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        this.isExportToExcelBusy = false;
      },
      error: err => {
        this.isExportToExcelBusy = false;
        console.error('Download failed:', err);
      },
    });
  }

  getListApprovers(id: string) {
    return this.proxyService.getListApprovers(id);
  }
}
