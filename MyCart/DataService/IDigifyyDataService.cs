using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiFyy.Models.AWS;

namespace DigiFyy.DataService
{
    public interface IDigifyyDataService
    {
        Task<User> LoginUser(string userName, string password);

        Task<UIDInfo> GetInfo(string userName, string token, string uuid);

        Task<FrameNumberStatus> GetStatus(string userName, string token, string uuid);

        Task<List<ManufacturerSpec>> GetSpecs(string userName, string token, string uuid);


        Task<bool> UpdateStatus(string userName, string token, string uuid, FrameNumberStatus frameNumberStatus);

        Task<List<Manufacturer>> GetManufacturers(string userName, string token, string uuid);

        Task<List<Message>> GetMessages(string userName, string token, string uuid);


        Task<bool> MarkReadMesages(string userName, string token, string uuid, List<int> massageIDList);

        Task<FrameNumber> RegisterUUID(UniqueID uniqueID);

        Task<FrameNumberExtra> RegisterExtra(string userName, string token, string uuid, FrameNumberExtra frameNumberExtra, bool deleteExtra);

        Task<FrameNumberImage> RegisterImage(string userName, string token, string uuid, FrameNumberImage frameNumberImage, bool deleteImage);

        Task<FrameNumberDocument> RegisterDocument(string userName, string token, string uuid, FrameNumberDocument frameNumberDocument, bool deleteDocument);

    }
}
