---
applyTo: "**"
---

# MEVN.FA.WF2025 ABP Framework Development Guidelines

## Project Architecture

-   Module dependency chain: Domain.Shared → Application.Contracts → Application → EntityFrameworkCore → HttpApi → HttpApi.Host
-   Use `ExtendedAuditedAggregateRoot<Guid>` for main entities, `ExtendedFullAuditedAggregateRoot<Guid>` for soft delete
-   Business logic in domain entities and domain services, not in application services

## Naming Conventions

-   Entities: `PriceOffer`, `PriceOfferDetail`
-   DTOs: `EntityCreateDto`, `EntityUpdateDto`, `EntityDto`, `GetEntitiesInput`, `EntityImportDto`
-   Constants: `EntityConsts` class with `PropertyNameMaxLength` pattern
-   Fields: `_fieldName` for private/protected, PascalCase for public

## Key Patterns

-   Application services: `[RemoteService(IsEnabled = false)]` + `[Authorize(WF2025Permissions.Entity.Default)]`
-   User context: Use `IEffectiveUserContext _currentUser` for audit fields, not `ICurrentUser` directly
-   Validation: DTO level with `[Required]`, `[StringLength]`, business logic in domain with `Check.NotNull()`
-   Controllers: Inherit `AbpController`, implement service interface, use `[Route("api/app/entity-names")]`

## Database Configuration

-   Entity configuration in `OnModelCreating`: `b.Property(x => x.Name).IsRequired().HasMaxLength(EntityConsts.NameMaxLength)`
-   Repository pattern: `EfCoreRepository<WF2025DbContext, Entity, Guid>` with filtering via `WhereIf()`

## Excel Import/Export

-   Use keyed services: `services.AddKeyedScoped<IExcelValidator<ImportDto>>(serviceKey, factory)`
-   Row validators implement `IExcelRowValidator<T>` with `ConvertToDto()` and `ValidateRow()`
-   Constants for cells/columns: `ExcelStartCell = "A4"`, `ExcelNameColumn = "B"`

## Error Handling

-   Business exceptions: `throw new BusinessException(WF2025DomainErrorCodes.ErrorCode)`
-   Validation: `throw new AbpValidationException(validationErrors: results)`
-   User-friendly: `throw new UserFriendlyException("message")`

## Status Management

-   Use `WF2025Statuses` constants: `Draft`, `Submitted`, `Approved`, `Rejected`
-   Extension methods: `entity.IsDraft()`, `entity.IsApproved()`

For detailed patterns, refer to CODING_STANDARDS.md in project root.
