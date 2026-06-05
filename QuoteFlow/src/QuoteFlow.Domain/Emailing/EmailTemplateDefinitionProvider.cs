using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Scriban;

namespace QuoteFlow.Emailing;

public class EmailTemplateDefinitionProvider : TemplateDefinitionProvider, ITransientDependency
{
    public override void Define(ITemplateDefinitionContext context)
    {
        // Layout template
        context.Add(
            new TemplateDefinition(
                name: EmailConsts.LayoutTemplateName,
                isLayout: true
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.LayoutTemplatePath,
                isInlineLocalized: true
            )
        );

        // Add your own templates here
        /* Code template
         context.Add(
            new TemplateDefinition(
                name: EmailConsts.<TemplateName>,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.<TemplatePath>,
                isInlineLocalized: true
            )
        );
         */

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.TestModelTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.TestModelTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.PriceOfferApprovalEmailTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.PriceOfferApprovalEmailTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.PriceOfferDiscussionEmailTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.PriceOfferDiscussionEmailTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.MaterialApprovalEmailTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.MaterialApprovalEmailTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.KeyAccountApprovalEmailTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.KeyAccountApprovalEmailTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
            new TemplateDefinition(
                name: EmailConsts.PSIApprovalEmailTemplateName,
                layout: EmailConsts.LayoutTemplateName,
                isLayout: false
            )
            .WithScribanEngine()
            .WithVirtualFilePath(
                EmailConsts.PSIApprovalEmailTemplatePath,
                isInlineLocalized: true
            )
        );

        context.Add(
          new TemplateDefinition(
              name: EmailConsts.DPODiscussionEmailTemplateName,
              layout: EmailConsts.LayoutTemplateName,
              isLayout: false
          )
          .WithScribanEngine()
          .WithVirtualFilePath(
              EmailConsts.DPODiscussionEmailTemplatePath,
              isInlineLocalized: true
          )
        );
        context.Add(
          new TemplateDefinition(
              name: EmailConsts.DPOApprovalEmailTemplateName,
              layout: EmailConsts.LayoutTemplateName,
              isLayout: false
          )
          .WithScribanEngine()
          .WithVirtualFilePath(
              EmailConsts.DPOApprovalEmailTemplatePath,
              isInlineLocalized: true
          )
        );

        context.Add(
          new TemplateDefinition(
              name: EmailConsts.GKRApprovalEmailTemplateName,
              layout: EmailConsts.LayoutTemplateName,
              isLayout: false
          )
          .WithScribanEngine()
          .WithVirtualFilePath(
              EmailConsts.GKRApprovalEmailTemplatePath,
              isInlineLocalized: true
          )
        );

        context.Add(
          new TemplateDefinition(
              name: EmailConsts.AssetLendingReminderEmailTemplateName,
              layout: EmailConsts.LayoutTemplateName,
              isLayout: false
          )
          .WithScribanEngine()
          .WithVirtualFilePath(
              EmailConsts.AssetLendingReminderEmailTemplatePath,
              isInlineLocalized: true
          )
        );

        context.Add(
          new TemplateDefinition(
              name: EmailConsts.AssetRequestApprovalEmailTemplateName,
              layout: EmailConsts.LayoutTemplateName,
              isLayout: false
          )
          .WithScribanEngine()
          .WithVirtualFilePath(
              EmailConsts.AssetRequestApprovalEmailTemplatePath,
              isInlineLocalized: true
          )
        );

        context.Add(
         new TemplateDefinition(
             name: EmailConsts.AssetLendingNotifyEmailTemplatePath,
             layout: EmailConsts.LayoutTemplateName,
             isLayout: false
         )
         .WithScribanEngine()
         .WithVirtualFilePath(
             EmailConsts.AssetLendingNotifyEmailTemplatePath,
             isInlineLocalized: true
         )
       );

    }
}