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
export class CurrentCostWarningService {

  constructor(   
     private http: HttpClient,
    private config: Configuration
  ) { }

  getDataCurrentCostWarning(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CurrentCostWarning/getDataCurrentCostWarning', model, httpOptions);
    return tr
  }

  excelCurrentCostWarning(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CurrentCostWarning/ExcelCurrentCostWarning', model, httpOptions);
    return tr
  }
}
