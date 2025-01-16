import { Subscription } from "rxjs";
import { MicroApplicationEvent, MicroApplicationOperation } from "../micro-application.event";

export class InputSelectFromListForm{
    public FormId: string = "";
    public EntityId: string = "";
    public Text: string = "";
}

export class SelectFromListFormInput {
    public FormTitle: string = "";
    public FormId: string = "";
    public EntityId: string = "";
    public FormDataLabel: string = "";
    public FormDataValue: string = "";
    public ItemLabel: string = "";
    public Items: SelectFromListFormInputItem[] = [];
}


export class SelectFromListFormInputItem {
    public ItemId: string = "";
    public Title: string = "";
    public DetailLines: string[] = [];
}

export class SelectFromListFormData {
    public FormId: string = "";
    public EntityId: string = "";
    public SelectedItems: string[] = [];
}

export class SelectFromListFormInputItemVM {
    public ItemId: string;
    public Title: string;
    public DetailLines: string[] = [];
    public IsSelected: boolean = false;
    constructor(itemId, title, detailLines, isSelected) {
        this.ItemId = itemId;
        this.Title = title;
        this.DetailLines = detailLines;
        this.IsSelected = isSelected;
    }

}


export class SelectFromListFormModel {
    subscription!: Subscription;
    selectedAll: boolean = false;
    selectFromListFormInputItemVM: SelectFromListFormInputItemVM[] = [];
   

    constructor(private applicationPageEvent: MicroApplicationEvent) {
        this.subscribe();
        
    }

    subscribe() {
        this.subscription = this.applicationPageEvent.event.subscribe((event) => {
            if (event.operation == MicroApplicationOperation.LoadSelectFromListFormInputItem)
                this.loadSelectFromListFormInputItem(event.data);
        })
    }

    loadSelectFromListFormInputItem(data: SelectFromListFormInputItem[]) {
        this.selectedAll = false;
        this.selectFromListFormInputItemVM.length = 0;
        data.forEach(element => {
            this.selectFromListFormInputItemVM.push(new SelectFromListFormInputItemVM(element.ItemId, element.Title,element.DetailLines, false));
        });

    }


    onclickedCheckbox() {
        let selectedPermissions = this.selectFromListFormInputItemVM.filter(e => e.IsSelected === true);
        if (selectedPermissions.length == this.selectFromListFormInputItemVM.length) this.selectedAll = true;
        else this.selectedAll = false;
    }

    convertListToString(permissons: any[]) {
        let roleperm;
        permissons.forEach(permission => {
            if(permission.IsSelected) roleperm = permissons.map(p => p.Name).join(", ");
        });
        return roleperm;
      }

    unsubscribe() {
        this.subscription.unsubscribe();
    }
}

