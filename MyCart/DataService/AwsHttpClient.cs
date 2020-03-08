using DigiFyy.Data;
using DigiFyy.Models.AWS;
using DigiFyy.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.DataService
{
    public enum RestType { LoginUser = 1, GetInfo = 2, RegisterUID  = 3, UpdateStatus  = 4, GetManufacturers  = 5, GetMessages = 6, MarkReadMessages  = 7}
    public class AwsHttpClient
    {
        private HttpClient client;
        IAnalyticsService AnalyticsService;
        public string LastErrorMessage { get; set; } = "";

        public AwsHttpClient(IAnalyticsService analyticsService, RestType restType)
        {
            AnalyticsService = analyticsService;

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
                //      Proxy = new System.Net.WebProxy("http://127.0.0.1:8888"),
                //      UseProxy = false,
            };

            LookupRestTypeParameters(restType, out string restUrl, out string apiKey);
           

            client = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 10, 0),
                BaseAddress = new Uri(restUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();

            //var authenticationBytes = Encoding.ASCII.GetBytes(Utils.ReadConfigString("Username", "") + ":" + Utils.ReadConfigString("Password", ""));
            //client.DefaultRequestHeaders.Authorization =
            //      new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void LookupRestTypeParameters(RestType restType, out string restUrl, out string apiKey)
        {
            restUrl = "";
            apiKey = "";
            switch (restType)
            {
                case RestType.LoginUser:
                    restUrl = Constants.RestUrl_LoginUser;
                    apiKey = Constants.ApiKey_LoginUser;
                    break;
                case RestType.GetInfo:
                    restUrl = Constants.RestUrl_GetInfo;
                    apiKey = Constants.ApiKey_GetInfo;
                    break;
                case RestType.RegisterUID:
                    restUrl = Constants.RestUrl_RegisterUID;
                    apiKey = Constants.ApiKey_RegisterUID;
                    break;
                case RestType.UpdateStatus:
                    restUrl = Constants.RestUrl_UpdateStatus;
                    apiKey = Constants.ApiKey_UpdateStatus;
                    break;
                case RestType.GetManufacturers:
                    restUrl = Constants.RestUrl_GetManufacturers;
                    apiKey = Constants.ApiKey_GetManufacturers;
                    break;

                case RestType.GetMessages:
                    restUrl = Constants.RestUrl_GetMessages;
                    apiKey = Constants.ApiKey_GetMessages;
                    break;
                case RestType.MarkReadMessages:
                    restUrl = Constants.RestUrl_MarkReadMessages;
                    apiKey = Constants.ApiKey_MarkReadMessages;
                    break;
            }
        }

        public void SetRestType(RestType restType)
        {
            LookupRestTypeParameters(restType, out string restUrl, out string apiKey);
            client.BaseAddress = new Uri(restUrl);
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }
        

      
        public async Task<Models.AWS.UserResponse> LoginUserAsync(Models.AWS.User user)
        {
            AnalyticsService.TrackEvent("Requesting LoginUserAsync()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(user);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.UserResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

   
        public async Task<Models.AWS.UIDInfoResponse> GetInfoAsync(Models.AWS.UIDInfoRequest uIDInfoRequest)
        {
            AnalyticsService.TrackEvent("Requesting GetInfoAsync()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.UIDInfoResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

        public async Task<Models.AWS.FrameNumberResponse> RegisterUIDAsync(Models.AWS.UniqueID uniqueID)
        {
            AnalyticsService.TrackEvent("Requesting RegisterUID()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uniqueID);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.FrameNumberResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

        public async Task<Models.AWS.FrameNumberStatusResponse> UpdateStatusAsync(Models.AWS.FrameNumberStatusRequest frameNumberStatusRequest)
        {
            AnalyticsService.TrackEvent("Requesting UpdateStatus()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(frameNumberStatusRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.FrameNumberStatusResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

        public async Task<Models.AWS.ManufacturerResponse> GetManufacturersAsync(Models.AWS.UIDInfoRequest uIDInfoRequest)
        {
            AnalyticsService.TrackEvent("Requesting GetManufacturersAsync()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.ManufacturerResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

        public async Task<Models.AWS.MessagesResponse> GetMessagesAsync(Models.AWS.UIDInfoRequest uIDInfoRequest)
        {
            AnalyticsService.TrackEvent("Requesting GetMessages()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.MessagesResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

        public async Task<Models.AWS.StatusResponse> MarkReadMassagesAsync(Models.AWS.MarkMessageRequest request)
        {
            AnalyticsService.TrackEvent("Requesting GetMessages()");
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(request);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync("", new StringContent(postBody, Encoding.UTF8, "application/json"));
                AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.StatusResponse>(str, settings);
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                    LastErrorMessage = str;
                    return null;
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
                LastErrorMessage = hre.Message;
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
                LastErrorMessage = ex.Message;
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return null;
        }

    }
}
