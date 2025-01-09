namespace BaseLibrary.Utilities
{
    public class OrganizationVendorStatus : StringOptionsBase
    {

        public static OrganizationVendorStatus Active = new OrganizationVendorStatus("Active");
        public static OrganizationVendorStatus Disabled = new OrganizationVendorStatus("Disabled");
        public OrganizationVendorStatus(string text) : base(text)
        {
        }
        public override List<StringOptionsBase> GetOptions()
        {
            return new List<StringOptionsBase>
            {
                Active,
                Disabled
            };
        }
        public static bool IsActive(string text)
        {
            GaurdForValidOption(text);
            return Active.Text == text;
        }
        public static void GaurdForValidOption(string text)
        {
            Active.IsValid(text);
        }
    }

    public abstract class StringOptionsBase
    {
        public string Text { get; set; }

        protected StringOptionsBase(string text)
        {
            Text = text;
        }

        public bool IsEqual(string text)
        {
            return text == Text;
        }

        public StringOptionsBase GetOption(string text)
        {
            var option = GetOptions().FirstOrDefault(o => o.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
            if (option == null) return null;
            return option;
        }

        protected void IsValid(string text)
        {
            var option = GetOption(text);
            if (option == null) throw new ValidationException($"{text} is not a valid input.");
        }

        public abstract List<StringOptionsBase> GetOptions();
    }
}
