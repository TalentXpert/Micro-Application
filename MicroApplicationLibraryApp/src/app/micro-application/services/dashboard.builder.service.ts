import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppControlVM } from '../form-builder/form.builder.model';
import { MenuModel } from '../menu/menu.model';
import { Content, DashboardSchema } from '../dashboard-builder/dashboard.builder.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})

export class DashboardBuilderService {

    private baseUrl = "";
    content: Map<string, Content[]>;

    constructor(private httpClient: HttpClient, @Inject('config') private config: any) {
        this.baseUrl = config.configuration + "DashboardBuilder";
        this.content = new Map<string, Content[]>();
    }

    getTopMenus(): Observable<MenuModel[]> {
        return this.httpClient.get<MenuModel[]>(this.baseUrl + '/GetTopMenu');
    }

    getDashboardSchema(): Observable<AppControlVM[]> {
        return this.httpClient.get<AppControlVM[]>(this.baseUrl + '/Gets');
    }

    getContentTypes(): Observable<string[]> {
        return this.httpClient.get<string[]>(this.baseUrl + '/GetContentTypes');
    }

    getContents(contentType: string): Observable<Content[]> {
        let contents = this.content.get(contentType);
        if ( contents && contents.length > 0) {
            var observable = Observable.create((observer: any)=>{
                observer.next(contents);
                observer.complete();
            });
            return observable;
        }
        else {
            return this.httpClient.get<Content[]>(this.baseUrl + '/GetContents/' + contentType).pipe(map((data) => {
                this.content.set(contentType, data);
                return data;
            })
            );
        }

    }
    saveDashboardSchema(dashboardSchema: DashboardSchema): Observable<DashboardSchema> {
        return this.httpClient.post<DashboardSchema>(this.baseUrl, dashboardSchema);
    }
}