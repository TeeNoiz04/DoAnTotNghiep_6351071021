using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public class MaterialApprovalRequestDetailManager : DomainService
{
    protected IMaterialApprovalRequestDetailRepository _materialApprovalRequestDetailRepository;
    protected IMaterialRepository _materialRepository;

    public MaterialApprovalRequestDetailManager(IMaterialApprovalRequestDetailRepository materialApprovalRequestDetailRepository, IMaterialRepository materialRepository)
    {
        _materialApprovalRequestDetailRepository = materialApprovalRequestDetailRepository;
        _materialRepository = materialRepository;
    }

    public virtual async Task<MaterialApprovalRequestDetail> CreateAsync(Guid materialId,
    MaterialApprovalRequestDetailCreateParams createParams)
    {


        var materialApprovalRequestDetail = new MaterialApprovalRequestDetail(
         GuidGenerator.Create(),
         createParams
         );

        return await _materialApprovalRequestDetailRepository.InsertAsync(materialApprovalRequestDetail);
    }

    public virtual async Task CreateBatchInProgressAsync(IEnumerable<MaterialApprovalRequestDetailCreateParams> createParamsList)
    {
        var materialDetails = new List<MaterialApprovalRequestDetail>();
        foreach (var createParams in createParamsList)
        {
            var materialDetail = new MaterialApprovalRequestDetail(GuidGenerator.Create(), createParams);
            //materialDetail.MaterialStatus = QuoteFlowStatuses.Approved;
            materialDetails.Add(materialDetail);
        }

        await _materialApprovalRequestDetailRepository.InsertManyAsync(materialDetails, autoSave: true);
    }
    public virtual async Task CreateBatchAsync(IEnumerable<MaterialApprovalRequestDetailCreateParams> createParamsList)
    {
        var materialDetails = new List<MaterialApprovalRequestDetail>();
        foreach (var createParams in createParamsList)
        {
            var materialDetail = new MaterialApprovalRequestDetail(GuidGenerator.Create(), createParams);
            //materialDetail.MaterialStatus = QuoteFlowStatuses.Approved;
            materialDetails.Add(materialDetail);
        }

        await _materialApprovalRequestDetailRepository.InsertManyAsync(materialDetails, autoSave: true);
    }
    public virtual async Task<MaterialApprovalRequestDetail> UpdateAsync(
Guid id,
MaterialApprovalRequestDetailUpdateParams updateParams
)
    {
        var materialApprovalRequestDetail = await _materialApprovalRequestDetailRepository.GetAsync(id);

        materialApprovalRequestDetail.MaterialApprovalId = updateParams.MaterialApprovalId;
        materialApprovalRequestDetail.GolfaCode = updateParams.GolfaCode;
        materialApprovalRequestDetail.Model = updateParams.Model;
        materialApprovalRequestDetail.EPA = updateParams.EPA;
        materialApprovalRequestDetail.Standard_Price = updateParams.Standard_Price;
        materialApprovalRequestDetail.MaterialStatus = updateParams.MaterialStatus;
        materialApprovalRequestDetail.ValidFrom = updateParams.ValidFrom;
        materialApprovalRequestDetail.ValidTo = updateParams.ValidTo;
        materialApprovalRequestDetail.SAP_Code = updateParams.SAP_Code;
        materialApprovalRequestDetail.Spec1 = updateParams.Spec1;
        materialApprovalRequestDetail.Spec2 = updateParams.Spec2;
        materialApprovalRequestDetail.Spec3 = updateParams.Spec3;
        materialApprovalRequestDetail.Spec4 = updateParams.Spec4;
        materialApprovalRequestDetail.Description_EN = updateParams.Description_EN;
        materialApprovalRequestDetail.Description_VN = updateParams.Description_VN;
        materialApprovalRequestDetail.MaterialType = updateParams.MaterialType;
        materialApprovalRequestDetail.Unit = updateParams.Unit;
        materialApprovalRequestDetail.Material_SEC_Classification = updateParams.Material_SEC_Classification;
        materialApprovalRequestDetail.Material_Group = updateParams.Material_Group;
        materialApprovalRequestDetail.SAPMatGroup = updateParams.SAPMatGroup;
        materialApprovalRequestDetail.Product_Hierarchy = updateParams.Product_Hierarchy;
        materialApprovalRequestDetail.ProductHierarchyDescription = updateParams.ProductHierarchyDescription;
        materialApprovalRequestDetail.CountryOfOrigin = updateParams.CountryOfOrigin;
        materialApprovalRequestDetail.ReferenceLeadTime = updateParams.ReferenceLeadTime;
        materialApprovalRequestDetail.WarrantyTime = updateParams.WarrantyTime;
        materialApprovalRequestDetail.InventoryCategory = updateParams.InventoryCategory;
        materialApprovalRequestDetail.Maxlot = updateParams.Maxlot;
        materialApprovalRequestDetail.StockWarning = updateParams.StockWarning;
        materialApprovalRequestDetail.VAT = updateParams.VAT;
        materialApprovalRequestDetail.HS_Code = updateParams.HS_Code;
        materialApprovalRequestDetail.SupplierBUId = updateParams.SupplierBUId;
        materialApprovalRequestDetail.SupplierBUCode = updateParams.SupplierBUCode;
        materialApprovalRequestDetail.Factory_Text = updateParams.Factory_Text;
        materialApprovalRequestDetail.Input_Price = updateParams.Input_Price;
        materialApprovalRequestDetail.InputCurrency = updateParams.InputCurrency;
        materialApprovalRequestDetail.INCOTERMS = updateParams.INCOTERMS;
        materialApprovalRequestDetail.ImportDuty = updateParams.ImportDuty;
        materialApprovalRequestDetail.AppliedExchangeRate = updateParams.AppliedExchangeRate;
        materialApprovalRequestDetail.LandedCost = updateParams.LandedCost;
        materialApprovalRequestDetail.MaxSalesOfferPrice = updateParams.MaxSalesOfferPrice;
        materialApprovalRequestDetail.MaxMangerOfferPrice = updateParams.MaxMangerOfferPrice;
        materialApprovalRequestDetail.SellingPrice1 = updateParams.SellingPrice1;
        materialApprovalRequestDetail.SellingPrice2 = updateParams.SellingPrice2;
        materialApprovalRequestDetail.SellingPrice3 = updateParams.SellingPrice3;
        materialApprovalRequestDetail.SellingPrice4 = updateParams.SellingPrice4;
        materialApprovalRequestDetail.SellingPrice5 = updateParams.SellingPrice5;
        //materialApprovalRequestDetail.DestinationDate = updateParams.DestinationDate;
        materialApprovalRequestDetail.RegistrationDate = updateParams.RegistrationDate;
        //materialApprovalRequestDetail.IndeactiveDate = updateParams.IndeactiveDate;
        //materialApprovalRequestDetail.Description_Group = updateParams.Description_Group;
        //materialApprovalRequestDetail.Origin = updateParams.Origin;
        //materialApprovalRequestDetail.Kind = updateParams.Kind;
        //materialApprovalRequestDetail.Factory = updateParams.Factory;
        //materialApprovalRequestDetail.Vendor = updateParams.Vendor;
        //materialApprovalRequestDetail.LeadTime = updateParams.LeadTime;
        //materialApprovalRequestDetail.RefExchangeRate = updateParams.RefExchangeRate;
        materialApprovalRequestDetail.Note = updateParams.Note;
        materialApprovalRequestDetail.Source = updateParams.Source;
        materialApprovalRequestDetail.MaterialClass = updateParams.MaterialClass;
        materialApprovalRequestDetail.CargoNote = updateParams.CargoNote;
        materialApprovalRequestDetail.Weight = updateParams.Weight;
        materialApprovalRequestDetail.Size = updateParams.Size;
        materialApprovalRequestDetail.QRCode = updateParams.QRCode;


        materialApprovalRequestDetail.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _materialApprovalRequestDetailRepository.UpdateAsync(materialApprovalRequestDetail);
    }

    public async Task UpdateStatus(Guid materialApprovalId, string action)
    {
        var materialDetail = await _materialApprovalRequestDetailRepository.GetListAsync(x => x.MaterialApprovalId == materialApprovalId);
        foreach (var material in materialDetail)
        {
            material.MaterialStatus = action;
        }
        await _materialApprovalRequestDetailRepository.UpdateManyAsync(materialDetail);
    }

}