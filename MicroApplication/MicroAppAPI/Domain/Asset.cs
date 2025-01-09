using System.Text.RegularExpressions;

namespace MicroAppAPI.Domain
{
    public class PollutionData : Entity
    {
        public string DataJson { get; set; }
        public string SensorId { get; set; }
        public decimal? NoisePollution { get; set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        public static PollutionData Create(string data, string sensorId)
        {
            PollutionData pd = new PollutionData()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                DataJson = data,
                SensorId = sensorId,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                NoisePollution = GetNoisePollution(data)
            };
            return pd;
        }
        private static decimal? GetNoisePollution(string dataJson)
        {
            if (string.IsNullOrWhiteSpace(dataJson) == false)
            {
                var parts = dataJson.Split(":");
                if (parts.Length == 2)
                {
                    string data = parts[1];
                    if (decimal.TryParse(data, out var noise))
                        return noise;
                }
            }
            return null;
        }
    }


    public class PollutionDataVM
    {
        public Guid Key { get; set; }
        public string Data { get; set; }
        public string SensorId { get; set; }
    }

    public class Asset : FormStoreBase
    {
        public string? InventoryNumber { get; set; }
        public string? Tag { get; set; }
        public decimal Quantity { get; set; }
        public Guid GroupId { get; set; }
        public Guid SubgroupId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        protected Asset() { }
        public Asset(ApplicationUser loggedInUser) : base(loggedInUser)
        {

        }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        public void Update(SmartFormTemplateRequest model)
        {
            UpdateData(model.ControlValues);
            Name = model.ControlValues.GetControlFirstValue(BaseControls.Name);
            Description = model.ControlValues.GetControlFirstValue(BaseControls.Description);
            GroupId = model.ControlValues.GetControlFirstValueAsGuid(ApplicationControls.AssetGroup).Value;
            SubgroupId = model.ControlValues.GetControlFirstValueAsGuid(ApplicationControls.AssetSubgroup).Value;
            InventoryNumber = model.ControlValues.GetControlFirstValue(ApplicationControls.InventoryNumber);
            Tag = model.ControlValues.GetControlFirstValue(BaseControls.Tag);
            Quantity = DecimalNumber.Convert(model.ControlValues.GetControlFirstValue(BaseControls.Quantity), 0.0m);
            SetUpdatedOn();
        }
    }
}
