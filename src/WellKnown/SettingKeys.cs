using System.Text.Json;

namespace Statiq.Extensions.WellKnown;

public static class SettingKeys
{
    public static class WebFingerAlias
    {
        /// <summary>
        /// Use this setting key for a static result.
        /// The value for this key can be a (json)-<see cref="string"/>,
        /// a <see cref="JsonDocument"/> or any other object that can be
        /// serialized to json.
        /// </summary>
        public const string StaticResult = nameof(WebFingerAlias)+nameof(StaticResult);   
    }
}