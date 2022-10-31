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
export class ReasonChangeIncomeService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'reason-change-income/search', model, httpOptions);
    return tr
  }
  getReasonById(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'reason-change-income/getReasonChangeIncomeInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'reason-change-income/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'reason-change-income/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'reason-change-income/update', model, httpOptions);
    return tr
  }
}
