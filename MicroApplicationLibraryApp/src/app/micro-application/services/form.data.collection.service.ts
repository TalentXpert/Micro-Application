

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { SelectFromListFormInput, SelectFromListFormData } from '../select-from-list-form/select.from.list.form.model';

@Injectable({
    providedIn: 'root'
})

export class FormDataCollectionService {

    private baseUrl ="";

    constructor(private httpClient: HttpClient, @Inject('config') private config:any) {
        this.baseUrl = config.configuration +  "FormDataCollection";
    }
    
    getSelectFromListFormInput(formId: string, formDataEntityId: string): Observable<SelectFromListFormInput> {
        return this.httpClient.get<SelectFromListFormInput>(this.baseUrl + '/GetSelectFromListFormInput/'+formId +"/"+ formDataEntityId);
    }

    saveSelectFromListForm(selectFromListFormData: SelectFromListFormData): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/SaveSelectFromListForm', selectFromListFormData);
    }
   
}