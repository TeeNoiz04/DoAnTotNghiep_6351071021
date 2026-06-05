using Bogus;
using QuoteFlow.KeyAccountEvaluations;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Seeders;

public class KeyAccountEvaluationSeeder
{
    public List<KeyAccountEvaluationDto> Generate(int count, int? seed = null)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value); // Deterministic

        var evaluationTypes = new[]
        {
            "KeyAccount_Evaluation_Financial",
            "KeyAccount_Evaluation_Product"
        };

        var faker = new Faker<KeyAccountEvaluationDto>()
            .RuleFor(e => e.Id, f => f.Random.Guid())
            .RuleFor(e => e.KeyAccountId, f => f.Random.Guid())
            .RuleFor(e => e.EvaluationId, f => f.Random.Guid())
            .RuleFor(e => e.EvaluationType, f => f.PickRandom(evaluationTypes))
            .RuleFor(e => e.BuyerInfo1, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Financial" ? f.Company.CompanyName() : null)
            .RuleFor(e => e.BuyerInfo2, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Financial" ? f.Company.CompanySuffix() : null)
            .RuleFor(e => e.MEVNInfo1, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Product" || e.EvaluationType == "KeyAccount_Evaluation_Financial"
                    ? f.Commerce.ProductName() : null)
            .RuleFor(e => e.MEVNInfo2, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Product" ? f.Commerce.ProductAdjective() : null)
            .RuleFor(e => e.CompetitorInfo1, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Product" ? f.Company.CompanyName() : null)
            .RuleFor(e => e.CompetitorInfo2, (f, e) =>
                e.EvaluationType == "KeyAccount_Evaluation_Product" ? f.Company.CompanyName() : null)
            .RuleFor(e => e.Note, f => f.Lorem.Sentence())
            .RuleFor(e => e.ConcurrencyStamp, f => f.Random.Guid().ToString())
            .RuleFor(e => e.CreationTime, f => f.Date.Past(1))
            .RuleFor(e => e.CreatorId, f => f.Random.Guid())
            .RuleFor(e => e.CreatorName, f => f.Name.FullName())
            .RuleFor(e => e.LastModificationTime, f => f.Date.Recent(30))
            .RuleFor(e => e.LastModifierId, f => f.Random.Guid())
            .RuleFor(e => e.LastModifierName, f => f.Name.FullName());

        return faker.Generate(count);
    }
}