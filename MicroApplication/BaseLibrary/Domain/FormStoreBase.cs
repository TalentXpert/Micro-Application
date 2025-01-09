using System.Reflection;

namespace BaseLibrary.Domain
{
    /// <summary>
    /// This is base class for each entity that will have custom dynamic controls
    /// </summary>
    public abstract class FormStoreBase : Entity
    {
        public Guid? OrganizationId { get; protected set; }
        public string? Data { get; protected set; } // hold data for custom fields. 
        public Guid? AddedByUserId { get; protected set; }
        protected FormStoreBase() { }
        protected FormStoreBase(ApplicationUser loggedInUser)
        {
            Id = IdentityGenerator.NewSequentialGuid();
            AddedByUserId = loggedInUser.Id;
            OrganizationId = loggedInUser.OrganizationId;
            SetCreatedOn();
            SetUpdatedOn();
        }
        protected FormStoreBase(string id)
        {
            Id = Guid.Parse(id);
            AddedByUserId = null;
            OrganizationId = null;
            SetCreatedOn();
            SetUpdatedOn();
        }
        protected void UpdateData(List<ControlValue> controlValues)
        {
            if (IsNull(controlValues))
                controlValues = new List<ControlValue>();
            var properties = GetPublicProperties();
            foreach (var property in properties)
            {
                var v = controlValues.FirstOrDefault(c => c.ControlIdentifier == property);
                if (v == null) continue;
                controlValues.Remove(v);
            }
            var formData = new FormData();
            foreach (var value in controlValues)
            {
                formData.AddValue(value.ControlIdentifier, value.GetFirstValue());
            }
            SetDataField(formData);
            SetUpdatedOn();
        }
        protected void UpdateData(Dictionary<string, string?> values)
        {
            var formData = new FormData();
            foreach (var value in values)
            {
                formData.AddValue(value.Key, value.Value);
            }
            SetDataField(formData);
            SetUpdatedOn();
        }

        private void SetDataField(FormData? formData)
        {
            Data = FormData.SerializeData(formData);
        }
        
        private FormData? formData ;
        public string? GetValue(string key)
        {
            if (formData == null)
                formData = FormData.DeserializeData(Data);
            if (formData.HasValue(key))
                return formData.GetValue(key);
            return GetPropValue(key)?.ToString();
        }

        private object? GetPropValue(string propName)
        {
            return this.GetType().GetProperty(propName)?.GetValue(this, null);
        }
        private List<string> GetPublicProperties()
        {
            Type type = GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public);
            return props.Select(p => p.Name).ToList();
        }
        public Guid GetOrganizationId()
        {
            if(OrganizationId.HasValue) return OrganizationId.Value;
            throw new ValidationException($"You must be part of an organization to perform this operation.");
        }
    }
}
