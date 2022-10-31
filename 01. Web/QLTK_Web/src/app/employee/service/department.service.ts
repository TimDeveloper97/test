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
export class DepartmentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/SearchDepartment', model, httpOptions);
    return tr
  }

  deleteDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/DeleteDepartment', model, httpOptions);
    return tr
  }

  createDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/AddDepartment', model, httpOptions);
    return tr
  }

  getDepartmentInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/GetDepartmentInfo', model, httpOptions);
    return tr
  }

  updateDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/UpdateDepartment', model, httpOptions);
    return tr
  }

  getCbbManager(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListManagers', httpOptions);
    return tr
  }
  getCbbSBU(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListSBU', httpOptions);
    return tr
  }
  SearchWorkByDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Department/SearchWorkByDepartment', model, httpOptions);
    return tr
  }
}
