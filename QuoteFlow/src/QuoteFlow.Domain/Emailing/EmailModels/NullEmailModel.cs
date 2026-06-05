namespace QuoteFlow.Emailing.EmailModels;

// TODO: Implement the NullEmailModel class
public class NullEmailModel : IEmailModel
{
    // 1. Create a template for null email model and bind the name of it to this property
    public string TemplateName => throw new System.NotImplementedException();

    // 2. Implement the TemplateDataModel property
    public object TemplateDataModel => throw new System.NotImplementedException();
}