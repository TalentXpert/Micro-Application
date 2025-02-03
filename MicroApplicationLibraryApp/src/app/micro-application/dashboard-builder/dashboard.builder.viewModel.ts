import { MicroApplicationEvent, MicroApplicationOperation } from "../micro-application.event";
import { DashboardSchema } from "./dashboard.builder.model";

export class DashboardBuilderViewModel {
    subscription: any = {};
    dashboardSchemaList: DashboardSchema[] = [];
    selectDashboardSchema:string ="";

    constructor(private applicationPageEvent: MicroApplicationEvent) {
        this.subscribe();
    }

    subscribe() {
        this.subscription.name = this.applicationPageEvent.event.subscribe((event) => {
            if (event.operation == MicroApplicationOperation.LoadAllDashboardSchema)
                this.loadAllDashboard(event.data);
        });
    }

    loadAllDashboard(dataSources:DashboardSchema[]){
        this.dashboardSchemaList.length =0;
        this.dashboardSchemaList = dataSources;

    }


    unsubscribe() {
        this.subscription.name.unsubscribe();
    }
}