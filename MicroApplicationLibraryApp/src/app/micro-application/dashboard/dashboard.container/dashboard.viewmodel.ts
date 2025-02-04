import { MicroApplicationEvent, MicroApplicationOperation } from "../../micro-application.event";
import { DashboardChart } from "./dashboard.model";


export class DashboardViewModel {
    subscription: any = {};
    summaryDataList: Map<string, DashboardChart>;

    constructor(private applicationPageEvent: MicroApplicationEvent) {
        this.summaryDataList = new Map<string, DashboardChart>();
        this.subscribe();
    }

    subscribe() {
        this.subscription.name = this.applicationPageEvent.event.subscribe((event) => {
            
        });
    }

    saveDashboardPanelData(panelId: string, dashboardChart: DashboardChart){
        this.summaryDataList.set(panelId, dashboardChart);
    }



    unsubscribe() {
        this.subscription.name.unsubscribe();
    }
}