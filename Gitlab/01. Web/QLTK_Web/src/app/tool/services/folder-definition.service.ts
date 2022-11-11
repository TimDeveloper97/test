import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class FolderDefinitionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListFolderDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FolderDefinition/GetListFolderDefinition', model, httpOptions);
    return tr
  }

  createFolderDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FolderDefinition/AddFolderDefinition', model, httpOptions);
    return tr
  }

  updateFolderDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FolderDefinition/UpdateFolderDefinition', model, httpOptions);
    return tr
  }

  deleteFolderDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FolderDefinition/DeleteFolderDefinition', model, httpOptions);
    return tr
  }

  getFolderDefinitionInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FolderDefinition/GetFolderDefinitionInfo', model, httpOptions);
    return tr
  }

  getFolderDefinitions(designType): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'FolderDefinition/GetFolderDefinitions?designType=' + designType, httpOptions);
    return tr
  }
}
