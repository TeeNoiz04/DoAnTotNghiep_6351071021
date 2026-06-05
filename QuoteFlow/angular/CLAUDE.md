# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Workflow

### Development Setup

- **Claude Code is only used for code changes** - All code modifications, debugging, and development tasks are handled through Claude Code
- **Frontend server runs separately** - The Angular dev server (`yarn local` or `yarn start`) should be run in a separate terminal/command prompt by user due to wsl2 limitation with Angular CLI
- **Error reporting** - If there are compilation errors or runtime issues, copy the error messages back to Claude Code for debugging
- **Linting and Formatting Note**: Don't need to npm run lint everytime at the end to check, and don't plan the test, I'll test and provide you errors if there's any

### Common Development Tasks

- **Local development with local backend**: `yarn local` (uses default environment file with local backend)
- **Local development with deployed backend**: `yarn start` (uses local environment with server's deployed backend)
- **Build for development**: `yarn build:dev`
- **Build for staging**: `yarn build:staging`
- **Build for production**: `yarn build:prod`
- **Run tests**: `npm test`
- **Lint code**: `npm run lint`
- **Format code**: `npm run format`
- **Format TypeScript only**: `npm run format:ts`
- **Check formatting**: `npm run format:check`

### Environment Configurations

The application supports multiple environments configured in `angular.json`:

- `local-dev` - Local development with default environment file and local backend
- `local` - Local development with local environment but deployed backend
- `development` - Development build with optimization
- `staging` - Staging environment
- `uat` - User acceptance testing
- `production` - Production build with full optimization

Environment files are located in `src/environments/`:

- `environment.ts` - Default environment
- `environment.local.ts` - Local development
- `environment.dev.ts` - Development
- `environment.staging.ts` - Staging
- `environment.uat.ts` - UAT
- `environment.prod.ts` - Production

## Application Architecture

### Technology Stack

- **Framework**: Angular 19 with standalone components
- **UI Framework**: ABP Framework with Lepton-X theme
- **Authentication**: OAuth2 with OIDC
- **Styling**: SCSS with Bootstrap
- **State Management**: Angular services with RxJS
- **HTTP Client**: Angular HttpClient with interceptors

### Project Structure

- `src/app/` - Main application code
- `src/app/shared/` - Shared components, services, and utilities
- `src/app/proxy/` - Generated API service proxies
- `src/app/proxy-custom/` - Custom extensions to generated proxies
- `src/environments/` - Environment configuration files
- `src/assets/` - Static assets (images, SCSS, etc.)

### Key Application Modules

The application is organized into feature modules:

- **Material Stock** (`materials/`) - Material inventory management
- **Special Price Offers** (`price-offers/`) - Price offer management with approval workflows
- **DPO Management** (`dpos/`) - Direct Purchase Order management
- **GIC Management** (`gics/`) - Goods In Custody management
- **PSI** (`psis/`) - Performance/Status Indicator management
- **Key Accounts** (`key-accounts/`) - Key account management with approval workflows
- **Stock Management** (`stock-management/`) - Stock import and management
- **Cargo Data** (`cargos/`) - Cargo import and reporting
- **System Categories** (`system-categories/`) - System configuration categories
- **Buyers** (`buyer/`) - Buyer management
- **Customers** (`customer/`) - Customer management

### Application Architecture Patterns

#### Module Structure

Each feature module follows this pattern:

```
module-name/
├── components/          # UI components
├── services/           # Business logic services
├── providers/          # Route providers
├── module-routing.module.ts
└── module.module.ts
```

#### Component Architecture

- **Abstract base components** - Define common functionality
- **Concrete components** - Implement specific features
- **Detail components** - Handle entity details and forms
- **Modal components** - Reusable modal dialogs

#### Service Architecture

- **Abstract service interfaces** - Define service contracts
- **Concrete service implementations** - Implement business logic
- **Generated proxy services** - Auto-generated API clients in `proxy/`
- **Custom proxy extensions** - Manual extensions in `proxy-custom/`

#### Shared Components

Located in `src/app/shared/components/`:

- **Data Table** - Reusable data grid component
- **Loading** - Loading indicators
- **Error Display** - Error handling components
- **Form Controls** - Custom form inputs
- **Modals** - Common modal dialogs
- **Upload Control** - File upload functionality
- **Smart Back Button** - Intelligent navigation component for detail pages

#### State Management

- **Loading Service** - Global loading state management
- **Title Service** - Dynamic page title management
- **Token Claims Service** - Authentication state
- **State Preservation** - Component state preservation across navigation

#### Routing

- **Route providers** - Centralized route configuration
- **AppRoutes class** - Route constants and structure
- **Lazy loading** - Feature modules loaded on demand

### Authentication & Authorization

- Uses ABP Framework authentication with OAuth2/OIDC
- Token-based authentication with JWT
- Role-based access control
- Automatic navigation to login for unauthenticated users

### API Integration

- Generated service proxies from backend API
- HTTP interceptors for request/response handling
- Centralized error handling
- Support for multiple API endpoints

### Development Notes

- Uses Angular 19 standalone components
- SCSS for styling with custom variables in `_variables.scss`
- FontAwesome icons and Bootstrap Icons
- Responsive design with Bootstrap grid system
- Dynamic environment configuration via `dynamic-env.json`
- Hot module replacement (HMR) enabled in development

### Code Writing Patterns

- **RxJS Syntax**: Prefer using RxJS syntax instead of async await promise coding pattern
- **Smart Back Button**: Always use SmartBackButtonComponent for detail pages instead of manual goBack() methods
- **Dependency Injection**: 
  - Prefer to use `inject()` over constructor injection

## Smart Back Button Implementation

### Overview

The SmartBackButtonComponent provides intelligent navigation for detail pages with automatic fallback handling and context awareness.

### Key Components

- **SmartBackButtonComponent** (`/shared/components/smart-back-button/`) - Reusable UI component
- **NavigationHistoryService** (`/shared/services/navigation-history/`) - Tracks navigation history
- **NavigationFallbackService** (`/shared/services/navigation-history/`) - Auto-determines fallback URLs

### Standard Implementation Pattern

#### Component Setup

```typescript
// Import the component
import { SmartBackButtonComponent } from '@app/shared/components/smart-back-button';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';

// Add to imports array
imports: [
  // ... other imports
  SmartBackButtonComponent,
]

// Inject navigation service for programmatic navigation
protected readonly navigationHistoryService = inject(NavigationHistoryService);

// Optional: Define custom fallback URL (auto-determined if not provided)
get fallbackUrl(): string[] {
  return [AppRoutes.MODULE.BASE, AppRoutes.MODULE.LIST.BASE];
}
```

#### Template Usage

```html
<!-- Simple usage (auto-determines fallback) -->
<app-smart-back-button></app-smart-back-button>

<!-- With custom fallback and styling -->
<app-smart-back-button
  [fallbackUrl]="fallbackUrl"
  buttonText="Back"
  buttonClass="btn btn-secondary"
  iconClass="bi bi-arrow-left-circle">
</app-smart-back-button>
```

#### Programmatic Navigation

```typescript
// In modal result handlers or other programmatic navigation
onApproveModalResult(result: ConfirmationNoteResult): void {
  if (result.confirmed) {
    // Call API first
    // Then navigate back
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }
}
```

### Migration Pattern

**Remove:** Old `goBack()` methods  
**Replace:** With SmartBackButtonComponent and navigationHistoryService.smartBack() calls

### Benefits

- ✅ Maintains user navigation context (back to previous page if within same module)
- ✅ Handles edge cases (direct access, new tabs, page refresh)
- ✅ Auto-determines appropriate fallback URLs
- ✅ Consistent UX across all detail pages
- ✅ Reduces code duplication

## Filter System Implementation

### Overview

The application uses a standardized filter system that provides consistent URL parameter handling, type safety, and Smart Back Button integration across all modules.

### Key Components

- **BaseFilterHelper** (`/shared/helpers/base-filter-helper.ts`) - Abstract base class for filter helpers
- **BaseFilterService** (`/shared/services/base-filter.service.ts`) - Abstract base service for filter management
- **BaseFilterComponent** (`/shared/components/base-filter.component.ts`) - Abstract base component for filter UI
- **QueryParamConverter** (`/shared/helpers/filter-helper.ts`) - Enhanced parameter conversion with validation
- **Templates** (`/shared/templates/`) - Copy-paste templates for new implementations

### Standard Implementation Pattern

#### 1. Create Filter Helper

```typescript
// your-module-filter-helper.ts
export class YourModuleFilterHelper extends BaseFilterHelper<GetYourModuleInput> {
  protected get paramMap(): Record<keyof GetYourModuleInput, ParamConfig> {
    return {
      filterText: { param: 'filter_text', type: 'string' },
      code: { param: 'code', type: 'string' },
      dateFrom: { param: 'date_from', type: 'date', format: 'YYYY-MM-DD' },
      status: { param: 'status', type: 'string' },
      isActive: { param: 'is_active', type: 'boolean', defaultValue: true },
      amount: { param: 'amount', type: 'number', validation: { min: 0 } },
      ...PagedAndSortedParamMap,
    };
  }
  
  protected get defaultValues(): Partial<GetYourModuleInput> {
    return {
      maxResultCount: 50,
      skipCount: 0,
      isActive: true,
    };
  }
}
```

#### 2. Create Filter Service

```typescript
// your-module-filter.service.ts
@Injectable({ providedIn: 'root' })
export class YourModuleFilterService extends BaseFilterService<
  GetYourModuleInput,
  YourModuleListDto,
  YourModuleDto
> {
  protected getFilterHelper(): BaseFilterHelper<GetYourModuleInput> {
    return YourModuleFilterHelper.getInstance();
  }
  
  protected getDefaultFilters(): GetYourModuleInput {
    return { maxResultCount: 50, skipCount: 0, isActive: true };
  }
  
  protected buildSearchFormControls(): { [key: string]: any } {
    return {
      filterText: [''],
      code: [''],
      dateFrom: [''],
      status: [''],
      isActive: [true],
      amount: [null],
    };
  }
  
  protected getListData(query: ABP.PageQueryParams & GetYourModuleInput): Observable<PagedResultDto<YourModuleListDto>> {
    return this.proxyService.getList(query);
  }
  
  protected mapFormToFilters(formValue: any): GetYourModuleInput {
    return {
      ...this.filters,
      filterText: formValue.filterText || '',
      code: formValue.code || '',
      dateFrom: formValue.dateFrom || '',
      status: formValue.status || '',
      isActive: formValue.isActive !== undefined ? formValue.isActive : true,
      amount: formValue.amount || null,
    };
  }
}
```

#### 3. Create Filter Component

```typescript
// your-module-filter.component.ts
@Component({
  selector: 'app-your-module-filter',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgbModule],
  template: `
    <form [formGroup]="searchForm!" (ngSubmit)="onSearch()" *ngIf="searchForm">
      <div class="row mb-3">
        <div class="col-md-3">
          <label class="form-label">Filter Text</label>
          <input type="text" class="form-control" formControlName="filterText">
        </div>
        <div class="col-md-3">
          <label class="form-label">Code</label>
          <input type="text" class="form-control" formControlName="code">
        </div>
        <!-- Add more form controls as needed -->
      </div>
      <div class="row">
        <div class="col-12">
          <button type="submit" class="btn btn-primary me-2">Search</button>
          <button type="button" class="btn btn-outline-secondary" (click)="onClear()">Clear</button>
        </div>
      </div>
    </form>
  `
})
export class YourModuleFilterComponent extends BaseFilterComponent<
  GetYourModuleInput,
  YourModuleListDto,
  YourModuleDto
> {
  protected readonly filterService = inject(YourModuleFilterService);
  
  protected mapFormToFilters(formValue: any): GetYourModuleInput {
    return {
      ...this.filters,
      filterText: formValue.filterText || '',
      code: formValue.code || '',
      // Map other form values
    };
  }
}
```

#### 4. Use in Main Component

```typescript
// your-module.component.ts
export class YourModuleComponent implements OnInit {
  protected readonly filterService = inject(YourModuleFilterService);
  
  ngOnInit() {
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });
    this.filterService.hookToQuery();
  }
  
  // Navigate to detail with filter state preservation
  onViewDetails(id: string) {
    this.filterService.navigateToDetail(id, 'details');
  }
}
```

### Parameter Configuration Options

```typescript
interface ParamConfig {
  param: string;                    // URL parameter name (use snake_case)
  type: 'string' | 'number' | 'date' | 'boolean' | 'array';
  format?: string;                  // Date format (e.g., 'YYYY-MM-DD')
  validation?: {
    required?: boolean;
    min?: number;                   // Min value for numbers, min length for strings
    max?: number;                   // Max value for numbers, max length for strings
    pattern?: RegExp;              // Pattern for string validation
    custom?: (value: any) => boolean; // Custom validation function
  };
  defaultValue?: any;               // Default value when parameter is missing
}
```

### URL Parameter Naming Conventions

- Use **snake_case** for URL parameters (e.g., `filter_text`, `date_from`, `is_active`)
- Use **descriptive names** (e.g., `price_offer_type` instead of `pot`)
- Use **consistent patterns** across modules:
  - `filter_text` for general text search
  - `date_from`, `date_to` for date ranges
  - `min_amount`, `max_amount` for number ranges
  - `is_active`, `is_approved` for boolean flags

### Smart Back Button Integration

The filter system automatically integrates with the Smart Back Button:

```typescript
// In detail components
export class YourModuleDetailComponent {
  protected readonly navigationHistoryService = inject(NavigationHistoryService);
  
  get fallbackUrl(): string[] {
    return [AppRoutes.YOUR_MODULE.BASE, AppRoutes.YOUR_MODULE.LIST.BASE];
  }
  
  // Smart back navigation preserves filter state
  onBack() {
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }
}
```

### Templates and Quick Start

Use the provided templates to quickly implement new filter systems:

1. **Filter Helper Template**: `/shared/templates/filter-helper.template.ts`
2. **Filter Service Template**: `/shared/templates/filter-service.template.ts`
3. **Filter Component Template**: `/shared/templates/filter-component.template.ts`

### Benefits

- ✅ **Consistent URL Structure**: All modules use the same parameter naming conventions
- ✅ **Type Safety**: Compile-time validation and type conversion
- ✅ **Filter State Preservation**: Filters persist across navigation and page reloads
- ✅ **Smart Back Button Integration**: Seamless navigation with filter state
- ✅ **Validation**: Built-in parameter validation with custom rules
- ✅ **Easy Implementation**: Templates reduce development time
- ✅ **Maintainable**: Centralized logic reduces code duplication
- ✅ **URL Sharing**: Shareable URLs with complete filter state

### Migration from Legacy Implementations

For existing modules, follow these steps:

1. **Create filter helper** extending `BaseFilterHelper`
2. **Update service** to extend `BaseFilterService`
3. **Update component** to extend `BaseFilterComponent`
4. **Test URL parameters** to ensure backward compatibility
5. **Update Smart Back Button** usage if needed

## Development Insights and Tools

- **Large Codebase Context**: When analyzing large codebases or multiple files that might exceed context limits, use the Gemini CLI with its massive context window. Use gemini -p to leverage Google Gemini's large context capacity.
```