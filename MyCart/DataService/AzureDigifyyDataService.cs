using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DigiFyy.Models.AWS;
using DigiFyy.Services;

namespace DigiFyy.DataService
{
    public class AzureDigifyyDataService : IDigifyyDataService
    {
        readonly IAnalyticsService AnalyticsService;
        private readonly HttpClient client;

        public AzureDigifyyDataService(IAnalyticsService analyticsService)
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
                BaseAddress = new Uri(Constants.AzureRestUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<UIDInfo> GetInfo(string userName, string token, string uuid)
        {
            if (uuid == "")
                return new UIDInfo() { UID = uuid, FrameNumber = null };
            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_GetInfo);
            
            try
            {
                string postBody = JsonConvert.SerializeObject(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_GetInfo, new StringContent(postBody, Encoding.UTF8, "application/json"));
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
                    Models.AWS.UIDInfoResponse infoResponse = JsonConvert.DeserializeObject<Models.AWS.UIDInfoResponse>(str, settings);

                    if (infoResponse == null || infoResponse?.UIDInfo == null)
                        return new UIDInfo() { UID = uuid, FrameNumber = null };

                    return infoResponse.UIDInfo;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return new UIDInfo() { UID = uuid, FrameNumber = null };
        }

        public async Task<List<Manufacturer>> GetManufacturers(string userName, string token, string uuid)
        {
            if (userName == "" || token == "" || uuid == "")
                return new List<Manufacturer>();

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_GetManufacturers);
            try
            {   

                string postBody = JsonConvert.SerializeObject(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_GetMessages, new StringContent(postBody, Encoding.UTF8, "application/json"));
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
                    ManufacturerResponse manufactuerersResponse = JsonConvert.DeserializeObject<ManufacturerResponse>(str, settings);

                    if (manufactuerersResponse == null || manufactuerersResponse?.Manufacturers == null)
                        return new List<Manufacturer>();

                    return manufactuerersResponse.Manufacturers;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return new List<Manufacturer>();



        }

        public async Task<List<Message>> GetMessages(string userName, string token, string uuid)
        {
            if (userName == "" || token == "" || uuid == "")
                return new List<Message>();


            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_GetMessages);

          
            try
            {
                string postBody = JsonConvert.SerializeObject(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_GetMessages, new StringContent(postBody, Encoding.UTF8, "application/json"));
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
                    Models.AWS.MessagesResponse messageResponse = JsonConvert.DeserializeObject<Models.AWS.MessagesResponse>(str, settings);

                    if (messageResponse == null || messageResponse?.Messages == null)
                        return new List<Message>();

                    return messageResponse.Messages;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
             return new List<Message>();

        }

        public async Task<List<ManufacturerSpec>> GetSpecs(string userName, string token, string uuid)
        {
            if (uuid == "")
                return new List<ManufacturerSpec>();

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_GetSpecs);


            try
            {
                string postBody = JsonConvert.SerializeObject(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_GetSpecs, new StringContent(postBody, Encoding.UTF8, "application/json"));
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
                    Models.AWS.ManufacturerSpecResponse specsResponse = JsonConvert.DeserializeObject<Models.AWS.ManufacturerSpecResponse>(str, settings);

                    if (specsResponse == null || specsResponse?.Specs == null)
                        return new List<ManufacturerSpec>();

                    return specsResponse.Specs;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return new List<ManufacturerSpec>();

        }


        public async Task<User> LoginUser(string userName, string password)
        {
            if (userName == "" || password == "")
                return new Models.AWS.User() { Username = userName, UserID = 0, Token = "" };

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_LoginUser);

            try
            {
                string postBody = JsonConvert.SerializeObject(new User() { Username = userName, Password = password });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_LoginUser, new StringContent(postBody, Encoding.UTF8, "application/json"));
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
                    Models.AWS.UserResponse userResponse = JsonConvert.DeserializeObject<Models.AWS.UserResponse>(str, settings);
                    if (userResponse == null || userResponse?.User == null)
                        return new User() { Username = userName, UserID = 0, Token = "" };

                    return userResponse.User;

                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return new User() { Username = userName, UserID = 0, Token = "" };
        }

        public async Task<bool> MarkReadMesages(string userName, string token, string uuid, List<int> messageIDList)
        {
            if (userName == "" || token == "" || uuid == "" || messageIDList == null)
                return false;

            if (messageIDList.Count == 0)
                return true;

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_MarkReadMessages);

            try
            {
                string postBody = JsonConvert.SerializeObject(new MarkMessageRequest() { Username = userName, Token=token, UID=uuid, MessageIDList=messageIDList});

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_MarkReadMessages, new StringContent(postBody, Encoding.UTF8, "application/json"));
           
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(str, settings);

                    return statusResponse != null;


                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }
            return false;
        }

       

        public async Task<FrameNumberExtra> RegisterExtra(string userName, string token, string uuid, FrameNumberExtra frameNumberExtra, bool deleteExtra)
        {
            if (userName == "" || token == "" || uuid == "" || frameNumberExtra == null)
                return new FrameNumberExtra();

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_RegisterExtra);

            try
            {
                string postBody = JsonConvert.SerializeObject(new FrameNumberExtraRequest() { Username = userName, Token = token, UID = uuid, FrameNumberExtra = frameNumberExtra, DeleteExtra = deleteExtra });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_RegisterExtra, new StringContent(postBody, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //     AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    FrameNumberExtraResponse frameNumberExtraResponse = JsonConvert.DeserializeObject<FrameNumberExtraResponse>(str, settings);
                    if (frameNumberExtraResponse == null || frameNumberExtraResponse?.FrameNumberExtra == null)
                        return new FrameNumberExtra(); // empty

                    return frameNumberExtraResponse.FrameNumberExtra;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);

            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }

            return new FrameNumberExtra();


        }

        public async Task<FrameNumberImage> RegisterImage(string userName, string token, string uuid, FrameNumberImage frameNumberImage, bool deleteImage)
        {
            if (uuid == "" || frameNumberImage == null)
                return new FrameNumberImage();

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_RegisterImage);

            try
            {
                string postBody = JsonConvert.SerializeObject(new FrameNumberImageRequest() { Username = userName, Token = token, UID = uuid, FrameNumberImage = frameNumberImage, DeleteImage = deleteImage });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_RegisterImage, new StringContent(postBody, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //     AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    FrameNumberImageResponse frameNumberImageResponse = JsonConvert.DeserializeObject<FrameNumberImageResponse>(str, settings);
                    if (frameNumberImageResponse == null || frameNumberImageResponse?.FrameNumberImage == null)
                        return new FrameNumberImage(); // empty

                    return frameNumberImageResponse.FrameNumberImage;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);

            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }

            return new FrameNumberImage();
        }

        public Task<FrameNumber> RegisterUUID(UniqueID uniqueID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatus(string userName, string token, string uuid, FrameNumberStatus frameNumberStatus)
        {
            if (userName == "" || token == "" || uuid == "" || frameNumberStatus == null)
                return false;

            if (client.DefaultRequestHeaders.Contains("code"))
                client.DefaultRequestHeaders.Remove("code");
            client.DefaultRequestHeaders.Add("code", Constants.AzureApiKey_UpdateStatus);

            try
            {
                string postBody = JsonConvert.SerializeObject(new FrameNumberStatusRequest() { Username = userName, Token = token, UID = uuid, FrameNumberStatus = frameNumberStatus });

                HttpResponseMessage response = await client.PostAsync(Constants.AzureEndPoint_UpdateStatus, new StringContent(postBody, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    //     AnalyticsService.TrackEvent(str);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    FrameNumberStatusResponse frameNumberStatusResponse = JsonConvert.DeserializeObject<FrameNumberStatusResponse>(str, settings);
                    return (frameNumberStatusResponse == null || frameNumberStatusResponse?.FrameNumberStatus == null) ? false : true;

                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    AnalyticsService.TrackEvent(str);
                }
            }
            catch (HttpRequestException hre)
            {
                AnalyticsService.TrackError(hre);             
            }
            catch (TaskCanceledException hca)
            {
                AnalyticsService.TrackError(hca);
                AnalyticsService.TrackEvent("Request canceled");
            }
            catch (Exception ex)
            {
                AnalyticsService.TrackError(ex);
             
            }
            finally
            {
                /*if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }*/
            }

            return true;

        }

        public Task<FrameNumberDocument> RegisterDocument(string userName, string token, string uuid, FrameNumberDocument frameNumberDocument, bool deleteDocument)
        {
            throw new NotImplementedException();
        }

        public Task<FrameNumberStatus> GetStatus(string userName, string token, string uuid)
        {
            throw new NotImplementedException();
        }
    }
}
