using System.Text.Json;
using JetBrains.Annotations;
using Statiq.Common;
using Statiq.Extensions.WellKnown;
using Statiq.Extensions.WellKnown.WebFingerAlias;

// ReSharper disable once CheckNamespace
namespace Statiq;

[PublicAPI]
public static partial class WellKnownBuilderExtensions
{
    /// <summary>
    /// Use this extension to modify the Input-<see cref="IPipeline"/>
    /// (e.g. of Statiq.Web or Statiq.Docs) and add a static file '/.well-known/webfinger'
    /// to the output. This can be used to simplify creating an alias for an account in
    /// the fediverse (like Mastodon) on a local server.
    /// The content of the file needs to be json.
    ///
    /// <para>
    /// Available SettingKeys are:
    /// <list type="bullet">
    ///   <item>
    ///     <term><see cref="SettingKeys.WebFingerAlias.StaticResult"/></term>
    ///     <description>Use some static content.
    ///     The value for this key can be a (json)-<see cref="string"/>,
    ///     a <see cref="JsonDocument"/> or any other object that can be
    ///     serialized to json.
    ///     </description>
    ///   </item>
    /// </list>
    /// </para>
    ///
    /// <example>
    /// Use some static content, using the <see cref="SettingKeys.WebFingerAlias.StaticResult"/> setting.
    /// <code>
    /// await Bootstrapper
    ///     .Factory
    ///     .CreateWeb(args)
    ///     .AddSetting(SettingKeys.StaticResult, new
    ///     {
    ///         subject = "acct:nils_andresen@mastodon.social",
    ///         aliases = new[]
    ///         {
    ///             "https://mastodon.social/@nils_andresen",
    ///             "https://mastodon.social/users/nils_andresen",
    ///         },
    ///         links = new object[]
    ///         {
    ///             new
    ///             {
    ///                 rel = "http://webfinger.net/rel/profile-page",
    ///                 type = "text/html",
    ///                 href = "https://mastodon.social/@nils_andresen",
    ///             },
    ///             new
    ///             {
    ///                 rel = "self",
    ///                 type = "application/activity+json",
    ///                 href = "https://mastodon.social/users/nils_andresen",
    ///             },
    ///             new
    ///             {
    ///                 rel = "http://ostatus.org/schema/1.0/subscribe",
    ///                 template = "https://mastodon.social/authorize_interaction?uri={uri}",
    ///             },
    ///         },
    ///     })
    ///     .WithWellKnown(x => x.WebFingerAlias())
    ///     .RunAsync();
    /// </code></example> 
    /// </summary>
    public static WellKnownBuilder<T> WebFingerAlias<T>(this WellKnownBuilder<T> builder)
        where T : IBootstrapper =>
        builder.AddToInputPipeline(new StaticResponseModule());

    /// <summary>
    /// This extension does the same, as <see cref="WebFingerAlias{T}(Statiq.WellKnownBuilder{T})"/>,
    /// except that no explicit setting is needed.
    /// The required setting will be generated on-the-fly.
    ///
    /// <example><code>
    /// await Bootstrapper
    ///     .Factory
    ///     .CreateWeb(args)
    ///     .WithWellKnown(x => x
    ///         .WebFingerAlias("@nils_andresen@mastodon.social"))
    ///     .RunAsync();
    /// </code></example>
    /// </summary>
    public static WellKnownBuilder<T> WebFingerAlias<T>(this WellKnownBuilder<T> builder, string userHandle)
        where T : IBootstrapper
    {
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

        return builder
            .WithBootstrapper(b => b.ConfigureSettings(x => x[SettingKeys.WebFingerAlias.StaticResult] = result))
            .WebFingerAlias();
    }
}