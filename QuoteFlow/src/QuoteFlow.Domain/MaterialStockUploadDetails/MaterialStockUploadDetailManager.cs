using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.MaterialStockUploadDetails;

public class MaterialStockUploadDetailManager : DomainService
{
    protected IMaterialStockUploadDetailRepository _materialStockUploadDetailRepository;

    public MaterialStockUploadDetailManager(IMaterialStockUploadDetailRepository materialStockUploadDetailRepository)
    {
        _materialStockUploadDetailRepository = materialStockUploadDetailRepository;
    }

    public virtual async Task<MaterialStockUploadDetail> CreateAsync(
    MaterialStockUploadDetailCreateParams createParams)
    {

        var materialStockUploadDetail = new MaterialStockUploadDetail(
         GuidGenerator.Create(),
         createParams.RequestId, createParams.MaterialCode, createParams.Model, createParams.Storage, createParams.StorageDestination, createParams.Qty, createParams.RefDoc, createParams.Remark
         );

        return await _materialStockUploadDetailRepository.InsertAsync(materialStockUploadDetail);
    }

    public virtual async Task CreateBatchAsync(IEnumerable<MaterialStockUploadDetailCreateParams> createParamsList)
    {
        var stockUploadDetails = new List<MaterialStockUploadDetail>();
        foreach (var createParams in createParamsList)
        {
            var stockUploadDetail = new MaterialStockUploadDetail(GuidGenerator.Create(), createParams);
            stockUploadDetails.Add(stockUploadDetail);
        }

        await _materialStockUploadDetailRepository.InsertManyAsync(stockUploadDetails, autoSave: true);
    }

    public virtual async Task<MaterialStockUploadDetail> UpdateAsync(
         Guid id,
         MaterialStockUploadDetailUpdateParams updateParams
     )
    {
        var materialStockUploadDetail = await _materialStockUploadDetailRepository.GetAsync(id);

        materialStockUploadDetail.RequestId = updateParams.RequestId;
        materialStockUploadDetail.MaterialCode = updateParams.MaterialCode;
        materialStockUploadDetail.Model = updateParams.Model;
        materialStockUploadDetail.Storage = updateParams.Storage;
        materialStockUploadDetail.StorageDestination = updateParams.StorageDestination;
        materialStockUploadDetail.Qty = updateParams.Qty;
        materialStockUploadDetail.RefDoc = updateParams.RefDoc;
        materialStockUploadDetail.Remark = updateParams.Remark;

        materialStockUploadDetail.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _materialStockUploadDetailRepository.UpdateAsync(materialStockUploadDetail);
    }


}