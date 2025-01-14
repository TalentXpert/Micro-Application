import { GridCell } from "../../application-page-container/application.page.model";
import { AppControlVM } from "../../form-builder/form.builder.model";


export class DashboardGraph {
    public Type: string;
}
export class DashboardSummaary {
    public DashboardGrids: DashboardGrid[] = [];
}

export class DashboardGrid {
    public Title: string;
    public Headers: DashboardGridHeader[] = [];
    public Rows: GridCell[] = [];
}

export class DashboardGridHeader {
    public HeaderIdentifier: string;
    public Position: number;
    public HeaderText: string;// visible to  user and can be localized. do not write any logic on it. 
    public Alignment: string;
    public DataType: string;
    public MinWidth: number;// mini width required for this column.
}

export class SaveDashboardGridVM {
    public Id?: string;
    public Name: string;
    public DataSource: string;
    public Description: string;
    public AppControls: AppControlVM[] = [];
}

export class DashboardChart {
    public ChartType: string="";
    public Title: string="";
    public Columns: ChartColumn[] = [];
    public MinHeight: number=0;
    public MaxWidth: number=0;
    public SeriesData: any[]=[];

}
export class ChartColumn
{
    public Title: string;
    public DataType: string;
}
