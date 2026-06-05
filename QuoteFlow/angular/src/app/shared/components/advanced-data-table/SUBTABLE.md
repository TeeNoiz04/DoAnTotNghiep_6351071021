# Advanced Data Table - Subtable Feature

## Overview

The subtable feature allows you to display hierarchical data with parent-child relationships in the Advanced Data Table component. Each parent row can be expanded to reveal a nested table of child items.

## Key Features

- ✅ **One-level nested tables** - Display child data within parent rows
- ✅ **Reuses column directives** - Same `app-table-column` and `app-table-column-group` work in subtables
- ✅ **Mutually exclusive expansion** - Only errors OR subtable shows at a time (errors have priority)
- ✅ **Flexible configuration** - Support for all column types, formatters, and templates
- ✅ **Type-safe** - Full TypeScript interfaces and validation
- ✅ **Easy migration** - Wrap existing columns with `app-subtable-columns` directive
- ✅ **Summary support** - Independent summary rows for subtables
- ✅ **Conditional expansion** - Custom logic to determine expandable rows

## Basic Usage

### 1. Enable Subtable in Component

```html
<app-advanced-data-table
  [data]="orders"
  [enableSubtable]="true"
  (rowExpansion)="onRowExpansion($event)">

  <!-- Parent table columns -->
  <app-table-column field="orderNumber" header="Order #"></app-table-column>
  <app-table-column field="customerName" header="Customer"></app-table-column>

  <!-- Subtable definition -->
  <app-subtable-columns dataField="items">
    <app-table-column field="productName" header="Product"></app-table-column>
    <app-table-column field="quantity" header="Qty"></app-table-column>
  </app-subtable-columns>
</app-advanced-data-table>
```

### 2. Prepare Data Structure

```typescript
interface Order {
  orderNumber: string;
  customerName: string;
  // Child data array - must match dataField in app-subtable-columns
  items: OrderItem[];
}

interface OrderItem {
  productName: string;
  quantity: number;
}

// Component
orders: Order[] = [
  {
    orderNumber: 'ORD-001',
    customerName: 'John Doe',
    items: [
      { productName: 'Product A', quantity: 2 },
      { productName: 'Product B', quantity: 1 },
    ]
  }
];
```

## Subtable Configuration

### Input Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `dataField` | `string` | `'children'` | Field in parent row containing child data array |
| `expandableCondition` | `(row: any) => boolean` | - | Custom function to determine if row can expand |
| `noDataMessage` | `string` | `'No items'` | Message when subtable has no data |
| `showRowNumber` | `boolean` | `false` | Show row numbers in subtable |
| `cssClass` | `string` | - | Custom CSS class for subtable wrapper |
| `showSummary` | `boolean` | `false` | Show summary row in subtable |
| `striped` | `boolean` | `true` | Striped rows in subtable |
| `trackByField` | `string` | - | Field to use for trackBy function |

### Example with All Options

```html
<app-subtable-columns
  dataField="lineItems"
  [expandableCondition]="canExpand"
  [showRowNumber]="true"
  [showSummary]="true"
  [striped]="true"
  cssClass="custom-subtable"
  noDataMessage="No line items found"
  trackByField="id">

  <!-- Subtable columns -->
  <app-table-column field="description" header="Description"></app-table-column>
  <app-table-column
    field="amount"
    header="Amount"
    [sum]="true"
    [formatter]="formatCurrency">
  </app-table-column>
</app-subtable-columns>
```

```typescript
canExpand(row: any): boolean {
  // Only allow expansion if row has items and is approved
  return row.lineItems?.length > 0 && row.status === 'Approved';
}

formatCurrency(value: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(value);
}
```

## Advanced Features

### 1. Column Groups in Subtable

```html
<app-subtable-columns dataField="items">
  <app-table-column-group name="Product Info">
    <app-table-column field="code" header="Code"></app-table-column>
    <app-table-column field="name" header="Name"></app-table-column>
  </app-table-column-group>

  <app-table-column-group name="Pricing">
    <app-table-column field="quantity" header="Qty"></app-table-column>
    <app-table-column field="price" header="Price"></app-table-column>
  </app-table-column-group>
</app-subtable-columns>
```

### 2. Custom Templates in Subtable

```html
<app-subtable-columns dataField="tasks">
  <app-table-column
    field="assignee"
    header="Assignee"
    componentType="template">

    <ng-template #cellTemplate let-value let-row="row">
      <div class="d-flex align-items-center">
        <img [src]="row.assigneeAvatar" class="rounded-circle me-2" width="24" height="24">
        <span>{{ value }}</span>
      </div>
    </ng-template>
  </app-table-column>
</app-subtable-columns>
```

### 3. Summary Rows

```html
<app-subtable-columns
  dataField="expenses"
  [showSummary]="true">

  <app-table-column field="category" header="Category"></app-table-column>
  <app-table-column
    field="amount"
    header="Amount"
    [sum]="true"
    [formatter]="formatCurrency">
  </app-table-column>
</app-subtable-columns>
```

### 4. Status Components

```html
<app-subtable-columns dataField="shipments">
  <app-table-column field="trackingNumber" header="Tracking #"></app-table-column>
  <app-table-column
    field="status"
    header="Status"
    componentType="status">
  </app-table-column>
</app-subtable-columns>
```

## Row Expansion Events

Listen to row expansion events to perform actions when rows are expanded or collapsed:

```typescript
onRowExpansion(event: RowExpansionEvent): void {
  console.log('Row:', event.row);
  console.log('Expanded:', event.expanded);
  console.log('Type:', event.type); // 'error' or 'subtable'

  if (event.expanded && event.type === 'subtable') {
    // Load additional data, log analytics, etc.
    this.loadAdditionalDetails(event.row);
  }
}
```

## Error Handling with Subtables

When both `enableErrorHandling` and `enableSubtable` are enabled:

- **Errors have priority**: If a row has errors, clicking it shows errors (not subtable)
- **Mutually exclusive**: Only one type of expansion per row at a time
- **Visual indicators**: Rows with errors or subtables show hover effect

```html
<app-advanced-data-table
  [data]="orders"
  [enableErrorHandling]="true"
  [enableSubtable]="true"
  errorField="validationErrors"
  hasErrorsField="hasErrors">

  <!-- Parent columns -->
  <app-table-column field="orderNumber" header="Order #"></app-table-column>

  <!-- Subtable -->
  <app-subtable-columns dataField="items">
    <app-table-column field="productName" header="Product"></app-table-column>
  </app-subtable-columns>
</app-advanced-data-table>
```

```typescript
orders = [
  {
    orderNumber: 'ORD-001',
    hasErrors: true,
    validationErrors: ['Missing shipping address'],
    items: [...] // Subtable won't show because errors take priority
  },
  {
    orderNumber: 'ORD-002',
    hasErrors: false,
    items: [...] // Subtable will show on click
  }
];
```

## Supported Column Types

### ✅ Fully Supported
- **Text** - Default text display
- **Status** - `componentType="status"`
- **Template** - `componentType="template"`
- **Audit Info** - `componentType="auditInfo"`
- **Click Cell** - `componentType="clickCell"`

### ⚠️ Read-Only Display
- **Input** - `componentType="input"` or `editable="true"` (displayed as text)

### ❌ Not Recommended
- **Action** - `componentType="action"` (use parent table for actions)
- **Checkbox** - Selection in subtables not supported

## Styling

### Default Styles

The subtable comes with default styling:
- Light gray background
- Blue left border
- White inner table with shadow
- Smaller font size (0.875rem)

### Custom Styling

Add custom CSS class to subtable:

```html
<app-subtable-columns
  dataField="items"
  cssClass="my-custom-subtable">
  <!-- columns -->
</app-subtable-columns>
```

```scss
.my-custom-subtable {
  .subtable-wrapper {
    background-color: #f0f8ff;
    border-left-color: #2196F3;

    table {
      thead {
        background-color: #e3f2fd;
      }
    }
  }
}
```

## Important Limitations

### ⚠️ Single Subtable Only

**ONLY ONE** `app-subtable-columns` directive is allowed per table. If multiple are defined, only the first one will be used.

```html
<!-- ❌ WRONG: Multiple subtables -->
<app-advanced-data-table [data]="data">
  <app-subtable-columns dataField="items"></app-subtable-columns>
  <app-subtable-columns dataField="comments"></app-subtable-columns> <!-- Ignored! -->
</app-advanced-data-table>

<!-- ✅ CORRECT: Single subtable -->
<app-advanced-data-table [data]="data">
  <app-subtable-columns dataField="items"></app-subtable-columns>
</app-advanced-data-table>
```

### Development Warning

In development mode, you'll see a console warning if multiple subtables are detected:

```
[AppAdvancedDataTable] Multiple Subtable Configuration Error

⚠️ Found 2 <app-subtable-columns> directives, but only ONE is allowed per table.

📋 Current subtables detected: [...]

✅ Solution: Remove extra <app-subtable-columns> and keep only one.

🔍 Note: Only the FIRST subtable will be rendered.
```

### Other Limitations

- **One level only** - No nested subtables (subtable within subtable)
- **No editing** - Subtable data is read-only (no inline editing)
- **No selection** - Cannot select subtable rows
- **No pagination** - All child rows shown (no client-side pagination in subtable)

## Performance Considerations

1. **Large datasets**: Avoid subtables if parent has 1000+ rows with many children
2. **Lazy loading**: Use `expandableCondition` to lazy-load child data:

```typescript
canExpand(row: any): boolean {
  // Load children on demand
  if (!row.childrenLoaded) {
    this.loadChildren(row.id).subscribe(items => {
      row.items = items;
      row.childrenLoaded = true;
    });
    return false; // Don't expand yet
  }
  return row.items?.length > 0;
}
```

## Migration Guide

### From Error-Only Expansion

If you previously used error expansion:

**Before:**
```html
<app-advanced-data-table [enableErrorHandling]="true">
  <!-- columns -->
</app-advanced-data-table>
```

**After (with subtables):**
```html
<app-advanced-data-table
  [enableErrorHandling]="true"
  [enableSubtable]="true">

  <!-- columns -->

  <app-subtable-columns dataField="items">
    <!-- subtable columns -->
  </app-subtable-columns>
</app-advanced-data-table>
```

No breaking changes - error expansion still works and takes priority.

## Troubleshooting

### Subtable Not Showing

**Problem**: Clicking row doesn't expand subtable

**Solutions**:
1. Verify `[enableSubtable]="true"` is set
2. Check `dataField` matches your data property name
3. Ensure child data is an array with items
4. Verify row doesn't have errors (errors take priority)
5. Check `expandableCondition` if used

### Multiple Subtables Warning

**Problem**: Console warning about multiple subtables

**Solution**: Remove extra `app-subtable-columns` directives, keep only one

### Styling Not Applied

**Problem**: Custom CSS class not working

**Solution**:
1. Check CSS specificity
2. Use `::ng-deep` if needed (though deprecated)
3. Verify class is applied to subtable-wrapper element

## Examples

See [subtable-usage.template.html](./templates/subtable-usage.template.html) for complete examples.

## API Reference

### Interfaces

```typescript
export interface SubtableConfig {
  dataField: string;
  columns?: TableColumnConfig[];
  columnGroups?: ColumnGroupConfig[];
  expandableCondition?: (row: any) => boolean;
  noDataMessage?: string;
  showRowNumber?: boolean;
  cssClass?: string;
  showSummary?: boolean;
  striped?: boolean;
  trackByField?: string;
}

export interface RowExpansionEvent {
  row: any;
  expanded: boolean;
  type: 'error' | 'subtable';
}
```

### Directive

```typescript
@Directive({
  selector: 'app-subtable-columns',
  standalone: true,
})
export class AppSubtableColumnsDirective {
  @Input() dataField: string = 'children';
  @Input() expandableCondition?: (row: any) => boolean;
  @Input() noDataMessage: string = 'No items';
  @Input() showRowNumber: boolean = false;
  @Input() cssClass?: string;
  @Input() showSummary: boolean = false;
  @Input() striped: boolean = true;
  @Input() trackByField?: string;

  @ContentChildren(AppTableColumnDirective) columns!: QueryList<AppTableColumnDirective>;
  @ContentChildren(AppTableColumnGroupDirective) columnGroups!: QueryList<AppTableColumnGroupDirective>;
}
```

## Support

For issues or questions:
1. Check this documentation
2. Review [subtable-usage.template.html](./templates/subtable-usage.template.html)
3. Consult the main [README.md](./README.md) for general table usage
