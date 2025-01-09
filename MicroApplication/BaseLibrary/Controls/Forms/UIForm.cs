using BaseLibrary.UI.Controls;

namespace BaseLibrary.Controls.Forms
{
    /// <summary>
    /// This class represent a user interface form that will be render on html page
    /// </summary>
    public class UIForm
    {
        public Guid Id { get; set; } //FormId of the application
        public string Title { get; set; } = string.Empty;
        public List<UIControl> UIControls { get; }
        public string SubmitButtonText { get; set; } = "Submit";
        /// <summary>
        /// Primary key of database table to uniquely represent related form data for a single record like JobId, CandidateId, AssetId
        /// </summary>
        public Guid? DataKey { get; set; }
        public UIForm(AppForm appForm) 
        {
            Id = appForm.Id;
            Title = appForm.Name;
            UIControls = new List<UIControl>();
        }
        
        public void AddControl(UIControl uiControl)
        {
            uiControl.SetPosition(UIControls.Count + 1);
            UIControls.Add(uiControl);
        } 
    }
}
