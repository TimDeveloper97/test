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
export class DashbroadProjectService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListProject(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardProject/GetListProject', model, httpOptions);
    return tr
  }

  viewDetailDesign(projectId, value): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardProject/ViewDetailDesign?projectId=' + projectId + '&value=' + value, httpOptions);
    return tr
  }

  viewDetailDocument(projectId, value): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardProject/ViewDetailDocument?projectId=' + projectId + '&value=' + value, httpOptions);
    return tr
  }

  viewDetailTransfer(projectId, value): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardProject/ViewDetailTranfer?projectId=' + projectId + '&value=' + value, httpOptions);
    return tr
  }
}
