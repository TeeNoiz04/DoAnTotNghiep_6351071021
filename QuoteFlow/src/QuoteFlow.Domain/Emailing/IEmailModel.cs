namespace QuoteFlow.Emailing;

public interface IEmailModel
{
    string TemplateName { get; }
    object TemplateDataModel { get; }
}
