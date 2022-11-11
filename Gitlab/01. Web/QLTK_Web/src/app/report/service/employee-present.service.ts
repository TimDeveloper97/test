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
export class EmployeePresentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }


  getEmployeePresent(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employees/GetReportEmployees', model, httpOptions);
    return tr
  }

  GetCbbProjectBySBUId_DepartmentId(SBUId, DepartmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employees/GetCbbProjectBySBUId_DepartmentId?SBUId=' + SBUId + '&DepartmentId=' + DepartmentId, httpOptions);
    return tr
  }

  GetGroupProducts(ProjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employees/GetGroupProducts?ProjectId=' + ProjectId, httpOptions);
    return tr
  }

  coefficientEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employees/CoefficientEmployee', model, httpOptions);
    return tr
  }

  updateCoefficientEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employees/UpdateCoefficientEmployee', model, httpOptions);
    return tr
  }
}
