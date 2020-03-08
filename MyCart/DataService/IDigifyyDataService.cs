using DigiFyy.Models.AWS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.DataService
{
    public interface IDigifyyDataService
    {
        Task<User> LoginUser(string userName, string password);

        Task<UIDInfo> GetInfo(string userName, string token, string uuid);

        Task<bool> UpdateStatus(string userName, string token, string uuid, FrameNumberStatus frameNumberStatus);

        Task<List<Manufacturer>> GetManufacturers(string userName, string token, string uuid);

        Task<List<Message>> GetMessages(string userName, string token, string uuid);

        Task<bool> MarkReadMesages(string userName, string token, string uuid, List<int> massageIDList);

        Task<FrameNumber> RegisterUUID(string userName, string token, string uuid, UniqueID uniqueID);

    }
}
