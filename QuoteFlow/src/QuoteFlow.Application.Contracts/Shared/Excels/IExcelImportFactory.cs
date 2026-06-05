namespace QuoteFlow.Shared.Excels;

public interface IExcelImportFactory
{
    IExcelValidator<T> CreateValidator<T>(string validationType);
    IExcelDtoConverter<TImportDto, TCreateParams> CreateCreateParamsConverter<TImportDto, TCreateParams>(string validationType)

        where TImportDto : class
        where TCreateParams : class;
}
