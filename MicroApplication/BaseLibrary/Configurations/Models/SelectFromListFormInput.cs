namespace BaseLibrary.Configurations.Models
{
    /// <summary>
    /// This class is used to hold data needed to render SelectFromList form.
    /// </summary>
    public class SelectFromListFormInput
    {
        /// <summary>
        /// Title that will be displayed on dialog header or panel header.
        /// </summary>
        public string FormTitle { get; set; }
        /// <summary>
        /// Which form is being rendered. This will also help to identify which form handler should be used to process this form.
        /// </summary>
        public Guid FormId { get; set; }
        /// <summary>
        /// The domain entity for which this form will render associated list of items like Users, Organizations etc.
        /// </summary>
        public Guid EntityId { get; set; }
        /// <summary>
        /// This label will show entity name like User, Organization etc.
        /// </summary>
        public string FormDataLabel { get; set; }
        /// <summary>
        /// This value will show entity value like User Name, Organization Name etc.
        /// </summary>
        public string FormDataValue { get; set; }
        /// <summary>
        /// This label will show items name like Roles, Groups etc from which user will select one or more items.
        /// </summary>
        public string ItemLabel { get; set; }
        /// <summary>
        /// List of items to show in the select from list form
        /// </summary>
        public List<SelectFromListFormInputItem> Items { get; set; }

        /// <summary>
        /// Prepare this object to hold data needed to render SelectFromList form.
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="formId"></param>
        /// <param name="entityId"></param>
        /// <param name="formDataLabel"></param>
        /// <param name="formDataValue"></param>
        /// <param name="itemLabel"></param>
        public SelectFromListFormInput(string formTitle, Guid formId, Guid entityId, string formDataLabel, string formDataValue,string itemLabel)
        {
            FormTitle = formTitle;
            FormId = formId;
            EntityId = entityId;
            FormDataLabel = formDataLabel;
            FormDataValue = formDataValue;
            ItemLabel = itemLabel;
            Items = [];
        }
        public void AddItem(SelectFromListFormInputItem item)
        {
            Items.Add(item);
        }
        public void AddItem(IEnumerable<SelectFromListFormInputItem> items)
        {
            Items.AddRange(items);
        }
    }

    /// <summary>
    /// This class hold data for each item that will be shown in SelectFromList form.
    /// </summary>
    public class SelectFromListFormInputItem
    {
        /// <summary>
        /// List item unique identifier e.g RoleId
        /// </summary>
        public Guid ItemId { get; set; }
        /// <summary>
        /// List item title that will be displayed e.g Role Name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Additional information that can be displayed below title as single line or multiple lines. e.g Role Permissions 
        /// </summary>
        public List<string> DetailLines { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the item is selected.
        /// </summary>
        public bool IsSelected { get; set; }
        public SelectFromListFormInputItem(string title, Guid itemId)
        {
            DetailLines = [];
            Title = title;
            ItemId = itemId;
        }
        public SelectFromListFormInputItem(string title, Guid itemId, bool isSelected)
        {
            DetailLines = [];
            Title = title;
            ItemId = itemId;
            IsSelected = isSelected;
        }
    }
}
