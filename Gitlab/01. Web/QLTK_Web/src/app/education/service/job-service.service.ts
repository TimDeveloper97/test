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
export class JobServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  SearchJob(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/SearchJob', model, httpOptions);
  return tr
  }

  ExPort(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/ExportExcel', model, httpOptions);
    return tr
  }

  deleteJob(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/DeleteJob', model, httpOptions);
    return tr
  }

  GetSubject(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/GetSubject', model, httpOptions);
    return tr
  }

  SearchSubject(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/SearchSubject', model, httpOptions);
    return tr
  }

  GetJobInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/GetJobInfo', model, httpOptions);
    return tr
  }

  AddJob(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/AddJob', model, httpOptions);
    return tr
  }

  Create(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/Create', model, httpOptions);
    return tr
  }

  getJobInfor(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/GetJobInfor', model, httpOptions);
    return tr
  }

  update(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/UpdateJob', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  getProductClassInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/GetProductClassInfo', model, httpOptions);
    return tr
  }
  getClassBySubjectId(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/GetClassRoomByIdSubject?Id='+id, httpOptions);
    return tr
  }

  getSubjectInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Job/getSubjectInfo', model, httpOptions);
    return tr
  }
}
