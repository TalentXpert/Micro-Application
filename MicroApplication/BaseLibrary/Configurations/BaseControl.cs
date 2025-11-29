namespace BaseLibrary.Configurations
{
    public class BaseControls
    {
        public const string Name = "Name";
        public const string Description = "Description";
        public const string Email = "Email";
        public const string ContactNumber = "ContactNumber";
        public const string LoginId = "LoginId";

        public const string Country = "Country";
        public const string State = "State";
        public const string City = "City";

        public const string Tag = "Tag";
        public const string Quantity = "Quantity";

        public const string ControlIdentifier = "ControlIdentifier";
        public const string ControlDataTypes = "DataType";
        public const string ControlType = "ControlType";
        public const string DisplayLabel = "DisplayLabel";
        public const string Options = "Options";
        public const string PermissionSelection = "PermissionSelection";
        public const string Role = "Role";
        public const string Aadhar = "Aadhar";
        public const string Password = "Password";
        public const string Username = "Username";
        public const string  Website = "Website";
        public const string Organization = "Organization";

        public const string UploadFile = "Upload File";
    }

    public abstract class BaseControl
    {
        /// <summary>
        /// Implement this method to return your application controls
        /// </summary>
        protected abstract List<AppControl> GetApplicationControls();
        /// <summary>
        ///  Override this method if you want to change base controls or their sequence. You can return empty list and return all controls from GetApplicationControls method.
        /// </summary>
        protected virtual List<AppControl> GetBaseControls()
        {
            var controls = new List<AppControl>() {Name,Description,Email,ContactNumber,LoginId,Country,State,City,Tag,Quantity,ControlIdentifier,ControlDataTypes,ControlType,
                DisplayLabel,Options,PermissionSelection,Role,Aadhar,Password,Username,Website,Organization,UploadFile
            };
            return controls;
        }
        
        public List<AppControl> GetControls()
        {
            var controls = new List<AppControl>();
            controls.AddRange(GetApplicationControls());
            controls.AddRange(GetBaseControls());
            return controls;
        }

        public static AppControl Name => AppControl.CreateSystemControl("F93F34C9-53FC-CB09-1958-08D9065B2081", BaseControls.Name, "Name", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Email => AppControl.CreateSystemControl("4A3CAE6B-4107-CA54-4027-08D90877C8EB", BaseControls.Email, "Email", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl ContactNumber => AppControl.CreateSystemControl("F963A5F3-E574-CCC3-A1A0-08D9087D5E46", BaseControls.ContactNumber, "Contact Number", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl LoginId => AppControl.CreateSystemControl("64C9DEBD-1B6C-C78D-2A96-08D9089FBA86", BaseControls.LoginId, "Login Id", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Description => AppControl.CreateSystemControl("4F1A05D7-0746-CB98-C368-08D90869EA67", BaseControls.Description, "Description", ControlDataType.String, ControlTypes.TextArea);
        public static AppControl Country => AppControl.CreateSystemControl("B47000D4-3648-C8C3-1E94-08D906190B31", BaseControls.Country, "Country", ControlDataType.Guid, ControlTypes.Dropdown).MakeParent();
        public static AppControl State => AppControl.CreateSystemControl("4A547E88-0E00-C7B8-FAF8-08D906551EAF", BaseControls.State, "State", ControlDataType.Guid, ControlTypes.Dropdown).ChildOf(Country);
        public static AppControl City => AppControl.CreateSystemControl("D9337743-F50E-C21E-DB88-08D9065541C7", BaseControls.City, "City", ControlDataType.Guid, ControlTypes.Dropdown).ChildOf(State);
        public static AppControl Tag => AppControl.CreateSystemControl("B8A2B859-30AE-C46E-44CC-08D908BBD81E", BaseControls.Tag, "Tag", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Quantity => AppControl.CreateSystemControl("F8941DDE-BD5D-CFA8-2472-08D908A9BAF9", BaseControls.Quantity, "Quantity", ControlDataType.Double, ControlTypes.TextBox);
        public static AppControl ControlIdentifier => AppControl.CreateSystemControl("D817328E-3845-CAE1-ED26-08D908AC741A", BaseControls.ControlIdentifier, "Control Identifier", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl ControlDataTypes => AppControl.CreateSystemControl("FE57B9A3-7FB3-CDC0-C00B-08D908AD127F", BaseControls.ControlDataTypes, "Data Type", ControlDataType.String, ControlTypes.Dropdown);
        public static AppControl ControlType => AppControl.CreateSystemControl("54B1CF24-A776-CE3F-1A81-08D908AD462D", BaseControls.ControlType, "Control Type", ControlDataType.String, ControlTypes.Dropdown);
        public static AppControl DisplayLabel => AppControl.CreateSystemControl("B0CC4006-B2EF-C95C-F483-08D908B8D802", BaseControls.DisplayLabel, "Display Label", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Options => AppControl.CreateSystemControl("330CAAE5-9C20-C67C-E021-08D908BA4BB4", BaseControls.Options, "Options", ControlDataType.String, ControlTypes.TextArea);
        public static AppControl PermissionSelection => AppControl.CreateSystemControl("502A073D-B8F7-C799-5047-08D908BC2392", BaseControls.PermissionSelection, "Permissions", ControlDataType.Guid, ControlTypes.MultipleSelectGrid).ChildOf(Role);
        public static AppControl Role => AppControl.CreateSystemControl("FDEA4789-63BF-C21A-44E1-08D908BD4480", BaseControls.Role, "Role", ControlDataType.Guid, ControlTypes.Dropdown).MakeParent();
        public static AppControl Aadhar => AppControl.CreateSystemControl("5DED355D-5588-CA6F-882D-08D908BD7903", BaseControls.Aadhar, "Aadhar Number", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Password => AppControl.CreateSystemControl("E6583FC4-3058-4D42-AF2C-B36C6F9DC0F3", BaseControls.Password, "Password", ControlDataType.String, ControlTypes.Password);
        public static AppControl Username => AppControl.CreateSystemControl("581377CE-2C8D-4A8F-B564-E181A6BF9819", BaseControls.Username, "Username", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Website => AppControl.CreateSystemControl("89CE6948-D7ED-482E-8310-37744F6DE480", BaseControls.Website, "Website", ControlDataType.String, ControlTypes.TextBox);
        public static AppControl Organization => AppControl.CreateSystemControl("FC080040-A163-433B-B55E-6EF2E10D2138", BaseControls.Organization, "Organization", ControlDataType.String, ControlTypes.Typeahead);
        public static AppControl UploadFile => AppControl.CreateSystemControl("F3C4B63C-3824-4454-9684-FC2C50217CEC", BaseControls.UploadFile, "Upload File", ControlDataType.String, ControlTypes.File);
    }
}

