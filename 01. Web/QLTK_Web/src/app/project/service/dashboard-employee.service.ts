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
export class DashboardEmployeeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchListEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardEmployee/SearchListEmployee', model, httpOptions);
    return tr
  }
  
  getGeneralDashboardEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardEmployee/GetGeneralDashboardEmployee', model, httpOptions);
    return tr
  }
  GetEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DashboardEmployee/GetEmployee', model, httpOptions);
    return tr
  }
}
