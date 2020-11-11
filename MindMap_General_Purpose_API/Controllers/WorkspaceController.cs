using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MindMap_General_Purpose_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Workspace> _workspacesCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public WorkspaceController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _database = mongoClient.GetDatabase("MindMapDb");
            _workspacesCollection = _database.GetCollection<Workspace>("Workspaces");
            _usersCollection = _database.GetCollection<User>("Users");
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
                Workspace workspace = await _workspacesCollection.Find(Builders<Workspace>.Filter.Eq(w => w.Id , id)).FirstOrDefaultAsync();
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
                //transaction
                await _workspacesCollection.InsertOneAsync(bodyPayload);
                User userToUpdate = await _usersCollection.Find<User>(Builders<User>.Filter.Eq(u => u.Id, bodyPayload.Creator.Id)).FirstOrDefaultAsync();
                User updatedUser = userToUpdate;
                updatedUser.ConnectedWorkspaces.Add(bodyPayload.Id);
                /*await _usersCollection.UpdateOne(
                   Builders<User>.Filter.Eq(u => u.Id, bodyPayload.Creator.Id),
                   Builders<User>.Update.Push("ConnectedWorkspaces", bodyPayload.Id)); */
                var result = await _usersCollection.ReplaceOneAsync(Builders<User>.Filter.Eq(u => u.Id, bodyPayload.Creator.Id), updatedUser);
                return Ok(bodyPayload.Id); 
            }
            catch (Exception)
            {
                return Conflict("Could not create Workspace");
            }
        }

        [HttpGet("GetWorkspacesOfUser/{id}")]
        public async Task<IActionResult> FetchWorkspacesByUser (string id) 
        {
            try
            {
                User currentUser = await _usersCollection.Find<User>(Builders<User>.Filter.Eq(u => u.Id, id)).FirstOrDefaultAsync();
                List<string> workspacesConnectedToUser = currentUser.ConnectedWorkspaces;
                List<Workspace> workspacesToReturn = new List<Workspace>();
                foreach (var workspace in workspacesConnectedToUser)
                {
                    workspacesToReturn.Add(await _workspacesCollection.Find<Workspace>(Builders<Workspace>.Filter.Eq(w => w.Id, workspace)).FirstOrDefaultAsync());
                }
                return Ok(workspacesToReturn);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
