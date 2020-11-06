using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MongoDB.Driver;

namespace MindMap_General_Purpose_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Workspace> _collection;

        public WorkspaceController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _database = mongoClient.GetDatabase("MindMapDb");
            _collection = _database.GetCollection<Workspace>("Workspaces");
        }

        // GET: api/<WorkspaceController>
        [HttpGet("Share/{id}")]
        public IActionResult AddUserToAWorkspace(string id)
        {
            return Ok("Test");
        }

        // GET api/<WorkspaceController>/5
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetWorkspaceById(string id)
        {
            try
            {
                Workspace workspace = await _collection.Find(Builders<Workspace>.Filter.Eq(w => w.Id , id)).FirstOrDefaultAsync();
                return Ok(workspace); 
            }
            catch (Exception)
            {
                return NotFound("Could not retrieve the workspace");
            }
        }

        // POST api/<WorkspaceController>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateWorkspace([FromBody] Workspace bodyPayload)
        {
            //inout validation

            try
            {
                //bodyPayload.ShareLink = Guid.NewGuid().ToString();
                await _collection.InsertOneAsync(bodyPayload);
                return Ok("Workspace created.");
            }
            catch (Exception)
            {
                return Conflict("Could not create Workspace");
            }
        }
    }
}
