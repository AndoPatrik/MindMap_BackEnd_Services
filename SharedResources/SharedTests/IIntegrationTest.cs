using System.Net.Http;
using System.Threading.Tasks;

namespace SharedResources.SharedTests
{
    interface IIntegrationTest
    {
        protected HttpResponseMessage SendPostAsync(HttpClient client);

        protected HttpResponseMessage SendGetAsync(HttpClient client);

        protected Task JwtAuthentication();
    }
}
