using BaseLibrary.Controls.Charts;
using BaseLibrary.Domain.ComponentSchemas;

namespace MicroAppAPI.Configurations
{
    public class ApplicationChart : BaseChart
    {
        public static Guid NoisePollutionId = Guid.Parse("3CD966DA-77D8-43AC-B65B-0B0019776BE0");
        public static Guid AvgNoisePollutionId = Guid.Parse("CA88DE1F-7401-4586-B28B-D13BE4CC763E");
        public static Guid MaxNoisePollutionId = Guid.Parse("549C96E0-F47F-40DC-BB19-26D876108E80");
        public static Guid MinNoisePollutionId = Guid.Parse("64E90CB5-CCBE-485B-8785-D712E921D248");
        public static List<ChartSchema> GetChartSchemas()
        {
            var charts = new List<ChartSchema>();
            var chart =
                new ChartSchema
                {
                    ChartType = ChartType.Bar.Type,
                    MinWidth = 380,
                    MinHeight = 350,
                    DataSourceId = Guid.Parse("E95B2FAC-29C0-C620-80BB-08DC01368872"),
                    Description = "",
                    Id = NoisePollutionId,
                    Name = "Noise Pollution",
                    Columns = new List<ChartColumnSchema>()
                };
            chart.Columns.Add(new ChartColumnSchema("TX_Sensor", "Area", "string", true));
            chart.Columns.Add(new ChartColumnSchema("NU_AvgNoise", "Average Noise", "number", true));
            chart.Columns.Add(new ChartColumnSchema("NU_MaxNoise", "Maximum Noise", "number", true));
            chart.Columns.Add(new ChartColumnSchema("NU_MinNoise", "Minimum Noise", "number", true));
            charts.Add(chart);


            chart =
            new ChartSchema
            {
                ChartType = ChartType.Bar.Type,
                MinWidth = 380,
                MinHeight = 350,
                DataSourceId = Guid.Parse("E95B2FAC-29C0-C620-80BB-08DC01368872"),
                Description = "",
                Id = AvgNoisePollutionId,
                Name = "Average Pollution",
                Columns = new List<ChartColumnSchema>()
            };
            chart.Columns.Add(new ChartColumnSchema("TX_Sensor", "Area", "string", true));
            chart.Columns.Add(new ChartColumnSchema("NU_AvgNoise", "Average Noise", "number", true));
            charts.Add(chart);


            chart =
               new ChartSchema
               {
                   ChartType = ChartType.HorizontalLine.Type,
                   MinWidth = 380,
                   MinHeight = 350,
                   DataSourceId = Guid.Parse("E95B2FAC-29C0-C620-80BB-08DC01368872"),
                   Description = "",
                   Id = MaxNoisePollutionId,
                   Name = "Noise Pollution",
                   Columns = new List<ChartColumnSchema>()
               };
            chart.Columns.Add(new ChartColumnSchema("TX_Sensor", "Area", "string", true));
            chart.Columns.Add(new ChartColumnSchema("NU_MaxNoise", "Maximum Noise", "number", true));
            charts.Add(chart);

            chart =
                new ChartSchema
                {
                    ChartType = ChartType.HorizontalLine.Type,
                    MinWidth = 380,
                    MinHeight = 350,
                    DataSourceId = Guid.Parse("E95B2FAC-29C0-C620-80BB-08DC01368872"),
                    Description = "",
                    Id = MinNoisePollutionId,
                    Name = "Noise Pollution",
                    Columns = new List<ChartColumnSchema>()
                };
            chart.Columns.Add(new ChartColumnSchema("TX_Sensor", "Area", "string", true));
            chart.Columns.Add(new ChartColumnSchema("NU_MinNoise", "Minimum Noise", "number", true));
            charts.Add(chart);
            return charts;
        }

        protected override List<ChartSchema> GetApplicationCharts()
        {
            return GetChartSchemas();
        }
    }



}
