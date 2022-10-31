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
export class CostWarningService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCostWarning(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CostWarning/GetCostWarning', model, httpOptions);
    return tr
  }

  searchCost(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CostWarning/SearchCost', model, httpOptions);
    return tr
  }

  createCost(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CostWarning/AddCost', model, httpOptions);
    return tr
  }
}
