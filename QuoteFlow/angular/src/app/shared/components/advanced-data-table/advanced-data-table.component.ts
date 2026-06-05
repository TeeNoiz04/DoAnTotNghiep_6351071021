import { CommonModule } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  isDevMode,
  NgZone,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { ApprovalHistoryDto } from '@proxy/approval-histories';
import { BehaviorSubject, Subject, Subscription, timer } from 'rxjs';
import { auditTime, debounceTime, filter, take, takeUntil } from 'rxjs/operators';
import { AppSubtableColumnsDirective } from './directives/subtable-columns.directive';
import { AppTableColumnGroupDirective } from './directives/table-column-group.directive';
import { AppTableColumnDirective } from './directives/table-column.directive';

export interface TableColumnConfig {
  field: string;
  header: string;
  width?: string;
  maxWidth?: string;
  minWidth?: string;
  fixed?: boolean;
  textAlign?: 'start' | 'center' | 'end';
  textClass?: string | ((value: any, row: any) => string);
  cellClass?: string | ((value: any, row: any) => string);
  showEllipsis?: boolean;
  editable?: boolean;
  sortable?: boolean;
  resizable?: boolean;
  computed?: boolean;
  alwaysShowInput?: boolean;
  formatter?: (value: any, row: any) => string;
  validator?: (value: any, row: any) => string | null;
  componentType?: 'status' | 'action' | 'checkbox' | 'clickCell' | 'input' | 'template' | 'clickCellIcon' | 'auditInfo';
  inputType?: 'text' | 'number' | 'email' | 'password' | 'textarea';
  sum?: boolean;
  summaryValue?: string | number;
  cellClickAction?: string;
  showHistory?: boolean;
  showEdit?: boolean;
  showEditModal?: boolean;
  showEditFn?: (row: any) => boolean;
  showDelete?: boolean;
  customActions?: ActionItem[];
  iconClass?: string | ((value: any, row: any) => string);

  warningTooltip?: string | ((value: any, row: any) => string | null);
  warningTooltipCondition?: (value: any, row: any) => boolean;
  warningTooltipIcon?: string;
  warningTooltipIconClass?: string;
  // Audit info field configuration
  creationTimeField?: string;
  creatorNameField?: string;
  lastModificationTimeField?: string;
  lastModifierNameField?: string;
}

export interface ActionItem {
  id: string;
  label: string;
  icon: string;
  tooltip: string;
  cssClass?: string;
  visible?: (row: any) => boolean;
  disabled?: (row: any) => boolean;
}

export interface ColumnGroupConfig {
  name: string;
  columns: TableColumnConfig[];
  class?: string;
  visible?: boolean;
}

export interface CellClickEvent {
  column: TableColumnConfig;
  row: any;
  field: string;
  action?: string;
}

export interface ActionClickEvent {
  action: string;
  row: any;
  column: TableColumnConfig;
  originalRow?: any;
}

export interface InputChangeEvent {
  field: string;
  value: any;
  row: any;
  column: TableColumnConfig;
}

export interface SelectionChangeEvent {
  selectedItems: any[];
  type: 'single' | 'multiple' | 'all' | 'none';
}

export interface SubtableConfig {
  /** Field in parent row that contains the child data array */
  dataField: string;

  /** Columns configuration for the subtable - reuses TableColumnConfig[] */
  columns?: TableColumnConfig[];

  /** Column groups for the subtable - reuses ColumnGroupConfig[] */
  columnGroups?: ColumnGroupConfig[];

  /** Condition to determine if a row can be expanded */
  expandableCondition?: (row: any) => boolean;

  /** Custom message when subtable has no data */
  noDataMessage?: string;

  /** Whether to show row numbers in subtable */
  showRowNumber?: boolean;

  /** Custom CSS class for subtable */
  cssClass?: string;

  /** Whether to show summary row in subtable */
  showSummary?: boolean;

  /** Whether subtable rows are striped */
  striped?: boolean;

  /** Key field for tracking subtable rows */
  trackByField?: string;
}

export interface RowExpansionEvent {
  row: any;
  expanded: boolean;
  type: 'error' | 'subtable';
}

@Component({
  selector: 'app-advanced-data-table',
  templateUrl: './advanced-data-table.component.html',
  styleUrls: ['./advanced-data-table.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    StatusLabelComponent,
    HistoryModalComponent,
    NgSelectModule,
    AuditInfoColumnComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppAdvancedDataTableComponent implements OnInit, AfterContentInit, OnChanges, OnDestroy {
  @ContentChildren(AppTableColumnDirective, { descendants: true })
  columnDirectives!: QueryList<AppTableColumnDirective>;

  @ContentChildren(AppTableColumnGroupDirective, { descendants: true })
  columnGroupDirectives!: QueryList<AppTableColumnGroupDirective>;

  // Subtable directives
  @ContentChild(AppSubtableColumnsDirective) subtableDirective?: AppSubtableColumnsDirective;
  @ContentChildren(AppSubtableColumnsDirective) private allSubtableDirectives?: QueryList<AppSubtableColumnsDirective>;

  @ViewChild('tableContainer', { static: false }) tableContainer!: ElementRef<HTMLDivElement>;

  allColumns: TableColumnConfig[] = [];
  processedColumnGroups: ColumnGroupConfig[] = [];
  expandedRows: { [key: number]: boolean } = {};
  expandedRowsType: { [key: number]: 'error' | 'subtable' | null } = {};
  editingRows: { [key: string]: boolean } = {};
  editingValues: { [key: string]: { [field: string]: any } } = {};
  public originalValues: { [key: string]: Record<string, unknown> } = {};
  savingRows: { [key: string]: boolean } = {};
  sortColumn: string | null = null;
  sortDirection: 'asc' | 'desc' = 'asc';
  calculatedSummary: { [key: string]: any } = {};
  subtableConfig?: SubtableConfig;
  private cachedSubtableColumns: TableColumnConfig[] = [];

  // Pagination state
  allData: any[] = [];
  paginatedData: any[] = [];
  currentPage: number = 1;
  totalItems: number = 0;
  Math = Math;

  private fixedColumnPositions: number[] = [];
  private resizeObserver?: ResizeObserver;
  private domObserver?: MutationObserver;
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly elementRef = inject(ElementRef);
  private readonly ngZone = inject(NgZone);
  private destroy$ = new Subject<void>();
  private columnStructureChange$ = new BehaviorSubject<number>(0);
  private tableUpdates$ = new Subject<void>();
  private contentReady$ = new BehaviorSubject<boolean>(false);
  private subscriptions = new Subscription();

  // History modal state
  showHistoryModal = false;
  historyModalTitle = 'History';
  currentApprovalHistories: ApprovalHistoryDto[] = [];

  // Edge scroller state
  canScrollNext = false;
  canScrollPrev = false;
  showPrevButton = false;
  showNextButton = false;
  mouseYPosition = 0;
  containerHeight = 0;
  private isMouseInside = false;
  private containerRect: DOMRect | null = null;

  @Input() data: any[] = [];
  @Input() height: string = '450px';
  @Input() maxHeight: string | null = null;
  @Input() loading: boolean = false;
  @Input() noDataMessage: string = 'No data available';

  @Input() columns: TableColumnConfig[] = [];
  @Input() columnGroups: ColumnGroupConfig[] = [];
  @Input() showRowNumber: boolean = false;
  @Input() rowNumberField?: string;
  @Input() rowNumberHeader: string = 'No.';
  @Input() rowNumberWidth: string = '50px';
  @Input() enableSelection: boolean = false;
  @Input() selectionType: 'single' | 'multiple' = 'multiple';
  @Input() selectedItems: any[] = [];
  @Input() selectionKey: string = 'id';
  @Input() selectionCondition?: (row: any) => boolean;
  @Input() enableErrorHandling: boolean = false;
  @Input() errorField: string = 'errors';
  @Input() hasErrorsField: string = 'hasErrors';
  @Input() rowIndexField: string = 'rowIndex';
  @Input() enableHighlightRow: boolean = false;
  @Input() showSummary: boolean = false;
  @Input() autoCalculateSummary: boolean = true;
  @Input() showGroupHeaders: boolean = true;
  @Input() striped: boolean = true;
  @Input() bordered: boolean = true;
  @Input() hover: boolean = true;
  @Input() condensed: boolean = false;
  @Input() actionColumnWidth: string = '120px';
  @Input() actionGroupThreshold: number = 3;

  // Pagination properties
  @Input() enableClientSidePagination: boolean = true;
  @Input() pageSize: number = 100;
  @Input() showPageSizeSelector: boolean = true;
  @Input() pageSizeOptions: number[] = [25, 50, 100, 200];
  @Input() showItemsInfo: boolean = true;
  @Input() paginationPosition: 'top' | 'bottom' | 'both' = 'bottom';

  // Edge scroller properties
  @Input() enableEdgeScroller: boolean = true;
  @Input() edgeSize: number = 80;
  @Input() scrollAmount: number = 400;

  // Subtable properties
  @Input() enableSubtable: boolean = false;

  @Input() trackBy: (index: number, item: any) => any = (index: number, item: any) => {
    return item?.id || index;
  };
  @Input() iconClass?: string;
  @Input() disableFirstRow: boolean = false;

  // Events
  @Output() rowClick = new EventEmitter<any>();
  @Output() cellClick = new EventEmitter<CellClickEvent>();
  @Output() actionClick = new EventEmitter<ActionClickEvent>();
  @Output() inputChange = new EventEmitter<InputChangeEvent>();
  @Output() rowSelectionChange = new EventEmitter<SelectionChangeEvent>();
  @Output() selectionChange = new EventEmitter<SelectionChangeEvent>();
  @Output() historyClick = new EventEmitter<any>();
  @Output() sortChange = new EventEmitter<{ field: string; direction: 'asc' | 'desc' }>();
  @Output() actionError = new EventEmitter<{ row: any; resetErrorState: () => void }>();
  @Output() editStart = new EventEmitter<any>();
  @Output() editCancel = new EventEmitter<any>();
  @Output() editSave = new EventEmitter<any>();
  @Output() pageChange = new EventEmitter<number>();
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() rowExpansion = new EventEmitter<RowExpansionEvent>();

  constructor() {}

  ngAfterContentInit(): void {
    // Validate single subtable in development
    this.validateSingleSubtable();

    // Initialize subtable after content children are available
    // @ContentChild directives are populated at this point
    if (this.enableSubtable) {
      this.initializeSubtable();
    }
  }

  ngOnInit(): void {
    // Initialize table structure
    this.initializeColumns();

    // Initialize pagination if enabled
    if (this.enableClientSidePagination && this.data?.length) {
      this.allData = [...this.data];
      this.totalItems = this.allData.length;
      this.updatePaginatedData();
    }

    if (this.autoCalculateSummary) {
      this.calculateSummary();
    }
    this.setupReactivePatterns();
    this.ngZone.runOutsideAngular(() => {
      this.setupDOMObservers();
    });
  }
  private setupReactivePatterns(): void {
    this.subscriptions.add(
      this.columnStructureChange$
        .pipe(
          takeUntil(this.destroy$),
          debounceTime(50),
          filter(count => count > 0),
        )
        .subscribe(() => {
          this.ngZone.run(() => {
            this.initializeColumns();
            this.scheduleLayoutUpdate();
          });
        }),
    );
    this.subscriptions.add(
      this.contentReady$
        .pipe(
          takeUntil(this.destroy$),
          filter(ready => ready),
        )
        .subscribe(() => {
          this.setupColumnDirectives();
          this.notifyColumnStructureChange();
        }),
    );

    this.subscriptions.add(
      this.tableUpdates$.pipe(takeUntil(this.destroy$), auditTime(30)).subscribe(() => {
        this.ngZone.run(() => {
          this.calculateFixedColumnPositions();
          this.cdr.markForCheck();
        });
      }),
    );
    if (this.columnDirectives) {
      this.subscriptions.add(
        this.columnDirectives.changes.pipe(takeUntil(this.destroy$)).subscribe(() => {
          this.setupColumnDirectives();
          this.notifyColumnStructureChange();
        }),
      );
    }

    if (this.columnGroupDirectives) {
      this.subscriptions.add(
        this.columnGroupDirectives.changes.pipe(takeUntil(this.destroy$)).subscribe(() => {
          this.notifyColumnStructureChange();
        }),
      );
    }
    timer(0)
      .pipe(take(1))
      .subscribe(() => {
        this.contentReady$.next(true);
      });
  }

  private setupDOMObservers(): void {
    if (typeof ResizeObserver !== 'undefined') {
      this.resizeObserver = new ResizeObserver(() => {
        // Throttle resize events to avoid performance issues
        setTimeout(() => {
          this.ngZone.run(() => {
            this.tableUpdates$.next();
          });
        }, 100);
      });
    }

    timer(0)
      .pipe(
        takeUntil(this.destroy$),
        filter(() => !!this.tableContainer?.nativeElement && !!this.elementRef?.nativeElement),
      )
      .subscribe(() => {
        if (this.resizeObserver && this.tableContainer?.nativeElement) {
          this.resizeObserver.observe(this.tableContainer.nativeElement);
        }
        this.calculateFixedColumnPositions();
      });
  }

  private notifyColumnStructureChange(): void {
    const currentCount = this.getTotalColumnCount();
    this.columnStructureChange$.next(currentCount);
  }

  private scheduleLayoutUpdate(): void {
    this.ngZone.runOutsideAngular(() => {
      requestAnimationFrame(() => {
        this.ngZone.run(() => {
          this.calculateFixedColumnPositions();
          this.cdr.markForCheck();
        });
      });
    });
  }

  private setupColumnDirectives(): void {
    if (!this.columnDirectives?.length) {
      return;
    }
    const allColumnDirectives = this.getAllColumnDirectives();
    allColumnDirectives.forEach(directive => {
      this.subscriptions.add(
        directive.summaryValueChange
          .pipe(takeUntil(this.destroy$))
          .subscribe((event: { field: string; value: string | number }) => {
            const column = this.allColumns.find(col => col.field === event.field);
            if (column) {
              column.summaryValue = event.value;
              this.calculateSummary();
              this.cdr.markForCheck();
            }
          }),
      );
    });
  }

  private getAllColumnDirectives(): AppTableColumnDirective[] {
    const allDirectives: AppTableColumnDirective[] = [];
    if (this.columnDirectives) {
      allDirectives.push(...this.columnDirectives.toArray());
    }
    if (this.columnGroupDirectives) {
      this.columnGroupDirectives.forEach(group => {
        if (group.columns) {
          allDirectives.push(...group.columns.toArray());
        }
      });
    }

    return allDirectives;
  }

  private getTotalColumnCount(): number {
    let count = 0;
    if (this.columnDirectives) {
      count += this.columnDirectives.filter(col => !col.isInGroup).length;
    }
    if (this.columnGroupDirectives) {
      this.columnGroupDirectives.forEach(group => {
        count += group.columns?.length || 0;
      });
    }

    return count;
  }

  private initializeColumns(): void {
    this.processedColumnGroups = [];
    this.allColumns = [];
    if (this.columnGroupDirectives?.length) {
      this.columnGroupDirectives.forEach(groupDirective => {
        const group: ColumnGroupConfig = {
          name: groupDirective.name,
          class: groupDirective.cssClass,
          visible: groupDirective.visible,
          columns: groupDirective.columns ? groupDirective.columns.map(col => this.convertDirectiveToConfig(col)) : [],
        };
        this.processedColumnGroups.push(group);
      });
    }

    if (this.columnDirectives?.length) {
      const standaloneColumns = this.columnDirectives.filter(col => !col.isInGroup && !col.isInSubtable);

      if (standaloneColumns.length > 0) {
        const standaloneGroup: ColumnGroupConfig = {
          name: '',
          columns: standaloneColumns.map(col => this.convertDirectiveToConfig(col)),
          visible: true,
        };
        this.processedColumnGroups.push(standaloneGroup);
      }
    }
    if (this.columnGroups?.length) {
      this.processedColumnGroups.push(...this.columnGroups);
    }

    // Process standalone programmatic columns
    if (this.columns?.length) {
      const standaloneGroup: ColumnGroupConfig = {
        name: '',
        columns: this.columns,
        visible: true,
      };
      this.processedColumnGroups.push(standaloneGroup);
    }

    // Flatten all columns
    this.allColumns = this.processedColumnGroups.reduce(
      (acc, group) => [...acc, ...group.columns],
      [] as TableColumnConfig[],
    );

    // Add row number column if enabled
    if (this.showRowNumber) {
      const rowNumberColumn: TableColumnConfig = {
        field: this.rowNumberField || '__rowNumber',
        header: this.rowNumberHeader,
        width: this.rowNumberWidth,
        textAlign: 'center',
        fixed: true,
        formatter: this.rowNumberField
          ? undefined
          : (_value, row) => String(this.getDisplayData().indexOf(row) + 1 + (this.currentPage - 1) * this.pageSize),
      };
      this.allColumns.unshift(rowNumberColumn);
    }

    // Add selection column if enabled
    if (this.enableSelection) {
      const selectionColumn: TableColumnConfig = {
        field: '__selection',
        header: '',
        width: '30px',
        minWidth: 'auto',
        maxWidth: 'none',
        textAlign: 'center',
        fixed: true,
        componentType: 'checkbox',
      };
      this.allColumns.unshift(selectionColumn);
    }
  }

  private processData(): void {
    if (!this.data) return;

    this.data.forEach((row, index) => {
      row[this.rowIndexField] = index;
      row[this.hasErrorsField] = row[this.errorField]?.length > 0;
    });
  }

  private calculateSummary(): void {
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

  private calculateFixedColumnPositions(): void {
    if (typeof document === 'undefined' || !this.tableContainer?.nativeElement) return;

    // 1. CHANGE: Find ALL tables, not just the first one
    const tables = this.tableContainer.nativeElement.querySelectorAll('table');

    // 2. CHANGE: Iterate through each table found
    tables.forEach((table: HTMLTableElement) => {
      // Check if THIS specific table is inside a wrapper
      const isSubTable = !!table.closest('.subtable-wrapper');

      // Debugging to verify it works
      // console.log(`Processing table. Is subtable: ${isSubTable}`);

      const headerCells = table.querySelectorAll('thead tr:last-child th');

      // Use 'return' inside forEach to act like 'continue' for the loop
      if (!headerCells.length) return;

      // 3. Apply your specific subtable offset logic
      let cumulativeWidth = 0;

      // Reset positions array for this scope (Note: If you need to store positions
      // for multiple tables, you might need a Map instead of a single array 'this.fixedColumnPositions')
      // Assuming this visual logic runs imperatively:
      const fixedColumnPositions: number[] = [];

      const numberOfFixedColumns = this.getNumberOfFixedColumns(); // careful: does this apply to all tables?

      for (let i = 0; i < Math.min(numberOfFixedColumns + 1, headerCells.length); i++) {
        fixedColumnPositions[i] = cumulativeWidth;

        const headerCell = headerCells[i] as HTMLElement;
        const startRow = this.showGroupHeaders ? 2 : 1;

        // Scoped queries to the current 'table' instance
        const columnHeaderCells = table.querySelectorAll(`thead tr:nth-child(${startRow}) th:nth-child(${i + 1})`);
        const summaryHeaderCells = table.querySelectorAll(`thead tr.summary-row th:nth-child(${i + 1})`);
        const bodyCells = table.querySelectorAll(`tbody td:nth-child(${i + 1})`);

        if (headerCell.classList.contains('fixed-column')) {
          columnHeaderCells.forEach(cell => {
            (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
            (cell as HTMLElement).style.zIndex = `${25 - i}`;
          });

          summaryHeaderCells.forEach(cell => {
            (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
            (cell as HTMLElement).style.zIndex = `${25 - i}`;
          });

          bodyCells.forEach(cell => {
            (cell as HTMLElement).style.left = `${cumulativeWidth}px`;
            (cell as HTMLElement).style.zIndex = `${15 - i - 1}`;
          });
        }

        cumulativeWidth += headerCell.offsetWidth;
      }
    });
  }

  private convertDirectiveToConfig(directive: AppTableColumnDirective): TableColumnConfig {
    return {
      field: directive.field,
      header: directive.header,
      width: directive.width,
      maxWidth: directive.maxWidth,
      minWidth: directive.minWidth,
      fixed: directive.fixed,
      textAlign: directive.textAlign,
      textClass: directive.textClass,
      cellClass: directive.cellClass,
      showEllipsis: directive.showEllipsis,
      editable: directive.editable,
      sortable: directive.sortable,
      resizable: directive.resizable,
      computed: directive.computed,
      alwaysShowInput: directive.alwaysShowInput,
      formatter: directive.formatter,
      validator: directive.validator,
      componentType: directive.componentType,
      inputType: directive.inputType,
      sum: directive.sum,
      summaryValue: directive.summaryValue,
      cellClickAction: directive.cellClickAction,
      showHistory: directive.showHistory,
      showEdit: directive.showEdit,
      showEditModal: directive.showEditModal,
      showEditFn: directive.showEditFn,
      showDelete: directive.showDelete,
      customActions: directive.customActions,
      iconClass: directive.iconClass,
      // NEW: Add cell tooltip properties
      warningTooltip: directive.warningTooltip,
      warningTooltipCondition: directive.warningTooltipCondition,
      warningTooltipIcon: directive.warningTooltipIcon,
      warningTooltipIconClass: directive.warningTooltipIconClass,
    };
  }

  private getNumberOfFixedColumns(): number {
    return this.allColumns.filter(col => col.fixed).length;
  }

  ngOnChanges(changes: SimpleChanges): void {
    let needsColumnUpdate = false;
    let needsDataUpdate = false;
    let needsSummaryUpdate = false;

    if (changes.columns || changes.columnGroups) {
      needsColumnUpdate = true;
    }

    if (changes.data && this.data) {
      this.handleDataChange();
      needsDataUpdate = true;

      if (this.autoCalculateSummary) {
        needsSummaryUpdate = true;
      }
    }

    if (changes.enableClientSidePagination || changes.pageSize) {
      this.handlePaginationChange();
    }

    // Reinitialize subtable when enableSubtable changes
    if (changes.enableSubtable && !changes.enableSubtable.firstChange) {
      this.initializeSubtable();
    }

    if (needsColumnUpdate) {
      this.notifyColumnStructureChange();
    }

    if (needsSummaryUpdate) {
      this.calculateSummary();
    }

    if ((needsColumnUpdate || needsDataUpdate) && this.tableContainer) {
      this.tableUpdates$.next();
    }
  }

  // === Pagination Methods ===
  private handleDataChange(): void {
    this.processData();

    if (this.enableClientSidePagination) {
      this.allData = [...this.data];
      this.totalItems = this.allData.length;
      this.currentPage = 1; // Reset to first page when data changes
      this.updatePaginatedData();
    }
  }

  private handlePaginationChange(): void {
    if (this.enableClientSidePagination) {
      this.allData = [...this.data];
      this.totalItems = this.allData.length;
      this.currentPage = 1; // Reset to first page when pagination settings change
      this.updatePaginatedData();
    }
  }

  private updatePaginatedData(): void {
    if (!this.enableClientSidePagination) {
      return;
    }

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedData = this.allData.slice(startIndex, endIndex);

    // Force change detection for OnPush strategy
    this.cdr.detectChanges();
  }

  getDisplayData(): any[] {
    return this.enableClientSidePagination ? this.paginatedData : this.data;
  }

  onPageChanged(page: number): void {
    // Always update pagination data when page change event is fired
    this.currentPage = page;
    this.updatePaginatedData();
    this.pageChange.emit(page);
  }

  onPageSizeChanged(): void {
    this.currentPage = 1; // Reset to first page when page size changes
    this.updatePaginatedData();
    this.pageSizeChange.emit(this.pageSize);
  }

  // === Warning Tooltip Methods ===

  /**
   * Checks if a cell should show a tooltip icon
   */
  shouldShowWarningTooltip(column: TableColumnConfig, row: any): boolean {
    if (!column.warningTooltip) return false;

    // If there's a condition, check it
    if (column.warningTooltipCondition) {
      const value = this.getCellValue(column, row);
      return column.warningTooltipCondition(value, row);
    }

    // If no condition, always show (as long as warningTooltip is defined)
    return true;
  }

  /**
   * Gets the tooltip message for a cell
   */
  getWarningTooltipMessage(column: TableColumnConfig, row: any): string {
    if (!column.warningTooltip) return '';

    // If warningTooltip is a function, call it
    if (typeof column.warningTooltip === 'function') {
      const value = this.getCellValue(column, row);
      const message = column.warningTooltip(value, row);
      return message || '';
    }

    // Otherwise return the string value
    return column.warningTooltip;
  }

  /**
   * Gets the icon class for cell tooltip (defaults to warning triangle)
   */
  getWarningTooltipIcon(column: TableColumnConfig): string {
    return column.warningTooltipIcon || 'bi-exclamation-triangle-fill';
  }

  /**
   * Gets the icon color class for cell tooltip (defaults to warning color)
   */
  getWarningTooltipIconClass(column: TableColumnConfig): string {
    return column.warningTooltipIconClass || 'text-warning';
  }

  shouldShowPagination(): boolean {
    return this.enableClientSidePagination && this.allData && this.allData.length > this.pageSize;
  }

  shouldShowPaginationTop(): boolean {
    return this.shouldShowPagination() && (this.paginationPosition === 'top' || this.paginationPosition === 'both');
  }

  shouldShowPaginationBottom(): boolean {
    return this.shouldShowPagination() && (this.paginationPosition === 'bottom' || this.paginationPosition === 'both');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.columnStructureChange$.complete();
    this.tableUpdates$.complete();
    this.contentReady$.complete();
    this.subscriptions.unsubscribe();

    // Clean up observers
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
      this.resizeObserver = undefined;
    }

    if (this.domObserver) {
      this.domObserver.disconnect();
      this.domObserver = undefined;
    }

    // Clear data references
    this.data = [];
    this.allColumns = [];
    this.processedColumnGroups = [];
    this.editingRows = {};
    this.savingRows = {};
    this.expandedRows = {};

    // Complete event emitters
    this.rowClick.complete();
    this.cellClick.complete();
    this.actionClick.complete();
    this.inputChange.complete();
    this.rowSelectionChange.complete();
    this.selectionChange.complete();
    this.historyClick.complete();
    this.sortChange.complete();
    this.actionError.complete();
    this.editStart.complete();
    this.editCancel.complete();
    this.editSave.complete();
  }

  public refreshTable(): void {
    this.tableUpdates$.next();
  }

  public reinitializeColumns(): void {
    this.notifyColumnStructureChange();
  }

  getCellValue(column: TableColumnConfig, row: any, index?: number): any {
    if ((column.field === '__rowNumber' || column.field === '__subtableRowNumber') && !this.rowNumberField) {
      return index !== undefined ? index + 1 : '';
    }

    const isEditing = this.isEditing(row);
    if (column.alwaysShowInput) {
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

    if (isEditing && column.editable) {
      return this.getEditingValue(row, column.field);
    }

    if (column.computed && column.formatter) {
      return column.formatter(null, row);
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

  formatCellValue(column: TableColumnConfig, row: any, index?: number): string {
    const value = this.getCellValue(column, row, index);

    if (column.formatter) {
      return column.formatter(value, row);
    }

    return value !== null && value !== undefined ? String(value) : '';
  }

  getCellClass(column: TableColumnConfig, row?: any): string {
    let classes = column.cellClass
      ? typeof column.cellClass === 'function'
        ? column.cellClass(this.getCellValue(column, row), row)
        : column.cellClass
      : '';

    if (column.textAlign) {
      classes += ` text-${column.textAlign}`;
    }

    if (column.fixed) {
      classes += ' fixed-column';
    }

    if (column.width) {
      const widthMatch = column.width.match(/(\d+)px/);
      if (widthMatch) {
        classes += ` width_${widthMatch[1]}`;
      }
    }

    return classes.trim();
  }

  getTextClass(column: TableColumnConfig, row?: any): string {
    if (!column.textClass) return '';

    return typeof column.textClass === 'function'
      ? column.textClass(this.getCellValue(column, row), row)
      : column.textClass;
  }

  getHeaderClass(column: TableColumnConfig): string {
    let classes = '';

    if (column.fixed) {
      classes += ' fixed-column';
    }

    if (column.width) {
      const widthMatch = column.width.match(/(\d+)px/);
      if (widthMatch) {
        classes += ` width_${widthMatch[1]}`;
      }
    }

    return classes.trim();
  }

  getRowClass(row: any): string {
    if (this.enableErrorHandling && row[this.hasErrorsField]) {
      return 'has-errors';
    }

    if (this.enableHighlightRow && row[this.hasErrorsField]) {
      return 'highlight-row';
    }

    if (row?.warnings?.length > 0) {
      return 'has-warnings';
    }

    return '';
  }

  getSummaryValue(column: TableColumnConfig): string {
    if (column.summaryValue !== undefined && column.summaryValue !== null) {
      const value = column.summaryValue;
      if (column.formatter && typeof value === 'number') {
        const formatted = column.formatter(value, null);
        return formatted;
      }
      return value !== undefined ? String(value) : '';
    }

    if (this.autoCalculateSummary && column.sum && this.calculatedSummary[column.field] !== undefined) {
      const value = this.calculatedSummary[column.field];
      if (column.formatter && typeof value === 'number') {
        const formatted = column.formatter(value, null);
        return formatted;
      }
      return value !== undefined ? String(value) : '';
    }

    return '';
  }

  // --- Selection Methods ---
  isRowSelectable(row: any): boolean {
    if (!this.enableSelection) return false;
    if (this.selectionCondition) {
      return this.selectionCondition(row);
    }
    return true;
  }

  isSelected(row: any): boolean {
    if (!this.enableSelection) return false;
    return this.selectedItems.some(item => item[this.selectionKey] === row[this.selectionKey]);
  }

  isAllSelected(): boolean {
    if (!this.enableSelection || this.data.length === 0) return false;
    const selectableRows = this.data.filter(row => this.isRowSelectable(row));
    if (selectableRows.length === 0) return false;
    return selectableRows.every(row => this.isSelected(row));
  }

  isPartiallySelected(): boolean {
    if (!this.enableSelection || this.data.length === 0) return false;
    const selectableRows = this.data.filter(row => this.isRowSelectable(row));
    if (selectableRows.length === 0) return false;
    const selectedCount = selectableRows.filter(row => this.isSelected(row)).length;
    return selectedCount > 0 && selectedCount < selectableRows.length;
  }

  toggleSelection(row: any, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }

    if (!this.enableSelection) {
      return;
    }

    const newSelectedItems = [...this.selectedItems];

    const index = newSelectedItems.findIndex(item => item[this.selectionKey] === row[this.selectionKey]);

    // For change events, use the checkbox checked state
    const isChecked = (event?.target as HTMLInputElement)?.checked;

    if (event?.type === 'change' && isChecked !== undefined) {
      // Handle based on checkbox state
      if (isChecked && index === -1) {
        if (this.selectionType === 'single') {
          newSelectedItems.length = 0;
        }
        newSelectedItems.push(row);
      } else if (!isChecked && index > -1) {
        newSelectedItems.splice(index, 1);
      }
    } else {
      // Default toggle behavior
      if (index > -1) {
        newSelectedItems.splice(index, 1);
      } else {
        if (this.selectionType === 'single') {
          newSelectedItems.length = 0;
        }
        newSelectedItems.push(row);
      }
    }

    this.selectedItems = newSelectedItems;
    const eventData = {
      selectedItems: newSelectedItems,
      type:
        index > -1
          ? 'single'
          : this.selectionType === 'single'
            ? 'single'
            : ('multiple' as 'single' | 'multiple' | 'all' | 'none'),
    };

    this.cdr.markForCheck();

    this.selectionChange.emit(eventData);
  }

  toggleAllSelection(): void {
    if (!this.enableSelection) {
      return;
    }

    const selectableRows = this.data.filter(row => this.isRowSelectable(row));

    if (selectableRows.length === 0) {
      return;
    }

    if (this.isAllSelected()) {
      const newSelectedItems = this.selectedItems.filter(
        item => !selectableRows.some(row => row[this.selectionKey] === item[this.selectionKey]),
      );

      // Force change detection
      this.cdr.markForCheck();

      this.selectionChange.emit({
        selectedItems: newSelectedItems,
        type: 'none',
      });
    } else {
      const newSelectedItems = [...this.selectedItems];

      selectableRows.forEach(row => {
        if (!this.isSelected(row)) {
          newSelectedItems.push(row);
        }
      });

      // Force change detection
      this.cdr.markForCheck();

      this.selectionChange.emit({
        selectedItems: newSelectedItems,
        type: 'all',
      });
    }
  }

  // --- Editing Methods ---
  isEditing(row: any): boolean {
    const rowKey = row[this.selectionKey];
    return this.editingRows[rowKey] || false;
  }

  isSaving(row: any): boolean {
    const rowKey = row[this.selectionKey];
    return this.savingRows[rowKey] || false;
  }

  isErrorSaving(row: any): void {
    const rowKey = row[this.selectionKey];
    delete this.savingRows[rowKey];
  }

  handleActionError(row: any): void {
    this.isErrorSaving(row);
  }

  handleActionSuccess(row: any): void {
    this.completeSave(row);
  }

  getEditingValue(row: any, field: string): any {
    const rowKey = row[this.selectionKey];
    return this.editingValues[rowKey]?.[field] ?? row[field];
  }

  getOriginalValue(row: any, field: string): any {
    const rowKey = row[this.selectionKey];
    return this.originalValues[rowKey]?.[field] ?? row[field];
  }

  onActionClick(action: string, row: any, column: TableColumnConfig, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }

    const rowKey = row[this.selectionKey];
    const currentRow = action === 'save' && this.editingValues[rowKey] ? this.editingValues[rowKey] : row;

    switch (action) {
      case 'edit':
        this.editingRows[rowKey] = true;
        this.editingValues[rowKey] = { ...row };
        this.originalValues[rowKey] = { ...row };
        this.editStart.emit(row);
        break;
      case 'cancel':
        this.editingRows[rowKey] = false;
        delete this.editingValues[rowKey];
        delete this.originalValues[rowKey];
        delete this.savingRows[rowKey];
        this.editCancel.emit(row);
        break;
      case 'save':
        this.savingRows[rowKey] = true;
        this.actionError.emit({
          row: currentRow,
          resetErrorState: () => this.isErrorSaving(row),
        });
        this.editSave.emit(currentRow);
        break;
      case 'delete':
        break;
    }

    this.actionClick.emit({
      action,
      row: currentRow,
      column,
      originalRow: this.originalValues[rowKey],
    });
  }

  updateCellValue(column: TableColumnConfig, row: any, value: any): void {
    let convertedValue: any = value;
    if (column.inputType === 'number') {
      convertedValue = parseFloat(value) || 0;
    }

    const rowKey = row[this.selectionKey];

    if (!this.editingValues[rowKey]) {
      this.editingValues[rowKey] = { ...row };
    }

    this.editingValues[rowKey][column.field] = convertedValue;

    this.inputChange.emit({
      field: column.field,
      value: convertedValue,
      row: this.editingValues[rowKey],
      column,
    });
  }

  onInputChange(field: string, event: Event, row: any, column: TableColumnConfig): void {
    const value = (event.target as HTMLInputElement).value;
    this.updateCellValue(column, row, value);
  }

  onCellClick(column: TableColumnConfig, row: any): void {
    // Block cell click for first row if disableFirstRow is enabled
    if (this.disableFirstRow && this.getDisplayData().indexOf(row) === 0) return;

    const cellClickEvent: CellClickEvent = {
      column: column,
      row: row,
      field: column.field,
      action: column.cellClickAction,
    };
    this.cellClick.emit(cellClickEvent);
  }

  onRowClickForErrors(event: Event, row: any): void {
    // Deprecated: Use onRowClickHandler instead
    this.onRowClickHandler(event, row);
  }

  onHistoryClick(row: any, event?: Event) {
    if (event) {
      event.stopPropagation();
    }
    this.historyClick.emit(row);
  }

  // Deprecated: Use new toggleRowExpansion(row, type) instead
  // This method is kept for backward compatibility but calls the new implementation

  showHistory(approvalHistories: ApprovalHistoryDto[], title?: string) {
    this.currentApprovalHistories = approvalHistories || [];
    this.historyModalTitle = title || 'History';
    this.showHistoryModal = true;
  }

  closeHistoryModal() {
    this.showHistoryModal = false;
    this.currentApprovalHistories = [];
  }

  hasAnyErrors(): boolean {
    if (!this.enableErrorHandling) return false;
    return this.data.some(row => row[this.hasErrorsField]);
  }

  hasSelectableRows(): boolean {
    if (!this.enableSelection || !this.data || this.data.length === 0) return false;
    return this.data.some(row => this.isRowSelectable(row));
  }

  countErrorRows(): number {
    if (!this.enableErrorHandling) return 0;
    return this.data.filter(row => row[this.hasErrorsField]).length;
  }

  getColumnSpan(group: ColumnGroupConfig): number {
    return group.columns.length;
  }

  resetEditing(): void {
    this.editingRows = {};
    this.editingValues = {};
    this.originalValues = {};
    this.savingRows = {};
  }

  completeSave(row: any): void {
    const rowKey = row[this.selectionKey];
    this.editingRows[rowKey] = false;
    delete this.editingValues[rowKey];
    delete this.originalValues[rowKey];
    delete this.savingRows[rowKey];
  }

  cancelSave(row: any): void {
    const rowKey = row[this.selectionKey];
    delete this.savingRows[rowKey];
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

  getReducedWidth(width: string | undefined, reduceBy: number = 10): string {
    if (!width) return '150px';

    const match = width.match(/^(\d+)px$/);
    if (match) {
      const numericValue = parseInt(match[1], 10);
      const reducedValue = Math.max(numericValue - reduceBy, 50);
      return `${reducedValue}px`;
    }

    return width;
  }

  getColumnTemplate(column: TableColumnConfig, templateType: 'cell' | 'header' | 'edit' | 'compute'): any {
    const directive = this.getColumnDirective(column.field);
    if (!directive) return null;

    switch (templateType) {
      case 'cell':
        return directive.cellTemplate;
      case 'header':
        return directive.headerTemplate;
      case 'edit':
        return directive.editTemplate;
      case 'compute':
        return directive.computeTemplate;
      default:
        return null;
    }
  }

  getCellTemplateContext(column: TableColumnConfig, row: any, index: number): any {
    return {
      $implicit: this.getCellValue(column, row, index),
      value: this.getCellValue(column, row, index),
      row: row,
      column: column,
      index: index,
      formattedValue: this.formatCellValue(column, row, index),
      isEditing: this.isEditing(row),
      isSelected: this.isSelected(row),
    };
  }

  private getColumnDirective(field: string): AppTableColumnDirective | undefined {
    return this.columnDirectives?.find(directive => directive.field === field);
  }

  trackByFn(index: number, item: any): any {
    return item?.id || index;
  }

  // Edge scroller methods
  onContainerMouseMove(event: MouseEvent): void {
    if (!this.enableEdgeScroller) return;

    this.isMouseInside = true;
    const container = this.tableContainer?.nativeElement?.querySelector('.table-responsive') as HTMLElement;
    if (!container) return;

    if (!this.containerRect) {
      this.containerRect = container.getBoundingClientRect();
      this.containerHeight = this.containerRect.height;
    }

    const { left, top, width } = this.containerRect;
    const relativeX = event.clientX - left;
    const relativeY = event.clientY - top;

    this.mouseYPosition = relativeY;

    const distanceFromLeft = relativeX;
    const distanceFromRight = width - relativeX;

    const shouldShowPrev = this.canScrollPrev && distanceFromLeft <= this.edgeSize;
    const shouldShowNext = this.canScrollNext && distanceFromRight <= this.edgeSize;

    this.showPrevButton = shouldShowPrev;
    this.showNextButton = shouldShowNext;

    this.cdr.markForCheck();
  }

  onContainerMouseEnter(): void {
    if (!this.enableEdgeScroller) return;

    this.isMouseInside = true;
    this.updateScrollButtons();
  }

  onContainerMouseLeave(): void {
    if (!this.enableEdgeScroller) return;

    this.isMouseInside = false;
    this.showPrevButton = false;
    this.showNextButton = false;
    this.containerRect = null;
    this.cdr.markForCheck();
  }

  scrollPrev(): void {
    if (!this.enableEdgeScroller) return;

    const container = this.tableContainer?.nativeElement?.querySelector('.table-responsive') as HTMLElement;
    if (!container) return;

    const targetScrollLeft = Math.max(0, container.scrollLeft - this.scrollAmount);
    container.scrollLeft = targetScrollLeft;

    setTimeout(() => this.updateScrollButtons(), 10);
  }

  scrollNext(): void {
    if (!this.enableEdgeScroller) return;

    const container = this.tableContainer?.nativeElement?.querySelector('.table-responsive') as HTMLElement;
    if (!container) return;

    const maxScrollLeft = container.scrollWidth - container.clientWidth;
    const targetScrollLeft = Math.min(maxScrollLeft, container.scrollLeft + this.scrollAmount);
    container.scrollLeft = targetScrollLeft;

    setTimeout(() => this.updateScrollButtons(), 10);
  }

  getButtonYPosition(): number {
    if (this.mouseYPosition > 0) {
      const buttonSize = 32;
      const halfButtonSize = buttonSize / 2;
      return (
        Math.max(halfButtonSize, Math.min(this.mouseYPosition, this.containerHeight - halfButtonSize)) - halfButtonSize
      );
    }
    return this.containerHeight / 2;
  }

  private updateScrollButtons(): void {
    if (!this.enableEdgeScroller) return;

    const container = this.tableContainer?.nativeElement?.querySelector('.table-responsive') as HTMLElement;
    if (!container) return;

    const maxHorizontalScroll = container.scrollWidth - container.clientWidth;

    this.canScrollPrev = container.scrollLeft > 2;
    this.canScrollNext = container.scrollLeft < maxHorizontalScroll - 2;

    this.cdr.markForCheck();
  }

  onTableScroll(): void {
    if (this.enableEdgeScroller) {
      this.containerRect = null;
      this.updateScrollButtons();
    }
  }

  // === Subtable Methods ===

  /**
   * Validate that only one subtable directive is used (development mode only)
   */
  private validateSingleSubtable(): void {
    if (!isDevMode()) return; // Only warn in development

    const count = this.allSubtableDirectives?.length || 0;

    if (count > 1) {
      console.warn(
        '%c[AppAdvancedDataTable] Multiple Subtable Configuration Error',
        'color: #ff6b6b; font-weight: bold; font-size: 14px;',
        `\n\n⚠️ Found ${count} <app-subtable-columns> directives, but only ONE is allowed per table.` +
          `\n\n📋 Current subtables detected:`,
        this.allSubtableDirectives?.map((dir, i) => ({
          index: i + 1,
          dataField: dir.dataField,
          columnCount: dir.getTotalColumnCount(),
        })),
        `\n\n✅ Solution: Remove extra <app-subtable-columns> and keep only one.` +
          `\n\n🔍 Note: Only the FIRST subtable will be rendered.`,
      );
    }
  }

  /**
   * Initialize subtable configuration from directive
   */
  private initializeSubtable(): void {
    if (!this.enableSubtable || !this.subtableDirective) {
      this.subtableConfig = undefined;
      this.cachedSubtableColumns = [];
      return;
    }

    const directive = this.subtableDirective;
    const columns: TableColumnConfig[] = [];
    const columnGroups: ColumnGroupConfig[] = [];

    // Process column groups
    if (directive.columnGroups?.length) {
      directive.columnGroups.forEach(groupDirective => {
        const group: ColumnGroupConfig = {
          name: groupDirective.name,
          class: groupDirective.cssClass,
          visible: groupDirective.visible,
          columns: groupDirective.columns ? groupDirective.columns.map(col => this.convertDirectiveToConfig(col)) : [],
        };
        columnGroups.push(group);
      });
    }

    // Process standalone columns
    if (directive.columns?.length) {
      const standaloneColumns = directive.columns.filter(col => !col.isInGroup);
      standaloneColumns.forEach(col => {
        columns.push(this.convertDirectiveToConfig(col));
      });
    }

    this.subtableConfig = {
      dataField: directive.dataField,
      columns,
      columnGroups,
      expandableCondition: directive.expandableCondition,
      noDataMessage: directive.noDataMessage,
      showRowNumber: directive.showRowNumber,
      cssClass: directive.cssClass,
      showSummary: directive.showSummary,
      striped: directive.striped,
      trackByField: directive.trackByField,
    };

    // Build and cache subtable columns to prevent recreating on every change detection
    this.buildSubtableColumns();
  }

  /**
   * Check if row can show subtable
   */
  canShowSubtable(row: any): boolean {
    if (!this.enableSubtable || !this.subtableConfig) {
      return false;
    }

    if (this.subtableConfig.expandableCondition) {
      return this.subtableConfig.expandableCondition(row);
    }

    const childData = row[this.subtableConfig.dataField];
    return Array.isArray(childData) && childData.length > 0;
  }

  /**
   * Get subtable data for a row
   */
  getSubtableData(row: any): any[] {
    if (!this.subtableConfig) return [];

    const childData = row[this.subtableConfig.dataField];
    return Array.isArray(childData) ? childData : [];
  }

  /**
   * Build subtable columns once and cache them to prevent DOM thrashing
   * This prevents Angular from recreating DOM elements on every change detection
   */
  private buildSubtableColumns(): void {
    if (!this.subtableConfig) {
      this.cachedSubtableColumns = [];
      return;
    }

    const columns: TableColumnConfig[] = [];

    // Add columns from groups
    if (this.subtableConfig.columnGroups?.length) {
      this.subtableConfig.columnGroups.forEach(group => {
        columns.push(...group.columns);
      });
    }

    // Add standalone columns
    if (this.subtableConfig.columns?.length) {
      columns.push(...this.subtableConfig.columns);
    }

    // Add row number column if enabled
    // IMPORTANT: This object must be created once and reused, not recreated on every call
    if (this.subtableConfig.showRowNumber) {
      columns.unshift({
        field: '__subtableRowNumber',
        header: 'No.',
        width: '50px',
        textAlign: 'center',
        fixed: true,
        // formatter: (_value, _row, index) => String((index || 0) + 1),
      });
    }

    this.cachedSubtableColumns = columns;
  }

  /**
   * Get all columns for subtable (flattened)
   * Returns cached columns to prevent recreating the row number column object on every change detection
   */
  getSubtableColumns(): TableColumnConfig[] {
    return this.cachedSubtableColumns;
  }

  /**
   * Toggle row expansion (handles both error and subtable)
   */
  toggleRowExpansion(row: any, type: 'error' | 'subtable'): void {
    const rowIndex = row[this.rowIndexField] || this.data.indexOf(row);
    const currentType = this.expandedRowsType[rowIndex];

    // If clicking same type, toggle off
    if (currentType === type) {
      this.expandedRowsType[rowIndex] = null;
      this.expandedRows[rowIndex] = false;
      this.rowExpansion.emit({ row, expanded: false, type });
      return;
    }

    // Otherwise, expand with new type
    this.expandedRowsType[rowIndex] = type;
    this.expandedRows[rowIndex] = true;
    this.rowExpansion.emit({ row, expanded: true, type });
  }

  /**
   * Check if row is expanded with specific type
   */
  isRowExpandedAs(row: any, type: 'error' | 'subtable'): boolean {
    const rowIndex = row[this.rowIndexField] || this.data.indexOf(row);
    return this.expandedRows[rowIndex] && this.expandedRowsType[rowIndex] === type;
  }

  /**
   * Handle row click - determine expansion type
   */
  onRowClickHandler(event: Event, row: any): void {
    const target = event.target as HTMLElement;

    // Don't handle clicks on interactive elements
    if (target.tagName === 'INPUT' || target.tagName === 'BUTTON' || target.tagName === 'A') {
      return;
    }

    // Don't handle clicks on clickable cells
    if (target.classList.contains('clickable-cell')) {
      return;
    }

    // Priority: errors > subtable
    if (this.enableErrorHandling && row[this.hasErrorsField]) {
      this.toggleRowExpansion(row, 'error');
      return;
    }

    if (this.enableSubtable && this.canShowSubtable(row)) {
      this.toggleRowExpansion(row, 'subtable');
      return;
    }

    // Just emit row click
    this.rowClick.emit(row);
  }
}
