import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, ContentChildren, EventEmitter, Input, Output, QueryList } from '@angular/core';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { NgbPaginationModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { TableActionComponent } from './action/table-action.component';
import { ActionCellComponent } from './cell/actionCell.component';
import { ClickCellComponent } from './cell/clickCell.component';
import { LinkCellComponent } from './cell/linkCell.component';
import { AppTextboxCellComponent } from './cell/textboxCell.component';
import { TextCellComponent } from './cell/textCell.component';
import { ChildTableComponent } from './child-table/child-table.component';
import { ColumnComponent } from './column/column.component';
import { TChildColumn, TChildHeader } from './data-table.model';
import { HeaderTableComponent } from './header/header.component';
import { InputNumberComponent } from '../input-number/input-number.component';
import { AppSelectCellComponent } from './cell/selectCell.component';
import { CheckboxCellComponent } from './cell/checkboxCell.component';
import { DateboxCellComponent } from './cell/dateboxCell.component';

export enum SelectionType {
  single = 'single',
  multi = 'multi',
}

export type PaginationPosition = 'top' | 'bottom' | 'top-left' | 'top-right' | 'bottom-left' | 'bottom-right' | 'both';

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgbTooltipModule,
    TextCellComponent,
    ActionCellComponent,
    ClickCellComponent,
    AppTextboxCellComponent,
    AppSelectCellComponent,
    ChildTableComponent,
    NgbPaginationModule,
    LinkCellComponent,
    InputNumberComponent,
    CheckboxCellComponent,
    DateboxCellComponent,
  ],
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss'],
})
export class DataTableComponent {
  @ContentChildren(HeaderTableComponent) headers: QueryList<HeaderTableComponent>;
  @ContentChildren(ColumnComponent) columns: QueryList<ColumnComponent>;
  @ContentChildren(TableActionComponent) actions: QueryList<TableActionComponent>;

  @Input() records: any[] = [];
  @Input() loading: boolean = true;
  @Input() errorData: string;
  @Input() showAllCheckbox: boolean = false;
  @Input() showCheckbox: boolean = false;
  @Input() showCheckboxIcon: boolean = false;
  @Input() isReadOnly: boolean = false;
  @Input() itemEdit: boolean;
  @Input() itemRemove: boolean;
  @Input() hasClickRow: boolean = false;
  @Input() allowWrap: boolean = false;
  @Input() allowCheckToEditRow: boolean = false;
  @Input() allRecords: any[] = []; // use for select all when paging
  @Input() noDataFound: string = 'No data found.';

  // child table
  @Input() fieldChild: string;
  @Input() childHeaders: TChildHeader[];
  @Input() childColumns: TChildColumn[];
  @Input() titleChildTable: string;
  @Input() disableChildAction: boolean;
  @Input() showChildActions: boolean = false;
  @Output() editChildItemHandler = new EventEmitter<any>();
  @Output() removeChildItemHandler = new EventEmitter<any>();

  // click single row or multiple row
  @Input() selectionType: string = SelectionType.single;

  // actions
  @Input() fieldReadOnlyAction: string;
  @Input() disableAction: boolean = false;

  // input for cell
  @Input() disabledInputCell: boolean = false;

  @Input() fixColumn: 'first' | 'last' | null = null;

  // Paging
  @Input() isActivePaging: boolean;
  @Input() totalCount: number;
  @Input() pageSize: number = DEFAULT_PAGE_SIZE;
  @Input() page: number = 1;
  @Input() positionPaging: PaginationPosition = 'bottom-right';
  @Input() headerDepth: number;
  @Input() hasTotalRow: boolean;
  @Input() totalItemsSummary: any;

  @Output() editItemHandler = new EventEmitter<any>();
  @Output() removeItemHandler = new EventEmitter<any>();
  @Output() checkboxChange = new EventEmitter<any>();
  @Output() checkboxCheckedFalse = new EventEmitter<any>();
  @Output() clickCellItemHandler = new EventEmitter<any>();
  @Output() clearCellItemHandler = new EventEmitter<any>();
  @Output() itemSelectChange = new EventEmitter<any>();
  @Output() loadMore = new EventEmitter<any>();
  @Output() clickedRowHandler = new EventEmitter<any>();
  @Output() changeValueCellItemHandler = new EventEmitter<any>();
  @Output() changedPaging = new EventEmitter<number>();
  @Output() actionBadgeClicked = new EventEmitter<{ action: string; entry: any }>();

  constructor() {}

  headerLevels = {
    headerLevel1: [],
    headerLevel2: [],
  };

  ngAfterContentInit() {
    this.groupHeaderRowSpan('headerLevel1');
    this.groupHeaderRowSpan('headerLevel2');
  }

  groupHeaderRowSpan(levelName: string) {
    this.headerLevels[levelName] = [];

    let currentGroup = null;
    let currentColspan = 0;
    let lastWasGrouped = null;

    const pushGroup = (title: string, colspan: number) => {
      if (colspan > 0) {
        this.headerLevels[levelName].push({ title, colspan });
      }
    };

    for (const header of this.headers) {
      const group = (levelName === 'headerLevel2' ? header.group : header.parentGroup) || null;

      const isGrouped = !!group;

      // When group changes or switches between grouped/ungrouped
      if (group !== currentGroup || isGrouped !== lastWasGrouped) {
        // Push the previous group segment before resetting
        pushGroup(currentGroup || '', currentColspan);

        // Reset for new group
        currentGroup = group;
        currentColspan = 1;
      } else {
        // Continue current group
        currentColspan++;
      }

      lastWasGrouped = isGrouped;
    }

    // Push the last group after loop ends
    pushGroup(currentGroup || '', currentColspan);
  }

  getValue(data: any, path: string) {
    if (!path) return undefined;

    // Split the path into an array of keys
    const keys = path.split('.').map(key => key.replace('?', '')); // Remove optional chaining symbols

    // Recursive traversal
    return keys.reduce((obj, key) => {
      // Return undefined if current object is null/undefined
      return obj == null ? undefined : obj[key];
    }, data);
  }

  editActionHandler(entry: any, column: any) {
    this.editItemHandler.emit({ entry, column });
  }

  removeActionHandler(entry: any, column: any) {
    this.removeItemHandler.emit({ entry, column });
  }

  onSelectAll(event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;

    const targetRecords = this.allRecords.length ? this.allRecords : this.records;

    targetRecords.forEach(record => {
      record.checked = isChecked;
      record.isCheckEditable = isChecked && this.allowCheckToEditRow;

      // Trigger itemSelectChange for each record to auto-fill quantities
      if (isChecked) {
        // Deep copy only the fields you allow editing (same as in checkboxSelectChange)
        record._original = { ...record };
      } else {
        this.resetItemToOriginal(record);
        record.checked = false;
      }

      // Emit itemSelectChange for each record to trigger auto-fill logic
      this.itemSelectChange.emit(record);
    });

    if (isChecked) {
      const checkedRecords = this.allRecords.filter(r => r.checked);
      this.checkboxChange.emit({ event, data: checkedRecords });
    } else {
      const checkedRecords = this.allRecords.filter(r => !r.checked);
      this.checkboxCheckedFalse.emit({ event, data: checkedRecords });
    }
  }

  isAllChecked(): boolean {
    return this.allRecords.length > 0 && this.allRecords.every(record => record.checked);
  }

  resetItemToOriginal(entry: any) {
    const original = entry._original;

    if (!original) return;

    // Remove fields not in original
    for (const key in entry) {
      if (!(key in original)) {
        entry[key] = '';
      }
    }

    // Copy fields from original
    for (const key in original) {
      entry[key] = original[key];
    }

    // Final cleanup
    delete entry._original;
    delete entry.errors;
  }

  checkboxSelectChange(entry: any, event: Event) {
    // Deep copy only the fields you allow editing
    if (entry?.checked) {
      entry._original = { ...entry };
    } else {
      this.resetItemToOriginal(entry);
      entry.checked = false;
    }

    this.itemSelectChange.emit(entry);

    if (entry?.checked) {
      const checkedRecords = this.allRecords.filter(r => r.checked);
      this.checkboxChange.emit({ event, data: checkedRecords });
    } else {
      const checkedRecords = this.allRecords.filter(r => !r.checked);
      this.checkboxCheckedFalse.emit({ event, data: checkedRecords });
    }
  }

  clickCellActionHandler(entry: any, column: any) {
    this.clickCellItemHandler.emit({ entry, column });
  }

  clearCellActionHandler(entry: any, column: any) {
    this.clearCellItemHandler.emit({ entry, column });
  }

  checkReadOnly(entry: any) {
    if (this.fieldReadOnlyAction) {
      const val = this.fieldReadOnlyAction.split('.').reduce((o, i) => o?.[i], entry);
      return !val;
    }
    if (this.disableAction) {
      return true;
    }
    return this.isReadOnly ? true : false;
  }

  onClickedRow(data, $event) {
    if (
      this.hasClickRow &&
      $event.target.type !== 'checkbox' &&
      !$event.target.classList.contains('check') &&
      this.selectionType === SelectionType.single
    ) {
      // this.records.map(r => (r.isFocused = false));
      data.isFocused = !data.isFocused;
      this.clickedRowHandler.emit(data);
    }
    // handle click row for multiple row
    if (this.selectionType === SelectionType.multi) {
      data.isFocused = !data.isFocused;
      this.clickedRowHandler.emit(data);
    }
  }

  handleCellValueChanges(entry: any, column: any) {
    this.changeValueCellItemHandler.emit({ entry, column });
  }

  toggleRow(row: any): void {
    row.expanded = !row.expanded;
  }

  getColumnCount(): number {
    let count = 0;
    if (this.showCheckbox) count++;
    if (this.actions && this.actions.length) count++;
    return count + (this.headers ? this.headers.length : 0);
  }

  shouldShowPagination(position: 'top' | 'bottom'): boolean {
    if (!this.isActivePaging || !this.records || this.records.length === 0) {
      return false;
    }

    const [basePosition] = this.positionPaging.split('-');
    return basePosition === position || this.positionPaging === 'both';
  }

  getPaginationClass(position: 'top' | 'bottom'): string {
    const classes = ['pagination-container'];

    if (this.positionPaging.includes('left')) {
      classes.push('justify-content-start');
    } else if (this.positionPaging.includes('right')) {
      classes.push('justify-content-end');
    } else {
      classes.push('justify-content-center');
    }

    if (position === 'top') {
      classes.push('pagination-top');
    } else {
      classes.push('pagination-bottom');
    }

    return classes.join(' ');
  }

  onChangePage($event: number) {
    this.changedPaging.emit($event);
  }

  getCountColumnExpand(): number {
    let count = 0;
    if (this.showCheckbox) count++;
    if (this.actions && this.actions.length) count++;
    return count;
  }
}
