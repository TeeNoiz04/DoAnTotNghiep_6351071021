import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { HistoryActions } from '@app/shared/components/approval-comment/approval-comment.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { GetPSIsInput, PSIDto, PSIService } from '@proxy/psis';
import { ActionDto } from '@proxy/shared';
import { Subscription } from 'rxjs';
import { finalize, switchMap, tap } from 'rxjs/operators';

export abstract class AbstractPSIViewService {
  public readonly proxyService = inject(PSIService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly listApproval = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly loadingService = inject(LoadingService);
  public readonly toasterService: ToasterService = inject(ToasterService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;
  isPerformActionModalOpen = false;
  isPerformActionModalBusy = false;
  actionOptions: string;
  currentAction: HistoryActions;
  approvedAction: HistoryActions = HistoryActions.Approved;
  rejectedAction: HistoryActions = HistoryActions.Rejected;
  cancelledAction: HistoryActions = HistoryActions.Cancelled;
  selected = {} as PSIDto;

  filters = {
    fy: new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear(),
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetPSIsInput;
  data: PagedResultDto<PSIDto> = {
    items: [],
    totalCount: 0,
  };

  filtersApproval = {
    fy: new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear(),
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetPSIsInput;
  dataApproval: PagedResultDto<PSIDto> = {
    items: [],
    totalCount: 0,
  };

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<PSIDto>) => {
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
      this.proxyService.getListPending({
        ...query,
        ...this.filtersApproval,
      });

    const setData = (list: PagedResultDto<PSIDto>) => {
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
      fy: new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear(),
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetPSIsInput;
    this.list.get();
  }

  refreshApprovalList() {
    this.listApproval.get();
  }

  clearFiltersApproval() {
    this.filtersApproval = {
      fy: new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear(),
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetPSIsInput;
    this.listApproval.get();
  }

  performAction(action: string, row: PSIDto) {
    this.actionOptions = action;
    this.isPerformActionModalOpen = true;
    this.selected = row;
  }

  onClosePerformAction() {
    this.isPerformActionModalOpen = false;
  }

  onPerformAction($event: { action: HistoryActions; comment: string }) {
    this.isPerformActionModalBusy = true;
    this.currentAction = $event.action;
    const input = {
      action:
        $event.action === HistoryActions.Approved
          ? 'Approved'
          : $event.action === HistoryActions.Rejected
            ? 'Rejected'
            : 'Cancelled',
      concurrencyStamp: this.selected.concurrencyStamp,
      comment: $event.comment,
    } as ActionDto;
    this.proxyService
      .performAction(this.selected.id, input)
      .pipe(
        finalize(() => (this.isPerformActionModalBusy = false)),
        tap(() => {
          if (input.action === 'Cancelled') {
            this.toasterService.success('PSI cancelled successfully.');
            this.isPerformActionModalOpen = false;
            this.hookToQuery();
          }
        }),
      )
      .subscribe();
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) =>
          this.proxyService.getListAsExcelFile({
            downloadToken: token,
            filterText: this.list.filter,
            ...this.filters,
          }),
        ),
        finalize(() => (this.isExportToExcelBusy = false)),
      )
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, 'PSI.xlsx');
      });
  }

  getListApprovers(id: string) {
    return this.proxyService.getListApprovers(id);
  }
}
