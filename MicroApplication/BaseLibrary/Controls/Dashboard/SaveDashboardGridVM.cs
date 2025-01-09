using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Controls.Dashboard
{
    public class SaveDashboardGridVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string DataSource { get; set; }
        public string Description { get; set; }
        public List<AppControlVM> AppControls { get; set; }
        public SaveDashboardGridVM() { AppControls = new List<AppControlVM>(); }
        public PageDataStoreVM GetPageDataStore()
        {
            var vm = new PageDataStoreVM
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Values = new Dictionary<string, string?>()
            };
            foreach(var item in AppControls)
                vm.Values[item.ControlIdentifier]=item.Id.ToString();
            return vm;
        }
    }
}
