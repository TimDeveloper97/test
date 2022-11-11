import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/search', model, httpOptions);
    return tr
  }
  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/getTaskInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/update', model, httpOptions);
    return tr
  }

  getCourses(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/get-courses', model, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'task-flow-stage/ExportExcel', model, httpOptions);
    return tr
  }
}
