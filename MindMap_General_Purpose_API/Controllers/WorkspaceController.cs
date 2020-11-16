using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;

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
            using (var session = _mongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    await _workspacesCollection.InsertOneAsync(bodyPayload);
                    User userToUpdate = await _usersCollection.Find<User>(Builders<User>.Filter.Eq(u => u.Id, bodyPayload.Creator.Id)).FirstOrDefaultAsync();
                    User updatedUser = userToUpdate;
                    updatedUser.ConnectedWorkspaces.Add(new ConnectedWorkspace(bodyPayload.Id));
                    var result = await _usersCollection.ReplaceOneAsync(Builders<User>.Filter.Eq(u => u.Id, bodyPayload.Creator.Id), updatedUser);
                    session.CommitTransaction();
                    return Ok(bodyPayload.Id);
                }
                catch (Exception)
                {
                    session.AbortTransaction();
                    return Conflict("Could not create Workspace");
                }
            }
        }

        [HttpGet("GetWorkspacesOfUser/{id}")]
        public async Task<IActionResult> FetchWorkspacesByUser (string id) 
        {
            try
            {
                User currentUser = await _usersCollection.Find<User>(Builders<User>.Filter.Eq(u => u.Id, id)).FirstOrDefaultAsync();
                IEnumerable<ConnectedWorkspace> workspacesConnectedToUser = currentUser.ConnectedWorkspaces;
                List<Workspace> workspacesToReturn = new List<Workspace>();
                foreach (var workspace in workspacesConnectedToUser)
                {
                    workspacesToReturn.Add(await _workspacesCollection.Find<Workspace>(Builders<Workspace>.Filter.Eq(w => w.Id, workspace.WorkspaceId)).FirstOrDefaultAsync());
                }
                return Ok(workspacesToReturn);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkspace(string id) 
        {
            using (var session = _mongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    Workspace workspaceToDelete = await _workspacesCollection.Find<Workspace>(Builders<Workspace>.Filter.Eq(w => w.Id, id)).FirstOrDefaultAsync();
                    foreach (var user in workspaceToDelete.Users)
                    {
                        var userFilter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                        var update = Builders<User>.Update.PullFilter(u => u.ConnectedWorkspaces, Builders<ConnectedWorkspace>.Filter.Where(cw => cw.WorkspaceId == workspaceToDelete.Id));
                        _usersCollection.UpdateOne(userFilter, update);
                    }
                    _workspacesCollection.DeleteOne(Builders<Workspace>.Filter.Eq(w => w.Id, id));
                    session.CommitTransaction();
                    return Ok("Workspace deleted");
                }
                catch (Exception)
                {
                    session.AbortTransaction();
                    return NotFound("Something went wrong");
                }
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateWorkspace([FromBody] Workspace bodyPayload) 
        {
            try
            {
                await _workspacesCollection.ReplaceOneAsync(Builders<Workspace>.Filter.Eq(w => w.Id, bodyPayload.Id), bodyPayload);
                return Ok("Sucessful change.");
            }
            catch (Exception)
            {
                return NotFound("Workspace could not be updated.");
            }
        }

        [HttpGet("clone/{id}")]
        public async Task<IActionResult> CloneWorkspaceById(string id) 
        {
            //TODO
            // change the creator of the cloned version ?!
            using (var session = _mongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    Workspace wsToClone = await _workspacesCollection.Find(Builders<Workspace>.Filter.Eq(w => w.Id, id)).FirstOrDefaultAsync();
                    Workspace wsClone = wsToClone;
                    wsClone.Id = null;
                    await _workspacesCollection.InsertOneAsync(wsClone);

                    foreach (User user in wsClone.Users) 
                    {
                        User userToUpdate = await _usersCollection.Find(Builders<User>.Filter.Eq(u => u.Id, user.Id)).FirstOrDefaultAsync();
                        User updatedUser = userToUpdate;
                        updatedUser.ConnectedWorkspaces.Add(new ConnectedWorkspace(wsClone.Id));
                        var result = await _usersCollection.ReplaceOneAsync(Builders<User>.Filter.Eq(u => u.Id, user.Id), updatedUser);
                    }

                    session.CommitTransaction();
                    return Ok(wsClone);
                }
                catch (Exception)
                {
                    session.AbortTransaction();
                    return Conflict("Workspace could not be cloned.");
                }
            }
      
        }

        [HttpPost("addUserToWorkspcae/{id}")]
        public async Task<IActionResult> AddUsersToWorkspace([FromBody] List<User> bodyPayload, string id ) 
        {
            using (var session = _mongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    bool hasChanged = false;
                    var filter = Builders<Workspace>.Filter.Eq(w => w.Id, id);
                    Workspace workspace = await _workspacesCollection.Find(filter).FirstOrDefaultAsync();
                    foreach (var user in bodyPayload)
                    {
                        var userFilter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
                        User userInDb = await _usersCollection.Find(userFilter).FirstOrDefaultAsync();
                        if (userInDb != null)
                        {
                            ConnectedWorkspace connectedWs = userInDb.ConnectedWorkspaces.Find(cw => cw.WorkspaceId == id);
                            if (connectedWs == null)
                            {
                                User userToAdd = new User();
                                userToAdd.Email = userInDb.Email;
                                userToAdd.Id = userInDb.Id;
                                workspace.Users.Add(userToAdd);

                                userInDb.ConnectedWorkspaces.Add(new ConnectedWorkspace(workspace.Id));
                                await _usersCollection.ReplaceOneAsync(userFilter, userInDb);
                                hasChanged = true;
                            }
                        }

                    }
                    if (hasChanged)
                    {
                        _workspacesCollection.ReplaceOne(filter, workspace);
                        session.CommitTransaction();
                    }

                    return Ok("User(s) added to workspace.");
                }
                catch (Exception)
                {
                    session.AbortTransaction();
                    return Conflict("User(s) could not be added.");
                }
            }
        }
    }
}
