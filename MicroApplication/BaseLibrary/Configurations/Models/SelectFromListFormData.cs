namespace BaseLibrary.Configurations.Models
{
    public class SelectFromListFormData
    {
        public Guid FormId { get; set; }
        public Guid EntityId { get; set; }
        public List<Guid> SelectedItems { get; set; }
        public SelectFromListFormData()
        {
            SelectedItems = new List<Guid>();
        }
    }
}
