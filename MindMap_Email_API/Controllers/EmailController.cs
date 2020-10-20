using Microsoft.AspNetCore.Mvc;
using MindMap_Email_API.Utils;

namespace MindMap_Email_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        // GET api/<EmailController>/smth
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            EmailCreator.CreateEmail(id);
            return Ok("Chcek your inbox....");
        }
        [HttpGet]
        public IActionResult Test()
        {

            return Ok("Hello");

        }
    }
}
