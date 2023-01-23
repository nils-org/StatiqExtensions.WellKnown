using Statiq.Common;
using Statiq.Extensions.WellKnown.Base;

namespace Statiq.Extensions.WellKnown.WebFingerAlias;

public sealed class FromTemplateModule : AddDocuments
{
    private readonly Config<object> _contentConfig;

    public FromTemplateModule()
        : this(Config.FromSetting<object>(SettingKeys.WebFingerAlias.FromTemplate))
    {
    }

    private FromTemplateModule(Config<object> contentConfig)
    {
        _contentConfig = contentConfig;
    }

    protected override async Task<IEnumerable<IDocument>> CreateDocuments(IExecutionContext context)
    {
        var userHandle = await _contentConfig.GetValueAsync<string>(null, context);
        if (userHandle == null)
        {
            context.LogWarning(null, "No fediverse handle configured.");
            return Array.Empty<IDocument>();
        }

        var split = userHandle.TrimStart('@').Split('@', 2);
        if (split.Length != 2)
        {
            throw new ArgumentOutOfRangeException(nameof(userHandle),
                "UserHandle must be in the format of 'user@server.domain'");
        }

        var user = split[0];
        var instance = split[1];

        var result = $$"""
{
  "subject": "acct:{{user}}@{{instance}}",
  "aliases": [
    "https://{{instance}}/@{{user}}",
    "https://{{instance}}/users/{{user}}"
  ],
  "links": [
    {
      "rel": "http://webfinger.net/rel/profile-page",
      "type": "text/html",
      "href": "https://{{instance}}/@{{user}}"
    },
    {
      "rel": "self",
      "type": "application/activity+json",
      "href": "https://{{instance}}/users/{{user}}"
    },
    {
      "rel": "http://ostatus.org/schema/1.0/subscribe",
      "template": "https://{{instance}}/authorize_interaction?uri={uri}"
    }
  ]
}
""";

        return context.CreateWellKnownDocument(
            "webfinger",
            new JsonContent(result)).Yield();
    }
}