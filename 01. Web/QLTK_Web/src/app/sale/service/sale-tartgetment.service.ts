import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class SaleTartgetmentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  addSaleTartgetment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleTartgetments/CreateUpdateSaleTartgetment', model, httpOptions);
    return tr
  }
  updateSaleTartgetment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleTartgetments/CreateUpdateSaleTartgetment', model, httpOptions);
    return tr
  }
  searchSaleTartgetment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleTartgetments/GetAllSaleTartgetments', model, httpOptions);
    return tr
  }

  deleteSaleTartgetment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleTartgetments/DeleteSaleTartgetment', model, httpOptions);
    return tr
  }

  
}
