using System;
using System.Collections.Generic;

namespace QuoteFlow.Shared.Excels;

public static class ExcelImportContextKeys
{
    // Common keys for all DTOs
    public const string ParentEntityId = "ParentEntityId"; // Guid: Parent entity ID
    public const string ImportGuid = "ImportGuid";
    public const string FY = "FY";

    // PriceOffer-specific keys
    public static class PriceOfferCustomer
    {
        public const string CustomerId = "CustomerId"; // Guid: Customer ID
        public const string CustomerIdLookupMap = "CustomerLookupMap"; // Dictionary<string, Guid>: TaxCode to Customer ID mapping
        public const string CustomerInfoLookupMap = "CustomerInfoLookupMap"; // Dictionary<string, CustomerInfoLookup>: TaxCode to full customer info mapping
    }

    /// <summary>
    /// Holds customer info for existing customers looked up by TaxCode
    /// </summary>
    public class CustomerInfoLookup
    {
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerType { get; set; }
        public string? Address { get; set; }
        public string? Industry { get; set; }
        public string? Country { get; set; }
    }

    public static class PriceOfferDetail
    {
        public const string ImportGuid = "ImportGuid";
    }

    public static class PriceOffer
    {
        public const string MaterialType = "MaterialType";
        public const string LocationId = "LocationId";
        public const string KeyAccountClassId = "KeyAccountClassId";
        public const string KeyAccountId = "KeyAccountId";
        public const string GetPriceAuto = "GetPriceAuto";
        public const string BuyerId = "BuyerId";
        public const string BuyerTypeId = "BuyerTypeId";
        public const string CurrentUserName = "CurrentUserName";
    }

    public static class Invoice
    {
        public const string InvoiceNo = "InvoiceNo";
        public const string StockDate = "StockDate";
        public const string InvoiceDate = "InvoiceDate"; // DateTime: Invoice date
        public const string ETD = "ETD";
        public const string ETA = "ETA";
        public const string ShipmentMethod = "ShipmentMethod";
        public const string DeliveryTerm = "DeliveryTerm";
        public const string SupplierCode = "SupplierCode";
    }

    public static class SpecialInputPrice
    {
        public const string AccountNo = "AccountNo";
        public const string AccountName = "AccountName";
    }

    public static class DPO
    {
        public const string MaterialType = "MaterialType";
        public const string BuyerId = "BuyerId";
        public const string SPOCodeAccountNoMap = "SPOCodeAccountNoMap";
    }

    public static class GKR
    {
        public const string MaterialType = "MaterialType";
        public const string BuyerId = "BuyerId_GKR";
        public const string SPOCodeAccountNoMap = "SPOCodeAccountNoMap";
    }

    public static class Cargo
    {
        public const string SupplierCode = "SuplierCode";
        public const string MaterialType = "CargoMaterialType";
    }

    public static class StockTracing
    {
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
    }

    public static class GIC
    {
        public const string MaterialType = "MaterialType";
        public const string GolfaLandedCostMap = "GolfaLandedCostMap"; // Dictionary<string, decimal?>: GolfaCode to LandedCost mapping
        public const string GolfaStandardPriceMap = "GolfaStandardPriceMap";
        public const string GICProcessCode = "GICProcessCode";
    }

    public static class Asset
    {
        public const string IsUpdate = "IsUpdate";
        public const string AssetRequestId = "RequestId";
    }


    public static readonly Dictionary<string, Type> KeyTypeMap = new()
    {
        { ParentEntityId, typeof(Guid) },
        { FY, typeof(int) },
        { PriceOfferCustomer.CustomerId, typeof(Guid) },
        { PriceOfferCustomer.CustomerIdLookupMap, typeof(Dictionary<string, Guid>) },
        { PriceOfferDetail.ImportGuid, typeof(Guid) },
        { PriceOffer.MaterialType, typeof(string) },
        { PriceOffer.KeyAccountClassId, typeof(Guid) },
        { PriceOffer.KeyAccountId, typeof(Guid) },
        { PriceOffer.GetPriceAuto, typeof(bool) },
        { PriceOffer.BuyerTypeId, typeof(Guid) },
        { PriceOffer.CurrentUserName, typeof(string) },
        { Invoice.InvoiceNo, typeof(string) },
        { Invoice.StockDate, typeof(DateTime) },
        { Invoice.InvoiceDate, typeof(DateTime) },
        { Invoice.ETD, typeof(DateTime) },
        { Invoice.ETA, typeof(DateTime) },
        { StockTracing.FromDate, typeof(DateTime) },
        { StockTracing.ToDate, typeof(DateTime) },
        { Invoice.ShipmentMethod, typeof(string) },
        { SpecialInputPrice.AccountNo, typeof(string)},
        { SpecialInputPrice.AccountName, typeof(string)},
        { Cargo.SupplierCode, typeof(string)},
        { Cargo.MaterialType, typeof(string)},
        { DPO.BuyerId, typeof(Guid) },
        { DPO.SPOCodeAccountNoMap, typeof(Dictionary<string, string?>) },
        { Invoice.DeliveryTerm, typeof(string) },
        { Invoice.SupplierCode, typeof(string) },
        { PriceOffer.LocationId, typeof(Guid) },
        { GIC.GolfaLandedCostMap, typeof(Dictionary<string, decimal?>) },
        { GIC.GolfaStandardPriceMap, typeof(Dictionary<string, decimal>) },
        { GIC.GICProcessCode, typeof(string) },
        { GKR.BuyerId, typeof(Guid?) },
        { PriceOfferCustomer.CustomerInfoLookupMap, typeof(Dictionary<string, CustomerInfoLookup>) },
        { Asset.IsUpdate, typeof(bool) },
        { Asset.AssetRequestId, typeof(Guid)   }
    };

    public static void ValidateKeyAndType<T>(string key, T valueToValidate)
    {
        if (!KeyTypeMap.TryGetValue(key, out Type? keyType))
        {
            throw new ArgumentException($"Unknown AdditionalData key: {key}");
        }

        if (valueToValidate != null && !keyType.IsAssignableFrom(valueToValidate.GetType()))
        {
            throw new ArgumentException($"Invalid type for key {key}. Expected {keyType.Name}, got {valueToValidate.GetType().Name}");
        }
    }

    public static bool IsExistingKey(string key)
    {
        return KeyTypeMap.ContainsKey(key);
    }
}
