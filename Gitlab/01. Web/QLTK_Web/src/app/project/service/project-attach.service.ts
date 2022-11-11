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
export class ProjectAttachService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getProjectAttach(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/GetProjectAttach', model, httpOptions);
    return tr
  }

  addProjectAttach(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/AddProjectAttach', model, httpOptions);
    return tr
  }
  getGroupInTemplate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/GetGroupInTemplate', model, httpOptions);
    return tr
  }

  exportExcel(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/ExportExcel?projectId=' + projectId, httpOptions);
    return tr
  }

  importFileProjectAttach(file,projectId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/ProjectAttachImportFile?projectId='+ projectId, formData);
    return tr
  }


  searchDocumentFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/search-document-file', model, httpOptions);
    return tr
  }

  getAttachProject(id :any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/GetAttachProject/' + id, httpOptions);
    return tr
  }

  createType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/AddType', model, httpOptions);
    return tr
  }

  getCbbType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProjectAttachTabType', httpOptions);
    return tr
  }

  getTypeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/GetTypeInfo', model, httpOptions);
    return tr
  }

  updateType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/UpdateType', model, httpOptions);
    return tr
  }

  deleteType(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/DeleteType?Id=' + Id, httpOptions);
    return tr
  }

  getProjectDocType(Id : any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetProjectAttachTabType?projectId='+ Id, httpOptions);
    return tr
  }

  CheckNameProjectAttach(model :any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/CheckNameProjectAttach',model , httpOptions);
    return tr
  }

  getProjectAttachInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectAttach/GetProjectAttachInfo', model, httpOptions);
    return tr
  }
}