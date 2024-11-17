using API.Controllers.v1.@base;
using Application.Common.Configs;
using Application.Common.Exceptions;
using Application.Queries.Companies.GetCompanyByUrl;
using Application.UseCases.Companies.CreateCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace API.Controllers.v1
{
    [AllowAnonymous]
    public class CompaniesController(IOptions<ApiConfig> apiConfig) : BaseController
    {
        private readonly ApiConfig _apiConfig = apiConfig.Value ?? throw new ArgumentNullException(nameof(apiConfig));

        [HttpGet("{url}/tenancy")]
        public async Task<GetCompanyDto> GetByUrl(string url)
        {
            return await Mediator.Send(new GetCompanyByUrlQuery(url));
        }
        
        [HttpPost("{token}/sync")]
        public async Task<CreateCompanyDto> SyncCompany(string token, [FromBody] CreateCompanyCommand command)
        {
            if (token == _apiConfig.TokenCreateCompany)
            {
                return await Mediator.Send(command);
            }
            
            throw new ForbiddenAccessException();
        }
    }
}
