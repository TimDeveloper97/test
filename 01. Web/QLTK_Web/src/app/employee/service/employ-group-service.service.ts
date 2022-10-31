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
export class EmployGroupServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchEmployeeGroups(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeGroup/GetListEmployeeGroup', model, httpOptions);
    return tr
  }

  deleteEmployeeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeGroup/DeleteEmployeeGroup', model, httpOptions);
    return tr
  }

  GetEmployeeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeGroup/GetEmployeeGroup', model, httpOptions);
    return tr
  }

  AddEmployeeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeGroup/AddEmployeeGroup', model, httpOptions);
    return tr
  }

  UpdateEmployeeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeGroup/UpdateEmployeeGroup', model, httpOptions);
    return tr
  }
}
