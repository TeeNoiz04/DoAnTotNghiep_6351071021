using QuoteFlow.Permissions;
using QuoteFlow.Suppliers.ParameterObject;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Suppliers;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.Supplier)]
public class SupplierAppService : QuoteFlowAppService, ISupplierAppService
{
    protected ISupplierRepository _supplierRepository;
    protected SupplierManager _supplierManager;
    public SupplierAppService(ISupplierRepository supplierRepository, SupplierManager supplierManager)
    {
        _supplierRepository = supplierRepository;
        _supplierManager = supplierManager;
    }
    public async Task<SupplierDto> CreateAsync(SupplierCreateDto input)
    {
        var createParams = ObjectMapper.Map<SupplierCreateDto, SupplierCreateParams>(input);
        var supplier = await _supplierManager.CreateAsync(createParams);
        return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
    }

    public async Task DeleteAsync(Guid id)
    {
        //await _supplierRepository.DeleteAsync(id);
        try
        {
            await _supplierRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.Category.RecordInUseCannotDelete);
        }
    }

    public async Task<SupplierDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Supplier, SupplierDto>(await _supplierRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<SupplierDto>> GetListAsync(GetSuppliersInput input)
    {
        var filter = ObjectMapper.Map<GetSuppliersInput, SupplierFilterParams>(input);
        var items = await _supplierRepository.GetListAsync(filter);
        var total = await _supplierRepository.GetCountAsync(filter);
        return new PagedResultDto<SupplierDto>
        {
            TotalCount = total,
            Items = ObjectMapper.Map<List<Supplier>, List<SupplierDto>>(items)
        };
    }

    public virtual async Task<SupplierDto> UpdateAsync(Guid id, SupplierUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SupplierUpdateDto, SupplierUpdateParams>(input);
        var supplier = await _supplierManager.UpdateAsync(id, updateParams);
        return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
    }
}
