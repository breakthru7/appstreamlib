using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppstreamLib.RestCall
{

    public class RequestHandler
    {
        public static async Task<T> MakeGetRequest<T>(string token, string servicename, string tokenname = "token", bool isIdsrvr = false)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (tokenname != string.Empty)
                {
                    if (tokenname == "Authorization")
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(tokenname, token);
                    }
                }

                var httpresponse = await client.GetAsync(ConfigurationManager.AppSettings["rest.call.endpoint"] + servicename);
                var jsonContent = await httpresponse.Content.ReadAsStringAsync();

                if (httpresponse.StatusCode == HttpStatusCode.Forbidden)
                {   
                    throw new Exception(httpresponse.ReasonPhrase);
                }
                else if (httpresponse.StatusCode != HttpStatusCode.OK)
                {
                    var responsestatus = JsonConvert.DeserializeObject<ResponseStatusRoot>(jsonContent);
                    throw new Exception(responsestatus.ResponseStatus.Message);
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<T>(jsonContent);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<T> MakePostRequest<T>(string token, string servicename, object body = null, string tokenname = "token")
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["rest.call.endpoint"]))
                {
                    throw new Exception("Client address not specified");
                }

                var client = new HttpClient();
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if(tokenname != string.Empty)
                {
                    if (tokenname == "Authorization")
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(tokenname, token);
                    }
                }

                string postbody = JsonConvert.SerializeObject(body);

                var httpresponse = await client.PostAsync(ConfigurationManager.AppSettings["rest.call.endpoint"] + servicename, new StringContent(postbody, Encoding.UTF8, "text/json"));
                var jsonContent = await httpresponse.Content.ReadAsStringAsync();

                if (httpresponse.StatusCode == HttpStatusCode.Forbidden || httpresponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(httpresponse.ReasonPhrase);
                }
                else if (httpresponse.StatusCode != HttpStatusCode.OK && httpresponse.StatusCode != HttpStatusCode.Created)
                {
                    if (string.IsNullOrEmpty(jsonContent))
                    {
                        // rest of the exception
                        throw new Exception(httpresponse.StatusCode + " " + httpresponse.ReasonPhrase);
                    }

                    var responsestatus = JsonConvert.DeserializeObject<ResponseStatusRoot>(jsonContent);

                    throw new Exception(responsestatus.ResponseStatus.Message);
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<T>(jsonContent);
                    return result;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static object IdentityserverTokenRequest<T>(string endpoint, string body = "") where T : new()
        {
            try
            {
                var client = new RestClient(endpoint);
                var request = new RestRequest();

                request.Method = Method.POST;
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);

                var taskCompletionSource = new TaskCompletionSource<T>();
                var response = client.Execute<T>(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.Content == "{\"error\":\"invalid_grant\"}")
                    {
                        throw new Exception("Invalid username or password");
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class ResponseStatusRoot
    {
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class ResponseStatus
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
