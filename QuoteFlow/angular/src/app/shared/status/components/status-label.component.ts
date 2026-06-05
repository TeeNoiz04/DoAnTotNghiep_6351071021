import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

export enum RequestStatusEnum {
  DRAFT = 'DRAFT',
  IN_PROGRESS = 'IN_PROGRESS',
  APPROVED = 'APPROVED',
  CONFIRMEDLOCKSTOCK = 'CONFIRMEDLOCKSTOCK',
  REJECTED = 'REJECTED',
  CANCELLED = 'CANCELLED',
  MODIFYING = 'MODIFYING',
  SUCCESS = 'SUCCESS',
  FAILED = 'FAILED',
  UNPROCESSED = 'UNPROCESSED',
  SUBMITTED = 'SUBMITTED',
  CLOSED = 'CLOSED',
  REOPEN = 'REOPEN',
  WON = 'WON',
  PRE_ORDER = 'PRE_ORDER',
  LOST = 'LOST',
  ACTIVE = 'ACTIVE',
  INACTIVE = 'INACTIVE',
  DEACTIVE = 'DEACTIVE',
  INVALID = 'INVALID',
  VALID = 'VALID',
  EXPIRED = 'EXPIRED',
  SELF_APPROVED = 'SELF_APPROVED',
  LOCKED = 'LOCKED',
  CONFIRMED = 'CONFIRMED',
  LOCKED_STOCK = 'LOCKED_STOCK',
  BILLING = 'BILLING',
  DOWNGRADED = 'DOWNGRADED',
  DISCONTINUE = 'DISCONTINUE',
  OUTDATED = 'OUTDATED',
  PENDING = 'PENDING',
  WAITFORCLOSE = 'WAIT_FOR_CLOSE',
  // History Tracking Actions
  CORRECTION = 'CORRECTION',
  REVERT_DELIVERY = 'REVERT_DELIVERY',
  DELIVERY = 'DELIVERY',
  ADJUSTMENT_STOCK = 'ADJUSTMENT_STOCK',
  MOVING_STOCK = 'MOVING_STOCK',
  IMPORT = 'IMPORT',
  MIGRATION = 'MIGRATION',
  INVENTORY = 'INVENTORY',
  TRANSFER = 'TRANSFER',
  UPDATE = 'UPDATE',
  CREATE = 'CREATE',
  RETURN = 'RETURN',
  RETURNING = 'RETURNING',
  WAIT_FOR_RETURN = 'WAIT_FOR_RETURN',
  CONFIRM_CLOSE = 'CONFIRM_CLOSE',
}

export enum OfferPriceStatusEnum {
  IN_PROGRESS = 'IN_PROGRESS',
  APPROVED = 'APPROVED',
  REJECTED = 'REJECTED',
  CLOSED = 'CLOSED',
  CANCELLED = 'CANCELLED',
}

export const OfferPriceStatusTextMap = {
  [OfferPriceStatusEnum.IN_PROGRESS]: 'In Progress',
  [OfferPriceStatusEnum.APPROVED]: 'Approved',
  [OfferPriceStatusEnum.REJECTED]: 'Rejected',
  [OfferPriceStatusEnum.CLOSED]: 'Closed',
  [OfferPriceStatusEnum.CANCELLED]: 'Cancelled',
};

export const RequestStatusTextMap = {
  [RequestStatusEnum.DRAFT]: 'Draft',
  [RequestStatusEnum.IN_PROGRESS]: 'In Progress',
  [RequestStatusEnum.APPROVED]: 'Approved',
  [RequestStatusEnum.CONFIRMEDLOCKSTOCK]: 'ConfirmedLockStock',
  [RequestStatusEnum.REJECTED]: 'Rejected',
  [RequestStatusEnum.CANCELLED]: 'Cancelled',
  [RequestStatusEnum.MODIFYING]: 'Modifying',
  [RequestStatusEnum.SUCCESS]: 'Success',
  [RequestStatusEnum.FAILED]: 'Failed',
  [RequestStatusEnum.UNPROCESSED]: 'Unprocessed',
  [RequestStatusEnum.SUBMITTED]: 'Submitted',
  [RequestStatusEnum.CLOSED]: 'Closed',
  [RequestStatusEnum.REOPEN]: 'Reopen',
  [RequestStatusEnum.WON]: 'Won',
  [RequestStatusEnum.PRE_ORDER]: 'Pre-Order',
  [RequestStatusEnum.LOST]: 'Lost',
  [RequestStatusEnum.ACTIVE]: 'Active',
  [RequestStatusEnum.INACTIVE]: 'Inactive',
  [RequestStatusEnum.DEACTIVE]: 'DeActive',
  [RequestStatusEnum.INVALID]: 'Invalid',
  [RequestStatusEnum.VALID]: 'Valid',
  [RequestStatusEnum.EXPIRED]: 'Expired',
  [RequestStatusEnum.SELF_APPROVED]: 'Self Approved',
  [RequestStatusEnum.LOCKED]: 'Locked',
  [RequestStatusEnum.CONFIRMED]: 'Confirmed',
  [RequestStatusEnum.LOCKED_STOCK]: 'Locked Stock',
  [RequestStatusEnum.BILLING]: 'Billing',
  [RequestStatusEnum.DOWNGRADED]: 'Downgraded',
  [RequestStatusEnum.DISCONTINUE]: 'Discontinue',
  [RequestStatusEnum.OUTDATED]: 'Outdated',
  [RequestStatusEnum.PENDING]: 'Pending',
  [RequestStatusEnum.WAITFORCLOSE]: 'Wait For Close',
  // History Tracking Actions
  [RequestStatusEnum.CORRECTION]: 'Correction',
  [RequestStatusEnum.REVERT_DELIVERY]: 'Revert Delivery',
  [RequestStatusEnum.DELIVERY]: 'Delivery',
  [RequestStatusEnum.ADJUSTMENT_STOCK]: 'Adjustment Stock',
  [RequestStatusEnum.MOVING_STOCK]: 'Moving Stock',
  [RequestStatusEnum.IMPORT]: 'Import',
  [RequestStatusEnum.MIGRATION]: 'Migration',
  [RequestStatusEnum.INVENTORY]: 'Inventory',
  [RequestStatusEnum.TRANSFER]: 'Transfer',
  [RequestStatusEnum.UPDATE]: 'Update',
  [RequestStatusEnum.CREATE]: 'Create',
  [RequestStatusEnum.RETURN]: 'Return',
  [RequestStatusEnum.WAIT_FOR_RETURN]: 'Wait for Return',
  [RequestStatusEnum.RETURNING]: 'Returning',
  [RequestStatusEnum.CONFIRM_CLOSE]: 'Confirm Close',
};

export const RequestStatusColorMap = {
  [RequestStatusEnum.DRAFT]: 'status-draft',
  [RequestStatusEnum.IN_PROGRESS]: 'status-inprogress',
  [RequestStatusEnum.APPROVED]: 'status-approved',
  [RequestStatusEnum.CONFIRMEDLOCKSTOCK]: 'status-confirmed-lockstock',
  [RequestStatusEnum.REJECTED]: 'status-rejected',
  [RequestStatusEnum.CANCELLED]: 'status-cancelled',
  [RequestStatusEnum.MODIFYING]: 'status-rejected',
  [RequestStatusEnum.SUCCESS]: 'status-success',
  [RequestStatusEnum.FAILED]: 'status-failed',
  [RequestStatusEnum.UNPROCESSED]: 'status-unprocessed',
  [RequestStatusEnum.SUBMITTED]: 'status-submitted',
  [RequestStatusEnum.CLOSED]: 'status-closed',
  [RequestStatusEnum.REOPEN]: 'status-reopen',
  [RequestStatusEnum.WON]: 'status-success',
  [RequestStatusEnum.PRE_ORDER]: 'status-inprogress',
  [RequestStatusEnum.LOST]: 'status-rejected',
  [RequestStatusEnum.ACTIVE]: 'status-approved',
  [RequestStatusEnum.INACTIVE]: 'status-inactive',
  [RequestStatusEnum.DEACTIVE]: 'status-deactive',
  [RequestStatusEnum.INVALID]: 'status-failed',
  [RequestStatusEnum.VALID]: 'status-success',
  [RequestStatusEnum.EXPIRED]: 'status-expired',
  [RequestStatusEnum.SELF_APPROVED]: 'status-approved',
  [RequestStatusEnum.LOCKED]: 'status-locked',
  [RequestStatusEnum.CONFIRMED]: 'status-confirmed',
  [RequestStatusEnum.LOCKED_STOCK]: 'status-locked-stock',
  [RequestStatusEnum.BILLING]: 'status-billing',
  [RequestStatusEnum.DISCONTINUE]: 'status-discontinue',
  [RequestStatusEnum.DOWNGRADED]: 'status-downgraded',
  [RequestStatusEnum.OUTDATED]: 'status-outdated',
  [RequestStatusEnum.PENDING]: 'status-pending',
  [RequestStatusEnum.WAITFORCLOSE]: 'status-wait-for-close',
  // History Tracking Actions
  [RequestStatusEnum.CORRECTION]: 'status-correction',
  [RequestStatusEnum.REVERT_DELIVERY]: 'status-revert-delivery',
  [RequestStatusEnum.DELIVERY]: 'status-delivery',
  [RequestStatusEnum.ADJUSTMENT_STOCK]: 'status-adjustment-stock',
  [RequestStatusEnum.MOVING_STOCK]: 'status-moving-stock',
  [RequestStatusEnum.IMPORT]: 'status-import',
  [RequestStatusEnum.MIGRATION]: 'status-migration',
  [RequestStatusEnum.INVENTORY]: 'status-inventory',
  [RequestStatusEnum.TRANSFER]: 'status-transfer',
  [RequestStatusEnum.UPDATE]: 'status-inprogress',
  [RequestStatusEnum.CREATE]: 'status-submitted',
  [RequestStatusEnum.RETURN]: 'status-return',
  [RequestStatusEnum.WAIT_FOR_RETURN]: 'status-wait-for-return',
  [RequestStatusEnum.RETURNING]: 'status-returning',
  [RequestStatusEnum.CONFIRM_CLOSE]: 'status-confirm-close',
};

@Component({
  selector: 'app-status-label',
  standalone: true,
  template: `
    <div class="badge rounded-pill text-status" [ngClass]="statusClass">
      {{ statusOut | titlecase }}
    </div>
  `,
  styleUrls: ['./status-label.component.scss'],
  imports: [CommonModule],
})
export class StatusLabelComponent {
  @Input() status: string | undefined;

  statusOut: string = '';

  private normalizeStatus(input?: string): string {
    if (!input) return '';
    const u = input.trim().toUpperCase();
    return u.includes('.') ? u.split('.').pop()! : u;
  }

  get statusClass(): string {
    if (!this.status) {
      this.statusOut = 'Unknown';
      return 'status-default';
    }

    if (this.status === '1st Round' || this.status === '2nd Round' || this.status === '3rd Round') {
      this.statusOut = this.status;
      return 'status-success';
    }

    const normalized = this.normalizeStatus(this.status);

    this.statusOut = RequestStatusTextMap[normalized] || normalized || 'Unknown';

    return RequestStatusColorMap[normalized] || 'status-default';
  }
}
