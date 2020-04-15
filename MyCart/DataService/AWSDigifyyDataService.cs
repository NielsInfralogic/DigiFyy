using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiFyy.Models.AWS;
using DigiFyy.Services;

namespace DigiFyy.DataService
{
    public class AWSDigifyyDataService : IDigifyyDataService
    {
        readonly AwsHttpClient awsHttpClient;
        readonly IAnalyticsService AnalyticsService;
        public AWSDigifyyDataService(IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
            awsHttpClient = new AwsHttpClient(AnalyticsService);
        }

        public async Task<User> LoginUser(string userName, string password)
        {
            if (userName == "" ||password == "" )
                return new User() { Username = userName, UserID = 0, Token = "" };

            Models.AWS.UserResponse response = await awsHttpClient.LoginUserAsync(new User() { Username = userName, Password = password });

            if (response == null || response?.User == null)
                return new User() { Username = userName, UserID = 0, Token = "" };

            return response.User;
        }

        public async Task<UIDInfo> GetInfo(string userName, string token, string uuid)
        {
            if ( uuid == "")
                return new UIDInfo() { UID = uuid, FrameNumber = null };

            Models.AWS.UIDInfoResponse response = await awsHttpClient.GetInfoAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.UIDInfo == null)
                return new UIDInfo() { UID = uuid, FrameNumber = null };

            return response.UIDInfo;
        }

        public async Task<List<ManufacturerSpec>> GetSpecs(string userName, string token, string uuid)
        {
            if (uuid == "")
                return new List<ManufacturerSpec>();

            Models.AWS.ManufacturerSpecResponse response = await awsHttpClient.GetSpecsAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.Specs == null)
                return new List<ManufacturerSpec>();

            return response.Specs;
        }

        public async Task<bool> UpdateStatus(string userName, string token, string uuid, FrameNumberStatus frameNumberStatus)
        {
            if (uuid == "" || frameNumberStatus == null)
                return false;
          
            FrameNumberStatusResponse response = await awsHttpClient.UpdateStatusAsync(new FrameNumberStatusRequest() { Username = userName, Token = token, UID = uuid, FrameNumberStatus = frameNumberStatus });

            if (response == null || response?.FrameNumberStatus == null)
                return false;

            return true;
        }

        public async Task<List<Manufacturer>> GetManufacturers(string userName, string token, string uuid)
        {
            if ( uuid == "" )
                return new List<Manufacturer>();

            ManufacturerResponse response = await awsHttpClient.GetManufacturersAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.Manufacturers == null)
                return new List<Manufacturer>();

            return response.Manufacturers;
        }

        public async Task<List<Message>> GetMessages(string userName, string token, string uuid)
        {
            if (userName == "" || token == "" || uuid == "")
                return new List<Message>();
       
            MessagesResponse response = await awsHttpClient.GetMessagesAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.Messages == null)
                return new List<Message>();

            return response.Messages;
        }

        public async Task<bool> MarkReadMesages(string userName, string token, string uuid, List<int> messageIDList)
        {
            if (userName == "" || token == "" || uuid == "" || messageIDList == null)
                return false;

            if (messageIDList.Count == 0)
                return true;

            StatusResponse response = await awsHttpClient.MarkReadMassagesAsync(new MarkMessageRequest() { Username = userName, Token = token, UID = uuid, MessageIDList = messageIDList });

            return response != null;
        }

        public async Task<FrameNumber> RegisterUUID(UniqueID uniqueID)
        {

            if (uniqueID == null)
                return new FrameNumber();
            if (uniqueID.UID == "" || uniqueID.Username == "" || uniqueID.Password == "")
                return new FrameNumber();

            FrameNumberResponse response = await awsHttpClient.RegisterUIDAsync(uniqueID);

            if (response == null || response?.FrameNumber == null)
               return new FrameNumber(); // empty

            return response.FrameNumber;
        }

        public async Task<FrameNumberExtra> RegisterExtra(string userName, string token, string uuid, FrameNumberExtra frameNumberExtra, bool deleteExtra)
        {
            if (userName == "" || token == "" || uuid == "" || frameNumberExtra == null)
                return new FrameNumberExtra();

            FrameNumberExtraResponse response = await awsHttpClient.RegisterExtra(new FrameNumberExtraRequest() { Username = userName, Token = token, UID = uuid, FrameNumberExtra = frameNumberExtra, DeleteExtra = deleteExtra });

            if (response == null || response?.FrameNumberExtra == null)
                return new FrameNumberExtra(); // empty

            return response.FrameNumberExtra;
        }

        public async Task<FrameNumberImage> RegisterImage(string userName, string token, string uuid, FrameNumberImage frameNumberImage, bool deleteImage)
        {
            if (uuid == "" || frameNumberImage == null)
                return new FrameNumberImage();

            FrameNumberImageResponse response = await awsHttpClient.RegisterImage(new FrameNumberImageRequest() { Username = userName, Token = token, UID = uuid, FrameNumberImage = frameNumberImage, DeleteImage = deleteImage });

            if (response == null || response?.FrameNumberImage == null)
                return new FrameNumberImage(); // empty

            return response.FrameNumberImage;
        }

        public async Task<FrameNumberDocument> RegisterDocument(string userName, string token, string uuid, FrameNumberDocument frameNumberDocument, bool deleteDocument)
        {
            if (uuid == "" || frameNumberDocument == null)
                return new FrameNumberDocument();

            FrameNumberDocumentResponse response = await awsHttpClient.RegisterDocument(new FrameNumberDocumentRequest() { Username = userName, Token = token, UID = uuid, FrameNumberDocument = frameNumberDocument, DeleteDocument = deleteDocument });

            if (response == null || response?.FrameNumberDocument == null)
                return new FrameNumberDocument(); // empty

            return response.FrameNumberDocument;
        }

        public async Task<FrameNumberStatus> GetStatus(string userName, string token, string uuid)
        {
            if (uuid == "")
                return new  FrameNumberStatus();

            Models.AWS.FrameNumberStatusResponse response = await awsHttpClient.GetStatusAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.FrameNumberStatus == null)
                return new FrameNumberStatus();

            return response.FrameNumberStatus;
        }
    }
}
