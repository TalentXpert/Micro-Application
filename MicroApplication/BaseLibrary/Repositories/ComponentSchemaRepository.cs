namespace BaseLibrary.Repositories
{
    public interface IComponentSchemaRepository : IRepository<ComponentSchema>
    {
        List<ComponentSchema> GetComponents(ComponentTypes componentType, ApplicationUser loggedInUser);
    }
    public class ComponentSchemaRepository : Repository<ComponentSchema>, IComponentSchemaRepository
    {
        IBaseDatabase unitOfWork;

        public ComponentSchemaRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<UserRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ComponentSchema> GetComponents(ComponentTypes componentType, ApplicationUser loggedInUser)
        {
            return unitOfWork.ComponentSchemas.Where(c=>c.ComponentType== componentType.Name && c.OrganizationId==loggedInUser.OrganizationId).ToList();
        }
    }
}
