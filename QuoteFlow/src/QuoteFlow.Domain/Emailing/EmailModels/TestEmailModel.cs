namespace QuoteFlow.Emailing.EmailModels;

public class TestEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.TestModelTemplateName;

    public string Name => "Test Model";

    public object TemplateDataModel => new
    {
        Name,
        ReceiverFullName
    };

    public string? ReceiverFullName { get; set; }
}
