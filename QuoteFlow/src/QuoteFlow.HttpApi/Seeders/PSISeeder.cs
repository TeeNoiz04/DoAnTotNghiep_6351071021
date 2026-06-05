using Bogus;
using QuoteFlow.PSIs;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Seeders;

public class PSISeeder
{
    public List<PSIDto> Generate(int count, int? seed = null)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value);

        var faker = new Faker<PSIDto>()
             .RuleFor(p => p.Id, f => f.Random.Guid())
             .RuleFor(p => p.PSICode, f => $"PSI-{f.IndexFaker + 1:000}")
             .RuleFor(p => p.FY, f => f.Date.Past(10).Year)
             .RuleFor(p => p.FileName, f => f.System.FileName())
             .RuleFor(p => p.Note, f => f.Lorem.Sentence())
             .RuleFor(p => p.Status, f => f.PickRandom("New", "InProgress", "Completed", "Rejected"))
             .RuleFor(p => p.CurrentApprovalRouteInstanceId, f => f.Random.Bool(0.7f) ? f.Random.Guid() : null)
             .RuleFor(p => p.CurrentApprovalStepSequence, f => f.Random.Bool(0.7f) ? f.Random.AlphaNumeric(6) : null)
             .RuleFor(p => p.CurrentApproverRoleName, f => f.Random.Bool(0.7f) ? f.Name.JobTitle() : null)
             .RuleFor(p => p.ConcurrencyStamp, f => f.Random.Guid().ToString())

             // ExtendedAuditedEntityDto properties
             .RuleFor(p => p.CreationTime, f => f.Date.Past(2))
             .RuleFor(p => p.CreatorId, f => f.Random.Guid())
             .RuleFor(p => p.CreatorName, f => f.Name.FullName())
             .RuleFor(p => p.LastModificationTime, f => f.Date.Recent(30))
             .RuleFor(p => p.LastModifierId, f => f.Random.Guid())
             .RuleFor(p => p.LastModifierName, f => f.Name.FullName());

        return faker.Generate(count);
    }
    public static List<PSIReportDto> GeneratePSIReports(int count, int? seed = 0)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value);

        var faker = new Faker<PSIReportDto>()
            .RuleFor(p => p.PlanApril, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanMay, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanJune, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanJuly, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanAugust, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanSeptember, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanOctober, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanNovember, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanDecember, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanJan, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanFeb, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.PlanMarch, f => f.Random.Decimal(1000, 5000))
            .RuleFor(p => p.ActualApril, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualMay, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualJune, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualJuly, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualAugust, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualSeptember, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualOctober, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualNovember, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualDecember, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualJan, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualFeb, f => f.Random.Int(800, 6000))
            .RuleFor(p => p.ActualMarch, f => f.Random.Int(800, 6000));
        return faker.Generate(count);
    }
}
