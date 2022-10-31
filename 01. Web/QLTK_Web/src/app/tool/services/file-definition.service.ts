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

export class FileDefinitionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListFileDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FileDefinition/GetListFileDefinition', model, httpOptions);
    return tr
  }

  createFileDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FileDefinition/AddFileDefinition', model, httpOptions);
    return tr
  }

  updateFileDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FileDefinition/UpdateFileDefinition', model, httpOptions);
    return tr
  }

  deleteFileDefinition(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FileDefinition/DeleteFileDefinition', model, httpOptions);
    return tr
  }

  getFileDefinitionInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FileDefinition/GetFileDefinitionInfo', model, httpOptions);
    return tr
  }

  getFileDefinitions(designType): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'FileDefinition/GetFileDefinitions?designType=' + designType, httpOptions);
    return tr
  }
}
