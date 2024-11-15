using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Queries.Users.ExistsUser;
using Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;
namespace Application.UseCases.Users.CreateUser
{
    [ExcludeFromCodeCoverage]
    public record struct CreateUserCommand(
        string Name,
        string Email
    ) : IRequest<CreateUserDto>;

    public class CreateUserCommandHandler(
        IApplicationDbContext context,
        IPasswordService passwordService,
        IMediator mediator
    ) : IRequestHandler<CreateUserCommand, CreateUserDto>
    {
        public async Task<CreateUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ExistsUserQuery(request.Email), cancellationToken);

            if (result.Exists)
                throw new ValidationException(ApiResponseMessages.EmailAlreadyRegistered(request.Email));
            
            var password = passwordService.GenerateRandomPassword();
            var user = new User(
                request.Name, 
                request.Email, 
                passwordService.Hash(password), []);

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateUserDto(user.Id, password);
        }
    }
}
