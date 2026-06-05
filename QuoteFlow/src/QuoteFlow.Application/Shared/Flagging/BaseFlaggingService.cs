using QuoteFlow.RequesterContexts;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace QuoteFlow.Shared.Flagging;

public abstract class BaseFlaggingService<TEntity, TFlagsDto, TFlaggingContext> : IFlaggingService<TEntity, TFlagsDto>
    where TFlagsDto : IBaseFlags
    where TEntity : Entity<Guid>
    where TFlaggingContext : BaseFlaggingContext
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    protected IEffectiveUserContext CurrentUser => LazyServiceProvider.LazyGetRequiredService<IEffectiveUserContext>();
    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();
    protected IIdentityUserRepository IdentityUserRepository => LazyServiceProvider.LazyGetRequiredService<IIdentityUserRepository>();

    public virtual async Task<TFlagsDto> CreateFlagsAsync(TEntity entity)
    {
        var flags = await CreateBulkFlagsAsync([entity]);
        return flags[entity.Id];
    }

    public virtual Task<Dictionary<Guid, TFlagsDto>> CreateBulkFlagsAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return Task.FromResult(new Dictionary<Guid, TFlagsDto>());

        var context = CreateFlaggingContext();
        return CreateBulkFlagsAsync(entities, context);
    }

    protected abstract Task<Dictionary<Guid, TFlagsDto>> CreateBulkFlagsAsync(IEnumerable<TEntity> entities, TFlaggingContext context);

    protected abstract TFlaggingContext CreateFlaggingContext();

    protected virtual Task<bool> IsCurrentUserAuthorizedAsync(string policyName)
    {
        return AuthorizationService.IsGrantedAsync(policyName);
    }
}
