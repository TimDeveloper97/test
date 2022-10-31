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
export class JobGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchJobGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobGroup/SearchJobGroups', model, httpOptions);
  return tr
  }

  DeleteJobGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobGroup/DeleteJobGroup', model, httpOptions);
  return tr
  }

  GetJobGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobGroup/GetJobGroup', model, httpOptions);
  return tr
  }

  AddJobGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobGroup/AddJobGroup', model, httpOptions);
  return tr
  }

  UpdateJobGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobGroup/UpdateJobGroup', model, httpOptions);
  return tr
  }
}
