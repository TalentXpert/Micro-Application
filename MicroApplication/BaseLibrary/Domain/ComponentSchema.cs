using BaseLibrary.Domain.ComponentSchemas;

namespace BaseLibrary.Domain
{
    public class ComponentTypes
    {
        public string Name { get; set; }
        private ComponentTypes(string name) { Name = name; }
        public static ComponentTypes Chart = new ComponentTypes("Chart");
        public static ComponentTypes Dashboard = new ComponentTypes("Dashboard");
    }

    public class ComponentSchema : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public Guid? OrganizationId { get; protected set; }
        public Guid? AddedByUserId { get; protected set; }
        public Guid? UpdatedByUserId { get; protected set; }
        public string? Data { get; protected set; } // hold data for custom fields. 
        public string ComponentType { get; private set; }
        protected ComponentSchema()
        {
            ComponentType = ComponentTypes.Dashboard.Name;
        }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public static ComponentSchema Create(Guid? id, ComponentTypes componentType, string name, string description, string data, Guid? organizationId, Guid? loggedInUserId)
        {
            if (id.HasValue == false)
                id = IdentityGenerator.NewSequentialGuid();
            var cs = new ComponentSchema()
            {
                Id = id.Value,
                ComponentType = componentType.Name,
                OrganizationId = organizationId,
                AddedByUserId = loggedInUserId
            };
            cs.Update(name, description, data, loggedInUserId);
            cs.SetCreatedOn();
            return cs;
        }
        public void Update(string name, string description, string data, Guid? loggedInUserId)
        {
            Name = name;
            Description = description;
            UpdatedByUserId = loggedInUserId;
            Data = data;
            SetUpdatedOn();
        }

        public DashboardSchema GetDashboardSchema()
        {
            var d = NewtonsoftJsonAdapter.DeserializeObject<DashboardSchema>(Data);
            d.Id = this.Id;
            return d;
        }
        public ChartSchema GetChartSchema()
        {
            var d = NewtonsoftJsonAdapter.DeserializeObject<ChartSchema>(Data);
            d.Id = this.Id;
            return d;
        }
    }
}
