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
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { RequestStatusTextMap, StatusLabelComponent } from '@app/shared/status/components/status-label.component';
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
  componentType?: 'status' | 'action' | 'custom';
  componentInput?: string;
  sum?: boolean;
  summaryValue?: string;
}

export interface ColumnGroup {
  name: string;
  columns: TableColumn[];
  class?: string;
}

@Component({
  selector: 'app-material-items-table',
  templateUrl: './material-items-table.component.html',
  styleUrls: ['./material-items-table.component.scss'],
  standalone: true,
  imports: [CommonModule, StatusLabelComponent, HistoryModalComponent],
})
export class MaterialItemsTableComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  @Input() data: any[] = [];
  @Input() columnGroups: ColumnGroup[] = [];
  @Input() height: string = '450px';
  @Input() enableErrorHandling: boolean = false;
  @Input() errorField: string = 'errors';
  @Input() hasErrorsField: string = 'hasErrors';
  @Input() rowIndexField: string = 'rowIndex';
  @Input() summaryData: any = {}; // External summary data

  @Output() rowClick = new EventEmitter<any>();
  @Output() historyClick = new EventEmitter<any>();

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

  formatCellValue(column: TableColumn, row: any): string {
    const fieldPath = column.field.split('.');
    let value = row;

    for (const key of fieldPath) {
      if (value === null || value === undefined) {
        return '';
      }
      value = value[key];
    }

    if (this.enableErrorHandling && row[this.hasErrorsField] && column.field.includes('.')) {
      const propertyName = column.field.split('.').pop() || '';
      const hasPropertyError = row[this.errorField]?.some((error: string) =>
        error.toLowerCase().includes(propertyName.toLowerCase()),
      );

      if (hasPropertyError) {
        const formattedValue = column.formatter ? column.formatter(value, row) : value;
        return `<span class="error-indicator">${formattedValue || ''}
          <i class="bi bi-exclamation-circle-fill text-danger ms-1"></i>
        </span>`;
      }
    }

    return column.formatter ? column.formatter(value, row) : value;
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
      return RequestStatusTextMap[value] || value;
    }

    return value;
  }
  getCellClass(column: TableColumn, index?: number): string {
    let classes = column.class || '';
    if (column.textAlign) {
      classes += ` text-${column.textAlign}`;
    }

    // Add fixed column class for the first 3 columns
    if (column.fixed || (index !== undefined && index < 3)) {
      classes += ' fixed-column';
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
  getHeaderClass(column: TableColumn, index: number): string {
    let classes = column.class || '';

    // Add fixed column class for the first 3 columns
    if (column.fixed || index < 3) {
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

    // Calculate positions for the first 3 columns
    for (let i = 0; i < Math.min(3, headerCells.length); i++) {
      this.fixedColumnPositions[i] = cumulativeWidth;

      // Apply the calculated position
      const headerCell = headerCells[i] as HTMLElement;

      // Only target specific header rows, NOT the group header row
      const columnHeaderCells = table.querySelectorAll(`thead tr:nth-child(2) th:nth-child(${i + 1})`); // Column headers
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
}
