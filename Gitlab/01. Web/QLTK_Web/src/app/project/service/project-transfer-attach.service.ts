import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProjectTransferAttachService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProjectTransferAttach(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/SearchProjectTransferAttach', model, httpOptions);
    return tr
  }

  addProjectTransferAttach(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/AddProjectTransferAttach', model, httpOptions);
    return tr
  }

  getProjectProductToTranfer(projectId, fileId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/GetProjectProductToTranfer?projectId=' + projectId + '&fileId=' + fileId, httpOptions);
    return tr
  }

  StatusTrangerProduct(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/StatusTrangerProduct?projectId=' + projectId, httpOptions);
    return tr
  }

  getListPlanTransferByProjectId(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/GetListPlanTransferByProjectId?projectId=' + projectId, httpOptions);
    return tr
  }

  updatePlanStatusByProjectId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectTransferAttach/UpdatePlanStatusByProjectId', model, httpOptions);
    return tr
  }
}
