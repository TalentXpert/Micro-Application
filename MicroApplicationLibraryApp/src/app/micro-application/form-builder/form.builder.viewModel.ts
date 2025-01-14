import { MicroApplicationEvent, MicroApplicationOperation } from "../micro-application.event";
import { UtilityService } from "../library/utility.service";
import { AppControlVM, AppFormControlListForDataTable, AppFormControlListVM, AppFormList, AppPageList, PageBuilderInfoVM } from "./form.builder.model";


const ColumnHeader = [
    { ColumnId: 1, columnName: "Application Control" },
    { ColumnId: 2, columnName: "Form Control" },
];


export class DragTable {
    rows: DragTableRows[] = [];
    columns = ColumnHeader;
    constructor(children: AppFormControlListForDataTable[], cardHeight: number) {
        this.rows.push(new DragTableRows(children, cardHeight));
    }
}

export class DragTableCell {
    children: AppFormControlListForDataTable[] = [];
    originalChildren: AppFormControlListForDataTable[] = [];
    columnId: string;
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
    children: AppFormControlListForDataTable[] = [];
    cardHeight: number;
    rowHeight: number;
    rowName: string;

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


export class FormBuilderViewModel {
    subscription: any = {};
    appControlVM: AppControlVM[] = [];
    copyAppControlVM: AppControlVM[] = [];
    appFormControlListVM: AppFormControlListVM[] = [];
    appFormControlListForDataTable: AppFormControlListForDataTable[] = [];
    dr: DragTable;

    pageBuilderInfoVM: PageBuilderInfoVM;
    appFormList: AppFormList[] = []

    constructor(private applicationPageEvent: MicroApplicationEvent, private utilityService: UtilityService) {
        this.subscribe();
    }

    subscribe() {
        this.subscription.name = this.applicationPageEvent.event.subscribe((event) => {
            if (event.operation == MicroApplicationOperation.LoadPageBuilderInfoVM)
                this.loadPageBuilderInfoVM(event.data);
            if (event.operation == MicroApplicationOperation.LoadFormFixedControls)
                this.loadFormFixedControls(event.data);

        });
    }

    loadPageBuilderInfoVM(data: PageBuilderInfoVM) {
        this.appFormControlListForDataTable = [];
        this.pageBuilderInfoVM = data;
        this.appControlVM.length = 0;
        this.appFormList.length = 0;
        this.appFormList = data.Pages;
        this.appControlVM = data.Controls;
        this.copyAppControlVM = [...data.Controls];
        console.log(this.copyAppControlVM)
        this.appControlVM.forEach(element => {
            this.appFormControlListForDataTable.push(new AppFormControlListForDataTable(element, 1));
        });

    }

    loadFormFixedControls(data) {
        if (this.appControlVM.length > 0) {
            this.appFormControlListVM.length = 0;
            this.appFormControlListVM = data;
            this.removeAppControls();
            this.appFormControlListVM.forEach(element => {
                this.appFormControlListForDataTable.push(new AppFormControlListForDataTable(element, 2));
            });
            this.createDataTable();
        }
    }

    removeAppControls() {
        this.appFormControlListVM.forEach(element => {
            var index = this.appFormControlListForDataTable.findIndex(c => c.Id == element.AppControlId);
            if (index > -1) this.appFormControlListForDataTable.splice(index, 1);
        });
    }

    ChangeColumnId(field: AppFormControlListForDataTable) {
        var index = this.appFormControlListForDataTable.findIndex(c => c.Id == field.Id);
        if (index > -1) {
            this.appFormControlListForDataTable[index].ColumnId = field.ColumnId == 1 ? 2 : 1;
            this.setPosition(field);
        }
    }

    setPosition(field: AppFormControlListForDataTable) {
        let columnId = field.ColumnId;
        var filteredData = this.appFormControlListForDataTable.filter(c => c.ColumnId == columnId);
        var index = this.appFormControlListForDataTable.findIndex(c => c.Id == field.Id)
        if (index > -1) this.appFormControlListForDataTable[index].Position = filteredData.length + 1;
        this.createDataTable();
    }

    createDataTable() {
        this.appFormControlListForDataTable = this.utilityService.sortDataForOptions(this.appFormControlListForDataTable, 'Position');
        this.dr = new DragTable(this.appFormControlListForDataTable, 20);
    }

    removeItemFromList() {
        this.appFormControlListForDataTable.length = 0;
        this.copyAppControlVM.forEach(element => {
            this.appFormControlListForDataTable.push(new AppFormControlListForDataTable(element, 1));
        });
    }

    deleteFormControl(field: AppFormControlListForDataTable) {
        if (field.AppControlId == null) this.ChangeColumnId(field);
        else {
            var index = this.appFormControlListForDataTable.findIndex(c => c.AppControlId == field.AppControlId);
            if (index > -1) this.appFormControlListForDataTable.splice(index, 1);
            this.createDataTable();
        }
    }



    unsubscribe() {
        this.subscription.name.unsubscribe();
    }
}