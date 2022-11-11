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
export class EmployeeTrainingService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchEmployeeTraining(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/SearchEmployeeTraining', model, httpOptions);
    return tr;
  }

  searchCourse(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/SearchCourse', model, httpOptions);
    return tr;
  }

  searchEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/SearchEmployee', model, httpOptions);
    return tr;
  }

  getEmployeeByCourseId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/GetEmployeeByCourseId?courseId=' + model, httpOptions);
    return tr;
  }

  addEmployeeTraining(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/AddEmployeeTraining', model, httpOptions);
    return tr;
  }

  updateEmployeeTraining(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/UpdateEmployeeTraining', model, httpOptions);
    return tr;
  }

  deleteEmployeeTraining(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/DeleteEmployeeTraining', model, httpOptions);
    return tr;
  }

  getEmployeeTrainingInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/GetEmployeeTrainingInfo', model, httpOptions);
    return tr;
  }

  getWorkKillEndEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/GetWorkKillEndEmployee', model, httpOptions);
    return tr;
  }

  updatePointEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/UpdatePointEmployee', model, httpOptions);
    return tr;
  }

  getEmployeeByCourse(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeTraining/GetEmployeeByCourse', model, httpOptions);
    return tr;
  }

}
