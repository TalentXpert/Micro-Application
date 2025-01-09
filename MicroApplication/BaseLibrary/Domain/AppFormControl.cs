
namespace BaseLibrary.Domain
{
    public class AppFormControl : Entity
    {
        public Guid AppFormId { get; protected set; }
        public Guid AppControlId { get; protected set; }
        public int Position { get; protected set; }
        public bool IsEditable { get; protected set; }
        public bool IsMandatory { get; protected set; }
        public Guid? OrganisationId { get; protected set; } // null - if it is fixed control.
        public Guid? LayoutControlId { get; protected set; } //if present then will associated this control with gloal control layout plan
        public bool IsUnique { get; protected set; }
        public Int64? Maximum { get; protected set; }
        public Int64? Minimum { get; protected set; }
        public string? DisplayLabel { get; protected set; }
        public bool? IsGlobalControl { get; protected set; }
        public string? Options { get; protected set; }
        public bool IsSingleLine { get; protected set; }
        public string? Tooltip { get; protected set; }
        public virtual AppForm AppForm { get; set; }
        public virtual AppControl AppControl { get; set; }
        protected AppFormControl()
        {

        }
        public static AppFormControl Create(Guid? organizationId, string id, AppForm appForm, AppControl appControl, int position, Int64? minimum, Int64? maximum, bool isSingleLine=false, bool isEditable = true, bool isMandatory = true, bool isUnique = true)
        {
            if (Guid.TryParse(id, out Guid cid) == false)
                throw new ValidationException($"{id} is not valid guid.");
            var fc = new AppFormControl
            {
                Id = cid,
                AppFormId = appForm.Id,
                AppControlId = appControl.Id,
                Position = position,
                IsEditable = isEditable,
                IsMandatory = isMandatory,
                IsUnique = isUnique,
                Minimum = minimum,
                Maximum = maximum,
                AppControl = appControl,
                AppForm = appForm,
                OrganisationId = organizationId,
                IsGlobalControl = null,
                Options = null,
                IsSingleLine = isSingleLine,
                DisplayLabel = null,

            };
            fc.SetCreatedOn();
            fc.SetUpdatedOn();
            return fc;
        }
        
        public static AppFormControl Create(Guid? organizationId, AppForm appForm, AppControl appControl, int position)
        {
            var fc = new AppFormControl
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                AppFormId = appForm.Id,
                AppControlId = appControl.Id,
                Position = position,
                AppControl = appControl,
                AppForm = appForm,
                OrganisationId = organizationId,
                IsGlobalControl = null,
                Options = null,
                DisplayLabel = null,

            };
            fc.SetCreatedOn();
            fc.SetUpdatedOn();
            return fc;
        }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(AppFormId, "AppFormId", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(AppControlId, "AppControlId", validationResults);

            X.Validator.IntegerValidator.CheckForEmpltyOrDefaulValue(Position, "position", validationResults);
            return validationResults;

        }

        public void Update(AppFormControl c)
        {
            Position = c.Position;
            IsEditable = c.IsEditable;
            IsMandatory = c.IsMandatory;
            IsUnique = c.IsUnique;
            Minimum = c.Minimum;
            Maximum = c.Maximum;
            IsGlobalControl = c.IsGlobalControl;
            Options = c.Options;
            IsSingleLine = c.IsSingleLine;
            DisplayLabel = c.DisplayLabel;
            Tooltip= c.Tooltip;
        }

        public void Update(int position, AppFormControlVM c)
        {
            if (IsFixedControl()) return;
            Position = position;
            IsEditable = c.IsEditable;
            IsMandatory = c.IsMandatory;
            IsUnique = c.IsUnique;
            Minimum = c.Minimum;
            Maximum = c.Maximum;
            IsSingleLine = c.IsSingleLine;
            if (AreNotEqualsIgnoreCase(AppControl.DisplayLabel, c.DisplayLabel))
                DisplayLabel = c.DisplayLabel;
            else
                DisplayLabel = null;
            if (AreNotEqualsIgnoreCase(AppControl.Options, c.Options))
                Options = c.Options;
            else
                Options = null;
            MakeGlobalControl(c);
        }
        public void UpdateFixedControl(int position, AppFormControlVM c)
        {
            Position = position;
            IsEditable = c.IsEditable;
            IsMandatory = c.IsMandatory;
            IsUnique = c.IsUnique;
            Minimum = c.Minimum;
            Maximum = c.Maximum;
            IsSingleLine = c.IsSingleLine;
            if (AreNotEqualsIgnoreCase(AppControl.DisplayLabel, c.DisplayLabel))
                DisplayLabel = c.DisplayLabel;
            else
                DisplayLabel = null;
            if (AreNotEqualsIgnoreCase(AppControl.Options, c.Options))
                Options = c.Options;
            else
                Options = null;

            MakeGlobalControl(c);
            Tooltip = c.Tooltip;
        }

        private void MakeGlobalControl(AppFormControlVM c)
        {
            IsGlobalControl = null;
            if (AppControl.IsGlobalControl)
                IsGlobalControl = c.IsGlobalControl;
        }
        public AppFormControl SetTooltip(string tooltip)
        {
            Tooltip = tooltip;
            return this;
        }
        public bool IsFixedControl()
        {
            return IsNull(OrganisationId);
        }

        public List<string>? GetFormControlValue(List<ControlValue> controlValues)
        {
            if (HasChildren(controlValues))
            {
                var controlValue = controlValues.FirstOrDefault(c => c.ControlId == AppControlId);
                return controlValue?.Values;
            }
            return null;
        }
        public string GetDisplayLabel()
        {
            if (IsNotNullOrEmpty(DisplayLabel))
                return DisplayLabel;
            return AppControl.DisplayLabel;
        }
        public AppFormControl MakeGlobalControl()
        {
            IsGlobalControl = true;
            return this;
        }
        public bool GetIsGlobalControl()
        {
            if (IsGlobalControl.HasValue)
                return IsGlobalControl.Value;
            return AppControl.IsGlobalControl;
        }
        public string? GetOptions()
        {
            if (IsNotNullOrEmpty(Options))
                return Options;
            return AppControl.Options;
        }

        public void SetLayoutControlOrDefault(Guid? layoutControlId)
        {
            if(layoutControlId == null)
                return;
            LayoutControlId= layoutControlId;
        }
    }

    public class AppFormControlListVM
    {
        public Guid Id { get; set; }
        public Guid AppControlId { get; set; }
        public string ControlIdentifier { get; set; }
        public string DisplayLabel { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public int Position { get; private set; }
        public bool IsEditable { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsUnique { get; set; }
        public Int64? Minimum { get; set; }
        public Int64? Maximum { get; set; }
        public bool IsFixed { get; set; }
        public bool IsSingleLine { get; set; }
        public bool? IsGlobalControl { get; set; }
        public string? Options { get; set; }
        public string? Tooltip { get; set; }
        public bool CanBeGlobalControl { get; set; }
        public AppFormControlListVM(AppFormControl formControl, AppControl control)
        {
            Id = formControl.Id;
            AppControlId = control.Id;
            ControlIdentifier = control.ControlIdentifier;
            DisplayLabel = formControl.GetDisplayLabel();
            DataType = control.DataType;
            ControlType = control.ControlType;
            Position = formControl.Position;
            IsEditable = formControl.IsEditable;
            IsMandatory = formControl.IsMandatory;
            IsUnique = formControl.IsUnique;
            Minimum = formControl.Minimum;
            Maximum = formControl.Maximum;
            IsFixed = formControl.IsFixedControl();
            IsSingleLine = formControl.IsSingleLine;
            CanBeGlobalControl = control.IsGlobalControl;
            IsGlobalControl = formControl.GetIsGlobalControl();
            Options = formControl.GetOptions();
            Tooltip = formControl.Tooltip;
        }
    }

    public class AppFormControlAddUpdateVM
    {
        public Guid FormId { get; set; }
        public Guid? GlobalControlValue { get; set; }
        public List<AppFormControlVM> AppFormControls { get; set; } = new List<AppFormControlVM>();
    }

    public class AppFormFixedControlAddUpdateVM
    {
        public Guid FormId { get; set; }
        public List<AppFormControlVM> AppFormControls { get; set; } = new List<AppFormControlVM>();
    }

    public class AppPageSaveUpdateVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public Guid MenuId { get; set; }
        public string EditPermission { get; set; }
        public string DeletePermission { get; set; }
        public string ViewPermission { get; set; }
    }

    public class AppFormControlVM
    {
        public Guid Id { get; set; }
        public Guid AppControlId { get; set; }
        public int Position { get; private set; }
        public bool IsEditable { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsUnique { get; set; }
        public Int64? Minimum { get; set; }
        public Int64? Maximum { get; set; }
        public bool IsSingleLine { get; set; }
        public string? DisplayLabel { get; set; }
        public bool? IsGlobalControl { get; set; }
        public string? Options { get; set; }
        public string? Tooltip { get; set; }
    }

    public class AppFormList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid MenuId { get; set; }
        public int Position { get; set; }
        public string EditPermission { get; set; }
        public string DeletePermission { get; set; }
        public string ViewPermission { get; set; }
        public AppFormList(AppForm form)
        {
            Id = form.Id;
            Name = form.Name;
            MenuId = form.MenuId;
            Position = form.Position;
            EditPermission = form.EditPermission;
            DeletePermission = form.DeletePermission;
            ViewPermission = form.ViewPermission;

        }
        public AppFormList(AppPage form)
        {
            Id = form.Id;
            Name = form.Name;
            MenuId = form.MenuId;
            Position = form.Position;
            EditPermission = form.EditPermission;
            DeletePermission = form.DeletePermission;
            ViewPermission = form.ViewPermission;
        }
    }

}
