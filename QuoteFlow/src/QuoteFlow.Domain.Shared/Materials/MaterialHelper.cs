using System;
using Volo.Abp;

namespace QuoteFlow.Materials;
public static class MaterialHelper
{
    public static string IncreaseRequestNo(string? currentRequestNo, string requestNoPrefix)
    {
        var currentDayMonth = DateTime.Now.ToString("dd.MM");

        if (currentRequestNo.IsNullOrWhiteSpace())
        {
            return $"{requestNoPrefix}_{currentDayMonth}_001";
        }

#if NET5_0_OR_GREATER
        var last3Digits = currentRequestNo[^3..];
        var dayMonth = currentRequestNo[^9..^4];
#else
        var last3Digits = currentRequestNo.Substring(currentRequestNo.Length - 3);
        var dayMonth = currentRequestNo.Substring(currentRequestNo.Length - 9, 5);
#endif

        {
            var incremented = int.Parse(last3Digits) + 1;
            if (incremented > 999)
            {
                throw new BusinessException("Maximum daily request number reached.");
            }
            last3Digits = incremented.ToString("D3");
        }

        return $"{requestNoPrefix}_{currentDayMonth}_{last3Digits}";
    }
}
