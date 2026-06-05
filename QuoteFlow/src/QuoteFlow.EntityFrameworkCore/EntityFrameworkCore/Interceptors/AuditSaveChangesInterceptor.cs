using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace QuoteFlow.EntityFrameworkCore.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IRequesterContext _requesterContext;

    public AuditSaveChangesInterceptor(ICurrentUser currentUser, IRequesterContext requesterContext)
    {
        _currentUser = currentUser;
        _requesterContext = requesterContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries();

        Guid currentUserId = _requesterContext.Id ?? _currentUser.Id ?? Guid.Empty;
        string currentUserUsername = _requesterContext.Username ?? _currentUser.UserName ?? "N/A";
        string currentUserFullName = _requesterContext.FullName ?? UserHelper.GetFullName(_currentUser.Name, _currentUser.SurName);

        foreach (var entry in entries)
        {
            // Common audit for creation and modification
            if (entry.Entity is IExtendedAuditedObject auditedEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    auditedEntity.CreatorId = currentUserId;
                    auditedEntity.CreatorUsername = currentUserUsername;
                    auditedEntity.CreatorName = currentUserFullName;
                    auditedEntity.CreationTime = DateTime.Now;

                    auditedEntity.LastModifierId = currentUserId;
                    auditedEntity.LastModifierUsername = currentUserUsername;
                    auditedEntity.LastModifierName = currentUserFullName;
                    auditedEntity.LastModificationTime = auditedEntity.CreationTime;
                }

                if (entry.State == EntityState.Modified)
                {
                    auditedEntity.LastModifierId = currentUserId;
                    auditedEntity.LastModifierUsername = currentUserUsername;
                    auditedEntity.LastModifierName = currentUserFullName;
                    auditedEntity.LastModificationTime = DateTime.Now;

                }
            }

            // Additional audit for deletion
            if (entry.Entity is ISoftDelete softDeleteEntity
                && entry.State == EntityState.Deleted
                && !softDeleteEntity.ForceDelete)
            {
                softDeleteEntity.DeleterId = currentUserId;
                softDeleteEntity.DeleterUsername = currentUserUsername;
                softDeleteEntity.DeleterName = currentUserFullName;
                softDeleteEntity.DeletionTime = DateTime.Now;
                softDeleteEntity.IsDeleted = true;

                // Optionally set state to Modified to mark as soft-deleted
                entry.State = EntityState.Modified;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

