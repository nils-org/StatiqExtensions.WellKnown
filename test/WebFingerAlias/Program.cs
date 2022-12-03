using Statiq.Extensions.WellKnown;

await Bootstrapper
    .Factory
    .CreateWeb(args)
    //.AddSetting(SettingKeys.WebFingerAlias.StaticResult, GetSerializableDemo())
    //.AddSetting(SettingKeys.WebFingerAlias.StaticResult, GetStringDemo())
    .WithWellKnown(x => x
        .WebFingerAlias("@nils_andresen@mastodon.social"))
    .RunAsync();


#pragma warning disable CS8321
object GetSerializableDemo()
{
    return new
    {
        subject = "acct:nils_andresen@mastodon.social",
        aliases = new[]
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
            },
        },
    };
}

object GetStringDemo()
{
    return """
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
}
#pragma warning restore CS8321