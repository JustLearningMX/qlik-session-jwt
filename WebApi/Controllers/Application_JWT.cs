using BusinessLogic.Dtos;
using BusinessLogic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Application_JWT : ControllerBase
    {
        private readonly ApplicationJWTService applicationJWTService;

        public Application_JWT(ApplicationJWTService applicationJWTService)
        {
            this.applicationJWTService = applicationJWTService;
        }

        [HttpPost("GenerateToken")]
        public async Task<ActionResult<GenericResult<JWTDtoResponse>>> Generate(JWTDtoRequest applicationJWTDto)
        {
            return applicationJWTService.generate(applicationJWTDto);
        }
    }
}
