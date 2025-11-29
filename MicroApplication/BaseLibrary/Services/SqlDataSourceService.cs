
using BaseLibrary.Domain.DataSources;
using BaseLibrary.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Data;

namespace BaseLibrary.Services
{
    public interface ISqlDataSourceService
    {
        DataTable GetDataSourceColumns(Guid id);
        List<MacroSqlDataSource> GetDatasources(SqlDataSourceType chart);
        void SaveUpdateDataSources(MacroSqlDataSource datasource);
        MacroSqlDataSource GetDataSource(Guid id);
    }

    public class SqlDataSourceService : ServiceLibraryBase, ISqlDataSourceService
    {
        public SqlDataSourceService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<SqlDataSourceService>())
        {

        }

        public MacroSqlDataSource GetDataSource(Guid id)
        {
            var source = SF.MicroAppContract.GetBaseSqlDataSource().GetSqlDataSources().FirstOrDefault(s=>s.Id==id);
            if(source is not null)
                return source;
           // source = SF.RF.SqlDataSourceRepository.Get(id);
            if(source is not null) return source;
            throw new ValidationException($"No data source found with given key-{id}");
        }

        public DataTable GetDataSourceColumns(Guid id)
        {
            //var ds = RF.SqlDataSourceRepository.Get(id);
            //var query = ds.Query;
            //var dt = new SqlCommandExecutor().GetDataTable(query);
            //return dt;
            return new DataTable();
        }

        public List<MacroSqlDataSource> GetDatasources(SqlDataSourceType dataSourceType)
        {
            return SF.MicroAppContract.GetBaseSqlDataSource().GetSqlDataSources().Where(s => s.TargetContentType == dataSourceType.Name).ToList();
        }

        public void SaveUpdateDataSources(MacroSqlDataSource datasource)
        {
            //MacroSqlDataSource ds = RF.SqlDataSourceRepository.Get(datasource.Id);
            //var sqlQuery = new MicroSqlQuery(datasource.Query);
            //if (ds == null)
            //{
            //    RF.SqlDataSourceRepository.Add(datasource);
            //}
            //else
            //{
            //    ds.Update(datasource.Name, sqlQuery, datasource.DataSourceType);
            //}
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
