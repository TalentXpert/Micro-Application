import { flatMap } from "rxjs";
import { MenuModel } from "../menu/menu.model";


export class AppFormList {
    public Id: string = "";
    public Name: string = "";
    public MenuId: string = "";
    public Position: number = 0;
    public EditPermission: string;
    public DeletePermission: string;
    public ViewPermission: string;
}

// export class AppFormControlRequestVM {
//     public FormId: string;
//     public GlobalFormValue?: string;
// }

export class AppControlVM {
    public Id: string = "";
    public ControlIdentifier: string = "";
    public DataType: string = "";
    public ControlType: string = "";
    public DisplayLabel: string = "";
    public IsParent: boolean = false;
    public ParentControlIdentifier?: string;
    public IsGlobalParent: boolean = false;
    public Options?: string;
}


export class AppFormControlListVM {
    public Id: string = "";
    public AppControlId: string = "";
    public ControlIdentifier: string = "";
    public DisplayLabel: string = "";
    public DataType: string = "";
    public ControlType: string = "";
    public Position: number = 0;
    public IsEditable: boolean = false;
    public IsMandatory: boolean = false;
    public IsUnique: boolean = false;
    public Maximum: string = "";
    public Minimum: string = "";
    public OrganizationId?: string;
    public IsFixed: boolean = false;
    public IsSingleLine: boolean = false;
    public IsGlobalControl?: boolean;
    public Options?: string;
    public Tooltip?: string
    public CanBeGlobalControl: boolean;
}

export class AppFormControlListForDataTable {
    public Id: string;
    public ControlIdentifier: string;
    public DataType: string;
    public ControlType: string;
    public DisplayLabel: string;
    public IsParent: boolean;
    public ParentControlIdentifier?: string;
    public IsGlobalParent: boolean;
    public Options?: string;
    public AppControlId: string;
    public Position: number;
    public IsEditable: boolean;
    public IsMandatory: boolean;
    public IsUnique: boolean;
    public Maximum: string;
    public Minimum: string;
    public IsFixed: boolean;
    public ColumnId: number;
    public IsSingleLine: boolean;
    public IsGlobalControl?: boolean;
    public Tooltip?: string
    public CanBeGlobalControl: boolean;


    constructor(data: any, columnId: number) {
        if(data == null) return;
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
        this.Position = data.Position;
        this.IsEditable = data.IsEditable ? data.IsEditable : false;
        this.IsMandatory = data.IsMandatory ? data.IsEditable : false;;
        this.IsUnique = data.IsUnique ? data.IsEditable : false;;
        this.Maximum = data.Maximum;
        this.Minimum = data.Minimum;
        this.IsFixed = data.IsFixed;
        this.ColumnId = columnId;
        this.IsSingleLine = data.IsSingleLine;
        this.IsGlobalControl = data.IsGlobalControl;
        this.Tooltip = data.Tooltip ? data.Tooltip : null;
        this.CanBeGlobalControl = (data.IsGlobalParent || data.ParentControlIdentifier) ? false : true;
    }
}



export class AppFormControlVM {
    public Id?: string;
    public AppControlId: string;
    public Position: number;
    public IsEditable: boolean;
    public IsMandatory: boolean;
    public IsUnique: boolean;
    public Minimum: string;
    public Maximum: string;
    public IsSingleLine: boolean;
    public DisplayLabel?: string;
    public IsGlobalControl?: boolean;
    public Options?: string;
    constructor(data: any) {
        this.AppControlId = data.AppControlId;
        this.Position = data.Position;
        this.IsEditable = data.IsEditable;
        this.IsMandatory = data.IsMandatory;
        this.IsUnique = data.IsUnique;
        this.Minimum = data.Minimum;
        this.Maximum = data.Maximum;
        this.IsSingleLine = data.IsSingleLine;
        this.DisplayLabel = data.DisplayLabel;
        this.IsGlobalControl = data.IsGlobalControl;
        this.Options = data.Options;
    }

}

// export class AppFormResetRequestVM {
//     public FormId: string;
//     public LayoutControlValue?: string;
// }

///22/11/2023
export class PageBuilderInfoVM {
    public Pages: AppFormList[] = [];
    public Controls: AppControlVM[] = [];
    public TopMenus: MenuModel[] = [];
}

export class AppPageList {
    public Id: string = "";
    public Name: string = "";
}

export class AppPageSaveUpdateVM {
    public Id: string;
    public Name: string;
    public Position: number;
    public MenuId: string;
    public EditPermission: string;
    public DeletePermission: string;
    public ViewPermission: string;
    constructor(data: any) {
        this.Name = data.Name;
        this.Position = parseInt(data.Position);
        this.MenuId = data.MenuId;
        this.EditPermission = data.EditPermission;
        this.DeletePermission = data.DeletePermission;
        this.ViewPermission = data.ViewPermission;
    }

}

export class AppFormFixedControlAddUpdateVM {
    public FormId: string = "";
    public AppFormControls: AppFormControlVM[] = [];
    constructor(formControlsToAdd: AppFormControlListForDataTable[]) {
        formControlsToAdd.forEach(element => {
            this.AppFormControls.push(new AppFormControlVM(element))
        })
    }
}