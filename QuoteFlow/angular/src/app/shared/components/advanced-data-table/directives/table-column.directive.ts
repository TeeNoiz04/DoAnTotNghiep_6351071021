import {
  ContentChild,
  Directive,
  EventEmitter,
  Host,
  Input,
  OnChanges,
  Optional,
  Output,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { ActionItem } from '../advanced-data-table.component';
import { AppTableColumnGroupDirective } from './table-column-group.directive';
import { AppSubtableColumnsDirective } from './subtable-columns.directive';

@Directive({
  selector: 'app-table-column',
  standalone: true,
})
export class AppTableColumnDirective implements OnChanges {
  // Basic column properties
  @Input() field: string = '';
  @Input() header: string = '';
  @Input() tooltip: string = '';
  @Input() width?: string;
  @Input() maxWidth?: string;
  @Input() minWidth?: string;

  // Layout and positioning
  @Input() fixed: boolean = false;
  @Input() textAlign: 'start' | 'center' | 'end' = 'start';
  @Input() textClass?: string | ((value: any, row: any) => string);
  @Input() cellClass?: string | ((value: any, row: any) => string);
  @Input() headerClass?: string;

  // Text handling
  @Input() showEllipsis: boolean = false;

  // Behavior properties
  @Input() editable: boolean = false;
  @Input() sortable: boolean = false;
  @Input() resizable: boolean = false;
  @Input() computed: boolean = false;
  /** When true, input components are always visible regardless of edit/cancel actions */
  @Input() alwaysShowInput: boolean = false;

  // Functions
  @Input() formatter?: (value: any, row: any) => string;
  @Input() validator?: (value: any, row: any) => string | null;

  // Component type and configuration
  @Input() componentType?:
    | 'status'
    | 'action'
    | 'checkbox'
    | 'clickCell'
    | 'input'
    | 'template'
    | 'clickCellIcon'
    | 'auditInfo';
  @Input() inputType?: 'text' | 'number' | 'email' | 'password' | 'textarea';

  // Summary and calculation
  @Input() sum: boolean = false;
  @Input() summaryValue?: string | number;

  // Click handling
  @Input() cellClickAction?: string;

  // Action column specific properties
  @Input() showHistory: boolean = false;
  @Input() showEdit: boolean = false;
  @Input() showEditModal: boolean = false;
  @Input() showEditFn?: (row: any) => boolean;
  @Input() showDelete: boolean = false;
  @Input() customActions?: ActionItem[];
  @Input() iconClass?: string;

  // NEW: Warning tooltip properties
  @Input() warningTooltip?: string | ((value: any, row: any) => string | null);
  @Input() warningTooltipCondition?: (value: any, row: any) => boolean;
  @Input() warningTooltipIcon?: string;
  @Input() warningTooltipIconClass?: string;

  // Audit info field configuration (with default values)
  @Input() creationTimeField: string = 'creationTime';
  @Input() creatorNameField: string = 'creatorUsername';
  @Input() lastModificationTimeField: string = 'lastModificationTime';
  @Input() lastModifierNameField: string = 'lastModifierUsername';

  @Output() summaryValueChange = new EventEmitter<{ field: string; value: string | number }>();

  // Template references
  @ContentChild('cellTemplate', { static: false }) cellTemplate?: TemplateRef<any>;
  @ContentChild('headerTemplate', { static: false }) headerTemplate?: TemplateRef<any>;
  @ContentChild('editTemplate', { static: false }) editTemplate?: TemplateRef<any>;
  @ContentChild('computeTemplate', { static: false }) computeTemplate?: TemplateRef<any>;

  constructor(
    @Optional() @Host() private columnGroup?: AppTableColumnGroupDirective,
    @Optional() @Host() private subtable?: AppSubtableColumnsDirective,
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['summaryValue']) {
      if (this.field && this.summaryValue !== undefined) {
        this.summaryValueChange.emit({ field: this.field, value: this.summaryValue });
      }
    }
  }

  /**
   * Indicates if this column is part of a column group
   */
  get isInGroup(): boolean {
    return !!this.columnGroup;
  }

  /**
   * Gets the parent column group if this column belongs to one
   */
  get parentGroup(): AppTableColumnGroupDirective | undefined {
    return this.columnGroup;
  }

  /**
   * Indicates if this column is part of a subtable
   */
  get isInSubtable(): boolean {
    return !!this.subtable;
  }

  /**
   * Gets the parent subtable if this column belongs to one
   */
  get parentSubtable(): AppSubtableColumnsDirective | undefined {
    return this.subtable;
  }

  /**
   * Validates the column configuration
   */
  validateConfiguration(): string[] {
    const errors: string[] = [];

    if (!this.field) {
      errors.push('Column field is required');
    }

    if (!this.header && this.componentType !== 'checkbox') {
      errors.push('Column header is required');
    }

    if (
      this.componentType === 'action' &&
      !this.showHistory &&
      !this.showEdit &&
      !this.showDelete &&
      (!this.customActions || this.customActions.length === 0)
    ) {
      errors.push('Action column must have at least one action enabled');
    }

    if (this.computed && !this.formatter && !this.computeTemplate) {
      errors.push('Computed column must have either a formatter function or computeTemplate');
    }

    if (this.componentType === 'template' && !this.cellTemplate) {
      errors.push('Template column must have a cellTemplate');
    }

    return errors;
  }

  /**
   * Gets the effective text class for a cell
   */
  getTextClass(value: any, row: any): string {
    if (typeof this.textClass === 'function') {
      return this.textClass(value, row);
    }
    return this.textClass || '';
  }

  /**
   * Gets the effective cell class for a cell
   */
  getCellClass(value: any, row: any): string {
    if (typeof this.cellClass === 'function') {
      return this.cellClass(value, row);
    }
    return this.cellClass || '';
  }

  /**
   * Formats a cell value using the column's formatter
   */
  formatValue(value: any, row: any): string {
    if (this.formatter) {
      return this.formatter(value, row);
    }
    return value !== null && value !== undefined ? String(value) : '';
  }

  /**
   * Validates a cell value using the column's validator
   */
  validateValue(value: any, row: any): string | null {
    if (this.validator) {
      return this.validator(value, row);
    }
    return null;
  }

  /**
   * Checks if the column is visible based on custom logic
   */
  isVisible(row?: any): boolean {
    // Override this method in derived classes for custom visibility logic
    return true;
  }

  /**
   * Gets the computed value for this column if it's a computed column
   */
  getComputedValue(row: any): any {
    if (!this.computed) {
      return null;
    }

    if (this.formatter) {
      return this.formatter(null, row);
    }

    // If using computeTemplate, the component will handle template rendering
    return null;
  }

  /**
   * Returns a summary object for debugging and development
   */
  getColumnSummary(): any {
    return {
      field: this.field,
      header: this.header,
      type: this.componentType || 'text',
      width: this.width,
      fixed: this.fixed,
      editable: this.editable,
      sortable: this.sortable,
      computed: this.computed,
      alwaysShowInput: this.alwaysShowInput,
      hasCustomTemplate: !!this.cellTemplate,
      hasHeaderTemplate: !!this.headerTemplate,
      hasEditTemplate: !!this.editTemplate,
      hasComputeTemplate: !!this.computeTemplate,
      isInGroup: this.isInGroup,
      groupName: this.columnGroup?.name,
      isInSubtable: this.isInSubtable,
      subtableDataField: this.subtable?.dataField,
      validationErrors: this.validateConfiguration(),
    };
  }
}
