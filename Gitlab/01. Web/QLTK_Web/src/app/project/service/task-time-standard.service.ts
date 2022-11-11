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
export class TaskTimeStandardService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchTaskTimeStandard(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/SearchTasksTimeStandard', model, httpOptions);
    return tr;
  }
  createTaskTimeStandard(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/CreateTaskTimeStandard', model, httpOptions );
    return tr;
  }
  deleteTaskTimeStandard(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/DeleteTaskTimeStandard', model, httpOptions);
    return tr;
  }
  updateTaskTimeStandard(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/UpdateTaskTimeStandard', model, httpOptions);
    return tr;
  }
  getTaskTimeStandardInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/GetTaskTimeStandardInfo', model, httpOptions);
    return tr
  }
  createListTaskTim(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/CreateListTaskTim', model, httpOptions);
    return tr
  }

  createModuleGroupTimeStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/CreateModuleGroupTimeStandard', model, httpOptions);
    return tr
  }

  updateModuleGroupTimeStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/UpdateModuleGroupTimeStandard', model, httpOptions);
    return tr
  }

  calculateAverageTaskTimeStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/CalculateAverageTaskTimeStandard', model, httpOptions);
    return tr
  }

  importTaskTimeStandard(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/ImportTaskTimeStandard', formData);
    return tr
  }

  importExcelTaskTimeStandard(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TasksTimeStandard/ImportExcelTaskTimeStandard', formData);
    return tr
  }
}
