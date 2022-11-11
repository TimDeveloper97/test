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
export class ProjectRoleService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCbbRole(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectRole/GetCbbRole', httpOptions);
    return tr
  }

  searchRole(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectRole/SearchRoles', model, httpOptions);
    return tr
  }

  searchRoleType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/searchRoleType', model, httpOptions);
  return tr
  }

  SearchPlanFunction(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ProjectRole/SearchPlanFunction', httpOptions);
    return tr
  }

  getRoleById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ProjectRole/GetRoleById/'+ id, httpOptions);
    return tr
  }

  searchRoleById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ProjectRole/SearchRoleById/'+ id, httpOptions);
    return tr
  }

  createRole(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectRole/CreateRole', model, httpOptions);
    return tr
  }

  updateRole(model:any):Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'ProjectRole/UpdateRole',model,httpOptions);
    return tr
  }

  deleteRole(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectRole/DeleteRole', model, httpOptions);
    return tr
  }

  ExportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectRole/ExportExcel', model, httpOptions);
    return tr
  }
}
