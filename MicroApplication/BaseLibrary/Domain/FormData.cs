namespace BaseLibrary.Domain
{
    public class FormData
    {
        public Dictionary<string, string?> Values { get; set; } = new Dictionary<string, string?>();
        public FormData() { }
        public FormData(List<ControlValue> controlValues)
        {
            foreach (var value in controlValues)
            {
                AddValue(value.ControlIdentifier, value.GetFirstValue());
            }
        }
        public void AddValue(string key, string? value)
        {
            Values[key] = value;
        }
        public string? GetValue(string key)
        {
            if (Values.TryGetValue(key, out var value))
                return value;
            return null;
        }
        public bool HasValue(string key)
        {
            return Values.ContainsKey(key);
        }
        public static string SerializeData(FormData? formData)
        {
            if (formData == null)
                formData = new FormData();

            var data = NewtonsoftJsonAdapter.SerializeObject(formData);
            return data;
        }
        public static string SerializeData(object data)
        {
            var d = NewtonsoftJsonAdapter.SerializeObject(data);
            return d;
        }
        public static FormData DeserializeData(string? data)
        {
            try
            {
                if (data == null)
                    return new FormData();

                var formData = GetFormData(data);
                if (formData != null)
                    return formData;

                formData = GetFormDataFromControlValues(data);
                if (formData != null)
                    return formData;


                return new FormData();
            }
            catch
            {
                return new FormData();
            }
        }

        

        private static FormData? GetFormData(string data)
        {
            try
            {
                var formData = NewtonsoftJsonAdapter.DeserializeObject<FormData>(data);
                return formData;
            }
            catch
            {
                return null;
            }
        }
        private static FormData? GetFormDataFromControlValues(string data)
        {
            try
            {
                var values = NewtonsoftJsonAdapter.DeserializeObject<List<ControlValue>>(data);
                if (values != null)
                    return new FormData(values);
                return null;
            }
            catch
            {
                return null;
            }
        }
        public static T? DeserializeData<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return default(T);
            var d = NewtonsoftJsonAdapter.DeserializeObject<T>(data);
            return d;
        }
    }
}
