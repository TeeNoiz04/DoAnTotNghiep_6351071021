using QuoteFlow.Buyers;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Utils;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace QuoteFlow.PriceOffers;

public class PriceOfferManager : DomainService
{
    protected IPriceOfferRepository _priceOfferRepository;
    protected IPriceOfferDetailRepository _priceOfferDetailRepository;
    protected IPriceOfferCustomerRepository _priceOfferCustomerRepository;
    protected ICurrentUser _currentUser;

    public PriceOfferManager(IPriceOfferRepository priceOfferRepository, IPriceOfferDetailRepository priceOfferDetailRepository, IPriceOfferCustomerRepository priceOfferCustomerRepository, ICurrentUser currentUser)
    {
        _priceOfferRepository = priceOfferRepository;
        _priceOfferDetailRepository = priceOfferDetailRepository;
        _priceOfferCustomerRepository = priceOfferCustomerRepository;
        _currentUser = currentUser;
    }

    public virtual async Task<PriceOffer> CreateAsync(PriceOfferCreateParams createParams)
    {
        // Populate denormalization fields if not already provided
        await PopulateDenormalizationFieldsAsync(createParams);

        var priceOffer = new PriceOffer(createParams.Id ?? GuidGenerator.Create(), createParams);

        var result = await _priceOfferRepository.InsertAsync(priceOffer, autoSave: true);

        var priceOfferDetails = (createParams.Details ?? [])
            .Select(detail => new PriceOfferDetail(GuidGenerator.Create(), detail));
        await _priceOfferDetailRepository.InsertManyAsync(priceOfferDetails, autoSave: true);

        var priceOfferCustomers = (createParams.Customers ?? [])
            .Select(customer => new PriceOfferCustomer(GuidGenerator.Create(), customer));
        await _priceOfferCustomerRepository.InsertManyAsync(priceOfferCustomers, autoSave: true);

        return result;
    }

    private async Task PopulateDenormalizationFieldsAsync(PriceOfferCreateParams createParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            createParams.ProjectTypeId, description => createParams.ProjectTypeDescription = description, createParams.ProjectTypeDescription,
            createParams.EUIndustryId, description => createParams.EUIndustryDescription = description, createParams.EUIndustryDescription,
            createParams.KeyAccountClassId, description => createParams.KeyAccountClassDescription = description, createParams.KeyAccountClassDescription,
            createParams.KeyAccountTypeId, description => createParams.KeyAccountTypeDescription = description, createParams.KeyAccountTypeDescription,
            createParams.LocationId, description => createParams.LocationDescription = description, createParams.LocationDescription,
            createParams.BuyerId, buyerCode => createParams.BuyerCode = buyerCode, createParams.BuyerCode,
            createParams.BuyerTypeId, buyerTypeCode => createParams.BuyerTypeDescription = buyerTypeCode, createParams.BuyerTypeDescription
        );
    }

    private async Task PopulateDenormalizationFieldsAsync(PriceOfferUpdateParams updateParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            updateParams.ProjectTypeId, description => updateParams.ProjectTypeDescription = description, updateParams.ProjectTypeDescription,
            updateParams.EUIndustryId, description => updateParams.EUIndustryDescription = description, updateParams.EUIndustryDescription,
            updateParams.KeyAccountClassId, description => updateParams.KeyAccountClassDescription = description, updateParams.KeyAccountClassDescription,
            updateParams.KeyAccountTypeId, description => updateParams.KeyAccountTypeDescription = description, updateParams.KeyAccountTypeDescription,
            updateParams.LocationId, description => updateParams.LocationDescription = description, updateParams.LocationDescription,
            updateParams.BuyerId, buyerCode => updateParams.BuyerCode = buyerCode, updateParams.BuyerCode,
            updateParams.BuyerTypeId, buyerTypeCode => updateParams.BuyerTypeDescription = buyerTypeCode, updateParams.BuyerTypeDescription
        );
    }

    private async Task PopulateDenormalizationFieldsHelperAsync(
        Guid? projectTypeId, Action<string?> setProjectTypeDescription, string? currentProjectTypeDescription,
        Guid? euIndustryId, Action<string?> setEUIndustryDescription, string? currentEUIndustryDescription,
        Guid? keyAccountClassId, Action<string?> setKeyAccountClassDescription, string? currentKeyAccountClassDescription,
        Guid? keyAccountTypeId, Action<string?> setKeyAccountTypeDescription, string? currentKeyAccountTypeDescription,
        Guid? locationId, Action<string?> setLocationDescription, string? currentLocationDescription,
        Guid? buyerId, Action<string?> setBuyerCode, string? currentBuyerCode,
        Guid? buyerTypeId, Action<string?> setBuyerTypeCode, string? currentBuyerTypeCode)
    {
        var systemCategoryRepository = LazyServiceProvider.LazyGetRequiredService<ISystemCategoryRepository>();
        var idsToLookup = new List<Guid>();

        if (projectTypeId.HasValue && string.IsNullOrEmpty(currentProjectTypeDescription))
            idsToLookup.Add(projectTypeId.Value);
        if (euIndustryId.HasValue && string.IsNullOrEmpty(currentEUIndustryDescription))
            idsToLookup.Add(euIndustryId.Value);
        if (keyAccountClassId.HasValue && string.IsNullOrEmpty(currentKeyAccountClassDescription))
            idsToLookup.Add(keyAccountClassId.Value);
        if (keyAccountTypeId.HasValue && string.IsNullOrEmpty(currentKeyAccountTypeDescription))
            idsToLookup.Add(keyAccountTypeId.Value);
        if (locationId.HasValue && string.IsNullOrEmpty(currentLocationDescription))
            idsToLookup.Add(locationId.Value);
        if (buyerTypeId.HasValue && string.IsNullOrEmpty(currentBuyerTypeCode))
            idsToLookup.Add(buyerTypeId.Value);

        if (idsToLookup.Any())
        {
            var systemCategories = await systemCategoryRepository.GetListAsync(sc => idsToLookup.Contains(sc.Id));
            var systemCategoryDict = systemCategories.ToDictionary(sc => sc.Id, sc => sc.Description);

            if (projectTypeId.HasValue && string.IsNullOrEmpty(currentProjectTypeDescription) &&
                systemCategoryDict.TryGetValue(projectTypeId.Value, out var projectTypeDesc))
                setProjectTypeDescription(projectTypeDesc);

            if (euIndustryId.HasValue && string.IsNullOrEmpty(currentEUIndustryDescription) &&
                systemCategoryDict.TryGetValue(euIndustryId.Value, out var euIndustryDesc))
                setEUIndustryDescription(euIndustryDesc);

            if (keyAccountClassId.HasValue && string.IsNullOrEmpty(currentKeyAccountClassDescription) &&
                systemCategoryDict.TryGetValue(keyAccountClassId.Value, out var keyAccountClassDesc))
                setKeyAccountClassDescription(keyAccountClassDesc);

            if (keyAccountTypeId.HasValue && string.IsNullOrEmpty(currentKeyAccountTypeDescription) &&
                systemCategoryDict.TryGetValue(keyAccountTypeId.Value, out var keyAccountTypeDesc))
                setKeyAccountTypeDescription(keyAccountTypeDesc);

            if (locationId.HasValue && string.IsNullOrEmpty(currentLocationDescription) &&
                systemCategoryDict.TryGetValue(locationId.Value, out var locationDesc))
                setLocationDescription(locationDesc);

            if (buyerTypeId.HasValue && string.IsNullOrEmpty(currentBuyerTypeCode) &&
                systemCategoryDict.TryGetValue(buyerTypeId.Value, out var buyerTypeCode))
                setBuyerTypeCode(buyerTypeCode);
        }

        var buyerRepository = LazyServiceProvider.LazyGetRequiredService<IBuyerRepository>();
        if (buyerId.HasValue && string.IsNullOrEmpty(currentBuyerCode))
        {
            var buyer = await buyerRepository.GetAsync(buyerId.Value);
            setBuyerCode(buyer.BuyerCode);
        }
    }

    public virtual async Task<PriceOffer> UpdateTotalAmountAsync(Guid id, decimal totalAmount)
    {
        var priceOffer = await _priceOfferRepository.GetAsync(id);

        priceOffer.TotalMEVNOfferAmount = totalAmount;

        return await _priceOfferRepository.UpdateAsync(priceOffer);
    }

    public virtual async Task<PriceOffer> UpdateAsync(Guid id, PriceOfferUpdateParams updateParams)
    {
        // Populate denormalization fields if not already provided
        await PopulateDenormalizationFieldsAsync(updateParams);

        var priceOffer = await _priceOfferRepository.GetAsync(id);

        priceOffer.PriceOfferCode = updateParams.PriceOfferCode;
        priceOffer.BuyerId = updateParams.BuyerId;
        priceOffer.MaterialType = updateParams.MaterialType;
        priceOffer.LocationId = updateParams.LocationId;
        priceOffer.LocationOld = updateParams.LocationOld;
        priceOffer.ProjectName = updateParams.ProjectName;
        priceOffer.ProjectTypeId = updateParams.ProjectTypeId;
        priceOffer.EUIndustryId = updateParams.EUIndustryId;
        priceOffer.Application = updateParams.Application;
        priceOffer.Country = updateParams.Country;
        priceOffer.Province = updateParams.Province;
        priceOffer.DetailedAddress = updateParams.DetailedAddress;
        priceOffer.CompetitorBrand = updateParams.CompetitorBrand;
        priceOffer.PriceGapWithCompetitor = updateParams.PriceGapWithCompetitor;
        priceOffer.DecisionRight = updateParams.DecisionRight;
        priceOffer.POPlannedDate = updateParams.POPlannedDate;
        priceOffer.DeliveryDate = updateParams.DeliveryDate;
        priceOffer.UpcomingPotentialProjects = updateParams.UpcomingPotentialProjects;
        priceOffer.OtherPJInformation = updateParams.OtherPJInformation;
        priceOffer.FileName = updateParams.FileName;
        priceOffer.Note = updateParams.Note;
        priceOffer.CloseDate = updateParams.CloseDate;
        priceOffer.TotalMEVNOfferAmount = updateParams.TotalMEVNOfferAmount;
        priceOffer.AccountNo = updateParams.AccountNo;
        priceOffer.KeyAccountId = updateParams.KeyAccountId;
        priceOffer.KeyAccountTypeId = updateParams.KeyAccountTypeId;
        priceOffer.KeyAccountClassId = updateParams.KeyAccountClassId;
        priceOffer.BuyerTypeDescription = updateParams.BuyerTypeDescription;
        priceOffer.ProjectTypeDescription = updateParams.ProjectTypeDescription;
        priceOffer.EUIndustryDescription = updateParams.EUIndustryDescription;
        priceOffer.KeyAccountClassDescription = updateParams.KeyAccountClassDescription;
        priceOffer.KeyAccountTypeDescription = updateParams.KeyAccountTypeDescription;
        priceOffer.LocationDescription = updateParams.LocationDescription;

        priceOffer.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _priceOfferRepository.UpdateAsync(priceOffer);
    }

    public string GetDraftCode(string prefix, string materialType)
    {
        Check.NotNullOrWhiteSpace(prefix, nameof(prefix));
        Check.NotNullOrWhiteSpace(materialType, nameof(materialType));

        return GenerateCode(prefix, materialType, 0); // 0 for draft code
    }

    public async Task<string> GenerateNewCodeAsync(string prefix, string materialType)
    {
        Check.NotNullOrWhiteSpace(prefix, nameof(prefix));
        Check.NotNullOrWhiteSpace(materialType, nameof(materialType));

        var latestCode = await _priceOfferRepository.GetLatestCodeAsync(prefix, materialType);
        if (latestCode == null)
        {
            // code 001
            return GenerateCode(prefix, materialType, 1);
        }
        else
        {
            // Extract the numeric suffix from the latest code
            var numericSuffix = CodeHelper.ExtractNumericSuffix(latestCode);
            var (year, month) = ExtractYearMonthFromCode(latestCode);
            if (numericSuffix == -1 || year != DateTime.Now.Year || month != DateTime.Now.Month)
            {
                return GenerateCode(prefix, materialType, 1);
            }

            // Increment the count for the new code
            var newCount = numericSuffix + 1;
            return GenerateCode(prefix, materialType, newCount);
        }
    }

    public async Task PopulateDenormalizationFieldsAsync()
    {
        var systemCategoryRepository = LazyServiceProvider.LazyGetRequiredService<ISystemCategoryRepository>();

        // Get all PriceOffers that need denormalization field population
        var priceOffers = await _priceOfferRepository.GetQueryableAsync();
        var priceOffersToUpdate = priceOffers
            .Where(p => p.ProjectTypeDescription == null ||
                       p.EUIndustryDescription == null ||
                       p.KeyAccountClassDescription == null ||
                       p.KeyAccountTypeDescription == null ||
                       p.LocationDescription == null)
            .ToList();

        var systemCategories = await systemCategoryRepository.GetListAsync();
        var systemCategoryDict = systemCategories.ToDictionary(sc => sc.Id, sc => sc.Description);

        foreach (var priceOffer in priceOffersToUpdate)
        {
            var hasChanges = false;

            // Update ProjectTypeDescription
            if (priceOffer.ProjectTypeId.HasValue && string.IsNullOrEmpty(priceOffer.ProjectTypeDescription))
            {
                if (systemCategoryDict.TryGetValue(priceOffer.ProjectTypeId.Value, out var projectTypeDesc))
                {
                    priceOffer.ProjectTypeDescription = projectTypeDesc;
                    hasChanges = true;
                }
            }

            // Update EUIndustryDescription
            if (priceOffer.EUIndustryId.HasValue && string.IsNullOrEmpty(priceOffer.EUIndustryDescription))
            {
                if (systemCategoryDict.TryGetValue(priceOffer.EUIndustryId.Value, out var euIndustryDesc))
                {
                    priceOffer.EUIndustryDescription = euIndustryDesc;
                    hasChanges = true;
                }
            }

            // Update KeyAccountClassDescription
            if (priceOffer.KeyAccountClassId.HasValue && string.IsNullOrEmpty(priceOffer.KeyAccountClassDescription))
            {
                if (systemCategoryDict.TryGetValue(priceOffer.KeyAccountClassId.Value, out var keyAccountClassDesc))
                {
                    priceOffer.KeyAccountClassDescription = keyAccountClassDesc;
                    hasChanges = true;
                }
            }

            // Update KeyAccountTypeDescription
            if (priceOffer.KeyAccountTypeId.HasValue && string.IsNullOrEmpty(priceOffer.KeyAccountTypeDescription))
            {
                if (systemCategoryDict.TryGetValue(priceOffer.KeyAccountTypeId.Value, out var keyAccountTypeDesc))
                {
                    priceOffer.KeyAccountTypeDescription = keyAccountTypeDesc;
                    hasChanges = true;
                }
            }

            // Update LocationDescription
            if (priceOffer.LocationId.HasValue && string.IsNullOrEmpty(priceOffer.LocationDescription))
            {
                if (systemCategoryDict.TryGetValue(priceOffer.LocationId.Value, out var locationDesc))
                {
                    priceOffer.LocationDescription = locationDesc;
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await _priceOfferRepository.UpdateAsync(priceOffer);
            }
        }
    }

    public async Task<List<ValidationResult>> ValidateActionAsync(PriceOffer priceOffer)
    {
        var validationResult = new List<ValidationResult>();

        return validationResult;
    }

    /// <summary>
    /// Generates a formatted code string based on the provided prefix, product type and count.
    /// </summary>
    /// <remarks>The generated code includes the last two digits of the year and the two-digit month extracted
    /// from <paramref name="dateTime"/>.</remarks>
    /// <param name="prefix">The prefix to include in the generated code. Typically represents a category or identifier.</param>
    /// <param name="productType">The type of product to include in the generated code. Cannot be null or empty.</param>
    /// <param name="count">The numeric count to include in the generated code. Must be greater than 0 to include a specific count;
    /// otherwise, a placeholder value ("xxx") will be used.</param>
    /// <returns>A formatted string representing the generated code. The format is:
    /// {prefix}.{productType}_{year}.{month}_{count:D3} if <paramref name="count"/> is greater than 0,  or
    /// {prefix}.{productType}_{year}.{month}_xxx if <paramref name="count"/> is less than or equal to 0.</returns>
    private string GenerateCode(string prefix, string materialType, int count)
    {
        var dateTime = DateTime.Now; // Use current date and time
        var year = dateTime.Year.ToString().Substring(2, 2); // Last two digits of the year
        var month = dateTime.Month.ToString("D2");

        if (count <= 0)
        {
            // draft code
            return $"{prefix}.{materialType}_{year}.{month}_xxx";
        }

        return $"{prefix}.{materialType}_{year}.{month}_{count:D3}";
    }

    private (int year, int month) ExtractYearMonthFromCode(string code)
    {
        Check.NotNullOrWhiteSpace(code, nameof(code));
        var parts = code.Split('_');
        if (parts.Length < 2)
        {
            throw new AbpException($"Invalid code format: '{code}'");
        }
        var datePart = parts[1]; // Get the part after the first underscore
        var dateParts = datePart.Split('.');
        if (dateParts.Length < 2)
        {
            throw new AbpException($"Invalid date format in code: '{code}'");
        }
        var year = int.Parse(dateParts[0]) + 2000;
        var month = int.Parse(dateParts[1]);
        return (year, month);
    }
}