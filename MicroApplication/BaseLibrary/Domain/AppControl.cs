


namespace BaseLibrary.Domain
{
    /// <summary>
    /// This class represent a control in application that can be place on a form
    /// </summary>
    public class AppControl : Entity
    {
        public string ControlIdentifier { get; protected set; } //Unique and must match with database column/Domain Entity if need to persist in a separate column
        public string DataType { get; protected set; }
        public string ControlType { get; protected set; }
        public string DisplayLabel { get; protected set; }
        public bool IsParent { get; protected set; } // this property specify that this control is a parent of other controls 
        public string? ParentControlIdentifier { get; protected set; } // this property specify that this is a child of a control with thsi control identifier.
        public bool IsGlobalControl { get; protected set; } // true means this will display on top of grid control and read only on data entry form
        public bool IsFormLayoutOwner { get; protected set; } //this enable this control to change form layout
        public string? Options { get; protected set; } // ; separated value for selection based control. 
        public Guid? OrganisationId { get; protected set; } //null for system control and not null for organization specific controls
        public Guid? OptionFormId { get; protected set; } //This is the id of form that will provide data for filling options
        public virtual List<AppFormControl> AppFormControls { get; set; }
        protected AppControl()
        {
            AppFormControls = new List<AppFormControl>();
        }

        /// <summary>
        /// This method is used to create system control which are common for all form and users.
        /// </summary>
        public static AppControl CreateSystemControl(string id, string controlIdentifer, string displayLabel, ControlDataType dataType, ControlTypes controlType)
        {
            if (Guid.TryParse(id, out Guid cid) == false)
                throw new ValidationException($"{id} is not valid guid.");
            var c = new AppControl
            {
                Id = cid,
                ControlIdentifier = controlIdentifer,
                DisplayLabel = displayLabel,
                ControlType = controlType.Name,
                DataType = dataType.Name,
                OrganisationId = null
            };
            c.SetCreatedOn();
            c.SetUpdatedOn();
            return c;
        }

        /// <summary>
        /// This method is used to create organization control which are only for an organization and its users.
        /// </summary>
        public static AppControl Create(Guid organisationId, AppControlVM vm)
        {
            var c = new AppControl
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                OrganisationId = organisationId
            };
            c.SetCreatedOn();
            c.Update(vm);
            return c;
        }
        public void Update(AppControlVM vm)
        {
            ControlIdentifier = vm.ControlIdentifier;
            DisplayLabel = vm.DisplayLabel;
            ControlType = vm.ControlType;
            DataType = vm.DataType;
            Options = vm.Options;
            SetUpdatedOn();
        }
        public AppControl MakeParent()
        {
            IsParent = true;
            return this;
        }
        public AppControl ChildOf(AppControl parent)
        {
            ParentControlIdentifier = parent.ControlIdentifier;
            return this;
        }
        public AppControl MakeGlobalParent()
        {
            IsGlobalControl = true;
            return this;
        }
        public AppControl MakeFormLayoutOwner()
        {
            IsFormLayoutOwner = true;
            return this;
        }
        public AppControl SetOptions(List<string> options)
        {
            Options = string.Join(";", options.Select(c => c.Trim()));
            return this;
        }
        public AppControl SetOptionForm(AppForm form)
        {
            OptionFormId= form.Id;
            return this;
        }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);

            X.Validator.StringValidator.CheckForNullOrEmpty(ControlIdentifier, "ControlIdentifier", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(ControlIdentifier, "ControlIdentifier", 32, validationResults);

            X.Validator.StringValidator.CheckForNullOrEmpty(DisplayLabel, "DisplayLabel", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(DisplayLabel, "DisplayLabel", 64, validationResults);

            X.Validator.StringValidator.CheckForNullOrEmpty(DataType, "DataType", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(DataType, "DataType", 16, validationResults);

            X.Validator.StringValidator.CheckForNullOrEmpty(ControlType, "ControlType", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(ControlType, "ControlType", 32, validationResults);

            if (ShouldHasOptions())
            {
                //if(string.IsNullOrWhiteSpace(Options) && OptionFormId.HasValue ==false)
                //    validationResults.Add(new ValidationResult("This control need either options or option form id to work correctly.", new string[] { "Options" }));
            }
                

            return validationResults;
        }

        public void Update(AppControl control)
        {
            DisplayLabel = control.DisplayLabel;
        }
        public bool IsFixedControl()
        {
            return OrganisationId == null;
        }
        public bool IsDropdown()
        {
            return ControlType == ControlTypes.Dropdown.Name;
        }
        public bool ShouldHasOptions()
        {
            if(ControlType == ControlTypes.Dropdown.Name)
                return true;
            if (ControlType == ControlTypes.Typeahead.Name)
                return true;
            return false;
        }
        public bool IsBoolean()
        {
            return DataType == ControlDataTypes.Bool;
        }

        public string? GetValue(string controlIdentifier)
        {
            throw new NotImplementedException();
        }

        public bool IsComplexControl()
        {
            return ControlTypes.IsComplexControl(this.ControlType);
        }
    }

    public class AppControlVM
    {
        public Guid Id { get; set; }
        public string ControlIdentifier { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public string DisplayLabel { get; set; }
        public bool IsParent { get; set; }
        public string? ParentControlIdentifier { get; set; }
        public bool IsGlobalParent { get; set; }
        public bool IsFormLayoutOwner { get; set; }
        public string? Options { get; set; }
        public AppControlVM() { }
        public AppControlVM(AppControl ac)
        {
            Id = ac.Id;
            ControlIdentifier = ac.ControlIdentifier;
            DataType = ac.DataType;
            ControlType = ac.ControlType;

            DisplayLabel = ac.DisplayLabel;
            IsParent = ac.IsParent;
            ParentControlIdentifier = ac.ParentControlIdentifier;
            Options = ac.Options;

            IsGlobalParent = ac.IsGlobalControl;
            IsFormLayoutOwner = ac.IsFormLayoutOwner;
        }
    }
}
