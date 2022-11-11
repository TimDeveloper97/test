import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProjectEmployeeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getProjectEmployeeByProjectId(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetProjectEmployeeByProjectId?projectId='+ projectId, httpOptions);
    return tr
  }

  getProjectExternalEmployeeByProjectId(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetProjectExternalEmployeeByProjectId?projectId='+ projectId, httpOptions);
    return tr
  }

  getProjectImplementationByProjectId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetProjectImplementationByProjectId', model, httpOptions);
    return tr
  }

  getSubsidyHistory(projectEmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetSubsidyHistory?projectEmployeeId='+ projectEmployeeId, httpOptions);
    return tr
  }

  updateSubsidyPE(model:any):Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/UpdateSubsidyPE', model, httpOptions);
    return tr
  }

  AddSubsidyHistory(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/AddSubsidyHistory', model, httpOptions);
    return tr
  }

  searchProjectByEmployeeId(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/SearchProjectByEmployeeId?EmployeeId=' + EmployeeId, httpOptions);
    return tr
  }

  getEmployeeName(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetEmployeeName?EmployeeId=' + EmployeeId, httpOptions);
    return tr
  }

  getExternalEmployeeName(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetExternalEmployeeName?EmployeeId=' + EmployeeId, httpOptions);
    return tr
  }

  getImplementEmployeeName(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetImplementEmployeeName?EmployeeId=' + EmployeeId, httpOptions);
    return tr
  }

  getDescriptionRoleById(RoleId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetDescriptionRoleById?RoleId=' + RoleId, httpOptions);
    return tr
  }

  searchProjectByExEmployeeId(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/SearchProjectByExEmployeeId?EmployeeId=' + EmployeeId, httpOptions);
    return tr
  }
  searchProjectEmployeeGroup(EmployeeId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/SearchProjectEmployeeGroup', EmployeeId, httpOptions);
    return tr
  }

  getProjectEmployeeNotByProjectId(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetProjectEmployeeNotByProjectId?projectId='+ projectId, httpOptions);
    return tr
  }

  ///
  getListEmployee(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetListEmployee', model, httpOptions);
    return tr;
  }
  getListExEmployee(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetListExEmployee', model, httpOptions);
    return tr;
  }

  getRole(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/GetRole', httpOptions);
    return tr
  }

  addMoreProjectEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/AddMoreProjectEmployee', model, httpOptions);
    return tr
  }

  CreateProjectEmployeeAndExternalEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/CreateProjectEmployeeAndExternalEmployee', model, httpOptions);
    return tr
  }

  deleteProjectDeleteEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/DeleteEmployee', model, httpOptions);
    return tr
  }

  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/ExportExcel', model, httpOptions);
    return tr
  }

  uploadImage(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model',JSON.stringify(model) );
    formData.append('File' , file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
    return tr
  }
  updateHasContractPlanPermit(model:any):Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectEmployee/updateHasContractPlanPermit', model, httpOptions);
    return tr
  }
} 
