using JetBrains.Annotations;
using QuoteFlow.Buyers;
using QuoteFlow.SalesAssignments.ParameterObjects;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemCategories;
using System;

namespace QuoteFlow.SalesAssignments;

public class SalesAssignment : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public string SaleUserName { get; set; }

    [CanBeNull]
    public string? SaleFullName { get; set; }

    [NotNull]
    public string MaterialType { get; set; }

    [NotNull]
    public Guid LocationId { get; set; }

    [NotNull]
    public Guid BuyerId { get; set; }

    [CanBeNull]
    public string? BuyerShortName { get; set; }

    [NotNull]
    public Guid BuyerTypeId { get; set; }

    [CanBeNull]
    public string? Note { get; set; }

    #region Navigation Properties

    public Buyer? Buyer { get; set; }
    public SystemCategory? BuyerType { get; set; }
    public SystemCategory? Location { get; set; }
    #endregion

    protected SalesAssignment()
    {
    }
    public SalesAssignment(Guid id, SalesAssignmentCreateParams createParams)
    {
        Id = id;
        BuyerId = createParams.BuyerId;
        BuyerTypeId = createParams.BuyerTypeId;
        LocationId = createParams.LocationId;
        SaleUserName = createParams.SaleUserName;
        SaleFullName = createParams.SaleFullName;
        MaterialType = createParams.MaterialType;
        BuyerShortName = createParams.BuyerShortName;
        Note = createParams.Note;
    }
    public SalesAssignment(Guid id, SalesAssignmentUpdateParams paramsCreate, string materialType, string saleFullName)
    {
        Id = id;
        BuyerId = paramsCreate.BuyerId;
        BuyerTypeId = paramsCreate.BuyerTypeId;
        LocationId = paramsCreate.LocationId;
        SaleUserName = paramsCreate.SaleUserName;
        SaleFullName = saleFullName;
        MaterialType = materialType;
        BuyerShortName = paramsCreate.BuyerShortName;
        Note = paramsCreate.Note;
    }
}

