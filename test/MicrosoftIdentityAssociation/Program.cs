using System;
using Statiq.App;
using Statiq.Web;
using Statiq;
using Statiq.Common;
using Statiq.Extensions.WellKnown;

return await Bootstrapper
    .Factory
    .CreateWeb(args)
    .AddPipeline<WellKnownFolderPipeline>()
    .RunAsync();