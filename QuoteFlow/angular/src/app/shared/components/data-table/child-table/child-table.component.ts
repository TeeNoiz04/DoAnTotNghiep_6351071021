import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NumberFormatPipe } from '@app/shared/pipes/number-format.pipe';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ActionCellComponent } from '../cell/actionCell.component';
import { TChildColumn, TChildHeader } from '../data-table.model';

export enum SelectionType {
  single = 'single',
  multi = 'multi',
}

@Component({
  selector: 'app-child-table',
  standalone: true,
  imports: [
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgbTooltipModule,
    ActionCellComponent,
    NumberFormatPipe,
  ],
  templateUrl: './child-table.component.html',
  styleUrls: ['./child-table.component.scss'],
})
export class ChildTableComponent {
  @Input() headers: TChildHeader[];
  @Input() columns: TChildColumn[];
  @Input() records: any[];

  @Input() showActions: boolean = false;
  @Input() disableAction: boolean = false;
  @Input() itemEdit: boolean = true;
  @Input() itemRemove: boolean = true;
  @Input() title = '';

  @Output() editChildItemHandler = new EventEmitter<any>();
  @Output() removeChildItemHandler = new EventEmitter<any>();

  isExpanded: boolean = true;

  constructor() {}

  toggleTable(): void {
    this.isExpanded = !this.isExpanded;
  }

  editActionHandler(row: any) {
    this.editChildItemHandler.emit(row);
  }

  removeActionHandler(row: any) {
    this.removeChildItemHandler.emit(row);
  }
}
