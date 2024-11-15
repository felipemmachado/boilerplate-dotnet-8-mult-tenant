using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException(string message) : Exception(message);
}
