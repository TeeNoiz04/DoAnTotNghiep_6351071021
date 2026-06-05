import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import {
  DPOAllocateGKRDto,
  DPOCancelDto,
  DPOConfirmNoteDto,
  DPODto,
  DpoGkrAllocationDto,
  DPOService,
} from '@app/proxy/dpos';
import { HistoryActions } from '@app/shared/components/approval-comment/approval-comment.component';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
import { ApprovalHistoryDto } from '@proxy/approval-histories';
import { DPODetailDto } from '@proxy/dpos/dpodetails';
import { LookupService } from '@proxy/general-lookups';
import { catchError, EMPTY, finalize, Observable, tap } from 'rxjs';

@Injectable()
export class DPODetailViewService {
  public readonly proxyService = inject(DPOService);
  // protected readonly proxyDetailService = inject(DPODetailService);
  protected readonly loadingService = inject(LoadingService);
  public readonly toasterService: ToasterService = inject(ToasterService);
  public readonly confirmationService = inject(ConfirmationService);
  protected readonly navigationService = inject(NavigationHistoryService);
  protected readonly router = inject(Router);
  protected readonly proxyLookupService = inject(LookupService);
  loading = false;
  loadingDetail = true;
  loadingGkrAllocations = false;
  gkrAllocationBusy: boolean = false;

  gkrAllocations: DpoGkrAllocationDto[] = [];
  availableGkrAllocations: DpoGkrAllocationDto[] = [];
  dpo: DPODto | null = null;
  dpoDetails: DPODetailDto[] = [];
  selectedItems: DPODetailDto[] = [];

  isPerformActionModalOpen = false;
  isPerformActionModalBusy = false;
  currentAction: HistoryActions;
  isExportToExcelBusy = false;

  loadDPO(id: string): void {
    this.loading = true;
    this.proxyService.get(id).subscribe({
      next: dpo => {
        this.dpo = dpo;
      },
      error: error => {
        console.error('Error loading DPO:', error);
      },
      complete: () => {
        this.loading = false;
      },
    });
  }

  loadDPODetails(dpoId: string): void {
    this.loadingDetail = true;
    this.proxyService
      .getListDPODetail({
        dpoId,
        maxResultCount: 1000,
        skipCount: 0,
      })
      .subscribe({
        next: result => {
          result?.items.forEach(item => {
            item.lockShipment = item.lockShipment || 0;
          });
          this.dpoDetails = result?.items || [];
        },
        error: error => {
          console.error('Error loading DPO details:', error);
        },
        complete: () => {
          this.loadingDetail = false;
        },
      });
  }

  loadGKRAllocations(dpoId: string) {
    this.loadingGkrAllocations = true;
    this.proxyService
      .getGkrAllocations(dpoId)
      .pipe(finalize(() => (this.loadingGkrAllocations = false)))
      .subscribe({
        next: result => {
          this.gkrAllocations = result || [];
        },
        error: error => {
          console.error('Error loading GKR Allocations:', error);
        },
      });
  }

  performAction(action: HistoryActions) {
    this.currentAction = action;

    // Load available GKRs when opening Confirm Lock Stock modal
    if (action === HistoryActions.ConfirmLockStock) {
      this.loadAvailableGKRAllocations();
    }

    this.isPerformActionModalOpen = true;
  }

  onClosePerformAction() {
    this.isPerformActionModalOpen = false;
  }

  onPerformAction($event: { action: HistoryActions; comment: string }) {
    this.isPerformActionModalBusy = true;
    this.currentAction = $event.action;

    switch ($event.action) {
      case HistoryActions.ConfirmLockOnOrder:
        this.onConfirmLockOnOrder($event.comment);
        break;
      case HistoryActions.ConfirmLockStock:
        this.onConfirmLockStock($event.comment);
        break;
      case HistoryActions.Cancelled:
        this.onCancelItems($event.comment);
        break;
      case HistoryActions.ConfirmNote:
        this.onConfirmNote($event.comment);
        break;
      default:
    }
  }

  onConfirmLockOnOrder(comment: string): void {
    this.isPerformActionModalBusy = true;
    this.proxyService
      .confirmLockOnOrder(this.dpo.id, {
        note: comment,
        concurrencyStamp: this.dpo.concurrencyStamp,
      })
      .pipe(finalize(() => (this.isPerformActionModalBusy = false)))
      .subscribe({
        next: () => {
          this.toasterService.success('Confirm lock on order successfully.');
          this.goBackList();
          this.isPerformActionModalOpen = false;
        },
        error: error => {
          console.error('Error confirm lock on order:', error);
        },
      });
  }

  onConfirmLockStock(comment: string): void {
    this.isPerformActionModalBusy = true;
    this.proxyService
      .confirmLockStock(this.dpo.id, { note: comment, concurrencyStamp: this.dpo.concurrencyStamp })
      .pipe(finalize(() => (this.isPerformActionModalBusy = false)))
      .subscribe({
        next: () => {
          this.toasterService.success('Confirm lock stock successfully.');
          this.goBackList();
          this.isPerformActionModalOpen = false;
        },
        error: error => {
          console.error('Error confirm lock stock:', error);
        },
      });
  }

  onCancelItems(comment: string): void {
    this.isPerformActionModalBusy = true;
    const ids = this.selectedItems?.map(item => item.id);

    const payload: DPOCancelDto = {
      dpoDetailIds: ids,
      concurrencyStamp: this.dpo?.concurrencyStamp,
      note: comment,
    };
    this.proxyService
      .cancel(this.dpo.id, payload)
      .pipe(finalize(() => (this.isPerformActionModalBusy = false)))
      .subscribe({
        next: () => {
          this.isPerformActionModalOpen = false;
          this.toasterService.success('Cancel Items successfully.');
          this.selectedItems = []; // Clear selection after successful cancellation
          this.loadDPODetails(this.dpo.id);
          this.loadDPO(this.dpo.id);
        },
        error: error => {
          console.error('Error confirm lock stock:', error);
        },
      });
  }

  onConfirmNote(comment: string): void {
    this.isPerformActionModalBusy = true;
    const ids = this.selectedItems?.map(item => item.id);

    const payload: DPOConfirmNoteDto = {
      dpoDetailIds: ids,
      note: comment,
    };
    this.proxyService
      .confirmNote(payload)
      .pipe(finalize(() => (this.isPerformActionModalBusy = false)))
      .subscribe({
        next: () => {
          this.toasterService.success('Confirm Note successfully.');
          this.loadDPODetails(this.dpo.id);
          this.isPerformActionModalOpen = false;
        },
        error: error => {
          console.error('Error confirm lock stock:', error);
        },
      });
  }

  goBackList(): void {
    const fallBackUrl = [AppRoutes.DPO.BASE, AppRoutes.DPO.LIST.BASE];
    this.navigationService.smartBack(fallBackUrl);
    // this.router.navigate([AppRoutes.DPO.BASE, AppRoutes.DPO.LIST.BASE]);
  }

  allocateGKRToDPO(gkrId: string, note: string): Observable<void> {
    if (!this.dpo) {
      return EMPTY;
    }

    this.gkrAllocationBusy = true;
    const input: DPOAllocateGKRDto = {
      dpoId: this.dpo.id,
      gkrId: gkrId,
      concurrencyStamp: this.dpo.concurrencyStamp,
      note: note,
    };

    return this.proxyService.allocateGkrToDpo(input).pipe(
      tap(() => {
        this.loadDPODetails(this.dpo.id);
        this.loadGKRAllocations(this.dpo.id);
        this.gkrAllocationBusy = false;
      }),
      catchError(error => {
        console.error('Error allocate GKR to DPO:', error);
        this.gkrAllocationBusy = false;
        return EMPTY;
      }),
    );
  }

  loadAvailableGKRAllocations() {
    if (!this.dpo) return;
    this.loadingGkrAllocations = true;
    this.proxyService
      .getAvailableGkrsForAllocation(this.dpo.id)
      .pipe(finalize(() => (this.loadingGkrAllocations = false)))
      .subscribe({
        next: result => {
          this.availableGkrAllocations = result || [];
        },
        error: error => {
          console.error('Error loading available GKR Allocations:', error);
        },
      });
  }

  get hasAvailableGkrsForAllocation(): boolean {
    return this.availableGkrAllocations && this.availableGkrAllocations.length > 0;
  }
  historyDPO: ApprovalHistoryDto[] = [];
  viewHistory(id: string) {
    this.proxyLookupService.getDPOApprovalHistories(id).subscribe(history => {
      // Handle the retrieved history data as needed
      this.historyDPO = history;
    });
  }
}
