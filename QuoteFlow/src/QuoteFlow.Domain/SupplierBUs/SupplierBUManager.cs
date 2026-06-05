using QuoteFlow.SupplierBUs.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SupplierBUs;

public class SupplierBUManager : DomainService
{
    protected ISupplierBURepository _supplierBURepository;

    public SupplierBUManager(ISupplierBURepository supplierBURepository)
    {
        _supplierBURepository = supplierBURepository;
    }

    public virtual async Task<SupplierBU> CreateAsync(
        SupplierBUCreateParams createParams)
    {

        var supplierBU = new SupplierBU(
         GuidGenerator.Create(),
         createParams
         );

        return await _supplierBURepository.InsertAsync(supplierBU);
    }

    public virtual async Task<List<SupplierBU>> CreateManyAsync(
        List<SupplierBUCreateParams> createParams)
    {
        var result = new List<SupplierBU>();
        foreach (var create in createParams)
        {
            var item = new SupplierBU(new Guid(), create);
            result.Add(item);
        }
        await _supplierBURepository.InsertManyAsync(result);
        return result;
    }

    public virtual async Task<List<SupplierBU>> UpdateManyAsync(
    List<SupplierBUImportUpdateParams> updateParamList)
    {
        var result = new List<SupplierBU>();

        var ids = updateParamList.Select(x => x.Id).ToList();

        var supplierBUs = await _supplierBURepository.GetListAsync(x => ids.Contains(x.Id));

        var supplierBUMap = supplierBUs.ToDictionary(x => x.Id, x => x);

        foreach (var updateParams in updateParamList)
        {
            if (supplierBUMap.TryGetValue(updateParams.Id, out var supplierBU))
            {
                supplierBU.SupplierBUCode = updateParams.SupplierBUCode ?? supplierBU.SupplierBUCode;
                supplierBU.SortOrder = supplierBU.SortOrder;
                supplierBU.SupplierBURemarks = updateParams.SupplierBURemarks ?? supplierBU.SupplierBURemarks;
                supplierBU.OrderMethod = updateParams.OrderMethod ?? supplierBU.OrderMethod;
                supplierBU.POTemplate = updateParams.POTemplate ?? supplierBU.POTemplate;
                supplierBU.Contact = updateParams.Contact ?? supplierBU.Contact;
                supplierBU.Email = updateParams.Email ?? supplierBU.Email;
                supplierBU.INCOTerm = updateParams.INCOTerm ?? supplierBU.INCOTerm;
                supplierBU.PaymentTermCode = updateParams.PaymentTermCode ?? supplierBU.PaymentTermCode;
                supplierBU.PaymentDescription = updateParams.PaymentDescription ?? supplierBU.PaymentDescription;
                supplierBU.Currency = updateParams.Currency ?? supplierBU.Currency;
                supplierBU.MaterialType = updateParams.MaterialType ?? supplierBU.MaterialType;
                supplierBU.SupplierId = updateParams.SupplierId ?? supplierBU.SupplierId;
                supplierBU.SupplierCode = updateParams.SupplierCode ?? supplierBU.SupplierCode;
                supplierBU.SupplierShortName = updateParams.SupplierShortName ?? supplierBU.SupplierShortName;
                supplierBU.SupplierAddress = updateParams.SupplierAddress ?? supplierBU.SupplierAddress;
                supplierBU.FASCMVendorCode = updateParams.FASCMVendorCode ?? supplierBU.FASCMVendorCode;
                supplierBU.FASCMBuyerCode = updateParams.FASCMBuyerCode ?? supplierBU.FASCMBuyerCode;
                supplierBU.FASCMConsigneeCode = updateParams.FASCMConsigneeCode ?? supplierBU.FASCMConsigneeCode;
                supplierBU.FASCMSectionCode = updateParams.FASCMSectionCode ?? supplierBU.FASCMSectionCode;
                supplierBU.FASCMPaymentTerm = updateParams.FASCMPaymentTerm ?? supplierBU.FASCMPaymentTerm;
                supplierBU.FASCMFreightMethod = updateParams.FASCMFreightMethod ?? supplierBU.FASCMFreightMethod;
                supplierBU.FASCMDeliveryTerms = updateParams.FASCMDeliveryTerms ?? supplierBU.FASCMDeliveryTerms;
                supplierBU.FASCMPlaceOfDeliveryTerms = updateParams.FASCMPlaceOfDeliveryTerms ?? supplierBU.FASCMPlaceOfDeliveryTerms;
                supplierBU.FASCMShippingMarkCode = updateParams.FASCMShippingMarkCode ?? supplierBU.FASCMShippingMarkCode;

                supplierBU.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

                result.Add(supplierBU);
            }
        }

        return result;
    }


    public virtual async Task<SupplierBU> UpdateAsync(
        Guid id,
        SupplierBUUpdateParams updateParams)
    {
        var supplierBU = await _supplierBURepository.GetAsync(id);

        supplierBU.SupplierBUCode = updateParams.SupplierBUCode;
        supplierBU.SortOrder = updateParams.SortOrder;
        supplierBU.SupplierBURemarks = updateParams.SupplierBURemarks;
        supplierBU.OrderMethod = updateParams.OrderMethod;
        supplierBU.POTemplate = updateParams.POTemplate;
        supplierBU.Contact = updateParams.Contact;
        supplierBU.Email = updateParams.Email;
        supplierBU.INCOTerm = updateParams.INCOTerm;
        supplierBU.PaymentTermCode = updateParams.PaymentTermCode;
        supplierBU.PaymentDescription = updateParams.PaymentDescription;
        supplierBU.Currency = updateParams.Currency;
        supplierBU.MaterialType = updateParams.MaterialType;
        supplierBU.SupplierId = updateParams.SupplierId;
        supplierBU.SupplierCode = updateParams.SupplierCode;
        supplierBU.SupplierShortName = updateParams.SupplierShortName;
        supplierBU.SupplierAddress = updateParams.SupplierAddress;
        supplierBU.FASCMVendorCode = updateParams.FASCMVendorCode;
        supplierBU.FASCMBuyerCode = updateParams.FASCMBuyerCode;
        supplierBU.FASCMConsigneeCode = updateParams.FASCMConsigneeCode;
        supplierBU.FASCMSectionCode = updateParams.FASCMSectionCode;
        supplierBU.FASCMPaymentTerm = updateParams.FASCMPaymentTerm;
        supplierBU.FASCMFreightMethod = updateParams.FASCMFreightMethod;
        supplierBU.FASCMDeliveryTerms = updateParams.FASCMDeliveryTerms;
        supplierBU.FASCMPlaceOfDeliveryTerms = updateParams.FASCMPlaceOfDeliveryTerms;
        supplierBU.FASCMShippingMarkCode = updateParams.FASCMShippingMarkCode;
        supplierBU.IsDeactive = updateParams.IsDeactive;

        supplierBU.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _supplierBURepository.UpdateAsync(supplierBU);
    }


}