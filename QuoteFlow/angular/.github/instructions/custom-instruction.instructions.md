---
applyTo: '**'
---

# Custom Instructions for AI

## Coding Standards

- Use Angular 19 syntax with standalone components
- Follow ABP framework conventions
- For modal, always create a separate component
- Use proper TypeScript typing and imports

## Technical Details

- Project runs with `yarn local` command (don't re-run)
- Ask for error files if compilation issues occur
- For reference of the coding pattern, refer to the `src/app/system-categories/` directory

## General Component Creation Pattern

### Adding New Feature Module with List/Detail

**Route Definition Flow:**

1. `app.routes.ts` - Define route constants with BASE, ORDER, TITLE
2. `[module]-routing.module.ts` - Map routes to components with guards
3. `app-routing.module.ts` - Lazy load module
4. Route provider pattern for menu integration

**Standard Module Structure:**

```
src/app/[feature]/
├── [feature]-routing.module.ts
├── [feature].module.ts
├── components/
│   ├── [feature].component.ts (list view)
│   ├── [feature].component.html (ABP data table)
│   ├── [feature].abstract.component.ts (business logic)
│   └── [feature]-detail.component.ts (modal)
├── services/
│   ├── [feature].service.ts
│   └── [feature]-detail.service.ts
└── providers/
    └── [feature]-route.provider.ts
```

**Component Pattern:**

- Main component extends abstract component
- Abstract handles data operations, filtering, CRUD
- Detail component for create/edit modals
- Use ABP ListService for data management
- Standalone components with proper imports

**Route Provider Pattern:**

- Use `provideAppInitializer` with `RoutesService`
- Define routes with path, name, icon, order, layout, policy
- Parent-child menu structure support

**Standard Imports:**

- `@abp/ng.core` (ListService, CoreModule, authGuard, permissionGuard)
- `@abp/ng.theme.shared` (ThemeSharedModule, DateAdapter, TimeAdapter)
- `@ng-bootstrap/ng-bootstrap` (UI components)
- `@abp/ng.components/page` (PageModule)
- `@volo/abp.commercial.ng.ui` (CommercialUiModule)
- `@ngx-validate/core` (NgxValidateCoreModule)

**Naming Conventions:**

- Files: kebab-case (`user-management.component.ts`)
- Classes: PascalCase (`UserManagementComponent`)
- Services: PascalCase with suffix (`UserManagementService`)
- Routes: UPPER_SNAKE_CASE (`USER_MANAGEMENT`)
- Menu names: `::Menu:FeatureName`
