using Microsoft.AspNetCore.Mvc;
using MindMap_General_Purpose_API.Models;
using MindMap_General_Purpose_API.Utils;
using MongoDB.Driver;
using System;
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
            if (!ValidatorUtil.ValidateUser(bodyPayload)) return BadRequest("Check the payload.");
            try
            {
                bodyPayload.IsActive = false;
                var existingUser = await _collection.Find(Builders<User>.Filter.Eq(u => u.Email, bodyPayload.Email)).FirstOrDefaultAsync();
                if (existingUser != null) return Conflict("This email is alrady registered.");
                await _collection.InsertOneAsync(bodyPayload);
                bool IsSuccesful = await HttpService.PostAsync("https://localhost:6001/api/email", bodyPayload, CancellationToken.None);
                // Check against 'IsSuccesful' to see if email status code.
                return Ok("New user been added");
            }
            catch (Exception)
            {
                return NotFound("Service could not create the user.");
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

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userParam)
        {
            var user = await _collection.Find(Builders<User>.Filter.Eq(u => u.Email, userParam.Email)
                & Builders<User>.Filter.Eq(u => u.Password, userParam.Password))
                .FirstOrDefaultAsync();

            if (user == null) return BadRequest("Email or password is not correct.");

            if(!user.IsActive) return BadRequest("User is not active.");

            return Ok(JWT.GenerateToken(user.Id));
        }
    }
}
