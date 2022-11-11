import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class TaskModuleGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchTaskModuleGroups(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TaskModuleGroups/SearchTaskModuleGroups', model, httpOptions);
    return tr;
  }
  createTaskModuleGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TaskModuleGroups/CreateTaskModuleGroup', model, httpOptions );
    return tr;
  }
  deleteTaskModuleGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TaskModuleGroups/DeleteTaskModuleGroup', model, httpOptions);
    return tr;
  }
  updateTaskModuleGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TaskModuleGroups/UpdateTaskModuleGroup', model, httpOptions);
    return tr;
  }
  getTaskModuleGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TaskModuleGroups/GetTaskModuleGroupInfo', model, httpOptions);
    return tr
  }
}
