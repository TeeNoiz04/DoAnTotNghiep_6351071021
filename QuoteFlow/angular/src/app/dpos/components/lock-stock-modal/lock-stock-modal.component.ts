import { PermissionDirective, PermissionService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppPermissions } from '@app/app.permissions';
import { BatchUnlockProgressEventDto } from '@app/proxy-custom/dpos/dpo-extended.models';
import { DPOExtendedService } from '@app/proxy-custom/dpos/dpo-extended.service';
import { BatchAutoUnlockStockDto, DPODto, DPOLockStockAutoV2Dto, DPOService } from '@app/proxy/dpos';
import { LookupService } from '@app/proxy/general-lookups';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupDto } from '@proxy/shared';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

export interface LockStockResult {
  confirmed: boolean;
  stockCategoryId?: string;
  note?: string;
  isAutoMode?: boolean;
  selectedItems?: any[];
}

interface ProgressResultItem {
  detailId: string;
  golfaCode?: string;
  status: 'processing' | 'success' | 'error';
  unlockedCount: number;
  skippedCount: number;
  errorMessage?: string;
}

interface ProgressSummary {
  totalProcessed: number;
  succeeded: number;
  failed: number;
  totalUnlocked: number;
  totalSkipped: number;
}

@Component({
  selector: 'app-lock-stock-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ThemeSharedModule, NgSelectModule, ReactiveFormsModule, PermissionDirective],
  templateUrl: './lock-stock-modal.component.html',
  styleUrls: ['./lock-stock-modal.component.scss'],
})
export class LockStockModalComponent implements OnInit {
  private readonly lookupService = inject(LookupService);
  private readonly service = inject(DPOService);
  private readonly extendedService = inject(DPOExtendedService);
  private readonly toasterService = inject(ToasterService);
  private readonly fb = inject(FormBuilder);
  protected readonly permissionService = inject(PermissionService);
  private readonly loadingService = inject(LoadingService);
  @Input() visible: boolean = false;
  @Input() selectedItemsCount: number = 0;
  @Input() selectedItems: any[] = [];
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';
  @Input() dpo: DPODto | null = null;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<any>();

  protected AppPermissions = AppPermissions;
  protected stockCategories: LookupDto<string>[] = [];
  protected loading = false;
  protected isBusy = false;
  protected form: FormGroup;
  protected releaseForm: FormGroup;
  protected activeTab: 'lock' | 'release' = 'lock';

  // Progress tracking for release stock
  protected showProgress = false;
  protected isProcessing = false;
  protected progressResults: ProgressResultItem[] = [];
  protected currentProgress = 0;
  protected totalItems = 0;
  protected summary: ProgressSummary | null = null;

  ngOnInit(): void {
    this.loadStockCategories();
    this.buildForm();
    this.buildReleaseForm();
  }

  private loadStockCategories(): void {
    this.loading = true;

    this.lookupService
      .getMainStockCategoryLookup()
      .pipe(
        tap(result => {
          this.stockCategories = result?.items || [];
        }),
        catchError(error => {
          console.error('Error loading stock categories:', error);
          this.stockCategories = [];
          return EMPTY;
        }),
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe();
  }

  buildForm(): void {
    this.form = this.fb.group({
      stockCategoryId: [null, [Validators.required]],
    });
  }

  buildReleaseForm(): void {
    this.releaseForm = this.fb.group({
      note: ['', Validators.maxLength(500)],
    });
  }

  get hasAnyFinalizedItems(): boolean {
    return (
      this.selectedItems?.some(
        item => item.status === RequestStatusEnum.CANCELLED || item.status === RequestStatusEnum.CLOSED,
      ) || false
    );
  }

  onLockStock(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        control.markAsTouched();
      });
      this.toasterService.warn('Please correct the form errors before saving', 'Validation Error');
      return;
    }

    if (!this.selectedItems || this.selectedItems.length === 0) {
      this.toasterService.warn('No items selected for lock stock', 'Validation Error');
      return;
    }

    this.isBusy = true;
    this.loadingService.show();
    const formValue = this.form.getRawValue();

    const updateDto: DPOLockStockAutoV2Dto = {
      stockCategoryId: formValue.stockCategoryId,
      dpoDetailIds: this.selectedItems.map(item => item.id),
    };

    this.service
      .lockStockAutoV2(updateDto)
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: result => {
          this.toasterService.success('Lock Stock successfully', 'Success');
          this.modalResult.emit(result);
          this.closeDialog();
        },
        error: error => {
          console.error('Error updating lock stock:', error);
          this.toasterService.error('Failed to update lock stock', 'Error');
        },
        complete: () => {
          this.loadingService.hide();
        },
      });
  }

  onReleaseStock(): void {
    if (!this.selectedItems || this.selectedItems.length === 0) {
      this.toasterService.warn('No items selected for release stock', 'Validation Error');
      return;
    }

    // Switch to progress view
    this.showProgress = true;
    this.progressResults = [];
    this.currentProgress = 0;
    this.totalItems = 0;
    this.summary = null;
    this.isProcessing = true;

    const request: BatchAutoUnlockStockDto = {
      dpoDetailIds: this.selectedItems.map(item => item.id),
    };

    this.extendedService
      .batchAutoUnlockStockWithProgress(request)
      .pipe(finalize(() => (this.isProcessing = false)))
      .subscribe({
        next: event => {
          this.handleProgressEvent(event);
        },
        error: error => {
          const errorMessage = error?.error?.error?.message || error?.message || 'Batch unlock failed';
          this.toasterService.error(errorMessage, 'Error');
          this.showProgress = false;
          this.isProcessing = false;
        },
      });
  }

  private handleProgressEvent(event: BatchUnlockProgressEventDto): void {
    if (event.eventType === 'started') {
      this.totalItems = event.total;
      this.currentProgress = event.current;
    } else if (event.eventType === 'progress') {
      this.currentProgress = event.current;

      // Find matching detail to get golfa code
      const matchedDetail = this.selectedItems.find(item => item.id === event.dpoDetailId);

      this.progressResults.push({
        detailId: event.dpoDetailId || '',
        golfaCode: matchedDetail?.golfaCode,
        status: event.status || 'processing',
        unlockedCount: event.unlockedCount || 0,
        skippedCount: event.skippedCount || 0,
        errorMessage: event.errorMessage,
      });
    } else if (event.eventType === 'complete') {
      this.showSummary(event);
    }
  }

  private showSummary(event: BatchUnlockProgressEventDto): void {
    this.summary = {
      totalProcessed: event.totalProcessed || 0,
      succeeded: event.succeeded || 0,
      failed: event.failed || 0,
      totalUnlocked: event.totalUnlocked || 0,
      totalSkipped: event.totalSkipped || 0,
    };

    if ((event.succeeded || 0) > 0) {
      this.toasterService.success(
        `Unlocked ${event.totalUnlocked} lock records from ${event.succeeded}/${event.totalProcessed} details`,
        'Unlock Complete',
      );
    }
  }

  get progressPercentage(): number {
    return this.totalItems > 0 ? (this.currentProgress / this.totalItems) * 100 : 0;
  }

  closeDialog(): void {
    // Emit result to parent to refresh data only if we completed a successful unlock
    if (this.showProgress && this.summary && (this.summary.succeeded || 0) > 0) {
      this.modalResult.emit({ success: true });
    }

    this.visible = false;
    this.activeTab = 'lock';
    this.form.reset();
    this.releaseForm.reset();
    this.showProgress = false;
    this.progressResults = [];
    this.currentProgress = 0;
    this.totalItems = 0;
    this.summary = null;
    this.isProcessing = false;
    this.visibleChange.emit(false);
  }
}
