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
export class ProjectServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchProject(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/SearchProject', model, httpOptions);
  return tr
  }

  excel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/Excel', model, httpOptions);
  return tr
  }

  Delete(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/Delete', model, httpOptions);
  return tr
  }
  AddProject(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/AddProject', model, httpOptions);
  return tr
  }
  GetProjectInfo(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/GetProjectInfo', model, httpOptions);
  return tr
  }

  GetCustomerTypeInfo(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/GetCustomerTypeInfo', model, httpOptions);
  return tr
  }

  UpdateProject(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/UpdateProject', model, httpOptions);
  return tr
  }

  Report(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/Report', model, httpOptions);
  return tr
  }

  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/ExportExcelReport', model, httpOptions);
    return tr
  }

  getMinYear():Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/GetMinYear', httpOptions);
    return tr
  }

  getListReportProject(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/GetListReportProject' , model, httpOptions);
    return tr
  }

  UpdateBedDebt(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/UpdateBedDebt', model, httpOptions);
  return tr
  }

  UpdateBadDebtDate(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/UpdateBadDebtDate', model, httpOptions);
  return tr
  }
  
  searchProjectNeedPublications (id:any): Observable<any>{
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ProductNeedPublication/SearchProject/'+ id , httpOptions);
  return tr
  }

  searchProductNeedPublications (model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductNeedPublication/SearchProduct', model, httpOptions);
  return tr
  }

  getTotalBadDebt(year:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/GetTotalBadDebt?year='+ year , httpOptions);
  return tr
  }
}
