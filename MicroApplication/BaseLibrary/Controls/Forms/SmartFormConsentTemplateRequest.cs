namespace BaseLibrary.Controls.Forms
{
    public class SmartFormConsentTemplateRequest
    {
        public Guid FormId { get; set; }
        public Guid DataKey { get; set; } 
        public bool IsAccepted { get; set; } 
        public string Feedback { get; set; }
       
    }
}
