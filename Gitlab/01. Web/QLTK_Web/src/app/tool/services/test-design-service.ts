import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class TestDesignService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestDesign/Excel', model, httpOptions);
    return tr
  }

  exportReportDMVT(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestDesign/ExportReportDMVT', model, httpOptions);
    return tr
  }

  exportReportTestDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestDesign/ExportReportTestDesignStruture', model, httpOptions);
    return tr
  }

  exportResultDMVT(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestDesign/ExportResultDMVT', model, httpOptions);
    return tr
  }

}
