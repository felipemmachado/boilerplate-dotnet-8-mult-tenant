using API.Controllers.v1.@base;
using Application.Common.Authorization;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Queries.Users.ExistsUser;
using Application.Queries.Users.GetUsers;
using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.DeleteUser;
using Application.UseCases.Users.UpdateUser;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public async Task<PaginatedList<UserDto>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{email}/exists")]
        public async Task<ExistsUserDto> ExistsUser(string email)
        {
            var query = new ExistsUserQuery(email);

            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<CreateUserDto> Create(CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPatch("{id:guid}")]
        public async Task Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.UserId) throw new NotFoundException(ApiResponseMessages.UserNotFound);

            await Mediator.Send(command);
        }

        [HttpDelete("{userId:guid}")]
        public async Task DeleteUser(Guid userId)
        {
            var query = new DeleteUserCommand(userId);
            await Mediator.Send(query);

        }
    }
}
