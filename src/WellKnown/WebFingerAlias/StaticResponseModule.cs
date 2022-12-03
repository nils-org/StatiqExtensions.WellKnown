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
    
    public StaticResponseModule(Config<object> contentConfig)
    {
        _contentConfig = contentConfig;
    }
    
    protected override async Task<IEnumerable<IDocument>> CreateDocuments(IExecutionContext context)
    {
        var staticResult = await _contentConfig.GetValueAsync<object>(null, context);
        if (staticResult == null)
        {
            context.LogTrace(null, "No static alias configured.");
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
        
        context.LogTrace(null, $"Using static configured value.");

        // something in the Content-Pipeline needs every document to have a valid source path.
        var source = context.FileSystem.InputPaths.Count > 0 
            ? context.FileSystem.InputPaths[0]
            : context.FileSystem.RootPath;
        if (source.IsRelative)
        {
            source = context.FileSystem.RootPath.Combine(source);
        }
        
        return context.CreateDocument(
            source.Combine(".well-known/webfinger"),
            new NormalizedPath(".well-known/webfinger", PathKind.Relative),
            new Dictionary<string, object>
            {
                { "IsWebFingerDocument", true },
                { "ShouldOutput", true },
                { "ContentType", "Content" },
                { "MediaType", "application/json" },
                { "IncludeInSitemap", false },
                { Common.Keys.DestinationExtension, null! }, // really, no extension. I mean it.
                { Common.Keys.DestinationFileName, "webfinger" },
            },
            content).Yield();

    }
}
