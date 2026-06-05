# 🎮 Keyboard Navigation Hint System

A comprehensive, reusable system for displaying keyboard navigation hints in modals and other components.

## ✨ Features

- **4 Different Display Styles** - Choose the perfect approach for your use case
- **Smart Show Logic** - Intelligent display patterns that don't annoy users
- **Reusable Across Modals** - One system for all your navigation hints
- **Fully Customizable** - Configure keys, positions, and behaviors
- **Responsive Design** - Works on all screen sizes
- **Accessibility Ready** - Follows ARIA guidelines

## 🎯 Display Styles

### 1. Minimalist Floating Tooltip ⭐ *Recommended*
- Clean, unobtrusive design
- Smart appearance logic
- Auto-hides after delay
- Perfect for production use

### 2. Card-Style with Details
- Professional appearance
- Clear keyboard shortcuts
- Great for onboarding
- Expandable for more shortcuts

### 3. Interactive Toggle Button
- Always accessible
- User-controlled visibility
- Compact when collapsed
- Perfect for power users

### 4. Bottom Notification Bar
- Highly visible
- Clear messaging
- Familiar notification pattern
- Good for important tips

## 🚀 Quick Start

### 1. Import the Component

```typescript
import { KeyboardNavigationHintComponent, KeyboardNavigationHintService } from '@app/shared/components/keyboard-navigation-hint';

@Component({
  imports: [
    // ... other imports
    KeyboardNavigationHintComponent,
  ],
})
```

### 2. Add to Template

```html
<!-- Basic usage with preset -->
<app-keyboard-navigation-hint
  *ngIf="visible && (hasNextDetail || hasPreviousDetail)"
  modalId="your-modal-id"
  [config]="KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL"
  displayStyle="minimal">
</app-keyboard-navigation-hint>
```

### 3. Custom Configuration

```typescript
readonly customConfig: KeyboardNavigationConfig = {
  keys: [
    { key: '←', description: 'Previous', icon: 'bi-arrow-left' },
    { key: '→', description: 'Next', icon: 'bi-arrow-right' },
    { key: 'Esc', description: 'Close', icon: 'bi-x-circle' }
  ],
  position: 'bottom-right',
  showMode: 'smart',
  autoHideDelay: 5000
};
```

## 📝 Configuration Options

### KeyboardNavigationConfig

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| `keys` | `KeyInfo[]` | Array of keyboard shortcuts to display | Required |
| `position` | `'top-right' \| 'bottom-left' \| 'bottom-right' \| 'center-bottom'` | Position of the hint | `'bottom-right'` |
| `showMode` | `'always' \| 'first-time' \| 'smart' \| 'toggleable'` | When to show hints | `'smart'` |
| `autoHideDelay` | `number` | Auto-hide delay in milliseconds | `5000` |
| `storageKey` | `string` | Custom localStorage key | Auto-generated |

### KeyInfo

| Property | Type | Description |
|----------|------|-------------|
| `key` | `string` | The keyboard key (e.g., '←', 'Esc') |
| `icon` | `string` | Bootstrap icon class (optional) |
| `description` | `string` | Description of the action |
| `action` | `string` | Action identifier (optional) |

### Show Modes

- **`always`**: Shows every time (can be annoying)
- **`first-time`**: Shows only once (may be missed)
- **`smart`**: Shows first 3 times, then reappears after 7 days ⭐ *Recommended*
- **`toggleable`**: User controls visibility (for interactive style)

## 🎨 Display Styles

Switch between styles using the `displayStyle` input:

```html
<!-- Minimalist (recommended) -->
<app-keyboard-navigation-hint displayStyle="minimal">

<!-- Card-style -->
<app-keyboard-navigation-hint displayStyle="card">

<!-- Interactive toggle -->
<app-keyboard-navigation-hint displayStyle="interactive">

<!-- Notification bar -->
<app-keyboard-navigation-hint displayStyle="notification">
```

## 🏭 Built-in Presets

The service provides ready-to-use presets:

```typescript
// Basic navigation (← →)
KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL

// Extended navigation (← → PgUp PgDn Esc)
KeyboardNavigationHintService.PRESETS.EXTENDED_NAVIGATION
```

## 🔧 Advanced Usage

### Custom Service Methods

```typescript
// Check if hint should be shown
shouldShowHint(modalId: string, config: KeyboardNavigationConfig): boolean

// Dismiss hint manually
dismissHint(modalId: string, config: KeyboardNavigationConfig): void

// Toggle hint visibility (for toggleable mode)
toggleHint(modalId: string): void

// Reset all stored preferences
resetAllHints(): void
```

### Multiple Styles in One Modal

You can easily switch between styles by commenting/uncommenting different approaches:

```html
<!-- Approach 1: Minimalist (Active) -->
<app-keyboard-navigation-hint
  *ngIf="visible && (hasNextDetail || hasPreviousDetail)"
  modalId="your-modal"
  [config]="keyboardHintConfig"
  displayStyle="minimal">
</app-keyboard-navigation-hint>

<!-- Approach 2: Card (Inactive) -->
<!-- 
<app-keyboard-navigation-hint
  *ngIf="visible && (hasNextDetail || hasPreviousDetail)"
  modalId="your-modal"
  [config]="keyboardHintConfig"
  displayStyle="card">
</app-keyboard-navigation-hint>
-->
```

## 🎯 Real-World Examples

### Modal with Navigation
```typescript
export class YourModalComponent {
  readonly keyboardHintConfig = KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL;
  
  get hasNextDetail(): boolean {
    return this.currentIndex < this.allItems.length - 1;
  }
  
  get hasPreviousDetail(): boolean {
    return this.currentIndex > 0;
  }
}
```

### Custom Keys Configuration
```typescript
readonly customKeys: KeyboardNavigationConfig = {
  keys: [
    { key: 'J', description: 'Next Item', icon: 'bi-arrow-down' },
    { key: 'K', description: 'Previous Item', icon: 'bi-arrow-up' },
    { key: 'Enter', description: 'Select', icon: 'bi-check' },
    { key: 'Esc', description: 'Cancel', icon: 'bi-x' }
  ],
  position: 'top-right',
  showMode: 'smart',
  autoHideDelay: 7000
};
```

## 📱 Responsive Behavior

The system automatically adjusts for mobile devices:
- Positions adjust to avoid overlapping with content
- Touch-friendly sizing on smaller screens
- Notification style flows to vertical layout

## 🎨 Customization

### CSS Custom Properties
You can override the default styles using CSS custom properties:

```scss
app-keyboard-navigation-hint {
  --hint-bg-color: rgba(0, 0, 0, 0.9);
  --hint-text-color: white;
  --hint-border-radius: 12px;
  --hint-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
}
```

### Custom Animations
The system includes smooth animations that can be customized:
- `slideInFade` - For minimalist style
- `slideInBounce` - For card style
- `popupSlideUp` - For interactive popup
- `slideInUp` - For notification style

## 🧪 Testing Different Approaches

Use the included `demo.html` file to preview all approaches:

1. Open `src/app/shared/components/keyboard-navigation-hint/demo.html` in a browser
2. Compare all 4 styles side by side
3. Choose the best fit for your use case

## 🔄 Migration Guide

### From Manual Keyboard Hints
Replace your existing keyboard hint implementations:

```typescript
// Before
@HostListener('document:keydown', ['$event'])
onKeyDown(event: KeyboardEvent) {
  // Custom keyboard handling
}

// After
readonly keyboardHintConfig = KeyboardNavigationHintService.PRESETS.NAVIGATION_MODAL;
// Add component to template
```

### Adding to Existing Modals
1. Import `KeyboardNavigationHintComponent`
2. Add configuration property
3. Add component to template with appropriate `*ngIf`
4. Test with different `displayStyle` options

## 🎉 Benefits

- ✅ **Consistent UX** - Same experience across all modals
- ✅ **User-Friendly** - Smart logic prevents annoyance
- ✅ **Maintainable** - Single source of truth
- ✅ **Reusable** - Works with any modal or component
- ✅ **Customizable** - Fits any design system
- ✅ **Accessible** - Screen reader friendly
- ✅ **Performant** - Lightweight and efficient

## 📋 Checklist for Implementation

- [ ] Import `KeyboardNavigationHintComponent`
- [ ] Add to component imports array
- [ ] Create configuration object
- [ ] Add component to template with proper conditions
- [ ] Choose appropriate `displayStyle`
- [ ] Test with different screen sizes
- [ ] Verify keyboard navigation still works
- [ ] Test show/hide behavior

## 🤝 Contributing

To add new display styles or improve existing ones:

1. Add new style option to `displayStyle` type
2. Implement template section in component
3. Add corresponding SCSS styles
4. Update demo.html with new example
5. Document in README

---

**Need help?** Check the demo file or examine the DPO modal implementation for a complete working example!