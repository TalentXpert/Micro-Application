namespace BaseLibrary.Configurations.Models
{
    public class SelectFromListFormData
    {
        /// <summary>
        /// This is the form id to get right form handler
        /// </summary>
        public Guid FormId { get; set; }
        /// <summary>
        /// This is the primary entity 
        /// </summary>
        public Guid EntityId { get; set; }
        /// <summary>
        /// Selected items to attach to primary entity.
        /// </summary>
        public List<Guid> SelectedItems { get; set; } = [];
    }
}
