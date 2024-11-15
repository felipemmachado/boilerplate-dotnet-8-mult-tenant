using Application.Common.Constants;
using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ForbiddenAccessException() : Exception(ApiResponseMessages.AccessDenied);
}
