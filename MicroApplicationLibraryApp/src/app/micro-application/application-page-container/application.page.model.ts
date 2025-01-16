

export class DateVM {
    Year: number = 0;
    Month: number = 0;
    Day: number = 0;
}

export class SmartPage {
    public PageId: string = "";
    public PageTitle: string = "";
    public CurrentGridFilterId?: string;
    public PageSize: number = 0;
    public PageActions: SmartAction[] = [];
    public GlobalControls: UIControl[] = [];
}


export class SmartAction {
    public FormId: string = "";
    public FormType: string = "";
    public Text: string = "";
    public Param: string = "";
    public ActionIdentifier: string = "";
    public FormMode: string = "";
}

export class UIControl {
    public ControlId: string = "";
    public Position: number = 0;
    public ControlIdentifier: string = "";
    public DataType: string = "";
    public ControlType: string = "";
    public DisplayLabel: string = "";
    public Value?: string;
    public IsEditable: boolean = false;
    public IsMandatory: boolean = false;
    public IsUnique: boolean = false;
    public Minimum?: number;
    public Maximum: number = 0;
    public IsParent: boolean = false;
    public ParentControlIdentifier: string = "";
    public IsGlobalControl: boolean = false;
    public Options: SmartControlOption[] = [];
    public IsFormLayoutOwner: boolean = false; //this enable this control to change form layout
    public IsPageRefreshNeeded: boolean = false;
    public Validators: any[] = []; // Added for client side use
    public ListOfSelectedValue: string[]; // Added for client side use
    constructor(data){
        this.ListOfSelectedValue=[];
        for (var prop in data) {
            this[prop] = data[prop];
          }
    }
}

export class SmartControlOption {
    public Value: string = "";
    public Label: string = "";
}


export class SmartGridConfigurationVM {
    public PageId: string;
    public GlobalControlValues: ControlValue[] = [];
    constructor(pageId) {
        this.PageId = pageId;
    }

}

export class ControlValue {
    public ControlId: string;
    public ControlIdentifier: string;
    public Values: string[]=[];
    constructor(controlId, controlIdentifier, value, values) {
        this.ControlId = controlId;
        this.ControlIdentifier = controlIdentifier;
        if(value) this.Values.push(value);
        if(values && values.length > 0) values.forEach(v => this.Values.push(v))
    }
}

export class SmartGrid {
    public PageId: string = "";
    public UserGridFilters: UserGridFilter[] = [];
    public Filters: UIControl[] = [];
}

export class UserGridFilter {
    public Id: string = "";
    public PageId: string;
    public FilterName: string;
    public Filters: ControlFilter[] = [];
    constructor(pageId: string, filterName: string, formValues: any[]=[]) {
        this.PageId = pageId;
        this.FilterName = filterName;
        formValues.forEach(f => {
            this.Filters.push(new ControlFilter(f.ControlId, f.ControlIdentifier, f.Value, f.Operator));
        })
    }
}

export class ControlFilter {
    public ControlId: string;
    public ControlIdentifier: string;
    public Value: string;
    public Operator: string;
    constructor(controlId, controlIdentifier, value, operator) {
        this.ControlId = controlId;
        this.ControlIdentifier = controlIdentifier;
        this.Value = value;
        this.Operator = operator;
    }
}


export class GridRequestVM {

    public PageId: string = "";
    public SortDirection: string = "";
    public PageNumber: number = 0;
    public PageSize: number = 0;
    public SortHeaderText: string = "";
    public Filters: ControlFilter[] = [];
    public GlobalFilters: ControlFilter[] = [];
    public FilterId?: string;
}

export class GridModel {
    public Headers: GridHeader[] = [];
    public Rows: GridCell[] = []
    public PagingInfo: GridPagingInfo[] = [];
}

export class GridCell {
    /// <summary>
    /// Text value visible to user
    /// </summary>
    public T: string = "";
    /// <summary>
    /// Style to be applied to grid cell
    /// </summary>
    public S: string = "";
    /// <summary>
    /// Value to be used for sorting for non string columns
    /// </summary>
    public V: number = 0;
}

export class GridHeader {
    public HeaderIdentifier: string;
    public Position: number;
    public HeaderText: string;// visible to  user and can be localized. do not write any logic on it. 
    public Alignment: string;
    public DataType: string;
    public IsVisible: boolean;//is header visible to user
    public MinWidth: number; // mini width required for this column.
    public Actions: SmartAction[] = [];// options for menus on this column.
    public SortingType: string;//Added for client side use sorting purpose
    public IsFilter: boolean; //Added for client side use filter purpose
    public IsFixed: boolean;
    public IsSingleLine :boolean;

    constructor(header) {
        this.HeaderIdentifier = header.HeaderIdentifier;
        this.Position = header.Position;
        this.HeaderText = header.HeaderText;
        this.Alignment = header.Alignment;
        this.DataType = header.DataType;
        this.IsVisible = header.IsVisible;
        this.MinWidth = header.MinWidth;
        this.Actions = header.Actions;
        this.SortingType = "";
        this.IsFilter = false;
        this.IsFixed = header.IsFixed;
        this.IsSingleLine = header.IsSingleLine;
    }

    sortRows(rows, position) {
        let that = this;

        if (this.SortingType === "")  this.SortingType = 'asc';
        

        if (this.SortingType === 'asc') {
            let sortedRows = this.sortAsAscending(rows, that, position);
            return sortedRows;
        }
        else if (this.SortingType === 'desc') {
            let sortedRows = this.sortAsDescending(rows, that, position);
            return sortedRows;
        }

        return rows;
    }

    sortAsAscending(rows, that, position) {
        let nullData = [];
        let dataToFilter = [];
        let data = [];
        if (that.DataType === 'date') {
            nullData = rows.filter(a => a[position]['T'] == "" || a[position]['T'] == null);
            data = rows.filter(a => a[position]['T'] != null || a[position]['T'] != undefined);
            data.sort((a, b) => new Date(a[position]['T']) < new Date(b[position]['T']) ? -1 : 1);
            dataToFilter = data;
        }
        if (that.DataType === 'int') {
            nullData = rows.filter(a => a[position]['T'] == undefined || a[position]['T'] == null);
            data = rows.filter(a => a[position]['T'] != null || a[position]['T'] != undefined);
            data.sort((a, b) => Number(a[position]['T']) > Number(b[position]['T']) ? 1 :
                Number(b[position]['T']) > Number(a[position]['T']) ? -1 : 0);
            dataToFilter = data;
        }
        if (that.DataType === 'string') {
            nullData = rows.filter(a => !a[position]['T']);
            data = rows.filter(a => a[position]['T']);
            data.sort((a:any, b: any) => a[position]['T'] && a[position]['T'].toLowerCase().replace('.', "") > b[position]['T'].toLowerCase().replace('.', "") ? -1 : 1);
            dataToFilter = data;
        }

        if (that.DataType === 'bool') {
            nullData = rows.filter(a => !a[position]['T'] && a[position]['T'] != false);
            data = rows.filter(a => a[position]['T'] || a[position]['T'] == false);
            data.sort((a, b) => {
                return (a[position]['T'] === b[position]['T']) ? 0 : a[position]['T'] ? -1 : 1;
            }
            );
            dataToFilter = data;
        }
        if (nullData) {
            nullData.forEach(a => {
                dataToFilter.push(a);
            })
        }
        this.SortingType = 'desc';
        return dataToFilter;

    }
    sortAsDescending(rows, that, position) {
        let nullData = [];
        let dataToFilter = [];
        let data = [];
        if (that.DataType === 'date') {
            nullData = rows.filter(a => a[position]['T'] == "" || a[position]['T'] == null);
            data = rows.filter(a => a[position]['T'] != null || a[position]['T'] != undefined);
            data.sort((a, b) => new Date(a[position]['T']) > new Date(b[position]['T']) ? -1 : 1);
            dataToFilter = data;
        }
        if (that.DataType === 'int') {
            nullData = rows.filter(a => a[position]['T'] == undefined || a[position]['T'] == null);
            data = rows.filter(a => a[position]['T'] != null || a[position]['T'] != undefined);
            data.sort((a, b) => Number(a[position]['T']) > Number(b[position]['T']) ? -1 :
                Number(b[position]['T']) > Number(a[position]['T']) ? 1 : 0);
            dataToFilter = data;
        }

        if (that.DataType === 'string') {
            nullData = rows.filter(a => !a[position]['T']);
            data = rows.filter(a => a[position]['T']);
            data.sort((a: any, b: any) => a[position]['T'].toLowerCase().replace('.', "") < b[position]['T'].toLowerCase().replace('.', "") ? -1 : 1);
            dataToFilter = data;
        }

        if (that.DataType === 'bool') {
            nullData = rows.filter(a => !a[position]['T'] && a[position]['T'] != false);
            data = rows.filter(a => a[position]['T'] || a[position]['T'] == false);
            data.sort((a, b) => {
                return (a[position]['T'] === b[position]['T']) ? 0 : a[position]['T'] ? 1 : -1;
            }
            );
            dataToFilter = data;
        }

        if (nullData) {
            nullData.forEach(a => {
                dataToFilter.push(a);
            })
        }
        this.SortingType = 'asc';
        return dataToFilter;
    }
}


export class GridPagingInfo {
    public PageNumber: number = 0;
    public TotalPages: number = 0;
    public PageSize: number = 0;
}

export class SmartFormGenerateRequest {
    public FormId: string; //programing use only
    public FormMode: string; //1-Add , 2 - Edit, 3 Copy
    public DataKey?: string;
    public GlobalControls: ControlValue[] = [];
    constructor(smartAction, dataKey) {
        this.FormId = smartAction.FormId;
        this.FormMode = smartAction.FormMode;
        this.DataKey = dataKey;
    }

}


export class SmartFormGenerateRequestInput {
    public ControlId?: string;
    public ControlIdentifer: string;
    public Value: string;
    constructor(controlId, controlIdentifer, value) {
        this.ControlId = controlId;
        this.ControlIdentifer = controlIdentifer;
        this.Value = value;
    }
}

export class UIForm {
    public Id: string = "";
    public Title: string = "";
    public UIControls: UIControl[] = [];
    public DataKey?: string;
}

export class SmartFormTemplateRequest {
    public FormId: string;
    public DataKey?: string; //row primary key
    public FormMode: string; //1-Add , 2 - Edit, 3 Copy
    public ControlValues: ControlValue[] = [];
    constructor(smartAction: SmartAction, smartControls: UIControl[], formValues) {
        this.FormId = smartAction.FormId;
        this.FormMode = smartAction.FormMode;
        smartControls.forEach(element => {
            let value ="";
            value = formValues[element.DisplayLabel];
            if (element.ControlType == 'DatePicker'){
                value = this.getDateFormat(formValues[element.DisplayLabel].date).toString();
            }
            if (element.ControlType == 'MultipleSelection' && formValues[element.DisplayLabel].length > 0) {
                let options = formValues[element.DisplayLabel].map(x => x).join(", ");
                value = options;
            }
            if(element.ControlType == 'MultipleSelection' && formValues[element.DisplayLabel].length == 0) value ="";
            this.ControlValues.push(new ControlValue(element.ControlId, element.ControlIdentifier, value, null))
        });
    }

    toDateVM(calenderFormatDate: any) {
        let dateVM = new DateVM();
        dateVM.Year = calenderFormatDate.year;
        dateVM.Month = calenderFormatDate.month;
        dateVM.Day = calenderFormatDate.day;
        return dateVM;
    }

    getDateFormat(fromDate: any) {
        return fromDate.year + "-" + fromDate.month + "-" + fromDate.day;
      }
}

export class UserGridHeadersVM {
    public PageId: string;
    public Headers: UserGridHeaderVM[] = [];
    constructor(pageId, headers: UserGridHeaderVM[]) {
        this.PageId = pageId;
        headers.forEach(element => {
            this.Headers.push(new UserGridHeaderVM(element.HeaderIdentifier, element.Position, element.HeaderText, element.IsVisible, element.IsFixed))
        });

    }

}
export class UserGridHeaderVM {
    public HeaderIdentifier: string;
    public HeaderText: string;
    public Position: number;
    public IsVisible: boolean;
    public IsFixed: boolean;
    constructor(headerIdentifier, position, headerText, isVisible, isFixed) {
        this.HeaderIdentifier = headerIdentifier;
        this.HeaderText = headerText;
        this.Position = position;
        this.IsVisible = isVisible;
        this.IsFixed = isFixed;

    }
}


export class ExcelImportTmplateRequest
{
    public FormId : string = "";
    public GlobalControls : ControlValue[]=[];
    public Rows : number = 0;
}

export class ExcelImportRequest
{
    public FormId : string = "";
    public File: File | null = null;
}

export class HeaderValue {
    public Value: string;
    public Header: string;
    constructor(header, value){
        this.Header = header;
        this.Value = value;
    }
}

export class ApplicationPageListViewModel
{
    public SingleLineList : string[]=[];
    public HeaderValuestring: HeaderValue[]=[];
    public Actions: SmartAction[]=[];
    public Row: GridCell[]=[];
    constructor(singleLineList, headerValuestring, row, actions){
        this.SingleLineList = singleLineList;
        this.HeaderValuestring = headerValuestring;
        this.Row = row;
        this.Actions = actions;

    }
}

