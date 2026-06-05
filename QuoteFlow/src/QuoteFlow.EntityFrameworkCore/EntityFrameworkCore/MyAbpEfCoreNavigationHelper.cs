using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.ChangeTrackers;
using Volo.Abp.OpenIddict.Tokens;
namespace QuoteFlow.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpEfCoreNavigationHelper))]
public class MyAbpEfCoreNavigationHelper : AbpEfCoreNavigationHelper
{
    public override void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        if (e.Entry.Entity.GetType() == typeof(OpenIddictToken))
        {
            return;
        }

        base.ChangeTracker_Tracked(sender, e);
    }

    public override void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity.GetType() == typeof(OpenIddictToken))
        {
            return;
        }

        base.ChangeTracker_StateChanged(sender, e);
    }
}

