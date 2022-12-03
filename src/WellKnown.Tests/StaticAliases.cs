using System.Text.Json;
using Statiq.Extensions.WellKnown;
using Statiq.Extensions.WellKnown.WebFingerAlias;
using Statiq.Testing;

namespace WellKnown.Tests;

public class StaticAliases
{
    private const string StaticJson = """
{
    "subject": "acct:nils_andresen@mastodon.social",
    "aliases": [
        "https://mastodon.social/@nils_andresen",
        "https://mastodon.social/users/nils_andresen"
    ],
    "links": [
        {
            "rel": "http://webfinger.net/rel/profile-page",
            "type": "text/html",
            "href": "https://mastodon.social/@nils_andresen"
        },
        {
            "rel": "self",
            "type": "application/activity+json",
            "href": "https://mastodon.social/users/nils_andresen"
        },
        {
            "rel": "http://ostatus.org/schema/1.0/subscribe",
            "template": "https://mastodon.social/authorize_interaction?uri={uri}"
        }
    ]
}
""";
    
    [Fact]
    public async Task ShouldReturnTheStaticString()
    {    
        // Given
        var context = new TestExecutionContext
        {
            Settings =
            {
                [SettingKeys.WebFingerAlias.StaticResult] = StaticJson,
            },
        };

        // When
        var results = await context.Execute(new StaticResponseModule());

        // Then
        await results.ShouldBeASingleDocumentWithJsonContent(StaticJson);
    }

    [Fact]
    public async Task ShouldReturnTheStaticJsonObject()
    {
        // Given
        var input = JsonDocument.Parse(StaticJson);

        var context = new TestExecutionContext
        {
            Settings =
            {
                [SettingKeys.WebFingerAlias.StaticResult] = input,
            },
        };
        
        // When
        var results = await context.Execute(new StaticResponseModule());

        // Then
        await results.ShouldBeASingleDocumentWithJsonContent(StaticJson);
    }
    
    [Fact]
    public async Task ShouldReturnTheStaticAnonymousObject()
    {
        // Given
        var context = new TestExecutionContext
        {
            Settings =
            {
                [SettingKeys.WebFingerAlias.StaticResult] = new
                {
                    subject = "acct:nils_andresen@mastodon.social",
                    aliases = new []
                    {
                        "https://mastodon.social/@nils_andresen",
                        "https://mastodon.social/users/nils_andresen",
                    },
                    links = new object[]
                    {
                        new
                        {
                            rel = "http://webfinger.net/rel/profile-page",
                            type = "text/html",
                            href = "https://mastodon.social/@nils_andresen",
                        },
                        new
                        {
                            rel = "self",
                            type = "application/activity+json",
                            href = "https://mastodon.social/users/nils_andresen",
                        },
                        new
                        {
                            rel = "http://ostatus.org/schema/1.0/subscribe",
                            template = "https://mastodon.social/authorize_interaction?uri={uri}",
                        }

                    },
                },
            },
        };
        
        // When
        var results = await context.Execute(new StaticResponseModule());
        
        // Then
        await results.ShouldBeASingleDocumentWithJsonContent(StaticJson);
    }
}