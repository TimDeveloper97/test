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
export class ReportErrorAffectService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  report(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorAffect/Report' , model, httpOptions);
    return tr
  }

  getWork(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorAffect/GetWork' , model, httpOptions);
    return tr
  }
  
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportErrorAffect/ExportExcel' , model, httpOptions);
    return tr
  }

}
