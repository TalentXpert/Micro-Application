
export class AppFormList {
    public Id: string="";
    public Name: string="";
    public MenuId: string="";
    public Position: number=0;;
}

export class AppFormControlRequestVM {
    public FormId: string="";
    public GlobalFormValue?: string="";
}

export class AppControlVM {
    public Id: string="";
    public ControlIdentifier: string="";
    public DataType: string="";
    public ControlType: string="";
    public DisplayLabel: string="";
    public IsParent: boolean= false;
    public ParentControlIdentifier?: string="";
    public IsGlobalParent: boolean= false;
    public Options?: string="";
}


export class AppFormControlListVM {
    public Id: string="";
    public AppControlId: string="";
    public ControlIdentifier: string="";
    public DisplayLabel: string="";
    public DataType: string="";
    public ControlType: string="";
    public Position: number =0;
    public IsEditable: boolean= false;
    public IsMandatory: boolean= false;
    public IsUnique: boolean= false;
    public Maximum: string="";
    public Minimum: string="";
    public OrganizationId?: string="";
    public IsFixed: boolean= false;
    public IsSingleLine: boolean= false;
    public IsGlobalControl?: boolean= false;
    public Options?: string="";
}

export class AppFormControlListForDataTable {
    public Id: string="";
    public ControlIdentifier: string="";
    public DataType: string="";
    public ControlType: string="";
    public DisplayLabel: string="";
    public IsParent: boolean= false;
    public ParentControlIdentifier?: string="";
    public IsGlobalParent: boolean= false;
    public Options?: string="";
    public AppControlId: string="";
    public Position: number =0;
    public IsEditable: boolean= false;
    public IsMandatory: boolean= false;
    public IsUnique: boolean= false;
    public Maximum: string="";
    public Minimum: string="";
    public IsFixed: boolean= false;
    public ColumnId: number=0;
    public IsSingleLine: boolean= false;
    public IsGlobalControl?: boolean= false;


    constructor(data, columnId) {
        if(data ==null ) return ;
        this.Id = data.Id;
        this.AppControlId = data.AppControlId ? data.AppControlId : data.Id;
        this.ControlIdentifier = data.ControlIdentifier;
        this.DataType = data.DataType;
        this.ControlType = data.ControlType;
        this.DisplayLabel = data.DisplayLabel;
        this.IsParent = data.IsParent;
        this.ParentControlIdentifier = data.ParentControlIdentifier ? data.ParentControlIdentifier : null;
        this.IsGlobalParent = data.IsGlobalParent;
        this.Options = data.Options;
        this.Position = data.Position ? data.Position : false;
        this.IsEditable = data.IsEditable ? data.IsEditable : false;
        this.IsMandatory = data.IsMandatory ? data.IsMandatory : false;
        this.IsUnique = data.IsUnique ? data.IsUnique : false;
        this.Maximum = data.Maximum;
        this.Minimum = data.Minimum;
        this.IsFixed = data.IsFixed ? true : false;
        this.ColumnId = columnId;
        this.IsSingleLine = data.IsSingleLine ? true : false;
        this.IsGlobalControl = data.IsGlobalControl == undefined ? "" : data.IsGlobalControl;
    }
}



export class AppFormControlVM {
    public Id?: string="";
    public AppControlId: string="";
    public Position: number;
    public IsEditable: boolean= false;
    public IsMandatory: boolean= false;
    public IsUnique: boolean= false;
    public Minimum: string="";
    public Maximum: string="";
    public IsSingleLine: boolean= false;
    public DisplayLabel?: string="";
    public IsGlobalControl?: boolean= false;
    public Options?: string="";
    constructor(data) {
        this.AppControlId = data.AppControlId;
        this.Position = data.Position;
        this.IsEditable = data.IsEditable;
        this.IsMandatory = data.IsMandatory;
        this.IsUnique = data.IsUnique;
        this.Minimum = data.Minimum;
        this.Maximum = data.Maximum;
        this.IsSingleLine = data.IsSingleLine;
        this.DisplayLabel = data.DisplayLabel;
        this.IsGlobalControl = data.IsGlobalControl? data.IsGlobalControl: null;
        this.Options = data.Options;
    }

}

export class AppFormResetRequestVM {
    public FormId: string="";
    public LayoutControlValue?: string="";
}



export class AppFormControlAddUpdateVM {
    public FormId: string="";
    public GlobalControlValue?: string="";
    public AppFormControls: AppFormControlVM[] = [];
    constructor(formControlsToAdd: AppFormControlListForDataTable[]) {
        formControlsToAdd.forEach(element => {
            this.AppFormControls.push(new AppFormControlVM(element))
        })
    }
}