## Error Display Component

A reusable Angular component for displaying error information with file names and error counts, featuring a scrollable popover for quick viewing and a full-featured modal for detailed error analysis.

### Features

- **Scrollable Popover** - Quick view with up to 200 errors, scrollable with visual indicators
- **Full Error Modal** - Detailed view with search, filter, and export capabilities
- **Smart Thresholds** - Auto-show "View All" button when errors exceed configurable limit (default: 20)
- **Search & Filter** - Real-time search across error locations and messages
- **Export Functionality** - Export errors to CSV or TXT format
- **Visual Scroll Indicators** - Fade gradients show when more content is available
- **Responsive Design** - Optimized for desktop, tablet, and mobile
- **Keyboard Support** - ESC to close modal, Enter for search
- **Customizable Layout** - Horizontal or vertical orientation
- **Copy-Friendly** - Text selection enabled for easy copying
- **Bootstrap-Compatible** - Integrates with Bootstrap styling

### Usage

#### Basic Usage

```html
<app-error-display
  [fileName]="'example.xlsx'"
  [errors]="['Row 1: Invalid data', 'Row 5: Missing field']">
</app-error-display>
```

#### Advanced Usage with Custom Thresholds

```html
<app-error-display
  [fileName]="'import-data.xlsx'"
  [errors]="errorArray"
  [customMessage]="'Validation Issues: ' + errorCount"
  [layout]="'vertical'"
  [showIcon]="true"
  [showFileName]="true"
  [popoverPlacement]="'top'"
  [popoverClass]="'custom-error-popover'"
  [showViewAllThreshold]="50"
  [showExport]="true">
</app-error-display>
```

#### Properties

| Property                | Type                                           | Default                   | Description                                                    |
| ----------------------- | ---------------------------------------------- | ------------------------- | -------------------------------------------------------------- |
| `fileName`              | `string`                                       | `undefined`               | Name of the file being processed                               |
| `errors`                | `string[]`                                     | `[]`                      | Array of error messages                                        |
| `customMessage`         | `string`                                       | `undefined`               | Custom message to display instead of default error count       |
| `showIcon`              | `boolean`                                      | `true`                    | Whether to show warning icon when errors exist                 |
| `showFileName`          | `boolean`                                      | `true`                    | Whether to display the file name                               |
| `layout`                | `'horizontal' \| 'vertical'`                   | `'horizontal'`            | Layout direction for file name and error count                 |
| `popoverPlacement`      | `'auto' \| 'top' \| 'bottom' \| 'left' \| 'right'` | `'auto'`           | Popover position                                               |
| `popoverClass`          | `string`                                       | `'custom-error-popover'`  | CSS class for popover styling                                  |
| `showViewAllThreshold`  | `number`                                       | `20`                      | Show "View All" button when errors exceed this number          |
| `showExport`            | `boolean`                                      | `true`                    | Whether to show export buttons in modal                        |

### Error Display Thresholds

The component intelligently handles different error list sizes:

| Error Count | Display Behavior                                                     |
| ----------- | -------------------------------------------------------------------- |
| **1-20**    | Popover only, auto-height, no "View All" button                      |
| **21-200**  | Scrollable popover (max 400px) + "View All" button                   |
| **201-500** | Scrollable popover + "View All" button (modal recommended)           |
| **500+**    | Scrollable popover + modal with optimized rendering                  |

### Modal Features

#### Search and Filter

The modal includes a powerful search feature:

- **Real-time filtering** - Results update as you type
- **Searches both location and message** - Comprehensive coverage
- **Case-insensitive** - User-friendly search behavior
- **Clear button** - Quick reset with maintained focus
- **Result counter** - Shows filtered vs total errors

#### Export Functionality

Export errors in two formats:

**CSV Export:**
- Includes headers: #, Location, Error Message
- Properly escaped values for Excel compatibility
- Filename: `{fileName}_errors.csv`

**TXT Export:**
- Human-readable format with report header
- Includes timestamp and total error count
- Formatted sections for easy reading
- Filename: `{fileName}_errors.txt`

#### Keyboard Shortcuts

- **ESC** - Close modal
- **Enter** - Submit search (while in search input)
- **Ctrl+F** - Focus search input (when modal is open)

### Styling

The component includes built-in CSS classes:

**Popover Classes:**
- `.error-display-container` - Main container
- `.layout-horizontal` / `.layout-vertical` - Layout variants
- `.error-count` - Error count styling
- `.has-errors` - Applied when errors exist (red gradient)
- `.error-popover-trigger` - Clickable trigger with hover effects
- `.error-popover-content` - Scrollable content area
- `.has-scroll-top` / `.has-scroll-bottom` - Scroll indicators
- `.error-popover-footer` - Footer with "View All" button

**Modal Classes:**
- `.error-modal` - Main modal wrapper
- `.error-modal-controls` - Search and export section
- `.error-modal-content` - Scrollable error list
- `.search-wrapper` - Search input container
- `.error-stats` - Stats and export buttons row
- `.no-errors-found` - Empty state display

### Error Message Format

The component supports two error message formats:

**Format 1: With Location (Structured)**
```
[Field Name] Error message here
```
Displays in two columns: Location | Message

**Format 2: Full Width (Unstructured)**
```
Error message without location
```
Displays in single column spanning full width

**Row Prefix Auto-Removal:**
```
Row 123: [Field Name] Error message
```
The `Row N:` prefix is automatically removed for cleaner display.

### Examples in Codebase

The component is used in 20+ modules across the application:

1. **Price Offers** - `price-offer.component.html`, `price-offer-detail.component.html`
2. **Stock Management** - `import-stock.component.html`
3. **GICs** - `gic.component.html`, `result-import-gic.component.html`
4. **DPOs** - `dpo.component.html`, `result-import-dpo.component.html`
5. **Materials** - `import-material.component.html`
6. **Stock Tracings** - `stock-tracing.component.html`
7. **Cargos** - `import-cargo.component.html`
8. **PSIs** - `import-psi.component.html`
9. **System Categories** - Various import result components
10. **Customers** - `customer.component.html`, `customer-detail.component.html`

### Migration from Legacy

Existing usage requires **no changes** - all new features are backward compatible:

```html
<!-- Old usage - still works perfectly -->
<app-error-display
  [fileName]="fileName"
  [errors]="errors">
</app-error-display>

<!-- Enhanced usage - opt-in to new features -->
<app-error-display
  [fileName]="fileName"
  [errors]="errors"
  [showViewAllThreshold]="30"
  [showExport]="true">
</app-error-display>
```

### Performance Considerations

- **1-200 errors**: Optimal performance, no special handling needed
- **201-500 errors**: Good performance, modal recommended for analysis
- **500+ errors**: Consider implementing pagination or virtual scrolling in future enhancement

### Browser Compatibility

- Modern browsers (Chrome, Firefox, Safari, Edge)
- Responsive design for mobile devices (iOS Safari, Chrome Mobile)
- Tested on desktop (1920px+), tablet (768-1024px), and mobile (320-767px)
