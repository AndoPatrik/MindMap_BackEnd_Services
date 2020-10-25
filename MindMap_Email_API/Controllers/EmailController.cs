using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MindMap_Email_API.Utils;
using MindMap_General_Purpose_API.Models;

namespace MindMap_Email_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SendActivationEmail([FromBody] User bodyPayload)
        {
            EmailManager.CreateEmail(bodyPayload.Id, bodyPayload.Email, _configuration["EmailPW"]);
            return Ok("Chcek your inbox....");
        }
    }
}
