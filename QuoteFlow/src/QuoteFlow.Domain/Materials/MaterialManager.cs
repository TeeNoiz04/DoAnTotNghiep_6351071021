using QuoteFlow.HistoryTrackings;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials;

public class MaterialManager : DomainService
{
    protected IMaterialRepository _materialRepository;
    protected IHistoryTrackingRepository _historyTrackingRepository;

    public MaterialManager(IMaterialRepository materialRepository, IHistoryTrackingRepository historyTrackingRepository)
    {
        _materialRepository = materialRepository;
        _historyTrackingRepository = historyTrackingRepository;
    }

    public virtual async Task<Material> CreateAsync(
    MaterialCreateParams createParams)
    {

        var material = new Material(
         GuidGenerator.Create(),
         createParams
         );

        return await _materialRepository.InsertAsync(material);
    }

    public virtual async Task<List<Material>> CreateListAsync(List<MaterialCreateParams> createParamsList)
    {
        var result = new List<Material>();
        foreach (var createParams in createParamsList)
        {
            var material = new Material(
                GuidGenerator.Create(),
                createParams
            );
            material.MaterialStatus = "Active";
            result.Add(await _materialRepository.InsertAsync(material));
        }

        return result;
    }

    public virtual async Task<Material> UpdateAsync(Guid id, MaterialUpdateParams p)
    {
        var m = await _materialRepository.GetAsync(id);

        m.GolfaCode = p.GolfaCode;
        m.Model = p.Model;
        m.EPA = p.EPA;
        m.Standard_Price = p.Standard_Price;
        m.MaterialStatus = p.MaterialStatus;
        m.ValidFrom = p.ValidFrom;
        m.ValidTo = p.ValidTo;
        m.SAP_Code = p.SAP_Code;
        m.Spec1 = p.Spec1;
        m.Spec2 = p.Spec2;
        m.Spec3 = p.Spec3;
        m.Spec4 = p.Spec4;
        m.Description_EN = p.Description_EN;
        m.Description_VN = p.Description_VN;
        m.MaterialType = p.MaterialType;
        m.Unit = p.Unit;
        m.Material_SEC_Classification = p.Material_SEC_Classification;
        m.Material_Group = p.Material_Group;
        m.SAPMatGroup = p.SAPMatGroup;
        m.Product_Hierarchy = p.Product_Hierarchy;
        m.ProductHierarchyDescription = p.ProductHierarchyDescription;
        m.CountryOfOrigin = p.CountryOfOrigin;
        m.ReferenceLeadTime = p.ReferenceLeadTime;
        m.WarrantyTime = p.WarrantyTime;
        m.InventoryCategory = p.InventoryCategory;
        m.Maxlot = p.Maxlot;
        m.StockWarning = p.StockWarning;
        m.VAT = p.VAT;
        m.HS_Code = p.HS_Code;
        m.SupplierBUId = p.SupplierBUId;
        m.SupplierBUCode = p.SupplierBUCode;
        m.Factory_Text = p.Factory_Text;
        m.Input_Price = p.Input_Price;
        m.InputCurrency = p.InputCurrency;
        m.INCOTERMS = p.INCOTERMS;
        m.ImportDuty = p.ImportDuty;
        m.AppliedExchangeRate = p.AppliedExchangeRate;
        m.LandedCost = p.LandedCost;
        m.MaxSalesOfferPrice = p.MaxSalesOfferPrice;
        m.MaxMangerOfferPrice = p.MaxMangerOfferPrice;
        m.SellingPrice1 = p.SellingPrice1;
        m.SellingPrice2 = p.SellingPrice2;
        m.SellingPrice3 = p.SellingPrice3;
        m.SellingPrice4 = p.SellingPrice4;
        m.SellingPrice5 = p.SellingPrice5;
        m.RegistrationDate = p.RegistrationDate;
        m.Note = p.Note;
        m.MaterialClass = p.MaterialClass;
        m.CargoNote = p.CargoNote;
        m.Weight = p.Weight;
        m.Size = p.Size;
        m.QRCode = p.QRCode;

        // Update concurrency stamp
        m.SetConcurrencyStampIfNotNull(p.ConcurrencyStamp);

        return await _materialRepository.UpdateAsync(m);
    }
    public virtual async Task<List<Material>> UpdateBatchAsync(
        List<ExcelMaterialUpdateParams> updateParamsList
    )
    {
        var materialData = new List<Material>();
        var materialIds = updateParamsList.Select(x => x.Id);
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        foreach (var updateParams in updateParamsList)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.Id == updateParams.Id)
                ?? throw new EntityNotFoundException(typeof(Material), updateParams.Id);

            entity.SAP_Code = updateParams.SAPCode;
            entity.Description_VN = updateParams.DescriptionVN;
            entity.Product_Hierarchy = updateParams.ProductHiearchy;
            entity.VAT = updateParams.VAT;
        }
        return existingMaterials;
    }

    public virtual async Task<List<Material>> UpdateBatchFactoryAsync(List<ExcelMaterialFactoryUpdateParams> updateParamsList)
    {
        var details = new List<Material>();

        var materialIds = updateParamsList.Select(x => x.Id);
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        foreach (var updateParams in updateParamsList)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.Id == updateParams.Id)
               ?? throw new EntityNotFoundException(typeof(Material), updateParams.Id);

            // int? fields
            entity.ReferenceLeadTime = updateParams.ReferenceLeadTime == -1
                ? null
                : updateParams.ReferenceLeadTime ?? entity.ReferenceLeadTime;

            entity.Maxlot = updateParams.Maxlot == -1
                ? null
                : updateParams.Maxlot ?? entity.Maxlot;

            // string fields
            entity.CountryOfOrigin = updateParams.CountryOfOrigin == "-1"
                ? null
                : updateParams.CountryOfOrigin ?? entity.CountryOfOrigin;

            entity.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

            details.Add(entity);
        }

        return details;
    }


    public virtual async Task CreateBatchStatusyAsync(
       List<ExcelMaterialStatusUpdateParams> updaterParams)
    {
        var details = new List<Material>();
        foreach (var update in updaterParams)
        {
            if (update.Id is not null)
            {
                //update
                var entity = await _materialRepository.GetAsync(update.Id!.Value);
                entity.MaterialStatus = "Active";
                entity.RegistrationDate = update.ActiveDate;
                //entity.Source = update.Source;
                //entity.Reason = update.Reason;
                entity.Factory_Text = update.FactoryRefDoc;
                entity.SetConcurrencyStampIfNotNull(update.ConcurrencyStamp);

                await _materialRepository.UpdateAsync(entity);
            }

        }
    }

    public virtual async Task<List<Material>> UpdateBatchFromExcelAsync(List<ExcelMaterialUpdateParams> materials)
    {
        var details = new List<Material>();
        var materialIds = materials.Select(m => m.Id).ToList();
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        foreach (var material in materials)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.Id == material.Id)
                ?? throw new EntityNotFoundException(typeof(Material), material.Id);

            // string
            entity.SAP_Code = material.SAPCode == "-1"
                ? null
                : material.SAPCode ?? entity.SAP_Code;

            entity.Description_VN = material.DescriptionVN == "-1"
                ? null
                : material.DescriptionVN ?? entity.Description_VN;

            entity.Product_Hierarchy = material.ProductHiearchy == "-1"
                ? null
                : material.ProductHiearchy ?? entity.Product_Hierarchy;

            //(decimal?)
            if (material.VAT == -1)
            {
                entity.VAT = null;
            }
            else
            {
                entity.VAT = material.VAT ?? entity.VAT;
            }

            details.Add(entity);
        }
        return details;
    }


    public virtual async Task<List<Material>> UpdateListM2UFromExcelAsync(List<MaterialApprovalRequestDetail> materials)
    {
        var historyCheckings = new List<HistoryTracking>();

        var details = new List<Material>();
        var golfaCodes = materials
            .Select(m => m.GolfaCode)
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Distinct()
            .ToList();

        var existingMaterials = await _materialRepository.GetListAsync(m => golfaCodes.Contains(m.GolfaCode));


        foreach (var material in materials)
        {
            var entity = existingMaterials
               .FirstOrDefault(em => em.GolfaCode == material.GolfaCode)
               ?? throw new BusinessException("Can not find Material");
            var historyTracking = new HistoryTracking(
                id: Guid.NewGuid(),
                trackingType: "Material",
                action: "UPDATE PRICE",
                golfaCode: entity.GolfaCode,
                objectId: material.MaterialApprovalId.ToString(),
                model: entity.Model,
                qty: null,
                previousValue: entity.Standard_Price,
                nextValue: material.Standard_Price ?? entity.Standard_Price,
                stockId: null,
                stockName: null,
                note: null
            );

            historyCheckings.Add(historyTracking);

            entity.GolfaCode = material.GolfaCode ?? entity.GolfaCode;
            entity.Model = material.Model ?? entity.Model;
            entity.Spec1 = material.Spec1 == "-1"
               ? null
               : material.Spec1 ?? entity.Spec1;
            entity.Material_Group = material.Material_Group ?? entity.Material_Group;
            entity.MaterialType = material.MaterialType ?? entity.MaterialType;
            entity.ValidFrom = material.ValidFrom ?? entity.ValidFrom;
            entity.ValidTo = material.ValidTo ?? entity.ValidTo;
            entity.Input_Price = material.Input_Price ?? entity.Input_Price;
            entity.InputCurrency = material.InputCurrency ?? entity.InputCurrency;
            entity.INCOTERMS = material.INCOTERMS ?? entity.INCOTERMS;
            entity.EPA = material.EPA ?? entity.EPA;
            if (material.ImportDuty == -1)
            {
                entity.ImportDuty = null;
            }
            else
            {
                entity.ImportDuty = material.ImportDuty ?? entity.ImportDuty;
            }
            entity.AppliedExchangeRate = material.AppliedExchangeRate ?? entity.AppliedExchangeRate;
            entity.LandedCost = material.LandedCost ?? entity.LandedCost;
            entity.MaxSalesOfferPrice = material.MaxSalesOfferPrice ?? entity.MaxSalesOfferPrice;
            entity.MaxMangerOfferPrice = material.MaxMangerOfferPrice ?? entity.MaxMangerOfferPrice;
            entity.Standard_Price = material.Standard_Price ?? entity.Standard_Price;
            entity.SellingPrice1 = material.SellingPrice1 == -1
            ? null
            : material.SellingPrice1 ?? entity.SellingPrice1;

            entity.SellingPrice2 = material.SellingPrice2 == -1
                ? null
                : material.SellingPrice2 ?? entity.SellingPrice2;

            entity.SellingPrice3 = material.SellingPrice3 == -1
                ? null
                : material.SellingPrice3 ?? entity.SellingPrice3;

            entity.SellingPrice4 = material.SellingPrice4 == -1
                ? null
                : material.SellingPrice4 ?? entity.SellingPrice4;

            entity.SellingPrice5 = material.SellingPrice5 == -1
                ? null
                : material.SellingPrice5 ?? entity.SellingPrice5;

            details.Add(entity);
        }

        await _historyTrackingRepository.InsertManyAsync(historyCheckings);

        return details;
    }

    //update list
    public virtual async Task<List<Material>> UpdateListM3UFromExcelAsync(
    List<ExcelMaterialUpdateWithoutPrriceParams> materials)
    {
        var details = new List<Material>();
        var materialIds = materials.Select(m => m.Id).ToList();
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        foreach (var updateParams in materials)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.Id == updateParams.Id)
                ?? throw new EntityNotFoundException(typeof(Material), updateParams.Id);

            // DateTime? (Allow NULL)
            if (updateParams.RegistrationDate == null)
            {
            }
            else if (updateParams.RegistrationDate == DateTime.MinValue)
            {
                entity.RegistrationDate = null;
            }
            else
            {
                entity.RegistrationDate = updateParams.RegistrationDate;
            }
            entity.Model = updateParams.Model ?? entity.Model;


            // DateTime? (Not allow NULL)
            entity.ValidFrom = updateParams.ValidFrom ?? entity.ValidFrom;
            entity.ValidTo = updateParams.ValidTo ?? entity.ValidTo;

            // String? (Allow NULL)
            entity.Spec1 = updateParams.Spec1 == "-1" ? null : updateParams.Spec1 ?? entity.Spec1;
            entity.Spec2 = updateParams.Spec2 == "-1" ? null : updateParams.Spec2 ?? entity.Spec2;
            entity.Spec3 = updateParams.Spec3 == "-1" ? null : updateParams.Spec3 ?? entity.Spec3;
            entity.Spec4 = updateParams.Spec4 == "-1" ? null : updateParams.Spec4 ?? entity.Spec4;

            // String? (Not allow NULL)
            entity.Description_EN = updateParams.Description_EN ?? entity.Description_EN;

            // String? (Allow NULL)
            entity.Description_VN = updateParams.Description_VN == "-1" ? null : updateParams.Description_VN ?? entity.Description_VN;

            // String? (Not allow NULL)
            entity.SupplierCode = updateParams.SupplierCode ?? entity.SupplierCode;
            entity.SupplierBUCode = updateParams.SupplierBUCode ?? entity.SupplierBUCode;
            entity.Factory_Text = updateParams.Factory_Text ?? entity.Factory_Text;
            entity.MaterialType = updateParams.MaterialType ?? entity.MaterialType;
            entity.Unit = updateParams.Unit ?? entity.Unit;
            entity.Material_Group = updateParams.Material_Group ?? entity.Material_Group;

            // String? (Allow NULL)
            entity.SAPMatGroup = updateParams.SAPMatGroup == "-1" ? null : updateParams.SAPMatGroup ?? entity.SAPMatGroup;
            entity.ProductHierarchyDescription = updateParams.ProductHierarchyDescription == "-1" ? null : updateParams.ProductHierarchyDescription ?? entity.ProductHierarchyDescription;
            entity.CountryOfOrigin = updateParams.CountryOfOrigin == "-1" ? null : updateParams.CountryOfOrigin ?? entity.CountryOfOrigin;

            // int? (Allow NULL)
            entity.ReferenceLeadTime = updateParams.ReferenceLeadTime == -1 ? null : updateParams.ReferenceLeadTime ?? entity.ReferenceLeadTime;

            // int? (Not allow NULL)
            entity.WarrantyTime = updateParams.WarrantyTime ?? entity.WarrantyTime;

            entity.InventoryCategory = updateParams.InventoryCategory == "-1" ? null : updateParams.InventoryCategory ?? entity.InventoryCategory;
            entity.CargoNote = updateParams.CargoNote == "-1" ? null : updateParams.CargoNote ?? entity.CargoNote;
            entity.Weight = updateParams.Weight == "-1" ? null : updateParams.Weight ?? entity.Weight;
            entity.Size = updateParams.Size == "-1" ? null : updateParams.Size ?? entity.Size;
            entity.QRCode = updateParams.QRCode == "-1" ? null : updateParams.QRCode ?? entity.QRCode;
            entity.HS_Code = updateParams.HS_Code == "-1" ? null : updateParams.HS_Code ?? entity.HS_Code;

            // int? (Allow NULL)
            entity.Maxlot = updateParams.Maxlot == -1 ? null : updateParams.Maxlot ?? entity.Maxlot;
            entity.StockWarning = updateParams.StockWarning == -1 ? null : updateParams.StockWarning ?? entity.StockWarning;

            entity.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

            details.Add(entity);
        }
        return details;
    }




    public virtual async Task<List<Material>> UpdateListM4UFromExcelAsync(List<ExcelMaterialStatusUpdateParams> materials)
    {
        var details = new List<Material>();
        var materialIds = materials.Select(m => m.Id).ToList();
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        foreach (var material in materials)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.Id == material.Id)
                ?? throw new EntityNotFoundException(typeof(Material), material.Id);

            //entity.FinalDPOAcceptanceDate = material.AcceptanceDate;
            //entity.DestinationDate = material.ActiveDate;
            //entity.MaterialStatus = material.Action;
            //entity.Source = material.Source;
            //entity.Reason = material.Reason;
            //entity.Factory_Text = material.FactoryRefDoc;

            details.Add(entity);
        }

        return details;
    }

    public virtual async Task<List<Material>> UpdateListM5UFromExcelAsync(
    List<ExcelMaterialUpdateInventoryPlanUpdateParams> materials)
    {
        var details = new List<Material>();
        var materialIds = materials.Select(m => m.GolfaCode);
        var existingMaterials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.GolfaCode));

        foreach (var updateParams in materials)
        {
            var entity = existingMaterials.FirstOrDefault(em => em.GolfaCode == updateParams.GolfaCode)
                ?? throw new EntityNotFoundException(typeof(Material), updateParams.Id);

            // string fields
            entity.InventoryCategory = updateParams.InventoryCategory == "-1"
                ? null
                : updateParams.InventoryCategory ?? entity.InventoryCategory;

            // int? fields
            entity.StockWarning = updateParams.StockWarning == -1
                ? null
                : updateParams.StockWarning ?? entity.StockWarning;

            //entity.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

            details.Add(entity);
        }

        return details;
    }



}