using API.Controllers.v1.@base;
using API.Dto;
using Application.Common.Interfaces;
using Application.Queries.Users.GetMe;
using Application.UseCases.Account.UpdatePassword;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1
{
    public class MeController(IUserService userService) : BaseController
    {

        [HttpPut("password")]
        public async Task UpdatePassword(UpdateActualPasswordDto userCommand)
        {
            var command = new UpdatePasswordCommand
            {
                RePassword = userCommand.RePassword,
                Password = userCommand.Password,
                UserId = userService.UserId,
                ActualPassword = userCommand.ActualPassword
            };

            await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<MeDto> Me()
        {
            var command = new GetMeQuery
            {
                UserId = userService.UserId
            };

            return await Mediator.Send(command);
        }
    }
}
