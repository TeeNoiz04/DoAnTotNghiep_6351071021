import { CommonModule } from '@angular/common'; // Add this line
import { Component, EventEmitter, Input, Output } from '@angular/core';

export enum HistoryActions {
  None = 'None',
  Submitted = 'Submitted',
  Approved = 'Approved',
  Rejected = 'Rejected',
  Cancelled = 'Cancelled',
  Closed = 'Closed',
  Reopen = 'Reopen',
  ImportSAPData = 'ImportSAPData',
  ImportIUChange = 'ImportIUChange',
  ConfirmedDelivery = 'ConfirmedDelivery',
  Updated = 'Update',
  SubmittedAsWin = 'SubmittedAsWin',
  SubmittedAsLost = 'SubmittedAsLost',
  SubmittedMoreItems = 'SubmittedMoreItems',
  LandingCostUpdated = 'LandingCostUpdated',
  SpecialInputPriceAssigned = 'SpecialInputPriceAssigned',
  QuantityMerged = 'QuantityMerged',
  MessageSent = 'MessageSent',
  ConfirmedLockStock = 'ConfirmedLockStock',
  ConfirmedLockOnOrder = 'ConfirmedLockOnOrder',
  Confirmed = 'Confirmed',
  Return = 'Return',
  WaitForReturn = 'WaitForReturn',
  ConfirmedClose = 'ConfirmedClose',
  WaitForClose = 'WaitForClose',
  ConfirmClose = 'ConfirmClose',
}

export const HistoryActionColorMap = {
  [HistoryActions.Approved]: 'action-approved',
  [HistoryActions.Rejected]: 'action-rejected',
  [HistoryActions.Cancelled]: 'action-cancelled',
  [HistoryActions.Submitted]: 'action-submitted',
  [HistoryActions.Closed]: 'action-closed',
  [HistoryActions.Reopen]: 'action-reopen',
  [HistoryActions.ImportSAPData]: 'action-import-sap-data',
  [HistoryActions.ImportIUChange]: 'action-import-iu-change',
  [HistoryActions.ConfirmedDelivery]: 'action-confirmed-delivery',
  [HistoryActions.Updated]: 'action-updated',
  [HistoryActions.SubmittedAsWin]: 'action-submitted-as-win',
  [HistoryActions.SubmittedAsLost]: 'action-submitted-as-lost',
  [HistoryActions.SubmittedMoreItems]: 'action-submitted-more-items',
  [HistoryActions.LandingCostUpdated]: 'action-landing-cost-updated',
  [HistoryActions.SpecialInputPriceAssigned]: 'action-special-input-price-assigned',
  [HistoryActions.QuantityMerged]: 'action-quantity-merged',
  [HistoryActions.MessageSent]: 'action-message-sent',
  [HistoryActions.ConfirmedLockStock]: 'action-confirmed-lock-stock',
  [HistoryActions.ConfirmedLockOnOrder]: 'action-confirmed-lock-on-order',
  [HistoryActions.Confirmed]: 'action-confirmed',
  [HistoryActions.Return]: 'action-return',
  [HistoryActions.WaitForReturn]: 'action-wait-for-return',
  [HistoryActions.ConfirmedClose]: 'action-confirmed-close',
  [HistoryActions.WaitForClose]: 'action-wait-for-close',
  [HistoryActions.ConfirmClose]: 'action-confirmed-close',
};

export const HistoryActionsTextMap = {
  [HistoryActions.None]: '',
  [HistoryActions.Approved]: 'Approved',
  [HistoryActions.Rejected]: 'Rejected',
  [HistoryActions.Cancelled]: 'Cancelled',
  [HistoryActions.Submitted]: 'Submitted',
  [HistoryActions.Closed]: 'Closed',
  [HistoryActions.Reopen]: 'Reopen',
  [HistoryActions.ImportSAPData]: 'Imported SAP Data',
  [HistoryActions.ImportIUChange]: 'Imported IU Change',
  [HistoryActions.ConfirmedDelivery]: 'Confirmed Delivery',
  [HistoryActions.Updated]: 'Updated',
  [HistoryActions.SubmittedAsWin]: 'Submitted as Win',
  [HistoryActions.SubmittedAsLost]: 'Submitted as Lost',
  [HistoryActions.SubmittedMoreItems]: 'Submitted More Items',
  [HistoryActions.LandingCostUpdated]: 'Landed Cost Updated',
  [HistoryActions.SpecialInputPriceAssigned]: 'Special Input Price Assigned',
  [HistoryActions.QuantityMerged]: 'Quantity Merged',
  [HistoryActions.MessageSent]: 'Message Sent',
  [HistoryActions.ConfirmedLockStock]: 'Confirmed Lock Stock',
  [HistoryActions.ConfirmedLockOnOrder]: 'Confirmed Lock On Order',
  [HistoryActions.Confirmed]: 'Confirmed',
  [HistoryActions.Return]: 'Return',
  [HistoryActions.WaitForReturn]: 'Wait for Return',
  [HistoryActions.ConfirmedClose]: 'Confirmed Close',
  [HistoryActions.WaitForClose]: 'Wait for Close',
  [HistoryActions.ConfirmClose]: 'Confirm Close',
};

@Component({
  selector: 'app-action-label',
  standalone: true,
  template: `
    <span
      class="badge rounded-pill text-status"
      [ngClass]="statusClass"
      [style.cursor]="isClickable ? 'pointer' : 'default'"
      (click)="onActionClick()"
      [attr.role]="isClickable ? 'button' : 'status'">
      {{ actionOut }}
    </span>
  `,
  styleUrls: ['./action-label.component.scss'],
  imports: [CommonModule],
})
export class ActionsLabelComponent {
  @Input() action: string | undefined; // Allow status to be undefined
  @Output() actionOut: string = ''; // Initialize statusOut to avoid undefined
  @Output() actionClicked = new EventEmitter<string>();

  get isClickable(): boolean {
    // return this.action === HistoryActions.SubmittedMoreItems;
    return false;
  }

  get statusClass(): string {
    if (!this.action) {
      return 'action-default';
    }

    const normalizedAction = HistoryActionsTextMap[this.action];

    this.actionOut = normalizedAction || this.action;

    return HistoryActionColorMap[this.action] || 'action-default';
  }

  onActionClick(): void {
    if (this.isClickable && this.action) {
      this.actionClicked.emit(this.action);
    }
  }
}
