using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MindMap_Email_API.Utils;
using MindMap_General_Purpose_API.Models;

namespace MindMap_Email_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        // GET api/<EmailController>/smth
        [HttpPost]
        public IActionResult Post([FromBody] User bodyPayload)
        {
            EmailCreator.CreateEmail(bodyPayload.Id, bodyPayload.Email);
            return Ok("Chcek your inbox....");
        }
        [HttpGet]
        public IActionResult Test()
        {

            return Ok("Hello");

        }
    }
}
