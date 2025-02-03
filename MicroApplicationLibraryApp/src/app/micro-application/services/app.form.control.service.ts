import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppControlVM, AppFormList } from '../form-builder/form.builder.model';
import { UIControl } from '../application-page-container/application.page.model';
import { AppFormControlAddUpdateVM, AppFormControlListVM, AppFormControlRequestVM, AppFormResetRequestVM } from '../form-configuration/form.configuration.model';

@Injectable({
    providedIn: 'root'
})

export class AppFormControlService {

    private baseUrl="";

    constructor(private httpClient: HttpClient, @Inject('config') private config:any) {
        this.baseUrl = config.configuration +  "AppFormControl";
    }

    getAppControls(): Observable<AppControlVM[]> {
        return this.httpClient.get<AppControlVM[]>(this.baseUrl + '/GetControls');
    }

    getFormControls(appFormControlRequestVM: AppFormControlRequestVM): Observable<AppFormControlListVM[]> {
        return this.httpClient.post<AppFormControlListVM[]>(this.baseUrl + '/GetFormControls', appFormControlRequestVM);
    }

    getAllForms(): Observable<AppFormList[]> {
        return this.httpClient.get<AppFormList[]>(this.baseUrl + '/GetAllForms');
    }
    
    getAllCustomizableForms(): Observable<AppFormList[]> {
        return this.httpClient.get<AppFormList[]>(this.baseUrl + '/GetAllCustomizableForms');
    }

    saveAppFormControls(appFormControlAddUpdateVM: AppFormControlAddUpdateVM): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/Post', appFormControlAddUpdateVM);
    }

    restForm(appFormResetRequestVM: AppFormResetRequestVM): Observable<boolean> {
        return this.httpClient.put<boolean>(this.baseUrl + '/RestForm', appFormResetRequestVM);
    }

    getGlobalControls(formId: string): Observable<UIControl[]> {
        return this.httpClient.get<UIControl[]>(this.baseUrl + '/GetGlobalControls/' + formId);
    }
}