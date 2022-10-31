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
export class TiviReportErrorService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  report(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TiviReportError/Report' , model, httpOptions);
    return tr
  }

  getWork(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TiviReportError/GetWork' , model, httpOptions);
    return tr
  }
  
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TiviReportError/ExportExcel' , model, httpOptions);
    return tr
  }

}
