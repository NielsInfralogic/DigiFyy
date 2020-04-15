using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DigiFyy.Models.AWS;
using DigiFyy.Services;

namespace DigiFyy.DataService
{
    public enum RestType { LoginUser = 1, GetInfo = 2, RegisterUID  = 3, UpdateStatus  = 4, GetManufacturers  = 5, GetMessages = 6, MarkReadMessages  = 7}
    public class AwsHttpClient
    {
        private readonly HttpClient client;
        readonly IAnalyticsService AnalyticsService;
        public string LastErrorMessage { get; set; } = "";

        public AwsHttpClient(IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
                //      Proxy = new System.Net.WebProxy("http://127.0.0.1:8888"),
                //      UseProxy = false,
            };


            client = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 10, 0),
                BaseAddress = new Uri(Constants.RestUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();

            //var authenticationBytes = Encoding.ASCII.GetBytes(Utils.ReadConfigString("Username", "") + ":" + Utils.ReadConfigString("Password", ""));
            //client.DefaultRequestHeaders.Authorization =
            //      new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
  //          client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
      
        public async Task<Models.AWS.UserResponse> LoginUserAsync(Models.AWS.User user)
        {
            //AnalyticsService.TrackEvent("Requesting LoginUserAsync()");

            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_LoginUser);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(user);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_LoginUser, new StringContent(postBody, Encoding.UTF8, "application/json"));
              //  AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //AnalyticsService.TrackEvent(str);
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
//            AnalyticsService.TrackEvent("Requesting GetInfoAsync()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_GetInfo);
            
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_GetInfo, new StringContent(postBody, Encoding.UTF8, "application/json"));
  //              AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //AnalyticsService.TrackEvent(str);
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
    //        AnalyticsService.TrackEvent("Requesting RegisterUID()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_RegisterUID);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uniqueID);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_RegisterUID, new StringContent(postBody, Encoding.UTF8, "application/json"));
      //          AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
        //            AnalyticsService.TrackEvent(str);
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

        public async Task<Models.AWS.FrameNumberExtraResponse> RegisterExtra(Models.AWS.FrameNumberExtraRequest frameNumberExtraRequest)
        {
            //        AnalyticsService.TrackEvent("Requesting RegisterUID()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_RegisterExtra);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(frameNumberExtraRequest);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_RegisterExtra, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //          AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //            AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.FrameNumberExtraResponse>(str, settings);
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

        public async Task<Models.AWS.FrameNumberImageResponse> RegisterImage(Models.AWS.FrameNumberImageRequest frameNumberImageRequest)
        {
            //        AnalyticsService.TrackEvent("Requesting RegisterUID()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_RegisterImage);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(frameNumberImageRequest);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_RegisterImage, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //          AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //            AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.FrameNumberImageResponse>(str, settings);
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

        public async Task<Models.AWS.FrameNumberDocumentResponse> RegisterDocument(Models.AWS.FrameNumberDocumentRequest frameNumberDocumentRequest)
        {
            //        AnalyticsService.TrackEvent("Requesting RegisterUID()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_RegisterDocument);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(frameNumberDocumentRequest);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_RegisterDocument, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //          AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //            AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.FrameNumberDocumentResponse>(str, settings);
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
           // AnalyticsService.TrackEvent("Requesting UpdateStatus()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_UpdateStatus);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(frameNumberStatusRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_UpdateStatus, new StringContent(postBody, Encoding.UTF8, "application/json"));
             //   AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
               //     AnalyticsService.TrackEvent(str);
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
            //AnalyticsService.TrackEvent("Requesting GetManufacturersAsync()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_GetManufacturers);
            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_GetManufacturers, new StringContent(postBody, Encoding.UTF8, "application/json"));
              //  AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                  //  AnalyticsService.TrackEvent(str);
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
           // AnalyticsService.TrackEvent("Requesting GetMessages()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_GetMessages);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_GetMessages, new StringContent(postBody, Encoding.UTF8, "application/json"));
              //  AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                //    AnalyticsService.TrackEvent(str);
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
            //AnalyticsService.TrackEvent("Requesting GetMessages()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_MarkReadMessages);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(request);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_MarkReadMessages, new StringContent(postBody, Encoding.UTF8, "application/json"));
              //  AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                //    AnalyticsService.TrackEvent(str);
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


        public async Task<Models.AWS.ManufacturerSpecResponse> GetSpecsAsync(Models.AWS.UIDInfoRequest uIDInfoRequest)
        {
            // AnalyticsService.TrackEvent("Requesting GetMessages()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_GetSpecs);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
                //JsonSerializer(product);

                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_GetSpecs, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //  AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //    AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<Models.AWS.ManufacturerSpecResponse>(str, settings);
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

        public async Task<Models.AWS.FrameNumberStatusResponse> GetStatusAsync(Models.AWS.UIDInfoRequest uIDInfoRequest)
        {
            //            AnalyticsService.TrackEvent("Requesting GetInfoAsync()");
            if (client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", Constants.ApiKey_GetStatus);

            LastErrorMessage = "";
            try
            {
                string postBody = JsonConvert.SerializeObject(uIDInfoRequest);
             
                HttpResponseMessage response = await client.PostAsync(Constants.EndPoint_GetStatus, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //              AnalyticsService.TrackEvent("Response: " + response.StatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //AnalyticsService.TrackEvent(str);
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

        
    }
}
