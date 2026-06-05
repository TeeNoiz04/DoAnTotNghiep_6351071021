# Filter Implementation TODO

## Remaining Modules to Implement Standardized Filter System

### ✅ **Completed Modules:**
- **Buyer Module** - ✅ Complete
- **Customer Module** - ✅ Complete  
- **Key Account Evaluations Module** - ✅ Complete

### 📋 **Pending Modules:**

#### 🔹 **GICs Module** (Medium Priority)
- **Path**: `src/app/gics/`
- **Task**: Create complete filter implementation (FilterHelper, FilterService, FilterComponent)
- **Status**: No existing filter system - needs full implementation
- **Pattern**: Follow Key Account Evaluations template

#### 🔹 **Cargos Module** (Medium Priority)  
- **Path**: `src/app/cargos/`
- **Task**: Replace existing `ImportCargoFilterComponent` with standardized system
- **Status**: Has basic filter component but not using base classes
- **Pattern**: Follow Buyer/Customer update template

#### 🔹 **Materials Module** (Medium Priority)
- **Path**: `src/app/materials/`
- **Task**: Consolidate multiple filter components into standardized system
- **Status**: Multiple sub-modules with individual filter components
- **Pattern**: Create unified filter for main material management view

#### 🔹 **Key Accounts Module** (Medium Priority)
- **Path**: `src/app/key-accounts/`
- **Task**: Create separate filter implementations for each view (main, class-adjustment, reports)
- **Status**: Multiple filter components for different views
- **Pattern**: Separate FilterHelper/Service/Component for each view type

#### 🔹 **Import Allocations Module** (Medium Priority)
- **Path**: `src/app/import-allocations/`
- **Task**: Create filter implementations for allocation-priority and import-allocation sub-modules
- **Status**: Multiple sub-modules each with their own filter components
- **Pattern**: Separate FilterHelper/Service/Component for each sub-module

#### 🔹 **PSIs Module** (Low Priority)
- **Path**: `src/app/psis/` (check if exists)
- **Task**: Create complete filter implementation
- **Status**: Filter logic embedded in components
- **Pattern**: Follow Key Account Evaluations template

### 🎯 **Implementation Pattern:**

For each module, create:
1. **FilterHelper** extending `BaseFilterHelper` with singleton pattern
2. **FilterService** extending `BaseFilterService` 
3. **FilterComponent** extending `BaseFilterComponent`
4. Update main component to use template reference pattern (`#filter`)
5. Update HTML template to use `filter.onSearch()` and `filter.onClear()`
6. Ensure proper imports and providers

### 📁 **Templates Available:**
- Filter Helper: `/shared/templates/filter-helper.template.ts`
- Filter Service: `/shared/templates/filter-service.template.ts`  
- Filter Component: `/shared/templates/filter-component.template.ts`

### 🔗 **Base Classes:**
- `BaseFilterHelper` - `/shared/helpers/base-filter-helper.ts`
- `BaseFilterService` - `/shared/services/base-filter.service.ts`
- `BaseFilterComponent` - `/shared/components/base-filter.component.ts`

---
*Last updated: 2025-07-18*
*Progress: 3/9 modules completed*