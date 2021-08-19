using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private const string API_URL_AM = "https://am1.badoo.com/webapi.phtml?";
        private const string API_URL_US = "https://us1.badoo.com/webapi.phtml?";
        private const string API_URL = "https://badoo.com/webapi.phtml?";

        public static async Task<string> SendAndReturn(dynamic payload, dynamic headers = null, string URI = API_URL)
        {

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload.data));
            var rr = headers;
            //STILL NEED TO MANUALY INPUT X- PING
            AddHeaders(content, payload, headers);

            var serializedObj = (string)JsonConvert.SerializeObject(payload.name);
            var t = payload.data;
            var newUri = serializedObj.Replace('"', ' ');

            //on app startup sometimes needs with US url and sometimes with AM, different session id format
            var uri = URI + newUri.Trim();
            content.Headers.ContentType = null;
            var h = content.Headers;
            var day = JsonConvert.SerializeObject(payload.data);
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

        public static void GenerateXPing(dynamic data)
        {
            throw new NotImplementedException();
        }
    }
}
