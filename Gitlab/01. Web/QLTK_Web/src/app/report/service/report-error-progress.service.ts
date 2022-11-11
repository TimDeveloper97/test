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
export class ReportErrorProgressService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  report(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorProgress/Report' , model, httpOptions);
    return tr
  }

  reportErrorChangePlan(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorProgress/reportErrorChangePlan' , model, httpOptions);
    return tr
  }

  getWork(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorProgress/GetWork' , model, httpOptions);
    return tr
  }

  getErrorChangePlan(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorProgress/GetWorkChangePlan' , model, httpOptions);
    return tr
  }
  
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorProgress/ExportExcel' , model, httpOptions);
    return tr
  }

}
