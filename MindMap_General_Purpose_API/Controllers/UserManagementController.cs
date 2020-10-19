using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MindMap_General_Purpose_API.Utils;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MindMap_General_Purpose_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collection;

        public UserManagementController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _database = mongoClient.GetDatabase("MindMapDb");
            _collection = _database.GetCollection<User>("Users");
        }
        // POST api/<UserManagementController>
        [HttpPost("registration")]
        public async Task<IActionResult> Post([FromBody] User bodyPayload)
        {
            if (!ValidatorUtil.ValidateUser(bodyPayload)) return BadRequest("Check the payload");
            bodyPayload.IsActive = false;
            await _collection.InsertOneAsync(bodyPayload);
            return Ok("All good");
        }
    }
}
