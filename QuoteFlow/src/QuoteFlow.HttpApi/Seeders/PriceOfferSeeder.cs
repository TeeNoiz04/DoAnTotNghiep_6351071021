namespace QuoteFlow.Seeders;
public static class PriceOfferSeeder
{
    //public static List<PriceOfferReportDetailDto> GenerateReportDetail(
    //int count,
    //int seed,
    //List<BuyerDto> buyers,
    //List<SystemCategoryDto> materialTypes)
    //{
    //    Randomizer.Seed = new Random(seed);

    //    var faker = new Faker<PriceOfferReportDetailDto>()
    //        .RuleFor(x => x.RowNo, f => f.IndexFaker + 1)
    //        .RuleFor(x => x.PriceOffer_Code, f => f.Random.AlphaNumeric(10))
    //        .RuleFor(x => x.PriceOffer_Name, f => f.Commerce.ProductName())
    //        .RuleFor(x => x.Status, f => f.PickRandom("Open", "Closed", "Pending"))
    //        .RuleFor(x => x.GolfaCode, f => f.Random.AlphaNumeric(6))
    //        .RuleFor(x => x.Model, f => f.Commerce.ProductAdjective())
    //        .RuleFor(x => x.PriceOffer_Type, f => f.PickRandom("Standard", "Special"))
    //        .RuleFor(x => x.EUTypeBusiness, f => f.Company.CompanyName())
    //        .RuleFor(x => x.Created, f => f.Date.Past(1))
    //        .RuleFor(x => x.Competitor, f => f.Company.CompanyName())
    //        .RuleFor(x => x.CloseDate, f => f.Date.Future(1))
    //        .RuleFor(x => x.PanelSI, f => f.Random.AlphaNumeric(5))
    //        .RuleFor(x => x.GP, f => f.Finance.Account())
    //        .RuleFor(x => x.EULocation, f => f.Address.City())
    //        .RuleFor(x => x.Qty, f => f.Random.Int(1, 100))
    //        .RuleFor(x => x.StandardPrice, f => f.Finance.Amount(100, 10000))
    //        .RuleFor(x => x.StandardPriceAmount, (f, x) => x.StandardPrice * x.Qty)
    //        .RuleFor(x => x.SaleOfferPrice, f => f.Finance.Amount(50, 9500))
    //        .RuleFor(x => x.SaleOfferPriceAmount, (f, x) => x.SaleOfferPrice * x.Qty)
    //        .RuleFor(x => x.DiscountRatio, (f, x) =>
    //        {
    //            if (x.StandardPrice == 0) return 0;
    //            return Math.Round(1 - (x.SaleOfferPrice / x.StandardPrice), 2);
    //        })
    //        .RuleFor(x => x.OfferMargin, f => f.Finance.Amount(5, 20))
    //        .RuleFor(x => x.PriceOffer_Location, f => f.Address.Country())
    //        .RuleFor(x => x.DPO_Qty, f => f.Random.Int(0, 100))
    //        .RuleFor(x => x.DPO_Amount, f => f.Random.Int(1000, 100000))
    //        .RuleFor(x => x.DeliveredQty, f => f.Random.Int(0, 100))
    //        .RuleFor(x => x.DeliveredAmount, f => f.Random.Int(1000, 100000))
    //        .RuleFor(x => x.Buyer, (f, x) =>
    //        {
    //            var dist = f.PickRandom(buyers);
    //            x.Buyer = dist.Name;
    //            return x.Buyer;
    //        })
    //        .RuleFor(x => x.Material_Group, (f, x) =>
    //        {
    //            var mat = f.PickRandom(materialTypes);
    //            x.Material_Group = mat.Description;
    //            return x.Material_Group;
    //        });

    //    return faker.Generate(count);
    //}

    //public static List<PriceOfferReportDetailDto> GenerateReportDetail(List<PriceOfferReportDetailDto> listDto)
    //{
    //    var generatlList = listDto.Select(MapToListDto).ToList();
    //    return generatlList;
    //}

    //public static List<PriceOfferReportGeneralDto> GenerateGeneralReportList(
    //int count,
    //int seed,
    //List<BuyerDto> buyers,
    //List<SystemCategoryDto> materialTypes)
    //{
    //    Randomizer.Seed = new Random(seed);

    //    var faker = new Faker<PriceOfferReportGeneralDto>()
    //        .RuleFor(x => x.RowNo, f => f.IndexFaker + 1)
    //        .RuleFor(x => x.PriceOffer_Code, f => f.Random.AlphaNumeric(10))
    //        .RuleFor(x => x.PriceOffer_Name, f => f.Commerce.ProductName())
    //        .RuleFor(x => x.PriceOffer_Type, f => f.PickRandom("Standard", "Special", "Urgent"))
    //        .RuleFor(x => x.EUTypeBusiness, f => f.Company.CompanyName())
    //        .RuleFor(x => x.Competitor, f => f.Company.CompanyName())
    //        .RuleFor(x => x.Created, f => f.Date.Past(1))
    //        .RuleFor(x => x.CloseDate, (f, x) => f.Date.Soon(60, x.Created ?? DateTime.Now))
    //        .RuleFor(x => x.TaxCode, f => f.Finance.Account())

    //        .RuleFor(x => x.CustomerName, f => f.Company.CompanyName())
    //        .RuleFor(x => x.TotalStandardPrice, f => f.Random.Int(10000, 100000))
    //        .RuleFor(x => x.TotalOfferPrice, (f, x) => x.TotalStandardPrice - f.Random.Int(1000, 5000))
    //        .RuleFor(x => x.TotalOrdered, f => f.Random.Int(0, 100))
    //        .RuleFor(x => x.DeliveredAmount, f => f.Random.Int(0, 100000))
    //        .RuleFor(x => x.DiscountRatio, (f, x) =>
    //        {
    //            if (x.TotalStandardPrice == 0) return 0;
    //            return Math.Round(1 - ((decimal)x.TotalOfferPrice / x.TotalStandardPrice), 2);
    //        })
    //        .RuleFor(x => x.OrderRatio, (f, x) =>
    //        {
    //            if (x.TotalOrdered == 0) return 0;
    //            return Math.Round((decimal)x.DeliveredAmount / (x.TotalOrdered * 1000), 2); // example ratio
    //        })

    //        .RuleFor(x => x.WarningDate, (f, x) => x.CloseDate?.AddDays(-7))
    //        .RuleFor(x => x.Buyer, (f, x) =>
    //         {
    //             var dist = f.PickRandom(buyers);
    //             x.Buyer = dist.Name;
    //             return x.Buyer;
    //         })
    //         .RuleFor(x => x.MaterialType, (f, x) =>
    //         {
    //             var mat = f.PickRandom(materialTypes);
    //             x.MaterialType = mat.Description;
    //             return x.MaterialType;
    //         })
    //         .RuleFor(x => x.Status, f => f.PickRandom(new[] { "IN_PROGRESS", "CLOSED", "APPROVED" }));

    //    return faker.Generate(count);
    //}

    //public static List<PriceOfferReportGeneralDto> GenerateGeneralReportList(List<PriceOfferReportGeneralDto> listDto)
    //{
    //    var generatlList = listDto.Select(MapToListDto).ToList();
    //    return generatlList;
    //}

}
