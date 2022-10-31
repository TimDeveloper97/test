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
export class SalaryTypeService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'salarytype/search', model, httpOptions);
    return tr
  }
  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'salarytype/getSalaryTypeInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'salarytype/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'salarytype/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'salarytype/update', model, httpOptions);
    return tr
  }
}
