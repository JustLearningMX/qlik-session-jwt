using BusinessLogic.Dtos;
using BusinessLogic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QlikAdminController : ControllerBase
    {
        private readonly QlikAdminService qlikAdminService;

        public QlikAdminController(QlikAdminService qlikAdminService)
        {
            this.qlikAdminService = qlikAdminService;
        }

        [HttpGet("token")]
        public async Task<ActionResult<GenericResult<AdminTokenDtoResponse>>> Generate()
        {
            return await qlikAdminService.generate();
        }
    }
}
