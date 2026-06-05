// Import components first
import { AppAdvancedDataTableComponent } from './advanced-data-table.component';
import { AppTableColumnGroupDirective } from './directives/table-column-group.directive';
import { AppTableColumnDirective } from './directives/table-column.directive';

// Main component
export { AppAdvancedDataTableComponent } from './advanced-data-table.component';

// Directives
export { AppTableColumnGroupDirective } from './directives/table-column-group.directive';
export { AppTableColumnDirective } from './directives/table-column.directive';

// Types and interfaces
export type {
  ActionClickEvent,
  ActionItem,
  CellClickEvent,
  ColumnGroupConfig,
  InputChangeEvent,
  RowExpansionEvent,
  SelectionChangeEvent,
  TableColumnConfig,
} from './advanced-data-table.component';

// Re-export for convenience
export const AdvancedDataTableComponents = [
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
] as const;
