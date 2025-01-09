

using System.Data;
using BaseLibrary.Configurations.DataSources.LinqDataSources;

namespace BaseLibrary.Repositories
{
    public interface IFormDataStoreRepository : IRepository<FormDataStore>
    {
        List<FormDataStore> GetFormData(Guid? userId, Guid? organizationId, Guid formId, Guid? parentId);
        List<SmartControlOption> GetSearchResult(Guid? organizationId, Guid formId, Guid? parentId, string searchTerm);
        List<BaseLinqFormDataSource> GetCityData();
    }
    public class FormDataStoreRepository : Repository<FormDataStore>, IFormDataStoreRepository
    {
        IBaseDatabase unitOfWork;

        public FormDataStoreRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<FormDataStoreRepository>())
        {
            this.unitOfWork = unitOfWork;
        }
        public List<FormDataStore> GetFormData(Guid? userId, Guid? organizationId, Guid formId, Guid? parentId)
        {
            var query = unitOfWork.PageDataStore.Where(d => d.FormId == formId);
            if (organizationId.HasValue)
                query = query.Where(d => d.OrganizationId == organizationId);
            if (userId.HasValue)
                query = query.Where(d => d.AddedByUserId == userId);
            if (parentId.HasValue)
                query = query.Where(d => d.ParentId == parentId);
            return query.ToList();
        }

        public List<SmartControlOption> GetSearchResult(Guid? organizationId, Guid formId, Guid? parentId, string searchTerm)
        {
            List<SmartControlOption> options = new List<SmartControlOption>();
            if (IsNullOrEmpty(searchTerm))
                return options;
            var query = $"select Id,Name from formdatastore where FormId='{formId}' and name like '{searchTerm}%'";
            if (parentId.HasValue)
                query += $" and parentid='{parentId}'";
            if (organizationId.HasValue)
                query += $" and OrganizationId='{organizationId}'";
            var dt = new SqlCommandExecutor().GetDataTable(query);
            foreach (DataRow dr in dt.Rows)
            {
                options.Add(new SmartControlOption(dr[1]?.ToString(), dr[0]?.ToString()));
            }
            return options;
        }

        public List<BaseLinqFormDataSource> GetCityData()
        {
            var query = from ct in unitOfWork.PageDataStore
                        join s in unitOfWork.PageDataStore on ct.ParentId equals s.Id
                        join c in unitOfWork.PageDataStore on s.ParentId equals c.Id
                        select new BaseLinqFormDataSource { City = ct, State = s, Country = c };
            return query.ToList();
        }
    }



    //public class DataSource
    //{
    //    public FormDataStore City { get; set; }
    //    public FormDataStore State { get; set; }
    //    public FormDataStore Country { get; set; }

    //    public GridCell GetColumnValue(string column)
    //    {
    //        switch (column)
    //        {

    //        }
    //        return new GridCell { T = "" };
    //    }
    //}
}
