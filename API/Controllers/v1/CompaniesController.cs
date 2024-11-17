using API.Controllers.v1.@base;
using Application.Common.Exceptions;
using Application.Queries.Companies.GetCompanyByUrl;
using Application.UseCases.Companies.CreateCompany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1
{
    [AllowAnonymous]
    public class CompaniesController : BaseController
    {
        [HttpGet("{url}/tenancy")]
        public async Task<GetCompanyDto> GetByUrl(string url)
        {
            return await Mediator.Send(new GetCompanyByUrlQuery(url));
        }
        
        [HttpPost("{token}/sync")]
        public async Task<CreateCompanyDto> SyncCompany(string token, [FromBody] CreateCompanyCommand command)
        {
            if (token == "joao1314")
            {
                return await Mediator.Send(command);
            }
            
            throw new ForbiddenAccessException();
        }
    }
}
