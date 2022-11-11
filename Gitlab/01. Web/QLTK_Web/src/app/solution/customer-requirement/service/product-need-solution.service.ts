import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProductNeedSolutionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  Creates(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/Creates', model, httpOptions);
    return tr
  }
  SearchModel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EmployeeDegrees/SearchModel', model, httpOptions);
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
