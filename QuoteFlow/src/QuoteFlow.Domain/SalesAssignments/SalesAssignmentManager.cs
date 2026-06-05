using QuoteFlow.SalesAssignments.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SalesAssignments;

public class SalesAssignmentManager : DomainService
{
    protected ISalesAssignmentRepository _salesAssignmentRepository;
    public SalesAssignmentManager(ISalesAssignmentRepository SalesAssignmentRepository)
    {
        _salesAssignmentRepository = SalesAssignmentRepository;
    }
    public virtual async Task<SalesAssignment> CreateAsync(SalesAssignmentCreateParams createParams)
    {
        var SalesAssignment = new SalesAssignment(GuidGenerator.Create(), createParams);
        return await _salesAssignmentRepository.InsertAsync(SalesAssignment);
    }

    public virtual async Task<SalesAssignment> UpdateAsync(Guid id, SalesAssignmentUpdateParams updateParams)
    {
        var salesAssignment = await _salesAssignmentRepository.GetAsync(id);
        updateParams.SaleUserName = salesAssignment.SaleUserName; // Không cho phép update SaleUserName
        var inputMaterialTypes = updateParams.MaterialType?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? Array.Empty<string>();

        if (inputMaterialTypes.Length > 0)
        {
            // Check duplicate cho tất cả MaterialType input (không bao gồm record hiện tại)
            var existingAssignments = await _salesAssignmentRepository.GetListAsync();
            var existingMaterialTypes = existingAssignments
                .Where(x =>
                    x.Id != id && // Không so sánh với record hiện tại
                    x.SaleUserName.Equals(updateParams.SaleUserName, StringComparison.OrdinalIgnoreCase) &&
                    x.BuyerId == updateParams.BuyerId &&
                    x.LocationId == updateParams.LocationId)
                .Select(x => x.MaterialType ?? "")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var duplicated = inputMaterialTypes.Where(mt => existingMaterialTypes.Contains(mt));
            if (duplicated.Any())
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.SalesTeam.UserAlreadyInSaleTeam);
            }
        }


        if (inputMaterialTypes.Length == 2)
        {
            // Trường hợp có 2 MaterialType: FA,LVS
            // Update record hiện tại với MaterialType đầu tiên
            //UpdateSalesAssignmentFields(salesAssignment, updateParams, inputMaterialTypes[0]);
            salesAssignment.BuyerId = updateParams.BuyerId;
            salesAssignment.BuyerTypeId = updateParams.BuyerTypeId;
            salesAssignment.SaleUserName = updateParams.SaleUserName;
            salesAssignment.LocationId = updateParams.LocationId;
            salesAssignment.MaterialType = inputMaterialTypes[0];
            salesAssignment.BuyerShortName = updateParams.BuyerShortName;
            salesAssignment.Note = updateParams.Note;
            var updatedAssignment = await _salesAssignmentRepository.UpdateAsync(salesAssignment);

            // Tạo record mới cho MaterialType thứ hai
            //var newAssignment = CreateNewSalesAssignment(updateParams, inputMaterialTypes[1]);
            var newAssignment = new SalesAssignment(Guid.NewGuid(), updateParams, inputMaterialTypes[1], salesAssignment.SaleFullName);
            await _salesAssignmentRepository.InsertAsync(newAssignment);

            return updatedAssignment;
        }
        else
        {
            // Trường hợp có 1 MaterialType hoặc không có: update bình thường
            UpdateSalesAssignmentFields(salesAssignment, updateParams, updateParams.MaterialType);
            return await _salesAssignmentRepository.UpdateAsync(salesAssignment);
        }
    }

    private void UpdateSalesAssignmentFields(SalesAssignment salesAssignment, SalesAssignmentUpdateParams updateParams, string materialType)
    {
        salesAssignment.BuyerId = updateParams.BuyerId;
        salesAssignment.BuyerTypeId = updateParams.BuyerTypeId;
        salesAssignment.SaleUserName = updateParams.SaleUserName;
        salesAssignment.LocationId = updateParams.LocationId;
        salesAssignment.MaterialType = materialType;
        salesAssignment.BuyerShortName = updateParams.BuyerShortName;
        salesAssignment.Note = updateParams.Note;
    }

    //private SalesAssignment CreateNewSalesAssignment(SalesAssignmentUpdateParams updateParams, string materialType)
    //{
    //    return new SalesAssignment
    //    {
    //        BuyerId = updateParams.BuyerId,
    //        BuyerTypeId = updateParams.BuyerTypeId,
    //        SaleUserName = updateParams.SaleUserName,
    //        LocationId = updateParams.LocationId,
    //        MaterialType = materialType,
    //        BuyerShortName = updateParams.BuyerShortName,
    //        Note = updateParams.Note
    //    };
    //}

    public virtual async Task<List<SalesAssignment>> CreateManyAsync(List<SalesAssignmentCreateParams> createParamsList)
    {
        await ValidateCreateParamsAsync(createParamsList);
        var entities = createParamsList.Select(param => new SalesAssignment(GuidGenerator.Create(), param)).ToList();

        await _salesAssignmentRepository.InsertManyAsync(entities);

        return entities;
    }
    public virtual async Task ValidateCreateParamsAsync(List<SalesAssignmentCreateParams> createParamsList)
    {
        var saleTeams = await _salesAssignmentRepository.GetListAsync();

        foreach (var createParam in createParamsList)
        {
            var inputMaterialTypes = createParam.MaterialType?
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? Array.Empty<string>();

            if (!inputMaterialTypes.Any())
            {
                continue;
            }

            var existingMaterialTypes = saleTeams
                .Where(x =>
                    x.SaleUserName.Equals(createParam.SaleUserName, StringComparison.OrdinalIgnoreCase) &&
                    x.BuyerId == createParam.BuyerId &&
                    x.LocationId == createParam.LocationId)
                .SelectMany(x => (x.MaterialType ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var duplicated = inputMaterialTypes
                .Where(mt => existingMaterialTypes.Contains(mt))
                .ToList();

            if (duplicated.Any())
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.SalesTeam.UserAlreadyInSaleTeam);
            }
        }
    }


}

