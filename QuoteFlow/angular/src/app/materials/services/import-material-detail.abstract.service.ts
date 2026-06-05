import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, forkJoin, tap, Observable } from 'rxjs';
import { HistoryActions } from '@app/shared/components/approval-comment/approval-comment.component';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests';
import { AppRoutes } from '@app/app.routes';
import { Router } from '@angular/router';
import { ActionDto } from '@proxy/shared';
import { ToasterService } from '@abp/ng.theme.shared';
import { MaterialService } from '@proxy/materials';
import { DatePipe } from '@angular/common';
import type { PagedResultDto } from '@abp/ng.core';
import type { MaterialDto } from '@proxy/materials/models';

export abstract class AbstractImportMaterialDetailViewService {
  protected readonly proxyService = inject(MaterialService);
  private readonly fb = inject(FormBuilder);
  protected readonly router = inject(Router);
  public readonly toasterService: ToasterService = inject(ToasterService);
  protected readonly datePipe = inject(DatePipe);

  form: FormGroup | undefined;
  formFilter: FormGroup | undefined;
  isBusy = false;
  selected = {} as MaterialApprovalRequestDto;
  isPerformActionModalOpen = false;
  isPerformActionModalBusy = false;
  actionOptions: string;
  requireComment = true;
  currentAction: HistoryActions;
  approvedAction: HistoryActions = HistoryActions.Approved;
  rejectedAction: HistoryActions = HistoryActions.Rejected;
  cancelledAction: HistoryActions = HistoryActions.Cancelled;

  loadDetailsAndImports(id: string) {
    return forkJoin({
      details: this.proxyService.getMaterialApprovalDetail(id),
      // importDetails: this.proxyService.getListByApprovalId(id),
    });
  }

  buildForm(): void {
    const { requestNo, creatorName, creationTime, fileName, note, importType } =
      this.selected || {};

    const formattedCreationTime = creationTime
      ? this.datePipe.transform(creationTime, 'dd/MM/yyyy HH:mm')
      : '';

    this.form = this.fb.group({
      requestNo: [requestNo, []],
      creatorName: [creatorName, []],
      creationTime: [formattedCreationTime, []],
      fileName: [fileName, []],
      importType: [importType, []],
      note: [note, []],
    });
    this.formFilter = this.fb.group({
      code: [''],
      name: [''],
    });
  }

  getCurrentStockWarnings(golfaCodes: string[]): Observable<PagedResultDto<MaterialDto>> {
    return this.proxyService.getList({
      golfaCodes: golfaCodes.join(','),
      skipCount: 0,
      maxResultCount: golfaCodes.length,
    });
  }

  performAction(action: string) {
    this.actionOptions = action;
    if (action === 'Approved') {
      this.requireComment = false;
    } else {
      this.requireComment = true;
    }
    this.isPerformActionModalOpen = true;
  }

  onClosePerformAction() {
    this.isPerformActionModalOpen = false;
    this.requireComment = true;
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
          this.selected = {} as MaterialApprovalRequestDto;

          if (input.action === 'Cancelled') {
            this.toasterService.success('Material cancelled successfully.');
            this.router.navigate([
              AppRoutes.MATERIAL_STOCK.BASE,
              AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.BASE,
            ]);
          } else {
            this.toasterService.success('Material submitted successfully.');
            this.router.navigate([
              AppRoutes.MATERIAL_STOCK.BASE,
              AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.BASE,
            ]);
          }
        }),
      )
      .subscribe();
  }

  getListApprovers(id: string) {
    return this.proxyService.getListApprovers(id);
  }
}
