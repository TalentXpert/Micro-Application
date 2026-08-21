
using Newtonsoft.Json;

namespace BaseLibrary.Utilities.Adapters
{

    public class NewtonsoftJsonAdapter : CleanCode
    {
        public static T? DeserializeObject<T>(string json)
        {
            try
            {
                if (IsNullOrEmpty(json))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception e) 
            {
                var m = e.Message;
                throw new ValidationException($"{typeof(T).Name} is not serializable from [{json}].");
            }
        }

        public static string SerializeObject(object inputObject)
        {
            return JsonConvert.SerializeObject(inputObject);
        }
    }
}
