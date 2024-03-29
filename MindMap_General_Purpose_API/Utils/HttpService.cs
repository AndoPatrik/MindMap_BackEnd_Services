﻿using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MindMap_General_Purpose_API.Utils
{
    public class HttpService
    {
        public static async Task<bool> PostAsync(string url, object content, CancellationToken cancellationToken)
        {
            
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var json = JsonConvert.SerializeObject(content);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        if (response.StatusCode == HttpStatusCode.OK) return true;
                        return false;
                    }
                }
            }
        }
    }
}
