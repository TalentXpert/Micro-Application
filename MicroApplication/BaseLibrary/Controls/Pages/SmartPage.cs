using BaseLibrary.UI.Controls;

namespace BaseLibrary.Controls.Pages
{
    /// <summary>
    /// This class return page state 
    /// </summary>
    public class SmartPage
    {
        public Guid PageId { get; set; }
        public string PageTitle { get; set; }
        /// <summary>
        /// Last selected filter id
        /// </summary>
        public Guid? CurrentGridFilterId { get; set; }
        /// <summary>
        /// Last selected page size (max rows of grid to be displayed on the page)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Global action those will be added on top of grid
        /// </summary>
        public List<SmartAction> PageActions { get; set; }
        public List<UIControl> GlobalControls { get; set; } // These control will be on list page at the top with a search button and read only on add or update form
        public SmartPage(AppPage appPage, List<SmartAction> pageActions, List<UIControl> globalControls, Guid? currentGridFilterId, int pageSize)
        {

            PageId = appPage.Id;
            PageTitle = appPage.Name;

            PageActions = pageActions;
            CurrentGridFilterId = currentGridFilterId;
            PageSize = pageSize;

            if (globalControls == null)
                globalControls = new List<UIControl>();
            GlobalControls = globalControls;
        }
    }


}
