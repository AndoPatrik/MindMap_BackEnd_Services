using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MindMap_General_Purpose_API.Models;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace xUnit_General_Purpose_API.IntegrationTests
{
    public class IntegrationDb
    {
        public IntegrationDb()
        {
        }

        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private IMongoCollection<User> _collection;

        public void Application_Start()
        {
            _runner = MongoDbRunner.StartForDebugging();
            _runner.Import("TestDatabase", "TestCollection", @"..\..\App_Data\test.json", true);

            MongoClient client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("MindMapDb");
            _collection = _database.GetCollection<User>("User");

            /* happy coding! */
        }

        public void Application_End()
        {
            _runner.Dispose();
        }
    }
}
