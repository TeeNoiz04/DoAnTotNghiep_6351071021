import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ApprovalHistoryDto } from '@proxy/approval-histories';

export interface TableColumn {
  field: string;
  header: string;
  width?: string;
  class?: string;
  fixed?: boolean;
  textAlign?: 'start' | 'center' | 'end';
  formatter?: (value: any, row: any) => string;
  useComponent?: boolean;
  componentType?: 'status' | 'action' | 'checkbox' | 'clickCell' | 'input';
  componentInput?: string;
  sum?: boolean;
  summaryValue?: string;
  // New property to define cell-specific click actions
  cellClickAction?: string;
  // New properties for action column
  showHistory?: boolean;
  showEdit?: boolean;
  showDelete?: boolean;
  // New property for input type
  inputType?: 'text' | 'number';
  editable?: boolean;
  cellClass?: string | ((value: any, row: any) => string);
  textClass?: string | ((value: any, row: any) => string);
}

export interface ColumnGroup {
  name: string;
  columns: TableColumn[];
  class?: string;
}

export interface CellClickEvent {
  column: TableColumn;
  row: any;
  field: string;
  action?: string;
}

export interface ActionClickEvent {
  action: 'edit' | 'delete' | 'save' | 'cancel';
  row: any;
  column: TableColumn;
}

export interface InputChangeEvent {
  field: string;
  value: any;
  row: any;
  column: TableColumn;
}

@Component({
  selector: 'app-price-offer-items-table',
  templateUrl: './price-offer-items-table.component.html',
  styleUrls: ['./price-offer-items-table.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, NgbModule, StatusLabelComponent, HistoryModalComponent],
})
export class PriceOfferItemsTableComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  @Input() data: any[] = [];
  @Input() columnGroups: ColumnGroup[] = [];
  @Input() height: string = '450px';
  @Input() enableErrorHandling: boolean = false;
  @Input() errorField: string = 'errors';
  @Input() hasErrorsField: string = 'hasErrors';
  @Input() rowIndexField: string = 'rowIndex';
  @Input() summaryData: any = {}; // External summary data
  @Input() enableSelection: boolean = false;
  @Input() selectedItems: any[] = [];
  @Input() selectionKey: string = 'id'; // Key to identify unique rows

  @Input() showGroupNameColumn: boolean = true;

  @Output() rowClick = new EventEmitter<any>();
  @Output() historyClick = new EventEmitter<any>();
  @Output() selectionChange = new EventEmitter<any[]>();
  @Output() cellClick = new EventEmitter<CellClickEvent>();
  @Output() actionClick = new EventEmitter<ActionClickEvent>();
  @Output() inputChange = new EventEmitter<InputChangeEvent>();

  @ViewChild('tableContainer', { static: false }) tableContainer!: ElementRef<HTMLDivElement>;

  // History modal properties
  showHistoryModal = false;
  historyModalTitle = 'History';
  currentApprovalHistories: ApprovalHistoryDto[] = [];

  private calculatedSummary: { [key: string]: any } = {};
  @Input() autoCalculateSummary: boolean = true;
  allColumns: TableColumn[] = [];
  expandedRows: { [key: number]: boolean } = {};
  private fixedColumnPositions: number[] = [];
  private resizeObserver?: ResizeObserver;
  editingRows: { [key: string]: boolean } = {};
  editingValues: { [key: string]: { [field: string]: any } } = {};

  ngOnInit() {
    this.allColumns = this.columnGroups.reduce((acc, group) => [...acc, ...group.columns], [] as TableColumn[]);
    if (this.autoCalculateSummary) {
      this.calculateSummary();
    }
  }
  ngAfterViewInit() {
    // Calculate fixed column positions after view init
    setTimeout(() => this.calculateFixedColumnPositions(), 200);

    // Set up resize observer to recalculate on size changes
    if (this.tableContainer && typeof ResizeObserver !== 'undefined') {
      this.resizeObserver = new ResizeObserver(() => {
        setTimeout(() => this.calculateFixedColumnPositions(), 50);
      });
      this.resizeObserver.observe(this.tableContainer.nativeElement);
    }
  }

  ngOnDestroy() {
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
    }
  }
  ngOnChanges(changes: SimpleChanges) {
    // Update allColumns when columnGroups changes
    if (changes.columnGroups) {
      this.allColumns = this.columnGroups.reduce((acc, group) => [...acc, ...group.columns], [] as TableColumn[]);
    }

    if (changes.data && this.data) {
      this.data.forEach((row, index) => {
        row[this.rowIndexField] = index;
        row[this.hasErrorsField] = row[this.errorField]?.length > 0;
      });
    }
    if (changes.data && this.autoCalculateSummary) {
      this.calculateSummary();
    }
    if (changes.columnGroups || changes.data) {
      // Recalculate fixed positions when columns or data change
      setTimeout(() => this.calculateFixedColumnPositions(), 100);
    }
  }
  get showSummary(): boolean {
    const result = this.allColumns.some(column => {
      return column.sum === true || column.summaryValue !== undefined;
    });
    return result;
  }

  calculateSummary(): void {
    this.calculatedSummary = {};

    if (!this.data || this.data.length === 0) {
      return;
    }
    const columnsToSum = this.allColumns.filter(col => col.sum === true);
    columnsToSum.forEach(column => {
      const fieldPath = column.field.split('.');
      const sum = this.data.reduce((total, row) => {
        let value = row;
        for (const key of fieldPath) {
          if (value === null || value === undefined) {
            return total;
          }
          value = value[key];
        }
        if (typeof value === 'number') {
          return total + value;
        }

        return total;
      }, 0);
      this.calculatedSummary[column.field] = sum;
    });
  }
  getSummaryValue(column: TableColumn): string {
    // If column has summaryValue, use it directly (it contains the actual value)
    if (column.summaryValue !== undefined && column.summaryValue !== null) {
      const value = column.summaryValue;
      if (column.formatter && typeof value === 'number') {
        return column.formatter(value, null);
      }
      return value !== undefined ? String(value) : '';
    }

    // Fallback to calculated summary for columns with sum=true
    if (column.sum && this.calculatedSummary[column.field] !== undefined) {
      const value = this.calculatedSummary[column.field];
      if (column.formatter && typeof value === 'number') {
        return column.formatter(value, null);
      }
      return value !== undefined ? String(value) : '';
    }

    return '';
  }

  hasAnyErrors(): boolean {
    if (!this.enableErrorHandling) return false;
    return this.data.some(row => row[this.hasErrorsField]);
  }

  countErrorRows(): number {
    if (!this.enableErrorHandling) return 0;
    return this.data.filter(row => row[this.hasErrorsField]).length;
  }

  toggleRowExpansion(row: any): void {
    if (!this.enableErrorHandling || !row[this.hasErrorsField]) {
      this.rowClick.emit(row);
      return;
    }

    const rowIndex = row[this.rowIndexField] || this.data.indexOf(row);
    this.expandedRows[rowIndex] = !this.expandedRows[rowIndex];
    this.rowClick.emit({ row, expanded: this.expandedRows[rowIndex] });
  }

  // Selection methods
  isSelected(row: any): boolean {
    if (!this.enableSelection) return false;
    return this.selectedItems.some(item => item[this.selectionKey] === row[this.selectionKey]);
  }

  isAllSelected(): boolean {
    if (!this.enableSelection || this.data.length === 0) return false;
    return this.data.every(row => this.isSelected(row));
  }

  isPartiallySelected(): boolean {
    if (!this.enableSelection || this.data.length === 0) return false;
    const selectedCount = this.data.filter(row => this.isSelected(row)).length;
    return selectedCount > 0 && selectedCount < this.data.length;
  }

  toggleSelection(row: any, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }

    if (!this.enableSelection) return;

    const newSelectedItems = [...this.selectedItems];
    const index = newSelectedItems.findIndex(item => item[this.selectionKey] === row[this.selectionKey]);

    if (index > -1) {
      newSelectedItems.splice(index, 1);
    } else {
      newSelectedItems.push(row);
    }

    this.selectionChange.emit(newSelectedItems);
  }

  toggleAllSelection(): void {
    if (!this.enableSelection) return;

    if (this.isAllSelected()) {
      // Deselect all current page items
      const newSelectedItems = this.selectedItems.filter(
        item => !this.data.some(row => row[this.selectionKey] === item[this.selectionKey]),
      );
      this.selectionChange.emit(newSelectedItems);
    } else {
      // Select all current page items
      const newSelectedItems = [...this.selectedItems];
      this.data.forEach(row => {
        if (!this.isSelected(row)) {
          newSelectedItems.push(row);
        }
      });
      this.selectionChange.emit(newSelectedItems);
    }
  }
  getSummaryClass(column: TableColumn): string {
    if (column.sum && this.calculatedSummary[column.field] !== undefined) {
      return 'summary-cell';
    }
    return '';
  }

  formatCellValue(column: TableColumn, row: any): string {
    const fieldPath = column.field.split('.');
    let value = row;

    for (const key of fieldPath) {
      if (value === null || value === undefined) {
        return '';
      }
      value = value[key];
    }

    let formattedValue = column.formatter ? column.formatter(value, row) : value;

    if (this.enableErrorHandling && row[this.hasErrorsField] && column.field.includes('.')) {
      const propertyName = column.field.split('.').pop() || '';
      const hasPropertyError = row[this.errorField]?.some((error: string) =>
        error.toLowerCase().includes(propertyName.toLowerCase()),
      );

      if (hasPropertyError) {
        return `<span class="error-indicator">${formattedValue || ''}
          <i class="bi bi-exclamation-circle-fill text-danger ms-1"></i>
        </span>`;
      }
    }

    // Apply text class if provided
    if (column.textClass) {
      let textClass = '';
      if (typeof column.textClass === 'function') {
        textClass = column.textClass(value, row);
      } else {
        textClass = column.textClass;
      }

      if (textClass) {
        return `<span class="${textClass}">${formattedValue || ''}</span>`;
      }
    }

    return formattedValue;
  }

  getCellValue(column: TableColumn, row: any): any {
    const isEditing = this.isEditing(row);

    if (isEditing && column.editable) {
      return this.getEditingValue(row, column.field);
    }

    const fieldPath = column.field.split('.');
    let value = row;

    for (const key of fieldPath) {
      if (value === null || value === undefined) {
        return '';
      }
      value = value[key];
    }

    return value;
  }

  getComponentData(column: TableColumn, row: any): any {
    const fieldPath = column.field.split('.');
    let value = row;

    for (const key of fieldPath) {
      if (value === null || value === undefined) {
        return '';
      }
      value = value[key];
    }

    if (value === '' || value === null || value === undefined) {
      return '';
    }

    if (column.componentType === 'status') {
      return value;
    }

    return value;
  }
  getCellClass(column: TableColumn, index?: number, row?: any): string {
    let classes = column.class || '';
    if (column.textAlign) {
      if (/text-\w+/.test(classes)) {
        classes = classes.replace(/text-\w+/, `text-${column.textAlign}`);
      } else {
        classes = `text-${column.textAlign} ${classes}`.trim();
      }
    }

    const numberOfFixedColumns = this.getNumberOfFixedColumns();

    if (column.fixed || (index !== undefined && index < numberOfFixedColumns)) {
      classes += ' fixed-column';
    }

    if (column.cellClass && row) {
      if (typeof column.cellClass === 'function') {
        const fieldPath = column.field.split('.');
        let value = row;
        for (const key of fieldPath) {
          if (value === null || value === undefined) {
            break;
          }
          value = value[key];
        }
        const customClass = column.cellClass(value, row);
        if (customClass) {
          classes += ` ${customClass}`;
        }
      } else {
        classes += ` ${column.cellClass}`;
      }
    }

    return classes;
  }

  formatCurrency(value: number): string {
    if (value === null || value === undefined) return '';
    return new Intl.NumberFormat('en-US', {
      style: 'decimal',
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
      useGrouping: true,
    }).format(value);
  }

  getColumnSpan(group: ColumnGroup): number {
    return group.columns.length;
  }
  getRowClass(row: any): string {
    if (this.enableErrorHandling && row[this.hasErrorsField]) {
      return 'has-errors';
    }
    return '';
  }

  onHistoryClick(row: any, event?: Event) {
    if (event) {
      event.stopPropagation();
    }
    this.historyClick.emit(row);
  }

  showHistory(approvalHistories: ApprovalHistoryDto[], title?: string) {
    this.currentApprovalHistories = approvalHistories || [];
    this.historyModalTitle = title || 'History';
    this.showHistoryModal = true;
  }

  closeHistoryModal() {
    this.showHistoryModal = false;
    this.currentApprovalHistories = [];
  }

  onCellClick(column: TableColumn, row: any): void {
    if (column.componentType === 'clickCell') {
      // Emit cell-specific click event with column and row information
      const cellClickEvent: CellClickEvent = {
        column: column,
        row: row,
        field: column.field,
        action: column.cellClickAction,
      };
      this.cellClick.emit(cellClickEvent);

      // Also emit the old rowClick event for backward compatibility
      this.rowClick.emit(row);
    }
  }

  getHeaderClass(column: TableColumn, index: number): string {
    let classes = column.class || '';

    // Add fixed column class for the first 3 columns
    const numberOfFixedColumns = this.getNumberOfFixedColumns();

    if (column.fixed || index < numberOfFixedColumns) {
      classes += ' fixed-column';
    }

    return classes;
  }

  private calculateFixedColumnPositions(): void {
    if (typeof document === 'undefined' || !this.tableContainer) return;

    const table = this.tableContainer.nativeElement.querySelector('table');
    if (!table) return;

    const headerCells = table.querySelectorAll('thead tr:last-child th');
    let cumulativeWidth = 0;
    this.fixedColumnPositions = [];

    // Calculate positions for the first 4 columns
    const numberOfFixedColumns = this.getNumberOfFixedColumns();
    for (let i = 0; i < Math.min(numberOfFixedColumns + 1, headerCells.length); i++) {
      this.fixedColumnPositions[i] = cumulativeWidth;

      // Apply the calculated position
      const headerCell = headerCells[i] as HTMLElement;

      // Only target specific header rows, NOT the group header row
      const startRow = this.showGroupNameColumn ? 2 : 1;

      const columnHeaderCells = table.querySelectorAll(`thead tr:nth-child(${startRow}) th:nth-child(${i + 1})`); // Column headers
      const summaryHeaderCells = table.querySelectorAll(`thead tr.summary-row th:nth-child(${i + 1})`); // Summary row if exists
      const bodyCells = table.querySelectorAll(`tbody td:nth-child(${i + 1})`);

      if (headerCell.classList.contains('fixed-column')) {
        // Set position for column header row only (not group headers)
        columnHeaderCells.forEach(cell => {
          (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
          (cell as HTMLElement).style.zIndex = `${25 - i}`;
        });

        // Set position for summary row if exists
        summaryHeaderCells.forEach(cell => {
          (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
          (cell as HTMLElement).style.zIndex = `${25 - i}`;
        });

        // Set position for body cells
        bodyCells.forEach(cell => {
          (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
          (cell as HTMLElement).style.zIndex = `${15 - i - 1}`;
        });
      }
      cumulativeWidth += headerCell.offsetWidth;
    }
  }

  private getNumberOfFixedColumns(): number {
    return this.columnGroups.reduce((count, group) => count + group.columns.filter(col => col.fixed).length, 0);
  }

  // Action handling methods
  onActionClick(action: 'edit' | 'delete' | 'save' | 'cancel', row: any, column: TableColumn, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }

    const rowKey = row[this.selectionKey];

    switch (action) {
      case 'edit':
        this.editingRows[rowKey] = true;
        this.editingValues[rowKey] = { ...row };
        break;
      case 'cancel':
        this.editingRows[rowKey] = false;
        delete this.editingValues[rowKey];
        break;
      case 'save':
      case 'delete':
        // These actions should be handled by the parent component
        break;
    }

    this.actionClick.emit({
      action,
      row,
      column,
    });
  }

  onInputChange(field: string, value: any, row: any, column: TableColumn): void {
    const rowKey = row[this.selectionKey];

    if (!this.editingValues[rowKey]) {
      this.editingValues[rowKey] = { ...row };
    }

    // Convert value to appropriate type
    let convertedValue = value;
    if (column.inputType === 'number') {
      convertedValue = parseFloat(value) || 0;
    }

    this.editingValues[rowKey][field] = convertedValue;

    this.inputChange.emit({
      field,
      value: convertedValue,
      row: this.editingValues[rowKey],
      column,
    });
  }

  onInputEvent(event: Event, field: string, row: any, column: TableColumn): void {
    const target = event.target as HTMLInputElement;
    this.onInputChange(field, target.value, row, column);
  }

  isEditing(row: any): boolean {
    const rowKey = row[this.selectionKey];
    return this.editingRows[rowKey] || false;
  }

  getEditingValue(row: any, field: string): any {
    const rowKey = row[this.selectionKey];
    return this.editingValues[rowKey]?.[field] ?? row[field];
  }

  resetEditing(): void {
    this.editingRows = {};
    this.editingValues = {};
  }
}
