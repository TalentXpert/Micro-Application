import { MicroApplicationEvent, MicroApplicationOperation } from "../micro-application.event";
import { ChartColumnSchema, ChartSchema, DataSources } from "./chart.builder.model";

const ColumnHeader = [
    { ColumnId: 1, columnName: "Data Source Columns" },
    { ColumnId: 2, columnName: "Columns For Chart " },
];


export class DragTable {
    rows: DragTableRows[] = [];
    columns = ColumnHeader;
    constructor(children: ChartColumnSchema[], cardHeight: number) {
        this.rows.push(new DragTableRows(children, cardHeight));
    }
}

export class DragTableCell {
    children: ChartColumnSchema[] = [];
    originalChildren: ChartColumnSchema[] = [];
    columnId: string = "";
    cardHeight: number;
    constructor(children, cardHeight: number) {
        this.children.length = 0;
        if (children !== undefined || children.length > 0)
            this.children = children;
        this.originalChildren = children;
        this.cardHeight = cardHeight;
    }
    getCellHeight() {
        return this.children.length * this.cardHeight;
    }
}

export class DragTableRows {
    columns: DragTableCell[] = [];
    children: ChartColumnSchema[] = [];
    cardHeight: number;
    rowHeight: number = 0;
    rowName: string = "";

    constructor(children, cardHeight) {
        this.children = children;
        this.cardHeight = cardHeight;

        ColumnHeader.forEach((column) => {
            this.addCell(column, this.cardHeight);
        });

        this.setRowHeight();
    }

    setRowHeight() {
        let height = this.cardHeight;
        this.columns.forEach((cell, index) => {
            let cellHeight = cell.getCellHeight()
            if (height < cellHeight)
                height = cellHeight;
        });
        this.rowHeight = height + this.cardHeight;
    }
    addCell(column, cardHeight: number) {
        let children = this.children.filter(c => c.ColumnId === column.ColumnId);
        this.columns.push(new DragTableCell(children, cardHeight));
    }
}


export class ChartBuilderViewModel {
    subscription: any = {};
    dataSource: DataSources[] = [];
    selectDashboardSchema: string = "";
    chartColumnSchema: ChartColumnSchema[] = [];
    dr!: DragTable;
    chartSchema: ChartSchema[] = [];
    selectedDataSourceColumns :ChartColumnSchema[] = [];


    constructor(private applicationPageEvent: MicroApplicationEvent) {
        this.subscribe();
    }

    subscribe() {
        this.subscription.name = this.applicationPageEvent.event.subscribe((event) => {
            if (event.operation == MicroApplicationOperation.LoadDataSourecesForChartBuilder)
                this.loadDataSources(event.data);
            if (event.operation == MicroApplicationOperation.LoadDataSoureceColumns)
                this.loadDataSoureceColumns(event.data);
            if (event.operation == MicroApplicationOperation.LoadAllCharts)
                this.loadAllCharts(event.data);
            if (event.operation == MicroApplicationOperation.LoadColumnsOfSelectedChart)
                this.loadColumnsOfSelectedChart(event.data);
                if (event.operation == MicroApplicationOperation.AddUpdateChartSchema)
                this.addUpdateChartSchema(event.data);
        });
    }

    loadDataSources(dataSources) {
        this.dataSource.length = 0;
        this.dataSource = dataSources;

    }

    loadDataSoureceColumns(data) {
        this.chartColumnSchema.length = 0;
        this.selectedDataSourceColumns = [...data];
        data.forEach(element => {
            this.chartColumnSchema.push(new ChartColumnSchema(element, 1));
        });
        this.createDataTable();
    }

    loadAllCharts(chartSchema: ChartSchema[]) {
        this.chartSchema.length = 0;
        this.chartSchema = chartSchema;
    }

    loadColumnsOfSelectedChart(chartId) {
        let index = this.chartSchema.findIndex(chart => chart.Id == chartId);
        if (index > -1) {
            this.chartSchema[index].Columns.forEach(element => {
                var index = this.chartColumnSchema.findIndex(c => c.DatabaseColumnName == element.DatabaseColumnName);
                if (index > -1) this.chartColumnSchema.splice(index, 1);
                this.chartColumnSchema.push(new ChartColumnSchema(element, 2));
            });
        }
        this.createDataTable();
    }

    createDataTable() {
        this.dr = new DragTable(this.chartColumnSchema, 20);
    }

    ChangeColumnId(field: ChartColumnSchema) {
        var index = this.chartColumnSchema.findIndex(c => c.DatabaseColumnName == field.DatabaseColumnName);
        if (index > -1) {
            this.chartColumnSchema[index].ColumnId = field.ColumnId == 1 ? 2 : 1;
        }
        this.createDataTable();
    }

    addUpdateChartSchema(chartSchema:ChartSchema){
        let index = this.chartSchema.findIndex(cs =>cs.Id ==chartSchema.Id);
        if(index > -1){
            this.chartSchema[index].Columns = chartSchema.Columns;
            this.chartSchema[index].DataSourceId = chartSchema.DataSourceId;
            this.chartSchema[index].ChartType = chartSchema.ChartType;
            this.chartSchema[index].Name = chartSchema.Name;
            this.chartSchema[index].Columns = chartSchema.Columns;
            this.chartSchema[index].Description = chartSchema.Description;
            this.chartSchema[index].MinWidth = chartSchema.MinWidth;
            this.chartSchema[index].MinHeight = chartSchema.MinHeight;
        }
        else this.chartSchema.push(chartSchema);

    }

    unsubscribe() {
        this.subscription.name.unsubscribe();
    }
}