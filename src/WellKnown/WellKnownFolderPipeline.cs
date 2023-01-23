using JetBrains.Annotations;
using Statiq.Common;
using Statiq.Core;
using Statiq.Extensions.WellKnown;
using Statiq.Extensions.WellKnown.MicrosoftIdentityAssociation;
using Statiq.Extensions.WellKnown.WebFingerAlias;

// ReSharper disable once CheckNamespace
namespace Statiq;

[PublicAPI]
public sealed class WellKnownFolderPipeline : Pipeline
{
    public WellKnownFolderPipeline()
    {
        InputModules = new ModuleList(
            // WebFingerAlias
            new ExecuteIf(
                    Config.FromContext(ctx =>
                    {
                        var o = ctx.Settings.ContainsKey(SettingKeys.WebFingerAlias.StaticResult);
                        return o;
                    }),
                    new StaticResponseModule())
                .ElseIf(
                    Config.FromContext(ctx =>
                    {
                        var o = ctx.Settings.ContainsKey(SettingKeys.WebFingerAlias.FromTemplate);
                        return o;
                    }),
                    new FromTemplateModule()),        );

        OutputModules = new ModuleList(new WriteFiles());
    }
}
