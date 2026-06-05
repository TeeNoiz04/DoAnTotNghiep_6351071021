using Bogus;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Seeders;

public class SystemCategorySeeder
{
    public List<SystemCategoryDto> Generate(int count, int? seed = null)
    {
        if (seed.HasValue)
            Randomizer.Seed = new Random(seed.Value);

        var faker = new Faker<SystemCategoryDto>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.ParentId, f => null)
            .RuleFor(c => c.Code, f => $"CAT-{f.IndexFaker + 1:000}")
            .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
            .RuleFor(c => c.Value, f => f.Random.Decimal(0, 1000))
            .RuleFor(c => c.CategoryType, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Note, f => f.Lorem.Sentence())
            .RuleFor(c => c.IsDeactive, f => f.Random.Bool(0.3f)) // 30% chance to be inactive
            .RuleFor(c => c.SortOrder, f => f.IndexFaker + 1)
            .RuleFor(c => c.ConcurrencyStamp, f => f.Random.Guid().ToString())
            .RuleFor(ka => ka.CreationTime, f => f.Date.Past(1))
            .RuleFor(ka => ka.CreatorId, f => f.Random.Guid())
            .RuleFor(ka => ka.CreatorName, f => f.Name.FullName())
            .RuleFor(ka => ka.LastModificationTime, f => f.Date.Past(1))
            .RuleFor(ka => ka.LastModifierId, f => f.Random.Guid())
            .RuleFor(ka => ka.LastModifierName, f => f.Name.FullName());

        return faker.Generate(count);
    }
}