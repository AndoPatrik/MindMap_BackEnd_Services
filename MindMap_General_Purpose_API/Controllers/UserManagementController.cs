using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MindMap_General_Purpose_API.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
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
            try
            {
                bodyPayload.IsActive = false;
                var existingUser = await _collection.Find(Builders<User>.Filter.Eq(u => u.Email, bodyPayload.Email)).FirstOrDefaultAsync();
                if (existingUser != null) return Conflict("This email is alrady registered...");
                await _collection.InsertOneAsync(bodyPayload);
                await HttpServiceCall.PostBasicAsync("https://localhost:6001/api/email", bodyPayload, CancellationToken.None) ;
                return Ok("New user been added");
            }
            catch (System.Exception)
            {
                return NotFound("Service could not create the user..."); // Should be changed to better response msg
                throw;
            }
        }
    
        [HttpGet("activate/{userId}")]
        public async Task<IActionResult> ActivateUser(string userId) 
        {
            var user = await _collection.Find(Builders<User>.Filter.Eq(u => u.Id, userId)).FirstOrDefaultAsync();
            if (user == null) return NotFound();
            user.IsActive = true;
            await _collection.ReplaceOneAsync(Builders<User>.Filter.Eq(u => u.Id, userId), user);
            return Ok("Your account has been updated. You can log in now.");
        }
    }
}
