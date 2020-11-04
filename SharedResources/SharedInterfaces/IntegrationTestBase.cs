using Microsoft.AspNetCore.Mvc.Testing;
using MindMap_General_Purpose_API;
using SharedResources.SharedTests;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharedResources
{
    public class IntegrationTestBase : IIntegrationTest
    {
        protected readonly HttpClient _client;
        //protected readonly WebApplicationFactory<IStartup> _appFactory;

        public IntegrationTestBase()
        {
            var _appFactory = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       //services.RemoveAll(typeof(MongoClient));
                       //services.AddSingleton<MongoClient>(new MongoClient("mongodb+srv://admin:mindmap2020@mindmappercluster.gtnqi.mongodb.net/<dbname>?retryWrites=true&w=majority"));
                   });
               });

            _client = _appFactory.CreateClient();
        }

        Task IIntegrationTest.JwtAuthentication()
        {
            throw new System.NotImplementedException();
        }

        HttpResponseMessage IIntegrationTest.SendGetAsync(HttpClient client)
        {
            throw new System.NotImplementedException();
        }

        HttpResponseMessage IIntegrationTest.SendPostAsync(HttpClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}
