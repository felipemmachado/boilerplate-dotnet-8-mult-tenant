using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class Query
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
