import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { GridModel, GridRequestVM, SmartGridConfigurationVM, SmartGrid, UserGridFilter, SmartFormGenerateRequest, UIForm, SmartFormTemplateRequest, UserGridHeadersVM, SmartPage, SmartControlOption, ExcelImportTmplateRequest } from '../application-page-container/application.page.model';
import { PageContentView } from '../view-page-content/view.page.content.model';
import { MenuModel } from '../menu/menu.model';

@Injectable({
    providedIn: 'root'
})

export class ApplicationPageService {

    private baseUrl="";
    
    constructor(@Inject('config') private config:any,private httpClient: HttpClient) {
           this.baseUrl = config.configuration +  "ApplicationPage";
    }

    getSmartPage(pageId: string): Observable<SmartPage> {
        return this.httpClient.get<SmartPage>(this.baseUrl + '/GetSmartPage/' + pageId);
    }

    getSmartGridConfiguration(smartGridConfigurationVM: SmartGridConfigurationVM): Observable<SmartGrid> {
        return this.httpClient.post<SmartGrid>(this.baseUrl + '/SmartGridConfiguration', smartGridConfigurationVM);
    }

    processPageDataRequest(gridRequestVM: GridRequestVM): Observable<GridModel> {
        return this.httpClient.post<GridModel>(this.baseUrl + '/ProcessPageDataRequest', gridRequestVM);
    }

    saveFilter(gridSavedFilter: UserGridFilter): Observable<UserGridFilter> {
        return this.httpClient.post<UserGridFilter>(this.baseUrl + '/SaveFilter', gridSavedFilter);
    }

    deleteFilter(filterId: string): Observable<boolean> {
        return this.httpClient.delete<boolean>(this.baseUrl + '/DeleteFilter/' + filterId);
    }

    processFormGenerateRequest(smartFormGenerateRequest: SmartFormGenerateRequest): Observable<UIForm> {
        return this.httpClient.post<UIForm>(this.baseUrl + '/ProcessFormGenerateRequest', smartFormGenerateRequest);
    }

    processFormSaveRequest(smartFormTemplateRequest: SmartFormTemplateRequest): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/ProcessFormSaveRequest', smartFormTemplateRequest);
    }

    deleteRecord(formId: string, id: string): Observable<boolean> {
        return this.httpClient.delete<boolean>(this.baseUrl + '/Delete/' + formId + '/' + id);
    }

    getSearchOptions(controlId: string, parentId: string, searchTerm: string): Observable<any> {
        return this.httpClient.get<any>(this.baseUrl + '/GetSearchOptions/' + controlId + '/' + parentId + '/' + searchTerm);
    }

    getChildren(controlId: string, parentId: string): Observable<SmartControlOption[]> {
        return this.httpClient.get<SmartControlOption[]>(this.baseUrl + '/GetChildren/' + controlId + '/' + parentId);
    }

    saveUserHeaders(userGridHeadersVM: UserGridHeadersVM): Observable<boolean> {
        return this.httpClient.post<boolean>(this.baseUrl + '/SaveUserHeaders', userGridHeadersVM);
    }

    getUserHeaders(smartGridConfigurationVM: SmartGridConfigurationVM): Observable<UserGridHeadersVM> {
        return this.httpClient.post<UserGridHeadersVM>(this.baseUrl + '/GetUserHeaders', smartGridConfigurationVM);
    }

    export(gridRequestVM: GridRequestVM): Observable<any> {
        return this.httpClient.post(this.baseUrl + "/Export", gridRequestVM, { observe: 'response', responseType: 'blob' });
    }

    getViewPageContents(formId: string, id: string): Observable<PageContentView> {
        return this.httpClient.get<PageContentView>(this.baseUrl + '/GetViewPageContents/' + formId + '/' + id);
    }

    getMenus(): Observable<MenuModel[]> {
        return this.httpClient.get<MenuModel[]>(this.baseUrl + '/GetMenus');
    }

    getImportTemplateRequest(excelImportTmplateRequest: ExcelImportTmplateRequest): Observable<any> {
        return this.httpClient.post(this.baseUrl + "/GetImportTemplateRequest", excelImportTmplateRequest, { observe: 'response', responseType: 'blob' });
    }

    importDataFromExcel(excelImportRequest:FormData):Observable<boolean>{
        return this.httpClient.post<boolean>(this.baseUrl + '/ImportDataFromExcel', excelImportRequest);
      }
}