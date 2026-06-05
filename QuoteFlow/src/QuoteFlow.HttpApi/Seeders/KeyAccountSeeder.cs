namespace QuoteFlow.Seeders;

public static class KeyAccountSeeder
{
    //public static List<KeyAccountDto> Generate(
    //    int count,
    //    int seed,
    //    List<SystemCategoryDto> keyAccountTypes,
    //    List<SystemCategoryDto> keyAccountClasses,
    //    List<SystemCategoryDto> evaluationFinancials,
    //    List<SystemCategoryDto> evaluationProducts,
    //    List<BuyerDto> buyers)
    //{
    //    Randomizer.Seed = new Random(seed);

    //    var statuses = new List<string>()
    //    {
    //        KeyAccountStatuses.Pending,
    //        KeyAccountStatuses.Approved,
    //        KeyAccountStatuses.Rejected,
    //        KeyAccountStatuses.InProgress
    //    };

    //    var faker = new Faker<KeyAccountDto>()
    //         .RuleFor(ka => ka.Id, f => f.Random.Guid())
    //         .RuleFor(ka => ka.Status, f => f.PickRandom(new[] { "APPROVED", "CANCELLED", "CLOSED", "IN_PROCESS", "DRAFT", "REJECTED" }))
    //         .RuleFor(ka => ka.Buyer, (f, ka) =>
    //         {
    //             var buyer = f.PickRandom(buyers);
    //             ka.BuyerId = buyer.Id;
    //             return ka.Buyer = buyer;
    //         })
    //        .RuleFor(ka => ka.TaxCode, f => f.Random.AlphaNumeric(10))
    //        .RuleFor(ka => ka.KeyAccountCode, f => f.Random.AlphaNumeric(8))
    //        .RuleFor(ka => ka.KeyAccountShortName, f => f.Company.CompanyName())
    //        .RuleFor(ka => ka.KeyAccountName, f => f.Company.CompanyName())
    //        .RuleFor(ka => ka.KeyAccountType, (f, ka) =>
    //        {
    //            var type = f.PickRandom(keyAccountTypes);
    //            ka.KeyAccountTypeId = type.Id;
    //            return ka.KeyAccountType = type;
    //        })
    //        .RuleFor(ka => ka.KeyAccountClass, (f, ka) =>
    //        {
    //            if (f.Random.Bool())
    //            {
    //                var clazz = f.PickRandom(keyAccountClasses);
    //                ka.KeyAccountClass = clazz;
    //                ka.KeyAccountClassId = clazz.Id;
    //            }
    //            else
    //            {
    //                ka.KeyAccountClass = null;
    //                ka.KeyAccountClassId = null;
    //            }
    //            return ka.KeyAccountClass;
    //        })
    //        .RuleFor(ka => ka.FinancialEvaluations, (f, ka) =>
    //        {
    //            var evaluations = evaluationFinancials.Select(cat => new FinancialEvaluationDto
    //            {
    //                Id = f.Random.Guid(),
    //                KeyAccountId = ka.Id,
    //                EvaluationId = cat.Id,
    //                Evaluation = new SystemCategoryListDto
    //                {
    //                    Id = cat.Id,
    //                    Code = cat.Code,
    //                    Description = cat.Description,
    //                    CategoryType = cat.CategoryType,
    //                    Value = cat.Value,
    //                    IsDeactive = cat.IsDeactive,
    //                    Note = cat.Note,
    //                    CreatorId = cat.CreatorId,
    //                    CreatorUsername = cat.CreatorUsername,
    //                    CreatorName = cat.CreatorName,
    //                    CreationTime = cat.CreationTime,
    //                    LastModifierId = cat.LastModifierId,
    //                    LastModifierUsername = cat.LastModifierUsername,
    //                    LastModifierName = cat.LastModifierName,
    //                    LastModificationTime = cat.LastModificationTime,
    //                },
    //                BuyerInfo1 = f.Company.CompanyName(),
    //                MEVNInfo1 = f.Company.CompanySuffix(),
    //                Note = f.Lorem.Sentence(),
    //                ConcurrencyStamp = f.Random.Guid().ToString(),
    //                KeyAccount = null,
    //                CreationTime = f.Date.Past(),
    //                CreatorName = f.Name.FullName(),
    //                CreatorUsername = f.Internet.UserName(),
    //                CreatorId = f.Random.Guid(),
    //                LastModificationTime = f.Date.Recent(),
    //                LastModifierId = f.Random.Guid(),
    //                LastModifierName = f.Name.FullName(),
    //                LastModifierUsername = f.Internet.UserName(),
    //            }).ToList();

    //            ka.FinancialEvaluations = evaluations;
    //            return evaluations;
    //        })
    //        .RuleFor(ka => ka.ProductEvaluations, (f, ka) =>
    //        {
    //            var evaluations = evaluationProducts.Select(cat => new ProductEvaluationDto
    //            {
    //                Id = f.Random.Guid(),
    //                KeyAccountId = ka.Id,
    //                EvaluationId = cat.Id,
    //                Evaluation = new SystemCategoryListDto
    //                {
    //                    Id = cat.Id,
    //                    Code = cat.Code,
    //                    Description = cat.Description,
    //                    CategoryType = cat.CategoryType,
    //                    Value = cat.Value,
    //                    IsDeactive = cat.IsDeactive,
    //                    Note = cat.Note,
    //                    CreatorId = cat.CreatorId,
    //                    CreatorUsername = cat.CreatorUsername,
    //                    CreatorName = cat.CreatorName,
    //                    CreationTime = cat.CreationTime,
    //                    LastModifierId = cat.LastModifierId,
    //                    LastModifierUsername = cat.LastModifierUsername,
    //                    LastModifierName = cat.LastModifierName,
    //                    LastModificationTime = cat.LastModificationTime,
    //                },
    //                MEVNInfo1 = f.Company.CatchPhrase(),
    //                MEVNInfo2 = f.Company.CatchPhrase(),
    //                CompetitorInfo1 = f.Company.CatchPhrase(),
    //                CompetitorInfo2 = f.Company.CatchPhrase(),
    //                Note = f.Lorem.Sentence(),
    //                ConcurrencyStamp = f.Random.Guid().ToString(),
    //                KeyAccount = null,
    //                CreationTime = f.Date.Past(),
    //                CreatorName = f.Name.FullName(),
    //                CreatorUsername = f.Internet.UserName(),
    //                CreatorId = f.Random.Guid(),
    //                LastModificationTime = f.Date.Recent(),
    //                LastModifierId = f.Random.Guid(),
    //                LastModifierName = f.Name.FullName(),
    //                LastModifierUsername = f.Internet.UserName(),
    //            }).ToList();

    //            ka.ProductEvaluations = evaluations;
    //            return evaluations;
    //        })
    //        .RuleFor(ka => ka.MEVNSalePIC, f => f.Name.FullName())
    //        .RuleFor(ka => ka.Status, f => f.PickRandom(statuses))
    //        .RuleFor(ka => ka.Province, f => f.Address.County())
    //        .RuleFor(ka => ka.Address, f => f.Address.FullAddress())
    //        .RuleFor(ka => ka.NationalityId, f => f.Random.Guid())
    //        .RuleFor(ka => ka.Website, f => f.Internet.Url())
    //        .RuleFor(ka => ka.TypeOfBusiness, f => f.Lorem.Sentence())
    //        .RuleFor(ka => ka.TargetEU, f => f.Lorem.Sentence())
    //        .RuleFor(ka => ka.CurrentSaleRoute, f => f.Lorem.Word())
    //        .RuleFor(ka => ka.LastPODate, f => f.Date.Past().ToString("yyyy-MM-dd"))
    //        .RuleFor(ka => ka.PersonInCharge, f => f.Name.FullName())
    //        .RuleFor(ka => ka.Phone, f => f.Phone.PhoneNumber())
    //        .RuleFor(ka => ka.Email, f => f.Internet.Email())
    //        .RuleFor(ka => ka.Note, f => f.Lorem.Sentence())
    //        .RuleFor(ka => ka.RegistrationYear, f => f.Date.Past(5).Year)
    //        .RuleFor(ka => ka.LastRegisteredProjectCode, f => f.Random.AlphaNumeric(10))
    //        .RuleFor(ka => ka.BuyerKeyAccountClass, (f, ka) =>
    //        {
    //            if (f.Random.Bool())
    //            {
    //                var class_ = f.PickRandom(keyAccountClasses);
    //                ka.BuyerKeyAccountClass = class_;
    //                ka.BuyerKeyAccountClassId = class_.Id;
    //            }
    //            else
    //            {
    //                ka.BuyerKeyAccountClass = null;
    //                ka.BuyerKeyAccountClassId = null;
    //            }
    //            return ka.BuyerKeyAccountClass;
    //        })
    //        .RuleFor(ka => ka.MEVNKeyAccountClass, (f, ka) =>
    //        {
    //            if (ka.BuyerKeyAccountClass is null)
    //            {
    //                ka.MEVNKeyAccountClass = null;
    //                ka.MEVNKeyAccountClassId = null;
    //                return ka.MEVNKeyAccountClass;
    //            }

    //            if (f.Random.Bool())
    //            {
    //                var class_ = f.PickRandom(keyAccountClasses);
    //                ka.MEVNKeyAccountClass = class_;
    //                ka.MEVNKeyAccountClassId = class_.Id;
    //            }
    //            else
    //            {
    //                ka.MEVNKeyAccountClass = null;
    //                ka.MEVNKeyAccountClassId = null;
    //            }
    //            return ka.MEVNKeyAccountClass;
    //        })
    //        .RuleFor(ka => ka.CustomerLocationId, f => f.Random.Guid())
    //        .RuleFor(ka => ka.ConcurrencyStamp, f => f.Random.Guid().ToString())
    //        .RuleFor(ka => ka.CreationTime, f => f.Date.Past(1))
    //        .RuleFor(ka => ka.CreatorId, f => f.Random.Guid())
    //        .RuleFor(ka => ka.CreatorName, f => f.Name.FullName())
    //        .RuleFor(ka => ka.LastModificationTime, f => f.Date.Past(1))
    //        .RuleFor(ka => ka.LastModifierId, f => f.Random.Guid())
    //        .RuleFor(ka => ka.LastModifierName, f => f.Name.FullName());

    //    return faker.Generate(count);
    //}
    //public static List<KeyAccountUpgradeDto> GenerateKeyAccountUpgrades(
    //    int count,
    //    int seed,
    //    List<SystemCategoryDto> keyAccountTypes,
    //    List<SystemCategoryDto> keyAccountClasses,
    //    List<string> materialTypes,
    //    List<string> classSuggestions,
    //    List<BuyerDto> buyers)
    //{
    //    Randomizer.Seed = new Random(seed);

    //    var faker = new Faker<KeyAccountUpgradeDto>()
    //        .RuleFor(ka => ka.Id, f => f.Random.Guid())
    //        .RuleFor(ka => ka.KeyAccountCode, f => f.Company.CompanySuffix())
    //        .RuleFor(ka => ka.KeyAccountName, f => f.Company.CompanyName())
    //        .RuleFor(ka => ka.SalePIC, f => f.Name.FullName())

    //        .RuleFor(ka => ka.KeyAccountTypeId, (f, ka) =>
    //        {
    //            var type = f.PickRandom(keyAccountTypes);
    //            ka.KeyAccountTypeCode = type.Code;
    //            ka.KeyAccountTypeName = type.Description;
    //            return type.Id;
    //        })

    //        .RuleFor(ka => ka.KeyAccountClassCode, (f, ka) =>
    //        {
    //            var cls = f.PickRandom(keyAccountClasses);
    //            ka.KeyAccountClassName = cls.Description;
    //            return cls.Code;
    //        })

    //        .RuleFor(ka => ka.Buyer, f => f.PickRandom(buyers).Name)
    //        .RuleFor(ka => ka.MaterialType, f => f.PickRandom(materialTypes))
    //        .RuleFor(ka => ka.Revenue, f => Math.Round(f.Finance.Amount(10000, 1000000), 2))
    //        .RuleFor(ka => ka.ClassSuggestion, f => f.PickRandom(classSuggestions))
    //        .RuleFor(ka => ka.ClassificationValue, f => f.PickRandom(classSuggestions));

    //    return faker.Generate(count);
    //}

    //public static List<KeyAccountWithNavigationListDto> GenerateGeneralListWithNavigation(
    //    int count,
    //    int seed,
    //    List<SystemCategoryDto> keyAccountTypes,
    //    List<SystemCategoryDto> keyAccountClasses,
    //    List<SystemCategoryDto> evaluationFinancials,
    //    List<SystemCategoryDto> evaluationProducts,
    //    List<BuyerDto> buyers)
    //{
    //    var listDto = Generate(count, seed, keyAccountTypes, keyAccountClasses, evaluationFinancials, evaluationProducts, buyers);
    //    return GenerateGeneralListWithNavigation(listDto);
    //}
    //public static List<KeyAccountUpgradeDto> GenerateGeneralListUpgrade(List<KeyAccountUpgradeDto> listDto)
    //{
    //    var generatlList = listDto.Select(MapToListDto).ToList();
    //    return generatlList;
    //}
    //public static List<KeyAccountWithNavigationListDto> GenerateGeneralListWithNavigation(
    //    List<KeyAccountDto> listDto)
    //{
    //    var generalListWithNavigation = listDto.Select(MapToNavigationListDto).ToList();
    //    return generalListWithNavigation;
    //}

    //public static List<KeyAccountListDto> GenerateGeneralList(
    //int count,
    //int seed,
    //List<SystemCategoryDto> keyAccountTypes,
    //List<SystemCategoryDto> keyAccountClasses,
    //List<SystemCategoryDto> evaluationFinancials,
    //List<SystemCategoryDto> evaluationProducts,
    //List<BuyerDto> buyers)
    //{
    //    var generalListWithNavigation = GenerateGeneralListWithNavigation(count, seed, keyAccountTypes, keyAccountClasses, evaluationFinancials, evaluationProducts, buyers);
    //    return GenerateGeneralList(generalListWithNavigation);
    //}

    //public static List<KeyAccountListDto> GenerateGeneralList(
    //    List<KeyAccountDto> listDto)
    //{
    //    var generalListWithNavigation = GenerateGeneralListWithNavigation(listDto);
    //    var generalList = generalListWithNavigation.Select(MapToListDto).ToList();
    //    return generalList;
    //}

    //public static List<KeyAccountListDto> GenerateGeneralList(
    //    List<KeyAccountWithNavigationListDto> listDto)
    //{
    //    var generalList = listDto.Select(MapToListDto).ToList();
    //    return generalList;
    //}


}

