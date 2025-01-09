
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
            catch
            {
                throw new ValidationException($"{typeof(T).Name} can is not serializable from [{json}].");
            }
        }

        public static string SerializeObject(object inputObject)
        {
            return JsonConvert.SerializeObject(inputObject);
        }
    }
}
