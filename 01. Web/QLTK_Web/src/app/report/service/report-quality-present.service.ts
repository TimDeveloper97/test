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
export class ReportQualityPresentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchEmployees(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportEmployees/GetErrorEmployees', model, httpOptions);
    return tr
  }

  errorWithLineProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportEmployees/ErrorWithLineProduct', model, httpOptions);
    return tr
  }

  errorGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportEmployees/ErrorGroup', model, httpOptions);
    return tr
  }

  errorRatio(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportEmployees/ErrorRatio', model, httpOptions);
    return tr
  }
}
