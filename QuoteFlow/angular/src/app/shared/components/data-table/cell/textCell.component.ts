import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2 } from '@angular/core';
import {
  ActionsLabelComponent,
  HistoryActions,
  HistoryActionsTextMap,
} from '@app/shared/action/components/action-label.component';

import { RequestStatusTextMap, StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { DateComponentPipe } from 'src/app/shared/pipes/date-component.pipe';
import { NumberFormatPipe } from 'src/app/shared/pipes/number-format.pipe';

@Component({
  selector: 'app-text-cell',
  templateUrl: 'textCell.component.html',
  styleUrls: ['./dataCell.component.scss'],
  standalone: true,
  imports: [
    NumberFormatPipe,
    DateComponentPipe,
    ActionsLabelComponent,
    NgbTooltipModule,
    CoreModule,
    ThemeSharedModule,
    StatusLabelComponent,
  ],
})
export class TextCellComponent implements OnInit {
  @Input() cssClass: string;
  @Input() value: any;
  @Input() entry: any;
  @Input() columnType: string = 'text';
  @Input() dateFormat: string = 'dd/MM/yyyy HH:mm a';
  @Input() formatter: (text: any, entry: any) => string;
  @Output() actionClicked = new EventEmitter<{ action: string; entry: any }>();

  public isShowTooltip: boolean = false;
  public isCollapse: boolean = true;

  constructor(
    protected elRef: ElementRef,
    protected render: Renderer2,
  ) {}

  ngOnInit() {
    if (!this.formatter) {
      this.formatter = this.plainTextFormatter;
    }
  }

  plainTextFormatter(text, entry) {
    return text || '';
  }

  getTooltipContent(text, entry) {
    if (entry && entry.error) {
      return entry.error;
    }
    return '';
  }

  findElement(collection: HTMLCollection, code) {
    for (let i = 0; i < collection.length; i++) {
      if (collection.item(i).classList[1] === code) {
        return collection.item(i);
      }
    }
  }

  formatKey(key, action) {
    return action === 6 ? key.replace(/([A-Z])/g, ' $1').trim() : key.replace(/([A-Z][a-z])/g, ' $1').trim();
  }

  getStatusLabel(status: number): string {
    const option = RequestStatusTextMap[status];
    return option ? this.formatKey(option.value, status) : '';
  }

  getActionLabel(action: number): string {
    const option = HistoryActionsTextMap[action];
    return option ? this.formatKey(option.value, action) : '';
  }

  onActionClicked(action: string): void {
    this.actionClicked.emit({ action, entry: this.entry });
  }

  get isSubmitted(): boolean {
    return this.value === HistoryActions.SubmittedMoreItems;
  }
  onViewMoreClicked(): void {
    if (this.isSubmitted) {
      this.actionClicked.emit({ action: this.value, entry: this.entry });
    }
  }
}
