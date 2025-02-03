import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DashboardChart, SaveDashboardGridVM } from '../dashboard/dashboard.container/dashboard.model';
import { AppControlVM } from '../form-builder/form.builder.model';
import { DashboardSchema } from '../dashboard-builder/dashboard.builder.model';

@Injectable({
    providedIn: 'root'
})

export class DashboardService {

    private baseUrl ="";

    constructor(private httpClient: HttpClient, @Inject('config') private config:any) {
        this.baseUrl = config.configuration +  "Dashboard";
    }
    
    getDashboard(id: string): Observable<DashboardSchema> {
        return this.httpClient.get<DashboardSchema>(this.baseUrl + '/GetDashboard/'+id);
    }

    saveDashboardGrid(saveDashboardGridVM: SaveDashboardGridVM): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/SaveDashboardGrid', saveDashboardGridVM);
    }

    getDashboardChart(panelId: string){
        return this.httpClient.get<DashboardChart>(this.baseUrl + '/GetDashboardChart/'+ panelId);
    }
   
}