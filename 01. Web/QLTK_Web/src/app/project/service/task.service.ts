import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchTask(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Tasks/SearchTasks', model, httpOptions);
    return tr;
  }
  createTask(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Tasks/CreateTask', model, httpOptions );
    return tr;
  }
  deleteTask(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Tasks/DeleteTask', model, httpOptions);
    return tr;
  }
  updateTask(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Tasks/UpdateTask', model, httpOptions);
    return tr;
  }
  getTaskInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Tasks/GetTaskInfo', model, httpOptions);
    return tr
  }
  
}
