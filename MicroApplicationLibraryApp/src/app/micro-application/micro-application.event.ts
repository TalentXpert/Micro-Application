//https://stackoverflow.com/questions/30501577/how-to-communicate-between-component-in-angularjs-2


import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class MicroApplicationEvent {

    private _subject: BehaviorSubject<MicroApplicationEventData> = new BehaviorSubject(new MicroApplicationEventData(MicroApplicationOperation.LoadNewPageState, ''));
    public readonly event: Observable<MicroApplicationEventData> = this._subject.asObservable();
    
    public publish(globalEventData: MicroApplicationEventData) {
        this._subject.next(globalEventData);
    }
}

export class MicroApplicationEventData {
    constructor(public operation: MicroApplicationOperation, public data: any) {

    }
}

export enum MicroApplicationOperation {
    LoadNewPageState,
    LoadSmartPageState,
    LoadSmartGrid,
    LoadGridModel,
    LoadListModel,
    AddUpdateUserFilter,
    DeleteUserFilter,
    LoadPageBuilderInfoVM,
    LoadFormFixedControls,
    LoadApplicationControlsOnFC,
    LoadAppFormControlListVMOnFC,
    LoadMenusForMicroApplication,
    LoadAllDashboardSchema,
    LoadDataSourecesForChartBuilder,
    LoadDataSoureceColumns,
    LoadAllCharts,
    LoadColumnsOfSelectedChart,
    AddUpdateChartSchema,
    LoadSelectFromListFormInputItem


}



