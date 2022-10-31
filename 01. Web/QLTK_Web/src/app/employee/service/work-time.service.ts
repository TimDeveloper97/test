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
export class WorkTimeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkTime(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTime/SearchWorkTime', model, httpOptions);
    return tr;
  }

  createWorkTime(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTime/AddWorkTime', model, httpOptions);
    return tr;
  }

  updateWorkTime(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTime/UpdateWorkTime', model, httpOptions);
    return tr;
  }

  getInforWorkTime(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTime/GetWorkTimeInfo', model, httpOptions);
    return tr;
  }

  deleteWorkTime(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTime/DeleteWorkTime', model, httpOptions);
    return tr;
  }
}
