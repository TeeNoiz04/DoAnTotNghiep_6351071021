import { ListService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { HistoryActions } from '@app/shared/components/approval-comment/approval-comment.component';

import { PSIDto, PSIService } from '@proxy/psis';
import { ActionDto } from '@proxy/shared';
import { finalize, forkJoin, tap } from 'rxjs';

export abstract class AbstractPSIDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  protected readonly router = inject(Router);
  public readonly toasterService: ToasterService = inject(ToasterService);
  public readonly proxyService = inject(PSIService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as PSIDto;
  form: FormGroup | undefined;
  isPerformActionModalOpen = false;
  isPerformActionModalBusy = false;
  actionOptions: string;
  currentAction: HistoryActions;
  approvedAction: HistoryActions = HistoryActions.Approved;
  rejectedAction: HistoryActions = HistoryActions.Rejected;
  cancelledAction: HistoryActions = HistoryActions.Cancelled;

  loadDetailsAndImports(id: string) {
    return forkJoin({
      item: this.proxyService.getDetail(id),
      // importResults: this.proxyService.getListDetails(id),
    });
  }

  buildForm() {
    const { fy, fileName, note, psiCode, materialType } = this.selected || {};

    this.form = this.fb.group({
      psiCode: [psiCode ?? null, []],
      financeYear: [fy ?? null, []],
      fileName: [fileName ?? null, []],
      materialType: [materialType ?? null, []],
      note: [note ?? null, []],
    });
  }

  showForm() {
    this.buildForm();
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: PSIDto) {
    this.selected = record;
    this.showForm();
  }

  hideForm() {
    this.isVisible = false;
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }

  performAction(action: string) {
    this.actionOptions = action;
    this.isPerformActionModalOpen = true;
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
          this.selected = {} as PSIDto;

          if (input.action === 'Cancelled') {
            this.toasterService.success('PSI cancelled successfully.');
            this.router.navigate([AppRoutes.PSI.BASE, AppRoutes.PSI.LIST.BASE]);
          } else {
            this.toasterService.success('PSI submitted successfully.');
            this.router.navigate([AppRoutes.PSI.BASE, AppRoutes.PSI.MY_APPROVALS.BASE]);
          }
        }),
      )
      .subscribe();
  }

  getListApprovers(id: string) {
    return this.proxyService.getListApprovers(id);
  }
}
