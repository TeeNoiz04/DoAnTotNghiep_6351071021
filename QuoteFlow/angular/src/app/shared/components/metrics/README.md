# Metrics Components

This folder contains reusable metric display components for showing various types of data with consistent styling and behavior.

## Components

### 1. MetricProgressComponent (`app-metric-progress`)

Displays a metric with current/max values and a progress bar. Ideal for budget tracking, percentages, etc.

**Basic Usage:**

```html
<app-metric-progress
  label="Budget"
  [currentValue]="7500000"
  [maxValue]="8800000"
  iconClass="bi bi-graph-up text-primary">
</app-metric-progress>
```

**Advanced Usage:**

```html
<app-metric-progress
  label="Discount"
  [currentValue]="15.5"
  [maxValue]="20"
  iconClass="bi bi-percent text-success"
  [formatter]="percentFormatter"
  positiveTooltipTemplate="Available discount: {remaining}%"
  negativeTooltipTemplate="Over limit by: {excess}%"
  [warningThreshold]="85">
</app-metric-progress>
```

**Properties:**

- `label` (string): Display label
- `currentValue` (number): Current value
- `maxValue` (number): Maximum value
- `iconClass` (string): CSS classes for icon
- `containerClass` (string): CSS classes for container
- `labelClass` (string): CSS classes for label
- `valueClass` (string): CSS classes for value text
- `progressBarClass` (string): CSS classes for progress bar
- `showTooltip` (boolean): Show/hide tooltip (default: true)
- `tooltipPlacement` (string): Tooltip placement (default: 'top')
- `positiveTooltipTemplate` (string): Template for positive remaining tooltip
- `negativeTooltipTemplate` (string): Template for exceeded tooltip
- `formatter` (function): Value formatter function
- `warningThreshold` (number): Warning threshold percentage (default: 80)
- `customStatusClass` (string): Override status class

### 2. MetricInfoComponent (`app-metric-info`)

Displays simple metric information without progress bar. Ideal for counts, dates, etc.

**Basic Usage:**

```html
<app-metric-info label="Items" [value]="42" iconClass="bi bi-calendar text-info"> </app-metric-info>
```

**Advanced Usage:**

```html
<app-metric-info
  label="Days Left"
  [value]="15"
  iconClass="bi bi-clock text-warning"
  customStatusClass="status-warning"
  [showTooltip]="true"
  tooltipText="Deadline approaching">
</app-metric-info>
```

**Properties:**

- `label` (string): Display label
- `value` (number | string): Value to display
- `suffix` (string): Text suffix after value
- `iconClass` (string): CSS classes for icon
- `containerClass` (string): CSS classes for container
- `labelClass` (string): CSS classes for label
- `valueClass` (string): CSS classes for value text
- `customStatusClass` (string): Custom status CSS class
- `showTooltip` (boolean): Show/hide tooltip (default: false)
- `tooltipText` (string): Tooltip text
- `tooltipPlacement` (string): Tooltip placement
- `formatter` (function): Value formatter function

### 3. MetricRangeComponent (`app-metric-range`)

Displays a metric with min-current-max range visualization. Ideal for budget ranges, thresholds, or any metric with boundaries.

**Basic Usage:**

```html
<app-metric-range
  label="Budget"
  [minValue]="1000000"
  [currentValue]="7500000"
  [maxValue]="10000000"
  iconClass="bi bi-graph-up text-primary">
</app-metric-range>
```

**Advanced Usage:**

```html
<app-metric-range
  label="Temperature"
  [minValue]="10"
  [currentValue]="85"
  [maxValue]="80"
  iconClass="bi bi-thermometer-half text-warning"
  minLabel="Safe Min"
  maxLabel="Safe Max"
  currentLabel="Current Temp"
  remainingAddLabel="Margin Available"
  remainingReduceLabel="Excess Amount"
  [abbreviationDecimalPlaces]="2">
</app-metric-range>
```

**Properties:**

- `label` (string): Display label
- `minValue` (number): Minimum value of the range
- `currentValue` (number): Current value
- `maxValue` (number): Maximum value of the range
- `iconClass` (string): CSS classes for icon
- `containerClass` (string): CSS classes for container
- `labelClass` (string): CSS classes for label
- `valueClass` (string): CSS classes for value text
- `scaleContainerClass` (string): CSS classes for scale container
- `showTooltip` (boolean): Show/hide tooltip (default: true)
- `tooltipPlacement` (string): Tooltip placement (default: 'top')
- `customStatusClass` (string): Override status class
- `minLabel` (string): Custom label for minimum value in tooltip (default: 'Minimum')
- `maxLabel` (string): Custom label for maximum value in tooltip (default: 'Maximum')
- `currentLabel` (string): Custom label for current value in tooltip (default: 'Current')
- `remainingAddLabel` (string): Custom label for remaining amount in tooltip (default: 'Remaining to Add')
- `remainingReduceLabel` (string): Custom label for excess amount in tooltip (default: 'Amount to Reduce')
- `formatter` (function): Value formatter function for full precision values
- `abbreviationDecimalPlaces` (number): Decimal places for abbreviated display (default: 1)
- `abbreviationThreshold` (number): Threshold for abbreviation (default: 1000)

**Status Classes:**

- `status-good`: Current value is within min-max range (green)
- `status-below`: Current value is below minimum (red)
- `status-above`: Current value is above maximum (red)

**Overflow Handling:**

The component handles overflow scenarios where current value exceeds the min-max range:
- Values above max: Scale extends to the right with overflow indicator
- Values below min: Scale extends to the left with overflow indicator
- Visual markers show the current position even when outside the normal range

### 4. MetricsPanelComponent (`app-metrics-panel`)

Responsive container for organizing multiple metrics.

**Usage:**

```html
<app-metrics-panel>
  <app-metric-progress
    label="Budget"
    [currentValue]="budgetCurrent"
    [maxValue]="budgetMax"
    iconClass="bi bi-graph-up text-primary">
  </app-metric-progress>

  <app-metric-progress
    label="Discount"
    [currentValue]="discountCurrent"
    [maxValue]="discountMax"
    iconClass="bi bi-percent text-success">
  </app-metric-progress>

  <app-metric-info label="Items" [value]="itemCount" iconClass="bi bi-calendar text-info">
  </app-metric-info>
</app-metrics-panel>
```

**Properties:**

- `containerClass` (string): CSS classes for main container
- `rowClass` (string): CSS classes for row
- `contentClass` (string): CSS classes for content area
- `metricsContainerClass` (string): CSS classes for metrics container

## Status Classes

The components automatically apply status classes based on values:

**MetricProgressComponent & MetricInfoComponent:**
- `status-good`: Normal/good state (green)
- `status-warning`: Warning state (orange) - when value >= warningThreshold
- `status-exceeded`: Exceeded state (red) - when current > max

**MetricRangeComponent:**
- `status-good`: Current value is within min-max range (green)
- `status-below`: Current value is below minimum (red)
- `status-above`: Current value is above maximum (red)

## Custom Styling

All components accept custom CSS classes for different parts:

- Container styling via `containerClass`
- Label styling via `labelClass`
- Value styling via `valueClass`
- Progress bar styling via `progressBarClass`

## Responsive Behavior

The `MetricsPanelComponent` automatically adjusts layout:

- Desktop: Horizontal flex layout with gaps
- Tablet: Reduced gaps
- Mobile: Vertical stacking

## Example Integration

```typescript
// In your component
export class MyComponent {
  budgetCurrent = 7500000;
  budgetMax = 8800000;

  discountCurrent = 15.5;
  discountMax = 20;

  itemCount = 42;

  currencyFormatter = (value: number) =>
    value.toLocaleString('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
    });

  percentFormatter = (value: number) => value.toFixed(2) + '%';
}
```

```html
<!-- In your template -->
<app-metrics-panel>
  <app-metric-progress
    label="Budget"
    [currentValue]="budgetCurrent"
    [maxValue]="budgetMax"
    [formatter]="currencyFormatter"
    iconClass="bi bi-graph-up text-primary"
    positiveTooltipTemplate="Remaining budget: {remaining}"
    negativeTooltipTemplate="Over budget by: {excess}">
  </app-metric-progress>

  <app-metric-range
    label="Target Range"
    [minValue]="targetMin"
    [currentValue]="targetCurrent"
    [maxValue]="targetMax"
    [formatter]="currencyFormatter"
    iconClass="bi bi-bullseye text-info"
    minLabel="Target Min"
    maxLabel="Target Max"
    currentLabel="Current Value">
  </app-metric-range>

  <app-metric-info label="Items" [value]="itemCount" iconClass="bi bi-calendar text-info">
  </app-metric-info>
</app-metrics-panel>
```
