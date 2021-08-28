using BadooAPI.Factories;
using BadooAPI.Interfaces;
using BadooAPI.Models;
using BadooAPI.Utills;
using Newtonsoft.Json;
using ServicesInterfaces;
using ServicesModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BadooAPI
{
    public class BadooService : IService
    {
        private readonly IJsonFactory JsonFactory;

        private const string API_URL_AM = "https://am1.badoo.com/webapi.phtml?";
        private const string API_URL_US = "https://us1.badoo.com/webapi.phtml?";
        private const string API_URL = "https://badoo.com/webapi.phtml?";

        //need my own container now to inject dependency?
        public BadooService()
        {
            JsonFactory = new JsonFactory();
        }
        /// <summary>
        /// Start up Badoo app with username and passowrd
        /// </summary>
        /// <returns>Data for future API calls like Session id & User id</returns>

        public async Task<Data> AppStartUp(Data data)
        {
            var serverStartupJson = JsonFactory.GetJson(JsonTypes.SERVER_APP_STARTUP);
            if (serverStartupJson != null)
            {
                var response = await Generator.SendAndReturn(serverStartupJson, serverStartupJson.headers, API_URL);
                var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);
                data.SessionId = parsedResponse.body[0].client_startup.anonymous_session_id;

                if (data.SessionId != null)
                {
                    data.XPing = "b3da80cc837c7a1cd30029ae9a129a82";
                    var loginResponse = await Login(data);
                    //if LoginUS contains error, try LoginAM 
                    if (loginResponse.Contains("error"))
                    {
                        //AM attempt
                        //response = await Generator.SendAndReturn(serverStartupJson, serverStartupJson.headers, API_URL_AM);
                        //parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);
                        //data.SessionId = parsedResponse.body[0].client_startup.anonymous_session_id;

                        //if (data.SessionId != null)
                        //{
                        //    loginResponse = await LoginAM(data);
                        //}
                        //else
                        //{
                        return data;

                    }
                    //continue with whatever worked
                    parsedResponse = JsonConvert.DeserializeObject<dynamic>(loginResponse);
                    Dictionary<string, string> CredentialsResponse = new Dictionary<string, string>();

                    if (parsedResponse.body[0].client_login_success.session_id != null && parsedResponse.body[0].client_login_success.user_info.user_id != null)
                    {
                        data.SessionId = (string)parsedResponse.body[0].client_login_success.session_id;
                        data.UserServiceId = (string)parsedResponse.body[0].client_login_success.user_info.user_id;
                        data.HiddenUrl = (string)parsedResponse.body[1].client_common_settings.external_endpoints[1].url;
                        data.Result = Result.Success;
                        return data;
                    }
                    else
                    {
                        return data;
                    }
                }
                else
                {
                    return data;
                }
            }
            else
            {
                return data;
            }
        }
        public async Task<string> Login(Data data)
        {
            data.XPing = "b3da80cc837c7a1cd30029ae9a129a82";
            var jsonMessage = JsonFactory.GetJson(JsonTypes.Login);

            var headers = jsonMessage.headers;

            //data.XPing = Generator.GenerateXPing(jsonMessage.data);

            jsonMessage.headers = ConstructHeaders(data, headers);

            jsonMessage.data.body[0].server_login_by_password.user = data.UserName;
            jsonMessage.data.body[0].server_login_by_password.password = data.Password;

            var response = (string)await Generator.SendAndReturn(jsonMessage, headers, API_URL);

            return response;
        }
        public async Task<string> LoginUS(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.LoginUS);

            var headers = jsonMessage.headers;

            jsonMessage.headers = ConstructHeaders(data, headers);

            jsonMessage.data.body[0].server_login_by_password.user = data.UserName;
            jsonMessage.data.body[0].server_login_by_password.password = data.Password;

            var response = (string)await Generator.SendAndReturn(jsonMessage, headers, API_URL_US);

            return response;
        }
        public async Task<string> LoginAM(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.LoginAM);

            var headers = jsonMessage.headers;

            jsonMessage.headers = ConstructHeaders(data, headers);

            jsonMessage.data.body[0].server_login_by_password.user = data.UserName;
            jsonMessage.data.body[0].server_login_by_password.password = data.Password;

            var response = (string)await Generator.SendAndReturn(jsonMessage, headers, API_URL_AM);

            return response;
        }
        public async Task<List<string>> GetEncounters(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.GetEncounters);
            var headers = jsonMessage.headers;

            //data.XPing = Generator.GenerateXPing(jsonMessage.data);

            jsonMessage.headers = ConstructHeaders(data, headers);

            var response = await Generator.SendAndReturn(jsonMessage, headers);
            var encounters = JsonConvert.DeserializeObject<dynamic>(response);

            var enumserable = encounters.body[0].client_encounters.results;
            List<string> results = new List<string>();
            foreach (var item in enumserable)
            {
                results.Add((string)item.user.user_id);
            }
            return results;
        }
        public async Task<int> Like(Data data)
        {
            List<string> encounters = new List<string>();
            for (int i = 0; i < data.Likes / 10; i++)
            {
                var results = await GetEncounters(data);
                encounters.Concat(results).ToList();
            }
            var jsonMessage = JsonFactory.GetJson(JsonTypes.Like);
            var headers = jsonMessage.headers;

            var likesLeft = data.Likes;
            for (int i = 0; i < likesLeft; i++)
            {
                jsonMessage.data.body[0].server_encounters_vote.person_id = encounters[i];

                //Method not ready yet, need to reverse js code into C#
                //data.XPing = Generator.GenerateXPing(jsonMessage.data);

                var dat = jsonMessage.data;
                var ping = "60ec44c0c19f3d9132c4d798c1f6799d";
                data.XPing = ping;

                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = await Generator.SendAndReturn(jsonMessage, headers);
                if (response.Contains("error"))
                {
                    return likesLeft;
                }
            }
            return likesLeft;
        }
        public async Task<string> UpdateAboutMe(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.UpdateAboutMe);

            var headers = jsonMessage.headers;
            jsonMessage.headers = ConstructHeaders(data, headers);

            jsonMessage.data.body[0].server_save_user.user.profile_fields[0].value = data.Input;
            // var newJson = JsonConvert.SerializeObject(jsonMessage);

            var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);
            if (response.Contains("error"))
            {
                return "error";
            }
            return response;
        }
        public async Task<IDictionary<string, string>> GetImages(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.GetImages);

            var headers = jsonMessage.headers;
            var x = jsonMessage.data.body[0].server_get_user.user_id;
            jsonMessage.data.body[0].server_get_user.user_id = data.UserServiceId;
            jsonMessage.data.body[0].server_get_user.user_field_filter.request_interests.user_id = data.UserServiceId;
            jsonMessage.data.body[0].server_get_user.visiting_source.person_id = data.UserServiceId;

            //data.XPing = Generator.GenerateXPing(jsonMessage.data);

            jsonMessage.headers = ConstructHeaders(data, headers);

            var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);
            var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);

            var images = parsedResponse.body[0].user.albums[0].photos;

            Dictionary<string, string> imagesLinks = new Dictionary<string, string>();
            foreach (var image in images)
            {
                imagesLinks.Add((string)image.id, (string)image.large_url);
            }
            return imagesLinks;
        }
        public async Task<IDictionary<string, string>> RemoveImage(Data data)
        {
            var jsonMessage = JsonFactory.GetJson(JsonTypes.RemoveImage);

            var headers = jsonMessage.headers;

            jsonMessage.data.body[0].server_delete_photo.photo_id = data.Input;

            //data.XPing = Generator.GenerateXPing(jsonMessage.data);
            jsonMessage.headers = ConstructHeaders(data, headers);

            var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);
            var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);
            var images = parsedResponse.body[0].album.photos;

            Dictionary<string, string> imagesLinks = new Dictionary<string, string>();
            foreach (var image in images)
            {
                imagesLinks.Add((string)image.id, (string)image.large_url);
            }
            return imagesLinks;

        }
        public async Task<string> UploadImage(Data data)
        {
            
            using HttpClient clientAsync = new HttpClient();
            using WebClient client = new WebClient();

            client.Headers.Set("Content-Type", "image/jpeg");
            using HttpContent content = new MultipartContent("undefined", data.SessionId);
            using HttpContent content2 = new MultipartContent("album_type", "2");

            using var file_content = new ByteArrayContent(new StreamContent(data.File.OpenReadStream()).ReadAsByteArrayAsync().Result);
            if (file_content != null)
            {
                file_content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                var formData = new MultipartFormDataContent();
                formData.Add(file_content, "file", "rand.jpeg");
                var res = await clientAsync.PostAsync(data.HiddenUrl, formData);
                var result = await res.Content.ReadAsStringAsync();
            
                return await res.Content.ReadAsStringAsync();
            }
            //log error
            return "error";
        }
        public async Task<string> UpdateLocation(Data data)
        {
            return null;
        }
        public async Task<string> GetLocation(Data data)
        {
            return null;
        }
        public async Task<string> UpdateDescription(Data data)
        {
            return null;
        }
        public async Task<string> GetCities(Data data)
        {
            return null;
        }
        private static dynamic ConstructCookie(Data data, dynamic headers)
        {
            string Cookie = headers.Cookie;
            var splited = Cookie.Split(";");

            Dictionary<string, string> CookieEntries = new Dictionary<string, string>(20);
            foreach (var entry in splited)
            {
                if (!entry.Contains("base_domain"))
                {
                    var pair = entry.Split("=");
                    CookieEntries.Add(pair[0], pair[1]);
                }
                else
                {
                    CookieEntries.Add("fbm_107433747809=base_domain", ".badoo.com");
                }
            }
            CookieEntries[" session"] = data.SessionId;
            CookieEntries[" HDR-X-User-id"] = data.UserServiceId;
            var newCookie = CookieEntries.DictionaryToString();
            headers.Cookie = newCookie;
            return headers;
        }
        private static dynamic ConstructUserId(Data data, dynamic headers)
        {
            string userIdObj = (string)headers.UserId;

            var splited = userIdObj.Split("=");
            Dictionary<string, string> dict = new Dictionary<string, string>(10);
            dict.Add(splited[0], data.UserServiceId);
            var userId = dict.DictionaryToString();

            headers.UserId = userId;

            return headers;
        }
        //should be able to make this into one function and just loop twice with different variables
        private static dynamic ConstructXPing(Data data, dynamic headers)
        {
            var userIdObj = (string)headers.Pingback;

            var splited = userIdObj.Split("=");
            Dictionary<string, string> dict = new Dictionary<string, string>(10);
            dict.Add(splited[0], data.XPing);
            var XPing = dict.DictionaryToString();

            headers.Pingback = XPing;

            return headers;
        }
        public static dynamic ConstructHeaders(Data data, dynamic headers)
        {
            headers = ConstructCookie(data, headers);
            //if (data.UserId != null)
            //{
            //    headers = ConstructUserId(data, headers);
            //}
            return ConstructXPing(data, headers);
        }

    }
}
