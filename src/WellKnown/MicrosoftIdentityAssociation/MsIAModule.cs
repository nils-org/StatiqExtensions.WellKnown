using System.Text.Json;
using Statiq.Common;
using Statiq.Extensions.WellKnown.Base;

namespace Statiq.Extensions.WellKnown.MicrosoftIdentityAssociation;

/// <summary>
/// This module generated the <c>microsoft-identity-association.json</c> file.
/// </summary>
public sealed class MsIaModule : AddDocuments
{
    private readonly Config<object> _contentConfig;

    public MsIaModule()
        : this(Config.FromSetting<object>(SettingKeys.MicrosoftIdentityAssociation.ApplicationId))
    {
    }

    private MsIaModule(Config<object> contentConfig)
    {
        _contentConfig = contentConfig;
    }

    protected override async Task<IEnumerable<IDocument>> CreateDocuments(IExecutionContext context)
    {
        var idFromSettings = await _contentConfig.GetValueAsync<object>(null, context);
        if (idFromSettings == null)
        {
            context.LogWarning(null, "No ApplicationID configured.");
            return Array.Empty<IDocument>();
        }

        var ids = new List<string>();
        switch (idFromSettings)
        {
            case IEnumerable<string> list:
                ids.AddRange(list);
                break;
            case string text:
                ids.AddRange(text.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries));
                break;
            default:
                throw new ArgumentException(
                    $"content of type {idFromSettings.GetType().Name} could not be parsed to a valid list of IDs.");
        }
        
        var content = new
        {
            associatedApplications = ids.Select(x =>
                new
                {
                    applicationId = x.Trim(),
                }).ToArray(),
        };
        
        return context.CreateWellKnownDocument(
                "microsoft-identity-association.json",
                new JsonContent(JsonSerializer.Serialize(content)))
            .Yield();
    }
}