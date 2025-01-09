using DocumentFormat.OpenXml.Wordprocessing;

namespace BaseLibrary.Domain
{
    public class AppPage : Entity
    {
        public string Name { get; set; }
        public Guid MenuId { get; protected set; }
        public int Position { get; protected set; }
        public string EditPermission { get; protected set; }
        public string DeletePermission { get; protected set; }
        public string ViewPermission { get; protected set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
        protected AppPage() { }
        public static AppPage Create(string id, string name, Guid menuId, int position, Permission permission)
        {
            return Create(id, name, menuId, position, permission, permission, permission);
        }
        public static AppPage Create(string id, string name, Guid menuId, int position, Permission editPermission, Permission deletePermission, Permission viewPermission)
        {
            if (Guid.TryParse(id, out Guid fid) == false)
                throw new ValidationException($"{id} is not valid guid.");
            var c = new AppPage
            {
                Id = fid,
            };
            c.Update(name, menuId, position, editPermission, deletePermission, viewPermission);
            c.SetCreatedOn();
            c.SetUpdatedOn();
            return c;
        }


        public void Update(AppPage appPage)
        {
            Name = appPage.Name;
            MenuId = appPage.MenuId;
            Position = appPage.Position;
            EditPermission = appPage.EditPermission;
            DeletePermission = appPage.DeletePermission;
            ViewPermission = appPage.ViewPermission;
        }
        public void Update(string name, Guid menuId, int position, Permission editPermission, Permission deletePermission, Permission viewPermission)
        {
            Name = name;
            MenuId = menuId;
            Position = position;
            EditPermission = editPermission.Code;
            DeletePermission = deletePermission.Code;
            ViewPermission = viewPermission.Code;
        }

        public static AppPage Create(AppPageSaveUpdateVM model)
        {
            var c = new AppPage
            {
                Id = IdentityGenerator.NewSequentialGuid(),
            };
            c.Update(model);
            c.SetCreatedOn();

            return c;
        }
        public void Update(AppPageSaveUpdateVM model)
        {
            Name = model.Name;
            MenuId = model.MenuId;
            Position = model.Position;
            EditPermission = model.EditPermission;
            DeletePermission = model.DeletePermission;
            ViewPermission = model.ViewPermission;
            SetUpdatedOn();
        }
    }
}
