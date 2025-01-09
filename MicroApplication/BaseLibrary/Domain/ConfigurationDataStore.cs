using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Domain
{
    public class ConfigurationDataStore : Entity
    {
        public string ConfigurationType { get; set; }
        public string Configuration { get; set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

    }
    
    public class ConfigurationTypes
    {
        public string ConfigurationType { get; private set; }
        private ConfigurationTypes(string configurationType)
        {
            ConfigurationType = configurationType;
        }
       // public static DashboardPanelContentType Graph = new DashboardPanelContentType("Graph");
       // public static DashboardPanelContentType Summary = new DashboardPanelContentType("Summary");
    }
}
