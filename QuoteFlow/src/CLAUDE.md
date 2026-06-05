# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Architecture

This is a layered monolith ABP Framework application using Domain Driven Design (DDD) with the following structure:

- **Domain Layer**: `MEVN.FA.WF2025.Domain` - Contains entities, domain services, and business logic
- **Application Layer**: `MEVN.FA.WF2025.Application` - Contains application services and business logic orchestration
- **Infrastructure Layer**: `MEVN.FA.WF2025.EntityFrameworkCore` - Contains data access implementations
- **Presentation Layer**: `MEVN.FA.WF2025.HttpApi.Host` - ASP.NET Core API host
- **Contracts**: `MEVN.FA.WF2025.Application.Contracts` - Contains DTOs and interfaces
- **Shared**: `MEVN.FA.WF2025.Domain.Shared` - Contains constants, enums, and shared domain logic

## Key Development Commands

### Backend (.NET)
```bash
# Build the entire solution
dotnet build

# Run the API host
dotnet run --project MEVN.FA.WF2025.HttpApi.Host

# Run database migrations
dotnet run --project MEVN.FA.WF2025.DbMigrator

# Install ABP client-side libraries
abp install-libs

# Generate OpenIddict certificate (development)
dotnet dev-certs https -v -ep openiddict.pfx -p f41b2c41-8b69-4a89-bac8-43987037b230
```

### Frontend (Angular)
```bash
# Navigate to Angular app
cd angular

# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Run e2e tests
npm run e2e
```

## Application Structure

### Entity Management Pattern
The application follows ABP's standard entity management pattern:
- **Entities**: Domain objects in `Domain/` folders
- **Repositories**: Data access interfaces in `Domain/` and implementations in `EntityFrameworkCore/`
- **Application Services**: Business logic in `Application/` folders
- **DTOs**: Data transfer objects in `Application.Contracts/` folders
- **Controllers**: Auto-generated API controllers via ABP conventions

### Excel Import System
The application includes a comprehensive Excel import system with:
- **ValidationConfig**: Defines Excel file structure and validation rules
- **ImportDto**: Data transfer objects for imported data
- **RowValidator**: Validates individual Excel rows
- **Full Table Validator**: Validates entire Excel files
- **Factory Pattern**: `ExcelValidatorFactory` for creating validators

**Important Error Propagation Pattern:**
When extending `BaseExcelValidator` and adding custom validation in `PostValidateAsync`, ensure row-level errors are propagated to the main result:

```csharp
protected override async Task PostValidateAsync(ExcelValidationResult<TDto> result, ExcelImportContext context)
{
    foreach (var row in result.ListData)
    {
        // Custom validation logic that adds errors to row.Errors
        
        // CRITICAL: Add row-level errors to the main result
        if (row.Errors.Any())
        {
            ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
        }
    }
    
    await base.PostValidateAsync(result, context);
}
```

Without this pattern, validation errors will be added to individual row objects but won't appear in the top-level `ExcelValidationResult.Errors` collection, making them invisible to the caller.

See `import-excel-instruction.md` for detailed implementation guide.

### Key Business Modules
- **Materials**: Material management with pricing and inventory
- **PriceOffers**: Price offer management with approval workflows
- **KeyAccounts**: Key account management with evaluations
- **StockTracing**: Stock tracking and management
- **Customers**: Customer management with PICs
- **Suppliers**: Supplier management
- **Workflows**: Approval workflow management

## Configuration

### Database
- Connection strings are configured in `appsettings.json` files
- Default configuration works out of the box
- Run `MEVN.FA.WF2025.DbMigrator` for initial database setup

### Authentication
- Uses OpenIddict for authentication
- Requires `openiddict.pfx` certificate file
- ABP Framework handles authentication/authorization automatically

## Development Workflow

1. **Backend Changes**: Make changes in appropriate layer following DDD principles
2. **Database Changes**: Add migrations, tell the user to confirm migration, and after got the permission, run `dotnet ef database update`
3. **Frontend Changes**: Update Angular components and services
4. **Testing**: Use ABP's built-in testing infrastructure
5. **Build**: Use `dotnet build` and `npm run build`

## ABP Framework Integration

This solution uses ABP Framework features:
- **Auto API Controllers**: Controllers are auto-generated from application services
- **Permission System**: Defined in `WF2025Permissions.cs`
- **Localization**: Resources in `WF2025Resource.cs`
- **Settings**: Configuration in `WF2025Settings.cs`
- **Background Jobs**: Located in `BackgroundJobs/` folder
- **Audit Logging**: Built-in auditing for entities

## ClosedXML Excel Export System

The application uses ClosedXML for Excel file manipulation and export functionality. Common patterns and features used:

### Template-Based Export
```csharp
// Load Excel template from file repository
var fileDescriptor = await _fileDescriptorRepository
    .FirstOrDefaultAsync(fd => fd.Name == "Template.xlsx");
var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

// Create workbook from template
using var workbook = new ClosedXML.Excel.XLWorkbook(templateStream);
var ws = workbook.Worksheets.First();
```

### Cell Value Setting
```csharp
// Set individual cell values
ws.Cell("C8").Value = supplierInfo;
ws.Cell("I9").Value = poDate?.ToString("dd/MM/yyyy");
ws.Cell("I11").Value = currency;
```

### Dynamic Row Insertion
```csharp
// Insert additional rows for dynamic data
if (dataCount > 1)
{
    ws.Row(startRow).InsertRowsBelow(dataCount - 1);
}

// Populate rows with data
for (int i = 0; i < dataItems.Count; i++)
{
    var row = ws.Row(startRow + i);
    row.Cell(2).Value = i + 1;           // Row number
    row.Cell(3).Value = item.Code;       // Data values
    row.Cell(4).Value = item.Description;
    // ... continue for other columns
}
```

### Formula and Calculation Cells
```csharp
// Set calculated totals
var totalQty = items.Sum(d => d.Qty);
var totalAmount = items.Sum(d => d.Amount);
ws.Cell(totalRow, 7).Value = totalQty;
ws.Cell(totalRow, 9).Value = totalAmount;
```

### Output Generation
```csharp
// Save to memory stream and return as downloadable content
var output = new MemoryStream();
workbook.SaveAs(output);
output.Position = 0;

return new RemoteStreamContent(
    output,
    "ExportedFile.xlsx",
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
);
```

### Best Practices
- Always use `using` statements for proper disposal of workbook resources
- Reset stream position to 0 before returning content
- Use meaningful cell references (e.g., "C8" instead of Cell(8,3))
- Handle dynamic row insertion before populating data
- Set appropriate MIME type for Excel files

## File Locations

### Key Configuration Files
- `appsettings.json`: Main configuration
- `WF2025*Module.cs`: Module configuration files
- `package.json`: Node.js dependencies (in HttpApi.Host for client libs)

### Business Logic
- Application Services: `Application/[Module](s/es)/[Module]AppService.cs`
- Domain Services: `Domain/[Module](s/es)/[Module]Manager.cs`
- Entities: `Domain/[Module](s/es)/[Module].cs`
- DTOs: `Application.Contracts/[Module](s/es)/[Module]Dto.cs`