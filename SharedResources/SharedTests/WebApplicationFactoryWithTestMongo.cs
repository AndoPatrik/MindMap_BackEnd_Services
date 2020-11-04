using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace SharedResources.SharedTests
{
    public class WebApplicationFactoryWithTestMongo<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(MongoClient));
                services.AddSingleton<MongoClient>(new MongoClient("mongodb+srv://admin:mindmap2020@mindmappercluster.gtnqi.mongodb.net/test?authSource=admin&replicaSet=atlas-diqg7a-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true"));
            });
        }
    }
}
