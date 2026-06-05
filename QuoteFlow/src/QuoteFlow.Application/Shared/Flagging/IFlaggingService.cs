using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared.Flagging;

/// <summary>
/// Defines a service for managing flags associated with entities, supporting both single and bulk operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to which flags are applied.</typeparam>
/// <typeparam name="TFlags">The type representing the flags associated with an entity.</typeparam>
/// <typeparam name="TFlaggingContext">The type representing the context required for flagging operations.</typeparam>
public interface IFlaggingService<TEntity, TFlags> : IScopedDependency
    where TFlags : IBaseFlags
    where TEntity : Entity<Guid>
{
    Task<TFlags> CreateFlagsAsync(TEntity entity);
    Task<Dictionary<Guid, TFlags>> CreateBulkFlagsAsync(IEnumerable<TEntity> entities);
}