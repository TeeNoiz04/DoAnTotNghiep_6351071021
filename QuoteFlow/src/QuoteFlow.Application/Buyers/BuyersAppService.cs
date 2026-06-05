using QuoteFlow.Buyers.ParameterObjects;
using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace QuoteFlow.Buyers;
[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.Buyer)]
public class BuyersAppService : QuoteFlowAppService, IBuyersAppService
{
    protected IBuyerRepository _buyerRepository;
    protected BuyerManager _buyerManager;

    public BuyersAppService(IBuyerRepository buyerRepository, BuyerManager buyerManager)
    {
        _buyerRepository = buyerRepository;
        _buyerManager = buyerManager;
    }

    public virtual async Task<BuyerDto> CreateAsync(BuyerCreateDto input)
    {
        var createParams = ObjectMapper.Map<BuyerCreateDto, BuyerCreateParams>(input);
        var buyer = await _buyerManager.CreateAsync(createParams);
        return ObjectMapper.Map<Buyer, BuyerDto>(buyer);
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _buyerRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {

            throw new BusinessException(QuoteFlowDomainErrorCodes.Category.RecordInUseCannotDelete);
        }
    }
    public async Task<BuyerDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Buyer, BuyerDto>(
            await _buyerRepository.GetAsync(id)
        );
    }

    public virtual async Task<PagedResultDto<BuyerDto>> GetListAsync(GetBuyersInput input)
    {
        var filterPrams = ObjectMapper.Map<GetBuyersInput, BuyerFilterParams>(input);
        var totalCount = await _buyerRepository.GetCountAsync(filterPrams);
        var items = await _buyerRepository.GetListAsync(
            filterPrams,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount
        );
        return new PagedResultDto<BuyerDto>(
            totalCount,
            ObjectMapper.Map<List<Buyer>, List<BuyerDto>>(items)
        );
    }

    public virtual async Task<BuyerDto> UpdateAsync(Guid id, BuyerUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<BuyerUpdateDto, BuyerUpdateParams>(input);
        var buyer = await _buyerManager.UpdateAsync(id, updateParams);
        return ObjectMapper.Map<Buyer, BuyerDto>(buyer);
    }
    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetBuyersInput input)
    {


        var filterParams = ObjectMapper.Map<GetBuyersInput, BuyerFilterParams>(input);
        var items = await _buyerRepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Buyer>, List<BuyerDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "Buyer.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

}

