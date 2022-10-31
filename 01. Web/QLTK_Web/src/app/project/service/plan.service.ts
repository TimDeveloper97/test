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
export class PlanService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/SearchPlans', model, httpOptions);
    return tr;
  }
  
  getPlanInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetPlanInfo', model, httpOptions);
    return tr
  }
 
  updateProgress(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/progress/update', model, httpOptions);
    return tr
  }

  getPlanView(id, type): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetPlanView?id=' + id + '&type=' + type, httpOptions);
    return tr
  }

  exportExel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/ExportPlan', model, httpOptions);
    return tr
  }

  searchWorkingTime(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/SearchWorkingTime', model, httpOptions);
    return tr
  }

  getInforEmployee(departmentId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetEmployee?departmentId=' + departmentId, httpOptions);
    return tr
  }

  getProjectProductInfo(projectProductId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetProjectProductInfo?projectProductId=' + projectProductId, httpOptions);
    return tr
  }

  addWorkDiary(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/AddWorkDiary', model, httpOptions);
    return tr
  }

  getHolidayInPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetHolidayInPlan', model, httpOptions);
    return tr
  }

  getListPlan(model: any, EmployeeCode): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetListPlan?EmployeeCode=' + EmployeeCode, model, httpOptions);
    return tr
  }


  createWork(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/CreatePlanWork', model, httpOptions);
    return tr
  }

  getEmployeeInfor(id : any,month : any, year :any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetEmployeeInfor?EmployeeId='+id+'&month='+month+'&year='+year, httpOptions);
    return tr
  }

  getListPlanHistory(id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetListPlanHistory?projectId=' + id, httpOptions);
    return tr
  }

  updateWorkTime(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/UpdateWorkTime' , model, httpOptions);
    return tr
  }

  GetWorkEmployeeByDate(id : any,date :any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetWorkEmployeeByDate?EmployeeId='+id+'&date='+date, httpOptions);
    return tr
  }

  updatePlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/UpdatePlan', model, httpOptions);
    return tr;
  }
}
