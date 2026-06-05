import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ApprovalHistoryDto } from '@proxy/approval-histories';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ColumnComponent } from '../data-table/column/column.component';
import { DataTableComponent } from '../data-table/data-table.component';
import { HeaderTableComponent } from '../data-table/header/header.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { HistoryActions } from '@app/shared/action/components/action-label.component';

@Component({
  selector: 'app-history-modal',
  standalone: true,
  imports: [
    CoreModule,
    CommercialUiModule,
    ThemeSharedModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    EscCloseModalDirective,
  ],
  providers: [],
  templateUrl: './history-modal.component.html',
  styleUrl: './history-modal.component.scss',
})
export class HistoryModalComponent implements OnChanges {
  @Input() approvalHistories: ApprovalHistoryDto[] = [];
  @Input() isVisible = false;
  @Input() title = 'History';
  @Output() disappear = new EventEmitter<void>();
  @Output() submittedMoreItemsClicked = new EventEmitter<ApprovalHistoryDto>();

  public options: NgbModalOptions = {
    size: 'xl',
    animation: true,
    centered: true,
    keyboard: true,
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.approvalHistories && this.approvalHistories) {
      // sort the approval histories by action date in descending order
      this.approvalHistories.sort((a, b) => new Date(b.actionDate).getTime() - new Date(a.actionDate).getTime());
    }
  }

  onDisappear() {
    this.disappear.emit();
  }

  onActionBadgeClicked(event: { action: string; entry: ApprovalHistoryDto }): void {
    // Emit event specifically for SubmittedMoreItems action
    if (event.action === HistoryActions.SubmittedMoreItems) {
      this.submittedMoreItemsClicked.emit(event.entry);
    }
  }
}
