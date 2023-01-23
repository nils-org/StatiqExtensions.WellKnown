using System.Text.Json;
using Statiq.Common;
using Statiq.Extensions.WellKnown.Base;

namespace Statiq.Extensions.WellKnown.WebFingerAlias;

public sealed class StaticResponseModule : AddDocuments
{
    private readonly Config<object> _contentConfig;

    public StaticResponseModule()
        : this(Config.FromSetting<object>(SettingKeys.WebFingerAlias.StaticResult))
    {}
    
    private StaticResponseModule(Config<object> contentConfig)
    {
        _contentConfig = contentConfig;
    }
    
    protected override async Task<IEnumerable<IDocument>> CreateDocuments(IExecutionContext context)
    {
        var staticResult = await _contentConfig.GetValueAsync<object>(null, context);
        if (staticResult == null)
        {
            context.LogWarning(null, "No static alias configured.");
            return Array.Empty<IDocument>();
        }

        JsonContent content;
        switch (staticResult)
        {
            case string stringResult:
                content = new JsonContent(stringResult);
                break;
            case JsonDocument jsonDocument:
                content = new JsonContent(jsonDocument.RootElement);
                break;
            default:
                try
                {
                    var json = JsonSerializer.Serialize(staticResult);
                    content = new JsonContent(json);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        $"content of type {staticResult.GetType().Name} could not be parsed to json. {e.GetType().Name}: {e.Message}");
                }

                break;
        }
        
        return context.CreateWellKnownDocument(
            "webfinger",
            content).Yield();

    }
}
