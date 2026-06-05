using QuoteFlow.Shared.Consts;
using QuoteFlow.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace QuoteFlow.Extensions;

public static class ConfigureExtensions
{
    // this method is used to configure
    public static void ConfigureCustomExtendedAuditing(this EntityTypeBuilder b)
    {
        b.TryConfigureExtendedAudited();
        b.TryConfigureExtendedFullAudited();
    }

    public static void ConfigureObjectHasApprovalRoute<TEntity>(this EntityTypeBuilder<TEntity> b)
    where TEntity : class
    {
        if (typeof(IHasApprovalRoute).IsAssignableFrom(typeof(TEntity)))
        {
            b.Property<int?>(nameof(IHasApprovalRoute.CurrentApprovalStepSequence)).HasColumnName(nameof(IHasApprovalRoute.CurrentApprovalStepSequence));
            b.Property<string?>(nameof(IHasApprovalRoute.CurrentApproverRoleName)).HasColumnName(nameof(IHasApprovalRoute.CurrentApproverRoleName)).HasMaxLength(ObjectHasApprovalRouteConsts.CurrentApproverRoleNameMaxLength);
            b.Property<string?>(nameof(IHasApprovalRoute.CurrentApproverRoleCode)).HasColumnName(nameof(IHasApprovalRoute.CurrentApproverRoleCode)).HasMaxLength(ObjectHasApprovalRouteConsts.CurrentApproverRoleCodeMaxLength);
            b.Property<Guid?>(nameof(IHasApprovalRoute.CurrentApprovalRouteInstanceId)).HasColumnName(nameof(IHasApprovalRoute.CurrentApprovalRouteInstanceId));
        }

        if (typeof(IHasStatus).IsAssignableFrom(typeof(TEntity)))
        {
            b.Property<string?>(nameof(IHasStatus.Status)).HasColumnName(nameof(IHasStatus.Status)).HasMaxLength(ObjectHasStatusConsts.StatusMaxLength);
        }

        if (typeof(IApprovalRouteAuditedObject).IsAssignableFrom(typeof(TEntity)))
        {
            b.Property<Guid?>(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorId)).HasColumnName(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorId));
            b.Property<string?>(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorUsername)).HasColumnName(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorUsername)).HasMaxLength(ApprovalRouteAuditedObjectConsts.MaxLastApprovalRouteCreatorUsernameLength);
            b.Property<string?>(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorName)).HasColumnName(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreatorName)).HasMaxLength(ApprovalRouteAuditedObjectConsts.MaxLastApprovalRouteCreatorNameLength);
            b.Property<DateTime?>(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreationTime)).HasColumnName(nameof(IApprovalRouteAuditedObject.LastApprovalRouteCreationTime));
        }
    }

    private static void TryConfigureExtendedAudited(this EntityTypeBuilder b)
    {
        b.Property<Guid?>(nameof(IExtendedAuditedObject.CreatorId)).HasColumnName(nameof(IExtendedAuditedObject.CreatorId));
        b.Property<string?>(nameof(IExtendedAuditedObject.CreatorUsername)).HasColumnName(nameof(IExtendedAuditedObject.CreatorUsername)).HasMaxLength(ExtendedAuditedObjectConsts.MaxCreatorUsernameLength);
        b.Property<string?>(nameof(IExtendedAuditedObject.CreatorName)).HasColumnName(nameof(IExtendedAuditedObject.CreatorName)).HasMaxLength(ExtendedAuditedObjectConsts.MaxCreatorNameLength);
        b.Property<DateTime?>(nameof(IExtendedAuditedObject.CreationTime)).HasColumnName(nameof(IExtendedAuditedObject.CreationTime));

        b.Property<Guid?>(nameof(IExtendedAuditedObject.LastModifierId)).HasColumnName(nameof(IExtendedAuditedObject.LastModifierId));
        b.Property<string?>(nameof(IExtendedAuditedObject.LastModifierUsername)).HasMaxLength(ExtendedAuditedObjectConsts.MaxLastModifierUsernameLength).HasColumnName(nameof(IExtendedAuditedObject.LastModifierUsername));
        b.Property<string?>(nameof(IExtendedAuditedObject.LastModifierName)).HasMaxLength(ExtendedAuditedObjectConsts.MaxLastModifierNameLength).HasColumnName(nameof(IExtendedAuditedObject.LastModifierName));
        b.Property<DateTime?>(nameof(IExtendedAuditedObject.LastModificationTime)).HasColumnName(nameof(IExtendedAuditedObject.LastModificationTime));
    }

    private static void TryConfigureExtendedFullAudited(this EntityTypeBuilder b)
    {
        var entityType = b.Metadata;
        if (entityType.ClrType.IsAssignableTo<ISoftDelete>())
        {
            // b.HasQueryFilter to filter out the deleted entities
            var filterExpression = ConvertFilterExpression<ISoftDelete>(e => !e.IsDeleted, entityType.ClrType);
            b.HasQueryFilter(filterExpression);

            b.Property<Guid?>(nameof(ISoftDelete.DeleterId)).HasColumnName(nameof(ISoftDelete.DeleterId));
            b.Property<string?>(nameof(ISoftDelete.DeleterUsername)).HasMaxLength(ExtendedFullAuditedObjectConsts.MaxDeleterUsernameLength).HasColumnName(nameof(ISoftDelete.DeleterUsername));
            b.Property<string?>(nameof(ISoftDelete.DeleterName)).HasMaxLength(ExtendedFullAuditedObjectConsts.MaxDeleterNameLength).HasColumnName(nameof(ISoftDelete.DeleterName));
            b.Property<DateTime?>(nameof(ISoftDelete.DeletionTime)).HasColumnName(nameof(ISoftDelete.DeletionTime));
            b.Property<bool>(nameof(ISoftDelete.IsDeleted)).HasColumnName(nameof(ISoftDelete.IsDeleted)).IsRequired();
            b.Ignore(nameof(ISoftDelete.ForceDelete));
        }
    }

    private static LambdaExpression ConvertFilterExpression<TInterface>(
                            Expression<Func<TInterface, bool>> filterExpression,
                            Type entityType)
    {
        var newParam = Expression.Parameter(entityType);
        var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

        return Expression.Lambda(newBody, newParam);
    }
}