export class DashboardSchema {
    public Id?: string;
    public Name: string="";
    public Description: string="";
    public MinimumRowHeight: number=0;
    public MenuId: string="";
    public Position: number=0;
    public Rows: DashboardRow[] = [];
}

export class DashboardRow {
    public Position: number;
    public Height: number;
    public Panels: DashboardPanel[] = [];
    constructor(formValue:any){
        this.Position = formValue.Position;
        this.Height = formValue.Height;
    }
}

export class DashboardPanel {
    public Title: string;
    public Width: number;
    public ContentType: string;
    public Position: number;
    public ContentId?: string;
    constructor(formValue:any){
        this.Title = formValue.Title;
        this.Width = formValue.Width;
        this.ContentType = formValue.ContentType;
        this.Position = formValue.Position;
        this.ContentId = formValue.ContentId? formValue.ContentId:null;
    }
}

export class Content{
    public Id: string="";
    public Name: string="";
}
