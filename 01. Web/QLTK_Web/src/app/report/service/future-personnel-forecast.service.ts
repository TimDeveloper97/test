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
export class FuturePersonnelForecastService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  Search(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FuturePersonnelForecast/Search', model, httpOptions);
    return tr
  }

  SearchSelectProject(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FuturePersonnelForecast/SearchSelectProject', model, httpOptions);
    return tr
  }

  SearchPlan(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FuturePersonnelForecast/SearchPlans', model, httpOptions);
    return tr
  }
}
