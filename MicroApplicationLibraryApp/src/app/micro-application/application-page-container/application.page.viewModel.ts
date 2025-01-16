
import { ApplicationPageListViewModel, GridCell, GridHeader, GridModel, HeaderValue, SmartAction, SmartGrid, SmartPage, UserGridFilter } from "./application.page.model";
import { MicroApplicationEvent, MicroApplicationOperation } from "../micro-application.event";



export class ApplicationPageViewModel {
    subscription: any = {};
    smartPage: SmartPage;
    smartGrid: SmartGrid = new SmartGrid;
    gridModel: GridModel = new GridModel;
    rows: GridCell[] = []
    filterId?: string ;
    pageSize: number = 0;
    paginationStartIndex: number = 0;
    paginationEndIndex: number = 0;
    disableNextBtn: boolean = false;
    disablePreviousBtn: boolean = true;
    gridHeaders: GridHeader[] = [];
    listView: ApplicationPageListViewModel[] = [];

    constructor(private applicationPageEvent: MicroApplicationEvent) {
        this.smartPage = new SmartPage();
        this.subscribe();
    }

    subscribe() {
        this.subscription.name = this.applicationPageEvent.event.subscribe((event) => {
            if (event.operation == MicroApplicationOperation.LoadSmartPageState)
                this.loadSmartPageState(event.data);
            if (event.operation == MicroApplicationOperation.LoadSmartGrid)
                this.loadSmartGrid(event.data);
            if (event.operation == MicroApplicationOperation.LoadGridModel)
                this.loadGridModel(event.data);
            if (event.operation == MicroApplicationOperation.LoadListModel)
                this.loadListModel();
            if (event.operation == MicroApplicationOperation.AddUpdateUserFilter)
                this.addUpdateUserFilter(event.data);
            if (event.operation == MicroApplicationOperation.DeleteUserFilter)
                this.deleteUserFilter(event.data);
        });
    }


    loadSmartPageState(smartPage: SmartPage) {
        if (smartPage) {
            this.smartPage.CurrentGridFilterId = smartPage.CurrentGridFilterId;
            this.smartPage.PageId = smartPage.PageId;
            this.smartPage.GlobalControls = smartPage.GlobalControls;
            this.smartPage.PageSize = smartPage.PageSize;
            this.smartPage.CurrentGridFilterId = smartPage.CurrentGridFilterId;
            this.smartPage.PageActions = smartPage.PageActions;
            this.smartPage.PageTitle = smartPage.PageTitle;
            this.pageSize = smartPage.PageSize;
        }
    }

    loadSmartGrid(smartGrid: SmartGrid) {
        this.smartGrid = smartGrid;
    }

    loadGridModel(gridModel: GridModel) {
        this.gridModel = gridModel;
        this.gridHeaders.length = 0;
        this.gridModel.Headers.forEach(header => {
            this.gridHeaders.push(new GridHeader(header))
        });
        this.rows = [...this.gridModel.Rows];
        this.onChangePageSize("Grid View");

    }

    loadListModel() {
        this.listView = [];
        this.rows.forEach(row => {
            let i = 0;
            let singleLineList: string[] = [];
            let HeaderValueList: HeaderValue[] = [];;
            let actions: SmartAction[]=[];
            this.gridModel.Headers.forEach(header => {
                if(header.Actions.length > 0) actions = header.Actions;
                if (header.IsSingleLine || header.Position == 2) singleLineList.push(row[i].T);
                else if (!header.IsSingleLine && header.IsVisible) {
                    HeaderValueList.push(new HeaderValue(header.HeaderText, row[i].T ))
                }
                i++;
            })
            this.listView.push(new ApplicationPageListViewModel(singleLineList, HeaderValueList, row, actions))
        })
    }

    addUpdateUserFilter(userGridFilter: UserGridFilter) {
        let index = this.smartGrid.UserGridFilters.findIndex(ugf => ugf.Id == userGridFilter.Id);
        if (index > -1) this.smartGrid.UserGridFilters[index] = userGridFilter;
        else this.smartGrid.UserGridFilters.push(userGridFilter);

    }

    deleteUserFilter(currentGridFilterId) {
        let index = this.smartGrid.UserGridFilters.findIndex(ugf => ugf.Id == currentGridFilterId);
        if (index > -1) this.smartGrid.UserGridFilters.splice(index, 1);
    }

    //#region Pagination methods start here 

    onChangePageSize(viewMode) {
        this.paginationStartIndex = 1;
        this.paginationEndIndex = this.pageSize;
        if (this.paginationEndIndex > this.gridModel.Rows.length) this.paginationEndIndex = this.gridModel.Rows.length;
        this.loadDataDependsOnPagination();
        this.checkForNextPreviousButton();
        if(viewMode == 'List View') this.loadListModel();
    }

    loadDataDependsOnPagination() {
        this.rows.length = 0;
        if (this.gridModel && this.gridModel.Rows) {
            for (let i = this.paginationStartIndex - 1; i < this.paginationEndIndex; i++) {
                let row = this.gridModel.Rows[i];
                if (row) this.rows.push(row);
            }
        }
    }

    loadPreviousData(viewMode) {
        this.paginationStartIndex = (this.paginationStartIndex - this.pageSize);
        if (this.paginationStartIndex <= 0) this.paginationStartIndex = 1;
        this.paginationEndIndex = ((this.paginationStartIndex - 1) + this.pageSize);
        if (this.paginationEndIndex > this.gridModel.Rows.length) this.paginationEndIndex = this.gridModel.Rows.length;
        this.loadDataDependsOnPagination();
        this.checkForNextPreviousButton();
        if(viewMode == 'List View') this.loadListModel();

    }

    loadNextData(viewMode) {
        this.paginationStartIndex = (this.paginationEndIndex + 1);
        this.paginationEndIndex = (this.paginationEndIndex + this.pageSize);
        if (this.paginationEndIndex > this.gridModel.Rows.length) this.paginationEndIndex = this.gridModel.Rows.length;
        this.loadDataDependsOnPagination();
        this.checkForNextPreviousButton();
        if(viewMode == 'List View') this.loadListModel();
    }

    checkForNextPreviousButton() {
        this.checkForNextBtnEnable();
        this.checkForPreviosBtnEnable();
    }

    checkForPreviosBtnEnable() {
        if (this.paginationStartIndex == 1)
            return this.disablePreviousBtn = true;
        else
            return this.disablePreviousBtn = false;
    }

    checkForNextBtnEnable() {
        if (this.paginationEndIndex >= this.gridModel.Rows.length)
            return this.disableNextBtn = true;
        else
            return this.disableNextBtn = false;
    }
    //#region Pagination methods ends here 

    sortData(gridHeader: GridHeader) {
        this.gridHeaders.forEach((header) => {
            if (header.Position !== gridHeader.Position) {
                header.SortingType = "";
            }
        });
        this.gridModel.Rows = gridHeader.sortRows(this.rows, gridHeader.Position-1);
        this.paginationStartIndex = 1;
        this.loadDataDependsOnPagination();
    }

    unsubscribe() {
        this.subscription.name.unsubscribe();
    }
}
