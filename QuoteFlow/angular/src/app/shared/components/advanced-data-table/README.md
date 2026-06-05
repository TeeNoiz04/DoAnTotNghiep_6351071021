# Advanced Data Table Component

A flexible, template-based data table component that addresses the limitations of the previous price-offer-items-table implementation. This component provides a modern approach to defining table columns through HTML templates while maintaining all existing functionality.

## Features

### ✅ Core Features Implemented

- **Template-based column definition** - Define columns in HTML templates instead of TypeScript
- **Column grouping** - Organize columns into logical groups with headers
- **Fixed columns** - Sticky columns with dynamic positioning
- **Inline editing** - Edit cells with save/cancel functionality
- **Selection support** - Single/multiple row selection with checkboxes
- **Error handling** - Expandable error rows with validation messages
- **Summary calculations** - Automatic and manual summary row calculations
- **Action columns** - Configurable action buttons (history, edit, delete, custom actions)
- **Advanced text handling** - Ellipsis with tooltips for long text
- **Smart row numbering** - Auto-generated or database-managed row numbers
- **Responsive design** - Mobile-friendly with action button grouping
- **Custom cell templates** - Full template outlet support for custom rendering
- **Formula-based computation** - Cross-column calculations
- **Comprehensive width control** - Precise column width classes (width_15 to width_300)

## Usage Examples

### Basic Programmatic Usage

```typescript
// Component
export class MyComponent {
  tableData = [
    { id: 1, name: 'Item 1', price: 100, status: 'active' },
    { id: 2, name: 'Item 2', price: 200, status: 'inactive' }
  ];

  columns: TableColumnConfig[] = [
    {
      field: 'name',
      header: 'Name',
      width: '150px',
      fixed: true,
      sortable: true
    },
    {
      field: 'price',
      header: 'Price',
      width: '100px',
      textAlign: 'end',
      formatter: (value) => `$${value.toLocaleString()}`
    },
    {
      field: 'status',
      header: 'Status',
      width: '80px',
      componentType: 'status'
    }
  ];
}
```

```html
<!-- Template -->
<app-advanced-data-table
  [data]="tableData"
  [columns]="columns"
  [height]="'400px'"
  [showRowNumber]="true"
  [enableSelection]="true"
  [showSummary]="true">
</app-advanced-data-table>
```

### Template-Based Column Definition

```html
<app-advanced-data-table 
  [data]="tableData"
  [height]="'450px'"
  [enableSelection]="true"
  [showRowNumber]="true">
  
  <!-- Column Group Example -->
  <app-table-column-group name="Basic Information" cssClass="item-group">
    <app-table-column 
      field="code" 
      header="Code" 
      width="120px" 
      [fixed]="true"
      [showEllipsis]="true"
      [showTooltip]="true">
    </app-table-column>
    
    <app-table-column 
      field="name" 
      header="Name" 
      width="200px"
      [sortable]="true">
      <ng-template #cellTemplate let-value="value" let-row="row">
        <strong [class.text-success]="row.active">{{ value }}</strong>
      </ng-template>
    </app-table-column>
  </app-table-column-group>

  <!-- Price Group -->
  <app-table-column-group name="Pricing" cssClass="price-group">
    <app-table-column 
      field="standardPrice" 
      header="Standard Price" 
      width="100px"
      textAlign="end"
      [formatter]="formatCurrency">
    </app-table-column>
    
    <app-table-column 
      field="totalAmount" 
      header="Total" 
      width="100px"
      textAlign="end"
      [computed]="true">
      <ng-template #computeTemplate let-row="row">
        {{ formatCurrency(row.quantity * row.standardPrice) }}
      </ng-template>
    </app-table-column>
  </app-table-column-group>

  <!-- Action Column -->
  <app-table-column 
    field="actions" 
    header="" 
    width="120px"
    componentType="action"
    [showHistory]="true"
    [showEdit]="true"
    [showDelete]="true">
  </app-table-column>
</app-advanced-data-table>
```

### Column Groups with Programmatic Definition

```typescript
columnGroups: ColumnGroupConfig[] = [
  {
    name: 'Item Information',
    class: 'item-group',
    columns: [
      {
        field: 'action',
        header: '',
        width: '40px',
        componentType: 'action',
        showHistory: true,
        fixed: true
      },
      {
        field: 'code',
        header: 'Material Code',
        width: '120px',
        fixed: true,
        showEllipsis: true,
        showTooltip: true
      }
    ]
  },
  {
    name: 'Pricing',
    class: 'price-group',
    columns: [
      {
        field: 'standardPrice',
        header: 'Standard Price',
        width: '100px',
        textAlign: 'end',
        formatter: (val) => this.formatCurrency(val)
      }
    ]
  }
];
```

### Advanced Features

#### Error Handling
```html
<app-advanced-data-table
  [data]="tableData"
  [enableErrorHandling]="true"
  errorField="errors"
  hasErrorsField="hasErrors">
</app-advanced-data-table>
```

#### Custom Actions
```typescript
customActions: ActionItem[] = [
  {
    id: 'approve',
    label: 'Approve',
    icon: 'bi bi-check-circle',
    tooltip: 'Approve item',
    cssClass: 'text-success',
    visible: (row) => row.status === 'pending',
    disabled: (row) => row.locked
  }
];
```

#### Inline Editing
```typescript
columns: TableColumnConfig[] = [
  {
    field: 'quantity',
    header: 'Quantity',
    width: '80px',
    editable: true,
    componentType: 'input',
    inputType: 'number',
    validator: (value) => value > 0 ? null : 'Must be greater than 0'
  }
];
```

## Configuration Options

### Table Component Inputs

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `data` | `any[]` | `[]` | Array of data objects to display |
| `height` | `string` | `'450px'` | Table container height |
| `columns` | `TableColumnConfig[]` | `[]` | Programmatic column definitions |
| `columnGroups` | `ColumnGroupConfig[]` | `[]` | Programmatic column group definitions |
| `showRowNumber` | `boolean` | `false` | Show auto-generated row numbers |
| `rowNumberField` | `string?` | - | Field for database-managed row numbers |
| `enableSelection` | `boolean` | `false` | Enable row selection |
| `selectionType` | `'single'\|'multiple'` | `'multiple'` | Selection mode |
| `selectedItems` | `any[]` | `[]` | Currently selected items |
| `enableErrorHandling` | `boolean` | `false` | Enable error row expansion |
| `showSummary` | `boolean` | `false` | Show summary row |
| `autoCalculateSummary` | `boolean` | `true` | Auto-calculate summary values |
| `summaryData` | `any` | `{}` | External summary data |

### Column Configuration

| Property | Type | Description |
|----------|------|-------------|
| `field` | `string` | Data field name |
| `header` | `string` | Column header text |
| `width` | `string?` | Column width (e.g., '120px') |
| `fixed` | `boolean` | Make column sticky |
| `textAlign` | `'start'\|'center'\|'end'` | Text alignment |
| `showEllipsis` | `boolean` | Truncate text with ellipsis |
| `showTooltip` | `boolean` | Show tooltip on hover |
| `editable` | `boolean` | Enable inline editing |
| `sortable` | `boolean` | Enable column sorting |
| `componentType` | `string?` | Cell component type |
| `formatter` | `function?` | Value formatting function |
| `sum` | `boolean` | Include in summary calculations |

### Available Component Types

- `'status'` - Status label component
- `'action'` - Action buttons (history, edit, delete)
- `'checkbox'` - Selection checkbox
- `'clickCell'` - Clickable cell with events
- `'input'` - Editable input field
- `'template'` - Custom template outlet

## Migration from PriceOfferItemsTableComponent

### Before (TypeScript-heavy)
```typescript
// Complex column definitions in TypeScript
columnGroups: ColumnGroup[] = [
  {
    name: 'Item Information',
    columns: [
      {
        field: 'code',
        header: 'Material Code',
        width: '120px',
        class: 'text-center width_120',
        fixed: true,
        formatter: (val) => val || ''
      }
      // ... many more complex definitions
    ]
  }
];
```

### After (Template-based)
```html
<!-- Clean, declarative template -->
<app-advanced-data-table [data]="data" height="450px">
  <app-table-column-group name="Item Information">
    <app-table-column 
      field="code" 
      header="Material Code" 
      width="120px"
      [fixed]="true">
    </app-table-column>
  </app-table-column-group>
</app-advanced-data-table>
```

## Width Classes

The component includes comprehensive width classes for precise control:

- `width_15` to `width_300` - Fixed width classes
- Automatic width class application based on column width property
- Support for `minWidth` and `maxWidth` properties

## Events

| Event | Payload | Description |
|-------|---------|-------------|
| `rowClick` | `any` | Row clicked |
| `cellClick` | `CellClickEvent` | Cell clicked |
| `actionClick` | `ActionClickEvent` | Action button clicked |
| `inputChange` | `InputChangeEvent` | Cell value changed |
| `selectionChange` | `SelectionChangeEvent` | Selection changed |
| `historyClick` | `any` | History button clicked |
| `sortChange` | `{field: string, direction: 'asc'\|'desc'}` | Sort changed |

## Benefits Over Previous Implementation

1. **Template-based Configuration** - Define columns in HTML instead of complex TypeScript objects
2. **Better Separation of Concerns** - UI structure in templates, logic in components
3. **Improved Developer Experience** - Visual column definition, better IntelliSense
4. **Enhanced Reusability** - No module-specific dependencies
5. **Modern Angular Patterns** - Standalone components, content projection
6. **Consistent API** - Same interface across all table usage
7. **Better Maintainability** - Centralized table logic, easier to extend

## Performance Considerations

- Efficient fixed column positioning with ResizeObserver
- Optimized change detection with OnPush strategy  
- Lazy template rendering for better performance
- Minimal DOM manipulation for smooth scrolling

## Browser Support

- Modern browsers with CSS Grid and Sticky positioning support
- Responsive design for mobile devices
- Print-friendly styles included