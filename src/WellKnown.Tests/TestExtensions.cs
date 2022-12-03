using System.Collections.Immutable;
using System.Text.Json;
using Shouldly;
using Statiq.Common;
using Statiq.Testing;

namespace WellKnown.Tests;

internal static class TestExtensions
{
    public static async Task<ImmutableArray<IDocument>> Execute(this TestExecutionContext context, params IModule[] modules)
    {
        return await context.ExecuteModulesAsync(modules, Array.Empty<IDocument>());
    }

    public static async Task ShouldBeASingleDocumentWithJsonContent(
        this IEnumerable<IDocument> documents,
        string jsonContent)
    {
        var enumerable = documents as IDocument[] ?? documents.ToArray();
        enumerable.Length.ShouldBe(1);
        var result = enumerable.First();
        result.Destination.ToString().ShouldBe(".well-known/webfinger");
        result.ContentProvider.MediaType.ShouldBe("application/json");

        var actual = JsonDocument.Parse(
                await result.ContentProvider.GetTextReader().ReadToEndAsync())
            .GetFormattedText();
        
        var expected = JsonDocument.Parse(jsonContent).GetFormattedText();
        
        actual.ShouldBe(expected);
    }

    private static string GetFormattedText(this JsonDocument document)
    {
        using var mem = new MemoryStream();
        using var writer = new Utf8JsonWriter(mem, new JsonWriterOptions{Indented = true});
        
        document.WriteTo(writer);
        writer.Flush();
        mem.Flush();
        mem.Position = 0;

        using var reader = new StreamReader(mem);
        var txt = reader.ReadToEnd();
        return txt;
    }
}