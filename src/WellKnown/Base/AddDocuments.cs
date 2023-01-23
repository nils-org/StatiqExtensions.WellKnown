using Statiq.Common;

namespace Statiq.Extensions.WellKnown.Base;

/// <summary>
/// Adds a list <see cref="IDocument"/> to the end of the
/// existing documents in the current pipeline.
/// </summary>
public abstract class AddDocuments : Module
{
    protected abstract Task<IEnumerable<IDocument>> CreateDocuments(IExecutionContext context);
    
    protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
    {
        var documentsToAdd = await CreateDocuments(context);
        
        return context.Inputs.AddRange(documentsToAdd);
    }
}
