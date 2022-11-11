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
export class EmployeeDegreeServiceService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }
    Add(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/Adds', model, httpOptions);
      return tr
    }
    SearchModel(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/SearchModel', model, httpOptions);
      return tr
    }
    SearchModels(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/SearchModels', model, httpOptions);
      return tr
    }
    Deletes(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/Deletes', model, httpOptions);
      return tr
    }
    GetInfos(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/GetInfos', model, httpOptions);
      return tr
    }
    Updates(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/Updates', model, httpOptions);
      return tr
    }
    
}
