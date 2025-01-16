export class DataSources {
    public Id: string = "";
    public Name: string = "";
}

export class ChartType {
    public Name: string = "";
}

export class ChartColumnSchema {
    public DatabaseColumnName: string;
    public Title: string
    public DataType: string;
    public IsMandatory: boolean;
    public Color: string;
    public ColumnId: number;
    constructor(data, colunmId) {
        this.DatabaseColumnName = data.DatabaseColumnName;
        this.Title = data.Title;
        this.DataType = data.DataType;
        this.DataType = data.DataType;
        this.IsMandatory = data.IsMandatory;
        this.Color = data.Color;
        this.ColumnId = colunmId;
    }
}

export class ChartSchema {
    public Id: string = "";
    public Name: string = "";
    public Description: string = "";
    public DataSourceId: string = "";
    public ChartType: string = "";
    public MinWidth: number = 0;
    public MinHeight: number = 0;
    public Columns: ChartColumnSchema[] = [];
}