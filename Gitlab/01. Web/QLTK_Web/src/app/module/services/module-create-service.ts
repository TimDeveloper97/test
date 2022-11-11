import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
    providedIn: 'root'
})

export class ModuleCreateService {

    constructor(
        private http: HttpClient,
        private config: Configuration
    ) { }
    createModule(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/AddModule', model, httpOptions);
        return tr
    }

    getById(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleInfo', model, httpOptions);
        return tr
    }

    updateModule(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UpdateModule', model, httpOptions);
        return tr
    }

    getListStageByGroupModuleId(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetListStageByModuleGroupId', model, httpOptions);
        return tr
    }

    searchDocument(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/SearchDocument', model, httpOptions);
        return tr
    }

    getListDocumentFile(documentId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetListDocumentFile?documentId=' + documentId, httpOptions);
        return tr
    }
}