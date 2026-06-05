import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { TextCellComponent } from './textCell.component';

@Component({
  selector: 'app-click-cell',
  standalone: true,
  template: `
    <div class="click-cell d-flex align-items-center {{ cssClass }}">
      @if (showAccountWithClear && value) {
        <div class="d-flex align-items-center gap-1">
          <span class="badge bg-primary">{{ formatter(value, entry) }}</span>
          <button
            type="button"
            class="btn btn-sm btn-outline-danger p-0"
            style="width: 20px; height: 20px;"
            (click)="clearAction.emit(entry); $event.stopPropagation()"
            title="Clear Account Code">
            <i class="bi bi-x"></i>
          </button>
        </div>
      } @else if (showAccountWithClear && !value && entry?.hasAccountToSelect === true) {
        <i
          class="bi bi-person text-primary"
          style="cursor: pointer;"
          (click)="clickAction.emit(); $event.stopPropagation()"
          title="Select Account Code"></i>
      } @else if (showAccountWithClear && !value && entry?.hasAccountToSelect === false) {
        <span></span>
      } @else {
        <div class="text-start">
          <a
            *ngIf="isActive"
            (click)="clickAction.emit(); $event.stopPropagation()"
            class="d-flex gap-2 link-cell {{ cssClass }}"
            [ngClass]="{ 'cell-wrap-text w-100': columnType === 'wrapText' }">
            <i *ngIf="showIcon" class="{{ iconClass }}"></i>
            <span *ngIf="columnType === 'int-not-null'">{{ formatter(value, entry) || 0 | number: '1.0-0' }}</span>
            <span *ngIf="columnType === 'int'">{{ formatter(value, entry) | number: '1.0-0' }}</span>
            <span *ngIf="columnType !== 'int' && columnType !== 'int-not-null'">{{ formatter(value, entry) }}</span>
          </a>
          <a
            *ngIf="!isActive"
            class="no-link-cell {{ cssClass }}"
            [ngClass]="{ 'cell-wrap-text w-100': columnType === 'wrapText' }"
            >{{ formatter(value, entry) }}</a
          >
        </div>
      }
    </div>
  `,
  styleUrls: ['dataCell.component.scss'],
  imports: [CommonModule, NgbTooltipModule],
})
export class ClickCellComponent extends TextCellComponent {
  @Output() clickAction = new EventEmitter<any>();
  @Output() clearAction = new EventEmitter<any>();
  @Input() cssClass: string;
  @Input() isActive: boolean;
  @Input() isLockIcon: boolean = false;
  @Input() showIcon: boolean;
  @Input() iconClass: string;
  @Input() showAccountWithClear: boolean = false;
}
