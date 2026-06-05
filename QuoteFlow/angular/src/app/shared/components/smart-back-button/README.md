# Smart Back Button Component

A smart back button component that provides intelligent navigation with fallback mechanisms for detail pages.

## Features

- **Smart History Navigation**: Uses browser history and internal tracking to navigate back intelligently
- **Context-Aware**: Ensures back navigation stays within the same module/context
- **Automatic Fallback**: Automatically determines the appropriate fallback URL based on current context
- **Edge Case Handling**: Handles new tabs, direct access, page refreshes, and cross-domain navigation
- **Customizable**: Supports custom styling, text, and icons

## Usage

### Basic Usage (Auto-Fallback)

```html
<app-smart-back-button></app-smart-back-button>
```

The component will automatically determine the appropriate fallback URL based on the current route context.

### With Custom Fallback URL

```html
<app-smart-back-button [fallbackUrl]="['/materials', 'list']"></app-smart-back-button>
```

### With Custom Styling

```html
<app-smart-back-button
  buttonText="Go Back"
  buttonClass="btn btn-outline-primary"
  iconClass="fas fa-arrow-left"
  [fallbackUrl]="fallbackUrl">
</app-smart-back-button>
```

### Component Integration Example

```typescript
// In your detail component
export class MyDetailComponent {
  // Optional: Define custom fallback URL
  get fallbackUrl(): string[] {
    return [AppRoutes.MY_MODULE.BASE, AppRoutes.MY_MODULE.LIST.BASE];
  }
}
```

```html
<!-- In your template -->
<app-smart-back-button [fallbackUrl]="fallbackUrl"></app-smart-back-button>
```

## Input Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `fallbackUrl` | `string \| string[]` | Auto-determined | URL to navigate to when smart back fails |
| `buttonText` | `string` | `'Back'` | Text displayed on the button |
| `buttonClass` | `string` | `'btn btn-secondary'` | CSS classes for button styling |
| `iconClass` | `string` | `'fas fa-arrow-left'` | CSS classes for the icon |
| `disabled` | `boolean` | `false` | Whether the button is disabled |
| `showIcon` | `boolean` | `true` | Whether to show the icon |

## Smart Navigation Logic

The component uses a multi-strategy approach:

1. **Strategy 1**: Use tracked navigation history within the same context
2. **Strategy 2**: Use browser back navigation if safe and within context
3. **Strategy 3**: Fall back to the provided or auto-determined URL

## Edge Cases Handled

- **Direct Access**: When user accesses the page directly via URL
- **New Tab**: When page is opened in a new tab/window
- **Page Refresh**: When user refreshes the page
- **Cross-Context**: When previous page is from a different module
- **Invalid Routes**: When navigation history contains invalid routes

## Auto-Fallback URLs

The component automatically determines fallback URLs based on route context:

- `/materials/*` → `/materials/list`
- `/price-offer/*` → `/price-offer/list`
- `/dpo/*` → `/dpo/list`
- `/key-accounts/*` → `/key-accounts/list`
- And more...

For "my-approvals" contexts, it automatically navigates to the appropriate my-approvals page.

## Migration from Old Back Buttons

### Before:
```typescript
goBack(): void {
  this.router.navigate([AppRoutes.MY_MODULE.BASE, AppRoutes.MY_MODULE.LIST.BASE]);
}
```

```html
<button class="btn btn-secondary" (click)="goBack()">
  <i class="bi bi-arrow-left"></i>
  Back
</button>
```

### After:
```typescript
// Remove the goBack() method - it's no longer needed!
// Optionally define fallback URL if you want to override auto-detection
get fallbackUrl(): string[] {
  return [AppRoutes.MY_MODULE.BASE, AppRoutes.MY_MODULE.LIST.BASE];
}
```

```html
<app-smart-back-button [fallbackUrl]="fallbackUrl"></app-smart-back-button>
```

## Services Used

- **NavigationHistoryService**: Tracks navigation history and provides smart back functionality
- **NavigationFallbackService**: Provides automatic fallback URL determination