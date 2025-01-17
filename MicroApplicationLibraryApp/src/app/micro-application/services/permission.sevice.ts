import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { ConfigService } from '../../services/config.service';



export class PermissionVM {
    public Id: string="";
    public Name: string;
    public Code: string;
    constructor(formValues:any) {
        this.Code = formValues.Code;
        this.Name = formValues.Name;
    }
}

@Injectable({
    providedIn: 'root'
})
export class PermissionService {
    private baseUrl="";
    constructor(private httpClient: HttpClient, private configService: ConfigService) {
        this.configService.dataServiceBaseUrl() + "Permission";
    }

    savePermission(data: PermissionVM): Observable<PermissionVM> {
        return this.httpClient.post<PermissionVM>(this.baseUrl, data);
    }

    getPermissions(): Observable<PermissionVM[]> {
        return this.httpClient.get<PermissionVM[]>(this.baseUrl + '/Gets');
    }

    deletePermission(id: string): Observable<boolean> {
        return this.httpClient.delete<boolean>(this.baseUrl + '/Delete/' + id);

    }

}