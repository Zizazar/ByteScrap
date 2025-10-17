using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace _Project.Scripts.ElectricitySystem.Systems.Responses
{
    public class UserMetaResponse
    {
        public UserMetaResponse FromJson(string json)
        {
            var data = JsonConvert.DeserializeObject<UserMetaResponse>(json);
            id = data.id;
            name = data.name;
            return this;
        }
        public int id;
        public string name;
    }

    public class SaveDataResponse
    {
        public SaveDataResponse FromJson(string json)
        {
            var dat = JsonConvert.DeserializeObject<SaveDataResponse>(json);
            id = dat.id;
            name = dat.name;
            owner_id = dat.owner_id;
            data = dat.data;
            return this;
        }
        public int id;
        public string name;
        public int owner_id;
        public string data;
    }

   
}