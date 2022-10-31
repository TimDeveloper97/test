import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../../shared';
import { Routes } from '@angular/router';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class ModuleTabDesignDocumentService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }
  getListFolderModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetListFolderModule', model, httpOptions);
    return tr
  }

  getListFileModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetListFileModule', model, httpOptions);
    return tr
  }

  uploadDesignDocument(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UploadDesignDocument', model, httpOptions);
    return tr
  }

  getChildrenFolderId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetChildrenFolderId', model, httpOptions);
    return tr
  }

  addModuleDesignDocument(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/AddModuleDesignDocument', model, httpOptions);
    return tr
  }

  deleteModuleDesignDocument(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/DeleteModuleDesignDocument', model, httpOptions);
    return tr
  }
}
