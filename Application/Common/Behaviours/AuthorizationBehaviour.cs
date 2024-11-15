using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Application.Common.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class AuthorizationBehaviour<TRequest, TResponse>(
        IUserService? userService) :
        IPipelineBehavior<TRequest, TResponse> where TRequest :
        IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            var attributes = authorizeAttributes as AuthorizeAttribute[] ?? authorizeAttributes.ToArray();

            if (attributes.Length == 0)
                return await next();

            // Must be authenticated user
            if (userService?.UserId == null) throw new UnauthorizedAccessException();

            // Role-based authorization
            var authorizeAttributesWithRoles = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

            var attributesWithRoles = authorizeAttributesWithRoles as AuthorizeAttribute[] ?? authorizeAttributesWithRoles.ToArray();
            if (!attributesWithRoles.Any())
                return await next();

            if (attributesWithRoles
                .Select(a => a.Roles?.Split(','))
                .OfType<string[]>()
                .Select(userService.HaveSomeRole)
                .Any(authorized => !authorized))
            {
                throw new ForbiddenAccessException();
            }

            return await next();
        }
    }
}
