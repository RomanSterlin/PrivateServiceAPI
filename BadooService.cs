using BadooAPI.Factories;
using BadooAPI.Utills;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using ServicesInterfaces;
using ServicesInterfaces.Global;
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
        private readonly IJsonFactory _jsonFactory;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string API_URL= "https://badoo.com/webapi.phtml?";

        public BadooService()
        {
            if (_jsonFactory is null)
            {
                _jsonFactory = new JsonRequestBodyFactory();
            }
           
        }
        public async Task<Data> AppStartUp(Data data)
        {
            try
            {
                var serverStartupJson = _jsonFactory.GetJson(JsonTypes.SERVER_APP_STARTUP);

                var response = await Generator.SendAndReturn(serverStartupJson, serverStartupJson.headers, API_URL);
                var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);

                data.SessionId = parsedResponse.body[0].client_startup.anonymous_session_id;

                if (data.SessionId is null)
                {
                    return data;
                }
                var loginResponse = await Login(data);

                if (loginResponse.Result == Result.Failed)
                {
                    return data;
                }

                parsedResponse = JsonConvert.DeserializeObject<dynamic>(loginResponse.ResultString);
                Dictionary<string, string> CredentialsResponse = new Dictionary<string, string>();

                data.SessionId = (string)parsedResponse.body[0].client_login_success.session_id;
                data.UserServiceId = (string)parsedResponse.body[0].client_login_success.user_info.user_id;
                data.HiddenUrl = (string)parsedResponse.body[1].client_common_settings.external_endpoints[1].url;
                data.Result = Result.Success;

                return data;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return data;
            }
        }
        public async Task<Data> Login(Data data)
        {
            try
            {
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.Login);

                var headers = jsonMessage.headers;

                jsonMessage.data.body[0].server_login_by_password.user = data.Username;
                jsonMessage.data.body[0].server_login_by_password.password = data.Password;

                var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);

                data.XPing = XPingGenerator.GenerateXPing(serializedObj);

                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = (string)await Generator.SendAndReturn(jsonMessage, headers, API_URL);

                if (!response.Contains("error"))
                {
                    data.ResultString = response;
                    data.Result = Result.Success;
                    return data;
                }
                return data;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return data;
            }
        }
        public async Task<List<string>> GetEncounters(Data data)
        {
            List<string> results = new List<string>();
            try
            {
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.GetEncounters);
                var headers = jsonMessage.headers;

                var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);

                data.XPing = XPingGenerator.GenerateXPing(serializedObj);

                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = await Generator.SendAndReturn(jsonMessage, headers);
                var encounters = JsonConvert.DeserializeObject<dynamic>(response);

                var enumserable = encounters.body[0].client_encounters.results;

                foreach (var item in enumserable)
                {
                    results.Add((string)item.user.user_id);
                }
                return results;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return results;
            }
        }
        public async Task<int> Like(Data data)
        {

            // for testing
            // return 0;
            var likesLeft = data.Likes;
            try
            {

                List<string> encounters = new List<string>();
                var results = await GetEncounters(data);
                 encounters.AddRange(results);
                for (int i = 0; i < (data.Likes / results.Count) - results.Count; i++)
                {
                    results = await GetEncounters(data);
                    encounters.AddRange(results);
                }
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.Like);
                var headers = jsonMessage.headers;

                for (int i = 0; i < likesLeft; i++)
                {
                    jsonMessage.data.body[0].server_encounters_vote.person_id = encounters[i];
                    var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);
                    data.XPing = XPingGenerator.GenerateXPing(serializedObj);

                    jsonMessage.headers = ConstructHeaders(data, headers);

                    var response = await Generator.SendAndReturn(jsonMessage, headers);
                    if (response.Contains("error"))
                    {
                        return likesLeft;
                    }
                    likesLeft--;
                }
                return likesLeft;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return likesLeft;
            }
        }
        public async Task<string> UpdateAboutMe(Data data)
        {
            try
            {
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.UpdateAboutMe);

                var headers = jsonMessage.headers;

                jsonMessage.data.body[0].server_save_user.user.profile_fields[0].value = data.About;
                jsonMessage.data.body[0].server_save_user.user.user_id = data.UserServiceId;

                var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);

                data.XPing = XPingGenerator.GenerateXPing(serializedObj);
                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);

                if (response.Contains("error"))
                {
                    return "error";
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return default;
            }
        }
        public async Task<IDictionary<string, string>> GetImages(Data data)
        {

            Dictionary<string, string> imagesLinks = new Dictionary<string, string>();
            try
            {
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.GetImages);

                var headers = jsonMessage.headers;

                jsonMessage.data.body[0].server_get_user.user_id = data.UserServiceId;
                jsonMessage.data.body[0].server_get_user.user_field_filter.request_interests.user_id = data.UserServiceId;
                jsonMessage.data.body[0].server_get_user.visiting_source.person_id = data.UserServiceId;
                //XPingGenerator xping = new XPingGenerator();
                var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);
                data.XPing = XPingGenerator.GenerateXPing(serializedObj);

                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);
                var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);

                var images = parsedResponse.body[0].user.albums[0].photos;

                foreach (var image in images)
                {
                    imagesLinks.Add((string)image.id, (string)image.large_url);
                }
                return imagesLinks;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return imagesLinks;
            }
        }

        public async Task<IDictionary<string, string>> RemoveImage(Data data)
        {
            Dictionary<string, string> imagesLinks = new Dictionary<string, string>();
            try
            {
                var jsonMessage = _jsonFactory.GetJson(JsonTypes.RemoveImage);

                var headers = jsonMessage.headers;

                jsonMessage.data.body[0].server_delete_photo.photo_id = data.ImageId;//
                                                                                     //XPingGenerator xping = new XPingGenerator();
                var serializedObj = (string)JsonConvert.SerializeObject(jsonMessage.data);
                data.XPing = XPingGenerator.GenerateXPing(serializedObj);

                jsonMessage.headers = ConstructHeaders(data, headers);

                var response = await Generator.SendAndReturn(jsonMessage, headers, API_URL);
                var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);
                var images = parsedResponse.body[0].album.photos;

                foreach (var image in images)
                {
                    imagesLinks.Add((string)image.id, (string)image.large_url);
                }
                return imagesLinks;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return imagesLinks;
            }

        }
        public async Task<string> UploadImage(Data data)
        {
            using HttpClient clientAsync = new();
          
            using WebClient client = new();
            if (data.File is null)
            {
                return "error";
            }
            try
            {
                clientAsync.DefaultRequestHeaders.Add("Content-Type", "image/jpeg");
              //  client.Headers.Set("Content-Type", "image/jpeg");

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
                return "error";
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return "error";
            }
        }
        //public async Task<string> UpdateLocation(Data data)
        //{
        //    return null;
        //}
        //public async Task<string> GetLocation(Data data)
        //{
        //    return null;
        //}
        //public async Task<string> UpdateDescription(Data data)
        //{
        //    return null;
        //}
        //public async Task<string> GetCities(Data data)
        //{
        //    return null;
        //}
        private dynamic ConstructCookie(Data data, dynamic headers)
        {
            try
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
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return "error";
            }
        }
        private dynamic ConstructUserId(Data data, dynamic headers)
        {
            try
            {
                string userIdObj = (string)headers.UserId;

                var splited = userIdObj.Split("=");
                Dictionary<string, string> dict = new Dictionary<string, string>(10);
                dict.Add(splited[0], data.UserServiceId);
                var userId = dict.DictionaryToString();

                headers.UserId = userId;

                return headers;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return headers;
            }
        }
        //should be able to make this into one function and just loop twice with different variables
        private dynamic ConstructXPing(Data data, dynamic headers)
        {
            try
            {
                var userIdObj = (string)headers.Pingback;

                var splited = userIdObj.Split("=");
                Dictionary<string, string> dict = new Dictionary<string, string>(10);
                dict.Add(splited[0], data.XPing);
                var XPing = dict.DictionaryToString();

                headers.Pingback = XPing;

                return headers;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Trace(e.StackTrace);
                return headers;
            }
        }
        public dynamic ConstructHeaders(Data data, dynamic headers)
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
