import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  SearchEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/SearchEmployee', model, httpOptions);
    return tr
  }
  ExPort(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/ExportExcel', model, httpOptions);
    return tr
  }
  DeleteEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/DeleteEmployee', model, httpOptions);
    return tr
  }
  getById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetEmployeeInfo', model, httpOptions);
    return tr
  }

  getByIds(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetEmployeeInfos', model, httpOptions);
    return tr
  }
  //Tab danh sách kỹ năng
  searchEmployeeSkillDetail(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeSkillDetails/SearchEmployeeSkillDetails', model, httpOptions);
    return tr
  }
  updateEmployeeSkillDetail(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeSkillDetails/UpdateEmployeeSkillDetails', model, httpOptions);
    return tr
  }

  importFileEmployee(file): Observable<any> {
    // var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/ImportFile', model, httpOptions);
    // return tr
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/ImportFile', formData);
    return tr
  }

  searchSkillEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillEmployee/GetSkillEmployeeInfos', model, httpOptions);
    return tr
  }

  addSkillEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillEmployee/AddSkillEmployee', model, httpOptions);
    return tr
  }

  ResetPassword(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'User/ResetPassword?Id=' + Id, httpOptions);
    return tr
  }

  ExportTemplate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/ExportExcelTemplate', model, httpOptions);
    return tr
  }

}
