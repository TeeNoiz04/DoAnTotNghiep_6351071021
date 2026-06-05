using QuoteFlow.CfgDiscountRatios.ParameterObjects;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.CfgDiscountRatios
{
    public class CfgDiscountRatioManager : DomainService
    {
        protected ICfgDiscountRatioRepository _cfgDiscountRatioRepository;

        public CfgDiscountRatioManager(ICfgDiscountRatioRepository cfgDiscountRatioRepository)
        {
            _cfgDiscountRatioRepository = cfgDiscountRatioRepository;
        }

        public virtual async Task<CfgDiscountRatio> CreateAsync(
        string? approval_Type = null, string? product_Type = null, string? accountClassify = null, decimal? value_Min = null, decimal? value_Max = null, decimal? discountRatio = null, string? note = null)
        {
            Check.Length(approval_Type, nameof(approval_Type), CfgDiscountRatioConsts.Approval_TypeMaxLength);
            Check.Length(product_Type, nameof(product_Type), CfgDiscountRatioConsts.Product_TypeMaxLength);
            Check.Length(accountClassify, nameof(accountClassify), CfgDiscountRatioConsts.AccountClassifyMaxLength);

            var cfgDiscountRatio = new CfgDiscountRatio(
             GuidGenerator.Create(),
             approval_Type, product_Type, accountClassify, value_Min, value_Max, discountRatio, note
             );

            return await _cfgDiscountRatioRepository.InsertAsync(cfgDiscountRatio);
        }

        public virtual async Task<CfgDiscountRatio> UpdateAsync(
            Guid id,
            string? approval_Type = null, string? product_Type = null, string? accountClassify = null, decimal? value_Min = null, decimal? value_Max = null, decimal? discountRatio = null, string? note = null
        )
        {
            Check.Length(approval_Type, nameof(approval_Type), CfgDiscountRatioConsts.Approval_TypeMaxLength);
            Check.Length(product_Type, nameof(product_Type), CfgDiscountRatioConsts.Product_TypeMaxLength);
            Check.Length(accountClassify, nameof(accountClassify), CfgDiscountRatioConsts.AccountClassifyMaxLength);

            var cfgDiscountRatio = await _cfgDiscountRatioRepository.GetAsync(id);

            cfgDiscountRatio.Approval_Type = approval_Type;
            cfgDiscountRatio.Product_Type = product_Type;
            cfgDiscountRatio.AccountClassify = accountClassify;
            cfgDiscountRatio.Value_Min = value_Min;
            cfgDiscountRatio.Value_Max = value_Max;
            cfgDiscountRatio.DiscountRatio = discountRatio;
            cfgDiscountRatio.Note = note;

            return await _cfgDiscountRatioRepository.UpdateAsync(cfgDiscountRatio);
        }
        //supplierBU.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        public virtual async Task<CfgDiscountRatio> UpdateAsync(
            Guid id,
            CfgDiscountRatioUpdateParams updateParams
        )
        {
            //Check.Length(approval_Type, nameof(approval_Type), CfgDiscountRatioConsts.Approval_TypeMaxLength);
            //Check.Length(product_Type, nameof(product_Type), CfgDiscountRatioConsts.Product_TypeMaxLength);
            //Check.Length(accountClassify, nameof(accountClassify), CfgDiscountRatioConsts.AccountClassifyMaxLength);

            var cfgDiscountRatio = await _cfgDiscountRatioRepository.GetAsync(id);
            var cfgMax = (await _cfgDiscountRatioRepository.GetListAsync(x =>
                x.Approval_Type == cfgDiscountRatio.Approval_Type &&
                x.Product_Type == cfgDiscountRatio.Product_Type &&
                x.Id != id))
                .OrderByDescending(x => x.Value_Min)
                .FirstOrDefault();
            if (updateParams.Value_Min < cfgMax.Value_Min && updateParams.Value_Max is null)
            {
                throw new UserFriendlyException($"To Value is required for this range."); //must have value
            }
            if (updateParams.Value_Min >= cfgMax.Value_Min && cfgMax.Value_Max is null)
            {

                throw new UserFriendlyException($"Another configuration has no To Value. Please update that configuration before you can save this one"); //must have value
            }

            //cfgDiscountRatio.Approval_Type = updateParams.Approval_Type;
            //cfgDiscountRatio.Product_Type = updateParams.Product_Type;
            //cfgDiscountRatio.AccountClassify = updateParams.AccountClassify;
            cfgDiscountRatio.Value_Min = updateParams.Value_Min;
            cfgDiscountRatio.Value_Max = updateParams.Value_Max;
            cfgDiscountRatio.DiscountRatio = updateParams.DiscountRatio;
            cfgDiscountRatio.Note = updateParams.Note;

            return await _cfgDiscountRatioRepository.UpdateAsync(cfgDiscountRatio);
        }
    }
}