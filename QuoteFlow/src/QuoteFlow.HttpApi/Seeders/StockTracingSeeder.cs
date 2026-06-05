using Bogus;
using QuoteFlow.StockTracingDetails;
using QuoteFlow.StockTracings;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Seeders;

public class StockTracingSeeder
{
    public List<StockTracingDto> GenerateStockTracings(int count, int? seed = null)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value); // Ensure repeatable generation
        var faker = new Faker<StockTracingDto>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.FileName, f => f.System.FileName())
            .RuleFor(s => s.ReportType, f => f.PickRandom<ReportType>())
            .RuleFor(s => s.FromDate, f => f.Date.Past(1))
            .RuleFor(s => s.ToDate, (f, s) => s.FromDate?.AddDays(f.Random.Int(1, 30)))
            .RuleFor(s => s.Note, f => f.Lorem.Sentence())
            .RuleFor(s => s.ConcurrencyStamp, f => f.Random.Guid().ToString())
            .RuleFor(s => s.CreationTime, f => f.Date.Past(1))
            .RuleFor(s => s.CreatorId, f => f.Random.Guid())
            .RuleFor(s => s.CreatorName, f => f.Name.FullName())
            .RuleFor(s => s.LastModificationTime, f => f.Date.Recent(30))
            .RuleFor(s => s.LastModifierId, f => f.Random.Guid())
            .RuleFor(s => s.LastModifierName, f => f.Name.FullName());
        return faker.Generate(count);
    }

    public List<StockTracingDetailDto> GenerateStockTracingDetails(List<StockTracingDto> parents, int detailsPerParent, int? seed = null)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value + 1); // Offset seed for differentiation but consistency

        var details = new List<StockTracingDetailDto>();

        foreach (var parent in parents)
        {
            // Create a Faker specifically for this parent's details
            var detailFaker = new Faker<StockTracingDetailDto>()
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.ReportType, f => f.PickRandom<ReportType>())
                // RowNo will be handled separately
                .RuleFor(d => d.PackingListCode, f => f.Commerce.Ean13())
                .RuleFor(d => d.CheckListCode, f => f.Commerce.Ean8())
                .RuleFor(d => d.DateEntered, f => f.Date.Recent(30))
                .RuleFor(d => d.Stock, f => f.Commerce.Department())
                .RuleFor(d => d.BU, f => f.Company.CompanyName())
                .RuleFor(d => d.Customer, f => f.Company.CompanyName())
                .RuleFor(d => d.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(d => d.GIV, f => f.Random.AlphaNumeric(10))
                .RuleFor(d => d.Invoice, f => f.Finance.Account())
                .RuleFor(d => d.SKUCode, f => f.Commerce.Ean8())
                .RuleFor(d => d.SKUName, f => f.Commerce.ProductName())
                .RuleFor(d => d.Quality, f => f.Commerce.ProductAdjective())
                .RuleFor(d => d.Warranty, f => $"{f.Random.Int(1, 24)} months")
                .RuleFor(d => d.Unit, f => f.Commerce.ProductAdjective())
                .RuleFor(d => d.Series, f => f.Random.AlphaNumeric(8))
                .RuleFor(d => d.OriginCode, f => f.Address.CountryCode())
                .RuleFor(d => d.ProductionDate, f => f.Date.Past(2))
                .RuleFor(d => d.Location, f => f.Address.City())
                .RuleFor(d => d.GolfaCode, f => f.Random.Guid().ToString().Substring(0, 8))
                .RuleFor(d => d.ConcurrencyStamp, f => f.Random.Guid().ToString())
                .RuleFor(d => d.CreationTime, f => f.Date.Past(1))
                .RuleFor(d => d.CreatorId, f => f.Random.Guid())
                .RuleFor(d => d.CreatorName, f => f.Name.FullName())
                .RuleFor(d => d.LastModificationTime, f => f.Date.Recent(30))
                .RuleFor(d => d.LastModifierId, f => f.Random.Guid())
                .RuleFor(d => d.LastModifierName, f => f.Name.FullName());

            // Generate details for this parent with sequential RowNo
            var parentDetails = detailFaker.Generate(detailsPerParent);
            for (int i = 0; i < parentDetails.Count; i++)
            {
                parentDetails[i].StockTracingId = parent.Id;
                parentDetails[i].RowNo = i + 1; // Sequential row numbers starting from 1
            }

            details.AddRange(parentDetails);
        }

        return details;
    }
}