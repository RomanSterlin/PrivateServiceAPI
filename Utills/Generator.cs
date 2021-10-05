using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BadooAPI.Utills
{
    public static class Generator
    {
        private static IConfigurationRoot Configuration;
        private static string API_URL;
        
          
        static Generator()
        {
            API_URL = BuildSettings();
        }
        public static string BuildSettings()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var value = Configuration.GetSection("AppSettings:APIUrl").Value;
            return value;
        }
        public static async Task<string> SendAndReturn(dynamic payload, dynamic headers = null)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload.data));

            AddHeaders(content, payload, headers);

            var serializedObj = (string)JsonConvert.SerializeObject(payload.name);
            var newUri = serializedObj.Replace('"', ' ');

            var uri = API_URL + newUri.Trim();
            content.Headers.ContentType = null;

            HttpResponseMessage response = await client.PostAsync(uri, content);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;

        }

        public static void AddHeaders(HttpContent content, dynamic payload, dynamic headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    var ff = content.Headers;
                    if (header.ToString().Contains("json"))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                    else
                    {
                        if (header.Name != "Pingback" && header.Name != "UserId")
                        {
                            content.Headers.Add(header.Name, (string)header.Value);
                        }
                        else
                        {
                            var name = (string)header.Value;
                            var splited = name.Split("=");

                            content.Headers.Add(splited[0], splited[1]);
                        }
                    }
                }
                content.Headers.ContentLength = null;
                content.Headers.ContentLength = null;
            }
        }

    }
}
