using System.Text.Json;

namespace Statiq.Extensions.WellKnown;

/// <summary>
/// Settings keys.
/// </summary>
public static class SettingKeys
{
    /// <summary>
    /// Use this keys to
    /// create a static <c>webfinger</c> file.
    /// This can be used to simplify creating an alias for an account in
    /// the fediverse (like Mastodon) on a local server.
    /// The content of the file needs to be json.
    /// <example>
    /// Use some static content, using the <see cref="SettingKeys.WebFingerAlias.StaticResult"/> setting.
    /// <code>
    /// await Bootstrapper
    ///     .Factory
    ///     .CreateWeb(args)
    ///     .AddSetting(SettingKeys.WebFingerAlias.StaticResult, new
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
    ///     .AddPipeline<WellKnownFolderPipeline>()
    ///     .RunAsync();
    /// </code></example> 
    /// </summary>
    public static class WebFingerAlias
    {
        /// <summary>
        /// Use this setting key for a static json result.
        /// The value for this key can be a (json)-<see cref="string"/>,
        /// a <see cref="JsonDocument"/> or any other object that can be
        /// serialized to json.
        /// </summary>
        public const string StaticResult = nameof(WebFingerAlias) + nameof(StaticResult);
        
        /// <summary>
        /// Use this setting key for a generated result from your fediverse handle.
        /// The value for this key must be a full fediverse handle: E.g. <c>nils_andresen@mastodon.social</c>.
        /// </summary>
        public const string FromTemplate = nameof(WebFingerAlias) + nameof(FromTemplate);
    }
}