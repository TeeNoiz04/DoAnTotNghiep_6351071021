using Volo.Abp.Settings;

namespace QuoteFlow.Settings;

public class QuoteFlowSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(QuoteFlowSettings.MySetting1));
    }
}
