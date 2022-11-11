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
export class ImportProfileReportService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  getReport(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfileReport/Ongoing', model, httpOptions);
    return tr;
  }

  exportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfileReport/OngoingExportExcel', model, httpOptions);
    return tr;
  }

  getReportSummary(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfileReport/Summary', model, httpOptions);
    return tr;
  }

  exportExcelSummary(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfileReport/SummaryExportExcel', model, httpOptions);
    return tr;
  }
}
