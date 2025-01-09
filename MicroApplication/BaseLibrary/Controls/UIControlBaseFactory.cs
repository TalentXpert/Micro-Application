
using BaseLibrary.UI.Controls;
using System.Data;

namespace BaseLibrary.Controls
{
    public abstract class UIControlBaseFactory : CleanCode
    {
        IBaseLibraryServiceFactory SF { get; }
        public ApplicationUser LoggedInUser { get; set; }

        public UIControlBaseFactory(IBaseLibraryServiceFactory serviceFactory)
        { 
            SF = serviceFactory; 
        }

        /// <summary>
        /// Return a UIControl to be render on a form
        /// </summary>
        public abstract UIControl GetUIControl(Guid? organizationId, AppControl appControl, AppFormControl appFormControl, List<string>? values, Guid? parentId, bool addEmptyEntry);
        public abstract UIControl GetComplexUIControl(AppControl appControl, AppFormControl appFormControl,Guid dataKey);

        /// <summary>
        /// Help child class to build a ui control 
        /// </summary>
        protected UIControl BuildUIControl(AppControl appControl, AppFormControl appFormControl, List<string>? values, List<SmartControlOption> options, bool addEmptyEntry)
        {
            UIControl smartControl = new UIControl(appControl, appFormControl, values);
            if (options == null)
                options = new List<SmartControlOption>();

            if (IsNotNullOrEmpty(appControl.Options))
                options = GetOptions(appControl.Options);


            if (ControlTypes.IsDropdownControl(appControl.ControlType))
            {
                if (addEmptyEntry && !appControl.ControlType.Equals("RadioButton"))
                    options.Insert(0, new SmartControlOption("Select", ""));

                smartControl.Options = options;
                if (options.Any())
                {
                    smartControl.SetValue(GetRightOptionValue(options, values));
                }
            }

            return smartControl;
        }

        private string? GetRightOptionValue(List<SmartControlOption> options, List<string> values)
        {
            var value = values?.FirstOrDefault();
            if (IsNotNullOrEmpty(value))
            {
                var option = options.FirstOrDefault(o => o.Value == value);
                if (option != null)
                    return option.Value;
            }
            return options.First().Value;
        }

        protected List<SmartControlOption> GetOptions(string? options)
        {
            if (options == null)
                return new List<SmartControlOption>();
            string separator = GetOptionSeparator(options);
            var opts = options.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return opts.Select(o => new SmartControlOption(o, o)).ToList();
        }

        private string GetOptionSeparator(string options)
        {
            if (options.Contains(";"))
                return ";";
            if (options.Contains(","))
                return ",";
            if (options.Contains("-"))
                return "-";
            return ":";
        }

        public virtual string? GetTextFromId(string controlIdentifier, Guid id)
        {
            var data = SF.PageDataStoreService.GetFormDatum(id);
            if (data == null)
                throw new ValidationException($"{controlIdentifier} is not stored in base data store. It has its private store so add this option in a class drived from UIControlBaseFactory in your project.");
            return data.Name;
        }
    }
}
