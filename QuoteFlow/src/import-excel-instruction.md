# Excel Import Implementation Guide

This guide provides step-by-step instructions for implementing Excel import functionality in the WF2025 application.

## Overview

The Excel import system consists of several components working together:
- **ValidationConfig**: Defines Excel file structure and validation rules
- **ImportDto**: Data transfer object for imported data
- **RowValidator**: Validates individual rows
- **Full Table Validator**: Optional validator for entire file validation
- **Service Registration**: Dependency injection setup
- **Factory Registration**: Central factory for validator access

## Step-by-Step Implementation

### Step 1: Create ValidationConfig Class

Create a validation configuration class that extends `ExcelValidationConfig`.

**Reference**: `PriceOfferPPCustomerValidationConfig.cs`

```csharp
public class [EntityName]ValidationConfig : ExcelValidationConfig
{
    public [EntityName]ValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "[SheetName]",
                startCell: "[StartCell]", // e.g., "B2"
                endCell: "[EndCell]",     // e.g., "I100"
                startColumn: "[StartColumn]", // e.g., "B"
                endColumn: "[EndColumn]",     // e.g., "I"
                hasHeader: false // or true if has header
            ));
    }
}
```

### Step 2: Create ImportDto

Create a data transfer object for the imported data.

**Reference**: `PriceOfferCustomerImportDto.cs`

```csharp
namespace MEVN.FA.WF2025.[Module].[SubModule];

public class [EntityName]ImportDto
{
    public string? Property1 { get; set; }
    public string? Property2 { get; set; }
    // Add properties as needed
}
```

### Step 3: Create RowValidator Class

Create a row validator that implements `IExcelRowValidator<ImportDto>`.

**Reference**: `PriceOfferPPCustomerRowValidator.cs`

```csharp
public class [EntityName]RowValidator : IExcelRowValidator<[EntityName]ImportDto>
{
    public [EntityName]RowValidator()
    {
        // Constructor logic if needed
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();
        
        // Extract values from Excel columns
        var value1 = ExcelParser.GetValue<string?>(rowData, "A");
        var value2 = ExcelParser.GetValue<string?>(rowData, "B");
        
        // Validation logic
        if (string.IsNullOrEmpty(value1)) 
            result.AddError("Property1 is required.");
        if (string.IsNullOrEmpty(value2)) 
            result.AddError("Property2 is required.");
        
        return result;
    }

    public [EntityName]ImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new [EntityName]ImportDto()
        {
            Property1 = ExcelParser.GetValue<string?>(rowData, "A"),
            Property2 = ExcelParser.GetValue<string?>(rowData, "B"),
            // Map other properties
        };
    }
}
```

### Step 4: Create Full Table Validator (Optional)

Choose one of the following approaches:

#### Option A: Extend BaseExcelValidator (for PreValidate/PostValidate)

**Reference**: `PriceOfferPPCustomerValidator.cs`

```csharp
public class [EntityName]Validator : BaseExcelValidator<[EntityName]ImportDto>
{
    public [EntityName]Validator(
        ExcelValidationConfig config,
        IExcelRowValidator<[EntityName]ImportDto> rowValidator,
        ILogger<BaseExcelValidator<[EntityName]ImportDto>> logger
    ) : base(config, rowValidator, logger)
    {
    }

    protected override async Task<ValidationResult> PreValidateAsync(Stream stream)
    {
        // Custom pre-validation logic
        // Example: Check specific columns or file structure
        
        return await base.PreValidateAsync(stream);
    }

    protected override async Task<ValidationResult> PostValidateAsync(
        IEnumerable<[EntityName]ImportDto> data)
    {
        // Custom post-validation logic
        // Example: Cross-reference validation
        
        return await base.PostValidateAsync(data);
    }
}
```

#### Option B: Implement IExcelValidator (for combining multiple validators)

```csharp
public class [EntityName]CombinedValidator : IExcelValidator<[EntityName]ImportDto>
{
    private readonly IExcelValidator<[SubEntity1]ImportDto> _subValidator1;
    private readonly IExcelValidator<[SubEntity2]ImportDto> _subValidator2;

    public [EntityName]CombinedValidator(
        IExcelValidator<[SubEntity1]ImportDto> subValidator1,
        IExcelValidator<[SubEntity2]ImportDto> subValidator2)
    {
        _subValidator1 = subValidator1;
        _subValidator2 = subValidator2;
    }

    public async Task<ExcelValidationResult<[EntityName]ImportDto>> ValidateAsync(Stream stream)
    {
        // Combine multiple validators
        // Custom validation logic
    }
}
```

### Step 5: Register Services in WF2025ApplicationModule

Add service registrations in the `AddExcelValidation` method:

#### 5.1: Row Validators

```csharp
// For multiple implementations (keyed service)
services.AddKeyedScoped<IExcelRowValidator<[EntityName]ImportDto>, [EntityName]RowValidator>("KeyName");

// For single implementation (scoped service)
services.AddScoped<IExcelRowValidator<[EntityName]ImportDto>, [EntityName]RowValidator>();
```

#### 5.2: Table Validators

```csharp
services.AddScoped<IExcelValidator<[EntityName]ImportDto>>(provider =>
{
    var config = new [EntityName]ValidationConfig();
    var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<[EntityName]ImportDto>>("KeyName");
    // OR for non-keyed: provider.GetRequiredService<IExcelRowValidator<[EntityName]ImportDto>>();
    var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<[EntityName]ImportDto>>>();
    
    return new [EntityName]Validator(config, rowValidator, logger);
});
```

### Step 6: Factory Registration (For Root DTOs Only)

#### 6.1: Add Key to ExcelValidators.cs

```csharp
public static class ExcelValidators
{
    // Existing keys...
    public const string [EntityName] = "[EntityName]";
    // Add more validation types as needed
}
```

#### 6.2: Update ExcelValidatorFactory.cs

```csharp
public IExcelValidator<T> CreateValidator<T>(string validationType)
{
    return validationType switch
    {
        // Existing cases...
        ExcelValidators.[EntityName] => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<[EntityName]ImportDto>>("RegisteredKeyName"),
        // OR for non-keyed service:
        ExcelValidators.[EntityName] => (IExcelValidator<T>)_serviceProvider.GetRequiredService<IExcelValidator<[EntityName]ImportDto>>(),
        
        _ => throw new ArgumentException($"Unknown validation type: {validationType}")
    };
}
```

## Usage in Application Service

### Single File Type

```csharp
[HttpPost]
[Route("validate-and-parse")]
public async Task<ExcelValidationResult<[EntityName]ImportDto>> ValidateAndParseAsync(IFormFile file)
{
    var validator = _excelValidatorFactory.CreateValidator<[EntityName]ImportDto>(ExcelValidators.[EntityName]);
    
    using var stream = file.OpenReadStream();
    return await validator.ValidateAsync(stream);
}
```

### Multiple File Types

```csharp
[HttpPost]
[Route("validate-and-parse/pp")]
public async Task<ExcelValidationResult<[EntityName]ImportDto>> ValidateAndParsePPAsync(IFormFile file)
{
    var validator = _excelValidatorFactory.CreateValidator<[EntityName]ImportDto>(ExcelValidators.[EntityName]PP);
    
    using var stream = file.OpenReadStream();
    return await validator.ValidateAsync(stream);
}

[HttpPost]
[Route("validate-and-parse/ds")]
public async Task<ExcelValidationResult<[EntityName]ImportDto>> ValidateAndParseDSAsync(IFormFile file)
{
    var validator = _excelValidatorFactory.CreateValidator<[EntityName]ImportDto>(ExcelValidators.[EntityName]DS);
    
    using var stream = file.OpenReadStream();
    return await validator.ValidateAsync(stream);
}
```

## File Naming Conventions

- ValidationConfig: `[EntityName][Type]ValidationConfig.cs`
- ImportDto: `[EntityName]ImportDto.cs`
- RowValidator: `[EntityName][Type]RowValidator.cs`
- Full Validator: `[EntityName][Type]Validator.cs`

## Directory Structure

```
Application/
  [Module]/
    Excels/
      [EntityType]s/
        [EntityName][Type]ValidationConfig.cs
        [EntityName][Type]RowValidator.cs
        [EntityName][Type]Validator.cs (optional)

Application.Contracts/
  [Module]/
    [SubModule]/
      [EntityName]ImportDto.cs
```

## Common Excel Column Mapping

Use `ExcelParser.GetValue<T>()` to extract values from Excel columns:

```csharp
// String values
var text = ExcelParser.GetValue<string?>(rowData, "A");

// Numeric values
var number = ExcelParser.GetValue<decimal?>(rowData, "B");
var integer = ExcelParser.GetValue<int?>(rowData, "C");

// Date values
var date = ExcelParser.GetValue<DateTime?>(rowData, "D");

// Boolean values
var isActive = ExcelParser.GetValue<bool?>(rowData, "E");
```

## Error Handling

Always validate required fields and add meaningful error messages:

```csharp
if (string.IsNullOrEmpty(requiredField))
    result.AddError("Field name is required.");

if (numericField < 0)
    result.AddError("Field must be greater than or equal to 0.");
```

This completes the Excel import implementation. Follow these steps in order, and you'll have a fully functional Excel import system integrated with the application's validation framework.
