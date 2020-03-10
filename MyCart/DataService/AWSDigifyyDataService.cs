using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.DataService
{
    public class AWSDigifyyDataService : IDigifyyDataService
    {
        readonly AwsHttpClient awsHttpClient;
        readonly IAnalyticsService AnalyticsService;
        public AWSDigifyyDataService(IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
            awsHttpClient = new AwsHttpClient(AnalyticsService, RestType.GetMessages);
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
            if (userName == "" || token == "" || uuid == "")
                return new UIDInfo() { UID = uuid, FrameNumber = null };

            Models.AWS.UIDInfoResponse response = await awsHttpClient.GetInfoAsync(new UIDInfoRequest() { Username = userName, Token = token, UID = uuid });

            if (response == null || response?.UIDInfo == null)
                return new UIDInfo() { UID = uuid, FrameNumber = null };

            return response.UIDInfo;
        }


        public async Task<bool> UpdateStatus(string userName, string token, string uuid, FrameNumberStatus frameNumberStatus)
        {
            if (userName == "" || token == "" || uuid == "" || frameNumberStatus == null)
                return false;
          
            FrameNumberStatusResponse response = await awsHttpClient.UpdateStatusAsync(new FrameNumberStatusRequest() { Username = userName, Token = token, UID = uuid, FrameNumberStatus = frameNumberStatus });

            if (response == null || response?.FrameNumberStatus == null)
                return false;

            return true;
        }

        public async Task<List<Manufacturer>> GetManufacturers(string userName, string token, string uuid)
        {
            if (userName == "" || token == "" || uuid == "" )
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

        public async Task<FrameNumber> RegisterUUID(string userName, string token, string uuid, UniqueID uniqueID)
        {

            if (userName == "" || token == "" || uuid == "" || uniqueID == null)
                return new FrameNumber();

            FrameNumberResponse response = await awsHttpClient.RegisterUIDAsync(uniqueID);

            if (response == null || response?.FrameNumber == null)
               return new FrameNumber(); // empty

            return response.FrameNumber;
        }


    }
}
