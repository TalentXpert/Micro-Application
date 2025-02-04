import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppFormControlListVM, AppFormFixedControlAddUpdateVM, AppFormList, AppPageSaveUpdateVM, PageBuilderInfoVM } from '../form-builder/form.builder.model';

@Injectable({
    providedIn: 'root'
})

export class PageBuilderService {

    private baseUrl ="";

    constructor(private httpClient: HttpClient, @Inject('config') private config:any) {
        this.baseUrl = config.configuration +  "PageBuilder";
    }
    
    getPageInfo(): Observable<PageBuilderInfoVM> {
        return this.httpClient.get<PageBuilderInfoVM>(this.baseUrl + '/GetPageInfo');
    }

    getFormFixedControls(formId: string): Observable<AppFormControlListVM[]> {
        return this.httpClient.get<AppFormControlListVM[]>(this.baseUrl + '/GetFormFixedControls/'+ formId);
    }

    addForm(appPageSaveUpdateVM: AppPageSaveUpdateVM): Observable<AppFormList> {
        return this.httpClient.post<AppFormList>(this.baseUrl + '/AddForm', appPageSaveUpdateVM);
    }

    save(appFormControlAddUpdateVM: AppFormFixedControlAddUpdateVM): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/Post', appFormControlAddUpdateVM);
    }
   
}