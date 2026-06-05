import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModalOptions, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DpoGkrAllocationDto } from '@proxy/dpos';

export enum HistoryActions {
  None = 0,
  Submitted = 1,
  Approved = 2,
  Rejected = 3,
  Cancelled = 4,
  Closed = 5,
  CompletedSettlement = 6,
  AFAdjusted = 7,
  Modified = 8,
  ConfirmedProcurement = 9,
  SubmittedR2 = 10,
  Assigned = 11,
  Unassigned = 12,
  Returned = 13,
  Resolve = 14,
  ResolveReset = 15,
  ConfirmClaim = 16,
  SystemUpdate = 17,
  ConfirmLockOnOrder = 18,
  ConfirmLockStock = 19,
  ConfirmNote = 20,
}

@Component({
  selector: 'app-approval-comment-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbModule, CoreModule, ThemeSharedModule],
  templateUrl: './approval-comment.component.html',
  styleUrls: ['./approval-comment.component.scss'],
})
export class ApprovalCommentModalComponent {
  @Input() visible = false;
  @Input() busy = false;
  @Input() actionTitle = 'Approval';
  @Input() currentAction: HistoryActions;
  @Input() requireComment = false;
  @Input() modalOptions: NgbModalOptions = {
    size: 'xl',
    animation: true,
    centered: true,
    backdrop: 'static',
    keyboard: true,
  };
  @Input() showDateTime = false;
  @Input() showGkrStatus = false;
  @Input() gkrAllocations: DpoGkrAllocationDto[] = [];
  @Input() hasAvailableGkrs = true;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() commentSubmitted = new EventEmitter<{
    action: HistoryActions;
    comment: string;
    dateTime?: string | null;
  }>();
  @Output() cancelClicked = new EventEmitter<void>();

  dateTimeValue: Date | null = null;
  commentText = '';
  acknowledgeGkrStatus = false;

  get actionEnum(): typeof HistoryActions {
    return HistoryActions;
  }

  closeModal(): void {
    this.visibleChange.emit(this.visible);
    this.cancelClicked.emit();
    this.resetModalState();
  }

  submitComment(): void {
    if (this.requireComment && !this.commentText.trim()) {
      return;
    }

    if (this.showGkrStatus && this.hasAvailableGkrs && !this.acknowledgeGkrStatus) {
      return;
    }

    this.commentSubmitted.emit({
      action: this.currentAction,
      dateTime: this.showDateTime ? this.dateTimeValue.toString() : null,
      comment: this.commentText,
    });
    this.resetModalState();
  }

  disableSubmitAction(): boolean {
    if (this.showDateTime) {
      return this.requireComment && (!this.commentText.trim() || !this.dateTimeValue);
    }

    if (this.showGkrStatus && this.hasAvailableGkrs && !this.acknowledgeGkrStatus) {
      return true;
    }

    return this.requireComment && !this.commentText.trim();
  }

  private resetModalState(): void {
    this.commentText = '';
    this.acknowledgeGkrStatus = false;
  }
}
