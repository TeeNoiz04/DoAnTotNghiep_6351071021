import { Directive, Input, ContentChildren, QueryList } from '@angular/core';
import { AppTableColumnDirective } from './table-column.directive';

@Directive({
  selector: 'app-table-column-group',
  standalone: true,
})
export class AppTableColumnGroupDirective {
  @Input() name: string = '';
  @Input() cssClass?: string;
  @Input() visible: boolean = true;
  @Input() collapsible: boolean = false;
  @Input() collapsed: boolean = false;
  @Input() iconClass?: string;

  @ContentChildren(AppTableColumnDirective) columns!: QueryList<AppTableColumnDirective>;

  /**
   * Gets the number of visible columns in this group
   */
  getVisibleColumnCount(): number {
    return this.columns.toArray().filter(col => col.isVisible()).length;
  }

  /**
   * Gets all column fields in this group
   */
  getColumnFields(): string[] {
    return this.columns.toArray().map(col => col.field);
  }

  /**
   * Checks if all columns in the group are fixed
   */
  areAllColumnsFixed(): boolean {
    return this.columns.length > 0 && this.columns.toArray().every(col => col.fixed);
  }

  /**
   * Checks if any column in the group is editable
   */
  hasEditableColumns(): boolean {
    return this.columns.toArray().some(col => col.editable);
  }

  /**
   * Checks if any column in the group is sortable
   */
  hasSortableColumns(): boolean {
    return this.columns.toArray().some(col => col.sortable);
  }

  /**
   * Gets the total width of all columns in the group
   */
  getTotalWidth(): string | null {
    const widths = this.columns
      .toArray()
      .map(col => col.width)
      .filter(width => width && width.endsWith('px'))
      .map(width => parseInt(width!.replace('px', ''), 10));

    if (widths.length === this.columns.length) {
      const total = widths.reduce((sum, width) => sum + width, 0);
      return `${total}px`;
    }

    return null;
  }

  /**
   * Validates the column group configuration
   */
  validateConfiguration(): string[] {
    const errors: string[] = [];

    if (!this.name && this.visible) {
      errors.push('Visible column group should have a name');
    }

    if (this.columns.length === 0) {
      errors.push('Column group must contain at least one column');
    }

    // Validate individual columns
    this.columns.toArray().forEach((column, index) => {
      const columnErrors = column.validateConfiguration();
      columnErrors.forEach(error => {
        errors.push(`Column ${index + 1} (${column.field}): ${error}`);
      });
    });

    // Check for duplicate field names within the group
    const fieldNames = this.columns.toArray().map(col => col.field);
    const duplicates = fieldNames.filter((field, index) => fieldNames.indexOf(field) !== index);
    if (duplicates.length > 0) {
      errors.push(`Duplicate column fields in group: ${duplicates.join(', ')}`);
    }

    return errors;
  }

  /**
   * Toggles the collapsed state if the group is collapsible
   */
  toggleCollapsed(): void {
    if (this.collapsible) {
      this.collapsed = !this.collapsed;
    }
  }

  /**
   * Sets the visibility of the entire group
   */
  setVisible(visible: boolean): void {
    this.visible = visible;
  }

  /**
   * Gets the CSS classes for the group header
   */
  getHeaderClass(): string {
    let classes = this.cssClass || '';

    if (this.collapsible) {
      classes += ' collapsible';
    }

    if (this.collapsed) {
      classes += ' collapsed';
    }

    if (!this.visible) {
      classes += ' d-none';
    }

    return classes.trim();
  }

  /**
   * Returns a summary object for debugging and development
   */
  getGroupSummary(): any {
    return {
      name: this.name,
      visible: this.visible,
      collapsible: this.collapsible,
      collapsed: this.collapsed,
      columnCount: this.columns.length,
      visibleColumnCount: this.getVisibleColumnCount(),
      columnFields: this.getColumnFields(),
      allFixed: this.areAllColumnsFixed(),
      hasEditable: this.hasEditableColumns(),
      hasSortable: this.hasSortableColumns(),
      totalWidth: this.getTotalWidth(),
      validationErrors: this.validateConfiguration(),
      columns: this.columns.toArray().map(col => col.getColumnSummary()),
    };
  }

  /**
   * Checks if the group should be rendered based on visibility and column content
   */
  shouldRender(): boolean {
    return this.visible && this.getVisibleColumnCount() > 0;
  }

  /**
   * Gets columns that match a specific criteria
   */
  getColumnsByType(componentType: string): AppTableColumnDirective[] {
    return this.columns.toArray().filter(col => col.componentType === componentType);
  }

  /**
   * Gets the first column that matches the given field name
   */
  getColumnByField(field: string): AppTableColumnDirective | undefined {
    return this.columns.toArray().find(col => col.field === field);
  }

  /**
   * Gets fixed columns in this group
   */
  getFixedColumns(): AppTableColumnDirective[] {
    return this.columns.toArray().filter(col => col.fixed);
  }

  /**
   * Gets non-fixed columns in this group
   */
  getNonFixedColumns(): AppTableColumnDirective[] {
    return this.columns.toArray().filter(col => !col.fixed);
  }
}
