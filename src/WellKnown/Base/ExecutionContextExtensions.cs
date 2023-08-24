using Statiq.Common;

namespace Statiq.Extensions.WellKnown.Base;

internal static class ExecutionContextExtensions
{
    internal static NormalizedPath GetWellKnownFolder(this IExecutionContext context)
    {
        // something in the Content-Pipeline needs every document to have a valid source path.
        // so we'll fake some source.

        var source = context.FileSystem.RootPath;

        if (context.FileSystem.InputPaths.Count < 1)
        {
            return source;
        }
        
        var exisingWellKnownFolders =
            context.FileSystem.InputPaths
                .Where(p => p.ContainsChild(".well-known"))
                .ToArray();
        source = exisingWellKnownFolders.Length > 1 
            ? exisingWellKnownFolders[0] 
            : context.FileSystem.InputPaths[0];

        if (source.IsRelative)
        {
            source = context.FileSystem.RootPath.Combine(source);
        }

        return source;
    }

    internal static IDocument CreateWellKnownDocument(
        this IExecutionContext context,
        string fileName,
        IContentProvider contentProvider,
        IEnumerable<KeyValuePair<string, object>>? items = null
    ) 
    {
        // something in the Content-Pipeline needs every document to have a valid source path.
        var source = context.GetWellKnownFolder();
        var extension = string.Empty;
        var pos = fileName.LastIndexOf('.');
        if(pos > 0)
        {
            extension = fileName[(pos+1)..];
        }

        var metadata = new Dictionary<string, object>
        {
            { MetadataKeys.IsWellKnownDocument, true },
            { "ShouldOutput", true },
            { "ContentType", "Content" },
            { "MediaType", contentProvider.MediaType },
            { "IncludeInSitemap", false },
            { Common.Keys.DestinationExtension, extension },
            { Common.Keys.DestinationFileName, fileName },
        };

        if(items != null)
        {
            items.ToList().ForEach(x => metadata[x.Key] = x.Value);
        }

        return context.CreateDocument(
            source.Combine(fileName),
            new NormalizedPath($".well-known/{fileName}", PathKind.Relative),
            metadata,
            contentProvider);
    }
}