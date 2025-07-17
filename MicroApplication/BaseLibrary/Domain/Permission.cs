
namespace BaseLibrary.Domain
{
    public class Permission 
    {
        public Guid Id { get; set; }
        public string Name { get; protected set; }=string.Empty;
        public string Code { get; protected set; } = string.Empty;
        public string? Description { get; protected set; }= string.Empty;
        protected Permission() { }
        public Permission(string id, string name, string description)
        {
            Id = Guid.Parse(id);
            Name = name;
            Description = description;
            Code = X.Extension.String.TitleCase(name).Replace(" ", "");
        }
    }

    public class PermissionVM
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }=string.Empty ;
        public string Code { get; set; }= string.Empty;
        public PermissionVM() { }
        public PermissionVM(Permission permission)
        {
            Id = permission.Id;
            Description = permission.Description;
            Name = permission.Name;
            Code = permission.Code;
        }
    }

   

    
}
