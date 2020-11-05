using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;

namespace MindMap_General_Purpose_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        // GET: api/<WorkspaceController>
        [HttpPost]
        public IActionResult AddUserToAWorkspace()
        {
            return Ok();
        }

        // GET api/<WorkspaceController>/5
        [HttpGet("{id}")]
        public IActionResult GetWorkspaceById(int id)
        {
            return Ok(); ;
        }

        // POST api/<WorkspaceController>
        [HttpPost]
        public IActionResult CreateWorkspace([FromBody] Workspace bodyPayload)
        {
            return Ok();
        }
    }
}
