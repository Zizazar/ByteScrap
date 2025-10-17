using Newtonsoft.Json;

namespace _Project.Scripts.ElectricitySystem.Systems.Responses
{
    public class BaseRequest
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
    public class SavesUploadRequest : BaseRequest
    {
        
        public string name;
        public string data; // TODO: заменить на данные сейва
    }

    public class SaveUpload : BaseRequest
    {
        public SaveUpload(string name, string data)
        {
            this.name = name;
            this.data = data;
        }

        public string name;
        public string data;
    }
}