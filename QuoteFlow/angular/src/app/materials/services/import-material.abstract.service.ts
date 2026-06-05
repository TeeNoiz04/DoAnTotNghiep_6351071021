import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests';
import { finalize, Subscription, tap } from 'rxjs';
import { GetMaterialsApprovalInput, MaterialService } from '@proxy/materials';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { HistoryActions } from '@app/shared/components/approval-comment/approval-comment.component';
import { ActionDto } from '@proxy/shared';
import { ToasterService } from '@abp/ng.theme.shared';

export abstract class AbstractImportMaterialViewService {
  protected readonly proxyService = inject(MaterialService);
  protected readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);
  public readonly toasterService: ToasterService = inject(ToasterService);
  private currentSubscription?: Subscription;
  isPerformActionModalOpen = false;
  isPerformActionModalBusy = false;
  actionOptions: string;
  currentAction: HistoryActions;
  approvedAction: HistoryActions = HistoryActions.Approved;
  rejectedAction: HistoryActions = HistoryActions.Rejected;
  cancelledAction: HistoryActions = HistoryActions.Cancelled;
  isExportToExcelBusy = false;

  data: PagedResultDto<MaterialApprovalRequestDto> = {
    items: [],
    totalCount: 0,
  };

  selected = {} as MaterialApprovalRequestDto;

  filters = {
    approvalStatus: '',
    isDeactive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetMaterialsApprovalInput;

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getListApproval({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<MaterialApprovalRequestDto>) => {
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
  performAction(action: string, row: MaterialApprovalRequestDto) {
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
            this.toasterService.success('Material cancelled successfully.');
            this.isPerformActionModalOpen = false;
            this.hookToQuery();
          }
        }),
      )
      .subscribe();
  }
  clearFilters() {
    this.filters = {
      approvalStatus: '',
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetMaterialsApprovalInput;
    this.list.get();
  }

  exportToExcel() {}

  getListApprovers(id: string) {
    return this.proxyService.getListApprovers(id);
  }
}
