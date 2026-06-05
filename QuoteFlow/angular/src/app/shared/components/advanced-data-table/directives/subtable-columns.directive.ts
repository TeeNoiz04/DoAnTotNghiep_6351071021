import { Directive, Input, ContentChildren, QueryList } from '@angular/core';
import { AppTableColumnDirective } from './table-column.directive';
import { AppTableColumnGroupDirective } from './table-column-group.directive';

/**
 * Defines subtable columns for the advanced data table.
 *
 * **IMPORTANT:** Only ONE app-subtable-columns directive is allowed per table.
 * If multiple are defined, only the first one will be used.
 *
 * @example
 * ```html
 * <app-advanced-data-table [enableSubtable]="true">
 *   <!-- Parent columns -->
 *   <app-table-column field="id" header="ID"></app-table-column>
 *
 *   <!-- Subtable (ONLY ONE allowed) -->
 *   <app-subtable-columns dataField="items">
 *     <app-table-column field="name" header="Name"></app-table-column>
 *     <app-table-column field="quantity" header="Qty"></app-table-column>
 *   </app-subtable-columns>
 * </app-advanced-data-table>
 * ```
 */
@Directive({
  selector: 'app-subtable-columns',
  standalone: true,
})
export class AppSubtableColumnsDirective {
  /** Field in parent row containing child data array */
  @Input() dataField: string = 'children';

  /** Condition to determine if row can be expanded */
  @Input() expandableCondition?: (row: any) => boolean;

  /** No data message for subtable */
  @Input() noDataMessage: string = 'No items';

  /** Show row numbers in subtable */
  @Input() showRowNumber: boolean = false;

  /** Custom CSS class for subtable */
  @Input() cssClass?: string;

  /** Show summary row */
  @Input() showSummary: boolean = false;

  /** Striped subtable rows */
  @Input() striped: boolean = true;

  /** Track by field for subtable rows */
  @Input() trackByField?: string;

  @ContentChildren(AppTableColumnDirective, { descendants: true })
  columns!: QueryList<AppTableColumnDirective>;

  @ContentChildren(AppTableColumnGroupDirective, { descendants: true })
  columnGroups!: QueryList<AppTableColumnGroupDirective>;

  /**
   * Check if row can be expanded
   */
  canExpand(row: any): boolean {
    if (this.expandableCondition) {
      return this.expandableCondition(row);
    }

    // Default: check if dataField exists and has items
    const childData = row[this.dataField];
    return Array.isArray(childData) && childData.length > 0;
  }

  /**
   * Get child data from parent row
   */
  getChildData(row: any): any[] {
    const childData = row[this.dataField];
    return Array.isArray(childData) ? childData : [];
  }

  /**
   * Validates the subtable configuration
   */
  validateConfiguration(): string[] {
    const errors: string[] = [];

    if (!this.dataField) {
      errors.push('Subtable dataField is required');
    }

    const totalColumns = (this.columns?.length || 0) + (this.columnGroups?.length || 0);
    if (totalColumns === 0) {
      errors.push('Subtable must have at least one column or column group');
    }

    return errors;
  }

  /**
   * Gets the total number of columns (including grouped columns)
   */
  getTotalColumnCount(): number {
    let count = 0;

    // Count standalone columns
    if (this.columns) {
      count += this.columns.filter(col => !col.isInGroup).length;
    }

    // Count grouped columns
    if (this.columnGroups) {
      this.columnGroups.forEach(group => {
        count += group.columns?.length || 0;
      });
    }

    return count;
  }

  /**
   * Returns a summary object for debugging
   */
  getSummary(): any {
    return {
      dataField: this.dataField,
      noDataMessage: this.noDataMessage,
      showRowNumber: this.showRowNumber,
      showSummary: this.showSummary,
      striped: this.striped,
      cssClass: this.cssClass,
      columnCount: this.getTotalColumnCount(),
      standaloneColumns: this.columns?.filter(col => !col.isInGroup).length || 0,
      columnGroups: this.columnGroups?.length || 0,
      validationErrors: this.validateConfiguration(),
    };
  }
}
