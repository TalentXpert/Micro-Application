
namespace BaseLibrary.Domain
{
    public class AppForm : Entity
    {
        public string Name { get; protected set; }
        public Guid MenuId { get; protected set; }
        public int Position { get; protected set; }
        public bool IsCustomizable { get; protected set; }
        public string EditPermission { get; protected set; }
        public string DeletePermission { get; protected set; }
        public string ViewPermission { get; protected set; }
        public virtual List<AppFormControl> AppFormControls { get; set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
        protected AppForm() { AppFormControls = new List<AppFormControl>(); }

        public static AppForm Create(string id, string name, Guid menuId, int position, bool isCustomizable, Permission permission)
        {
           return Create(id, name, menuId, position, isCustomizable, permission, permission, permission);
        }
        public static AppForm Create(string id, string name, Guid menuId, int position,bool isCustomizable, Permission editPermission, Permission deletePermission, Permission viewPermission)
        {
            if (Guid.TryParse(id, out Guid fid) == false)
                throw new ValidationException($"{id} is not valid guid.");
            var c = new AppForm
            {
                Id = fid,
            };
            c.Update(name,menuId,position,isCustomizable,editPermission,deletePermission,viewPermission);
            c.SetCreatedOn();
            c.SetUpdatedOn();
            return c;
        }
        public AppFormControl AddFormControl(string id, AppControl appControl, bool isSingleline)
        {
            var control = AddFormControl(id, appControl, isSingleline, true, true, false);
            return control;
        }
        public AppFormControl AddFormControl(string id, AppControl appControl,bool isSingleline, bool isEditable , bool isMandatory , bool isUnique)
        {
            var control = AppFormControl.Create(null, id, this, appControl, AppFormControls.Count + 1, null, null, isSingleline, isEditable, isMandatory, isUnique);
            AppFormControls.Add(control);
            return control;
        }

        public void Update(AppForm form)
        {
            Name = form.Name;
            EditPermission = form.EditPermission;
            DeletePermission = form.DeletePermission;
            ViewPermission = form.ViewPermission;
        }
        public void Update(string name, Guid menuId, int position, bool isCustomizable, Permission editPermission, Permission deletePermission, Permission viewPermission)
        {
            Name = name;
            MenuId = menuId;
            Position = position;
            IsCustomizable = isCustomizable;
            EditPermission = editPermission.Code;
            DeletePermission = deletePermission.Code;
            ViewPermission = viewPermission.Code;
        }

        public static AppForm? Create(string id, AppPageSaveUpdateVM model, bool isCustomizable)
        {
            if (Guid.TryParse(id, out Guid fid) == false)
                throw new ValidationException($"{id} is not valid guid.");
            var c = new AppForm
            {
                Id = fid,
            };
            c.Update(model, isCustomizable);
            c.SetCreatedOn();
            c.SetUpdatedOn();
            return c;
        }

        public void Update(AppPageSaveUpdateVM model, bool isCustomizable)
        {
            Name = model.Name;
            MenuId = model.MenuId;
            Position = model.Position;
            IsCustomizable = isCustomizable;
            EditPermission = model.EditPermission;
            DeletePermission = model.DeletePermission;
            ViewPermission = model.ViewPermission;
        }
    }

    public class AppFormControlRequestVM
    {
        public Guid FormId { get; set; }
        public Guid? GlobalFormValue { get; set; }
    }
    public class AppFormResetRequestVM
    {
        public Guid FormId { get; set; }
        public Guid? LayoutControlValue { get; set; }
    }
}
