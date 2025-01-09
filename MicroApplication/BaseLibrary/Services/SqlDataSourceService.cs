
using BaseLibrary.Domain.DataSources;
using BaseLibrary.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Data;

namespace BaseLibrary.Services
{
    public interface ISqlDataSourceService
    {
        DataTable GetDataSourceColumns(Guid id);
        List<SqlDataSource> GetDatasources(SqlDataSourceType chart);
        void SaveUpdateDataSources(SqlDataSource datasource);
    }

    public class SqlDataSourceService : ServiceLibraryBase, ISqlDataSourceService
    {
        public SqlDataSourceService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {

        }

        public DataTable GetDataSourceColumns(Guid id)
        {
            var ds = RF.SqlDataSourceRepository.Get(id);
            var query = ds.Query;
            var dt = new SqlCommandExecutor().GetDataTable(query);
            return dt;
        }

        public List<SqlDataSource> GetDatasources(SqlDataSourceType dataSourceType)
        {
            return RF.SqlDataSourceRepository.GetFiltered(s => s.DataSourceType == dataSourceType.Name).ToList();
        }

        public void SaveUpdateDataSources(SqlDataSource datasource)
        {
            SqlDataSource ds = RF.SqlDataSourceRepository.Get(datasource.Id);
            if (ds == null)
            {
                RF.SqlDataSourceRepository.Add(datasource);
            }
            else
            {
                ds.Update(datasource.Name, datasource.Query, datasource.DataSourceType);
            }
        }
    }
}

/*
 
 ComponentSchema? dc = null;
            var data = NewtonsoftJsonAdapter.SerializeObject(vm);
            if (vm.Id.HasValue)
                dc = RF.ComponentSchemaRepository.Get(vm.Id.Value);
            if (dc == null)
            {

                dc = ComponentSchema.Create(vm.Id, ComponentTypes.Dashboard, vm.Name, vm.Description, data, organizationId, loggedInUserId);
                RF.ComponentSchemaRepository.Add(dc);
            }
            else
            {
                dc.Update(vm.Name, vm.Description, data, loggedInUserId);
            }
 */
