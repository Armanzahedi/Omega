using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure.Helpers
{
    public interface IBzApiHelper
    {
        Task<object> GetEndpoint(string endpoint);
        string GetBaseUrl();
    }

    public class BzApiHelper : IBzApiHelper
    {
        private readonly IConfiguration _configuration;
        public BzApiHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<object> GetEndpoint(string endpoint)
        {
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(_configuration["BzApiRoot"]+endpoint);
                request.Method = HttpMethod.Get;
                request.Headers.Add("APIKey", _configuration["BzApiKey"]);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject(result);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }
        public string GetBaseUrl()
        {
            return _configuration["BzApiRoot"];
        }
    }
}
