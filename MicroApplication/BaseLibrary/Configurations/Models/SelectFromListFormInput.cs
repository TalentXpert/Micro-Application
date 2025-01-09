namespace BaseLibrary.Configurations.Models
{
    public class SelectFromListFormInput
    {
        public string FormTitle { get; set; }
        public Guid FormId { get; set; }
        public Guid EntityId { get; set; }
        public string FormDataLabel { get; set; }
        public string FormDataValue { get; set; }
        public string ItemLabel { get; set; }
        public List<SelectFromListFormInputItem> Items { get; set; }
        public SelectFromListFormInput(string formTitle, Guid formId, Guid entityId, string formDataLabel, string formDataValue,string itemLabel)
        {
            FormTitle = formTitle;
            FormId = formId;
            EntityId = entityId;
            FormDataLabel = formDataLabel;
            FormDataValue = formDataValue;
            ItemLabel = itemLabel;
            Items = new List<SelectFromListFormInputItem>();
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

    public class SelectFromListFormInputItem
    {
        public Guid ItemId { get; set; }
        public string Title { get; set; }
        public List<string> DetailLines { get; set; }
        public bool IsSelected { get; set; }
        public SelectFromListFormInputItem(string title, Guid itemId)
        {
            DetailLines = new List<string>();
            Title = title;
            ItemId = itemId;
        }
        public SelectFromListFormInputItem(string title, Guid itemId, bool isSelected)
        {
            DetailLines = new List<string>();
            Title = title;
            ItemId = itemId;
            IsSelected = isSelected;
        }
    }
}
