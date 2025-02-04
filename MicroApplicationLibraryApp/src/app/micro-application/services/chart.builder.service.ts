import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppControlVM } from '../form-builder/form.builder.model';
import { Content } from '../dashboard-builder/dashboard.builder.model';
import { ChartColumnSchema, ChartSchema, ChartType, DataSources } from '../chart-builder/chart.builder.model';


@Injectable({
    providedIn: 'root'
})

export class ChartBuilderService {

    private baseUrl = "";
    content: Map<string, Content[]>;

    constructor(private httpClient: HttpClient, @Inject('config') private config: any) {
        this.baseUrl = config.configuration + "ChartBuilder";
        this.content = new Map<string, Content[]>();
    }


    getDashboardSchema(): Observable<DataSources[]> {
        return this.httpClient.get<DataSources[]>(this.baseUrl + '/GetDataSources');
    }

    
    getChartTypes(): Observable<ChartType[]> {
        return this.httpClient.get<ChartType[]>(this.baseUrl + '/GetChartTypes');
    }

    getDataSourceColumns(dataSourecId: string): Observable<ChartColumnSchema[]> {
        return this.httpClient.get<ChartColumnSchema[]>(this.baseUrl + '/GetDataSourceColumns/'+ dataSourecId);
    }

    save(chartSchema: ChartSchema): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl, chartSchema);
    }

    getCharts(): Observable<ChartSchema[]> {
        return this.httpClient.get<ChartSchema[]>(this.baseUrl + '/GetCharts');
    }
    
}