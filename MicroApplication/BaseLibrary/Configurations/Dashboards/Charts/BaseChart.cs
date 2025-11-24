using BaseLibrary.Controls.Charts;
using BaseLibrary.Domain.ComponentSchemas;

namespace BaseLibrary.Configurations.Dashboards.Charts
{
    public abstract class BaseChart
    {
        /// <summary>
        /// Implement this method to return your application charts 
        /// </summary>
        protected abstract List<ChartSchema> GetApplicationCharts();

        public static Guid UserStatusChartId = Guid.Parse("05BEFE97-C159-4861-B095-5745BFE1B069");

        /// <summary>
        /// Override this method if you want to change base charts or their sequence. You can return empty list and return all charts from GetApplicationCharts method.
        /// </summary>
        protected virtual List<ChartSchema> GetBaseCharts()
        {
            var charts = new List<ChartSchema>();
            var chart =
                new ChartSchema
                {
                    ChartType = ChartType.Bar.Type,
                    MinWidth = 380,
                    MinHeight = 350,
                    DataSourceId = Guid.Parse("621225CA-CDE7-4756-9D0C-AA6CA350C3A3"),
                    Description = "",
                    Id = UserStatusChartId,
                    Name = "User Status",
                    Columns = new List<ChartColumnSchema>()
                };
            chart.Columns.Add(new ChartColumnSchema("TX_Status", "Status", "string", true));
            chart.Columns.Add(new ChartColumnSchema("NU_Users", "Users", "number", true));
            charts.Add(chart);
            return charts;
        }

        public List<ChartSchema> GetCharts()
        {
            var sqlDataSources = new List<ChartSchema>();
            sqlDataSources.AddRange(GetApplicationCharts());
            sqlDataSources.AddRange(GetBaseCharts());
            return sqlDataSources;
        }
    }
}

