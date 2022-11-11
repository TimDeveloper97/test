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
export class EmployeeWorkTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/SearchWorkType', model, httpOptions);
    return tr;
  }

  createWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/AddWorkType', model, httpOptions);
    return tr;
  }

  updateWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/UpdateWorkType', model, httpOptions);
    return tr;
  }

  getInforWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/GetWorkTypeInfo', model, httpOptions);
    return tr;
  }

  deleteWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/DeleteWorkType', model, httpOptions);
    return tr;
  }

  searchWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/SearchWorkSkill', model, httpOptions);
    return tr;
  }

  searchEmployeeWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkType/SearchEmployeeWorkSkill', model, httpOptions);
    return tr;
  }
}
