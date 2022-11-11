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
export class ReportStatusModuleService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusModule/GetModule' , model, httpOptions);
    return tr
  }

  
  excel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusModule/ExportExcel' , model, httpOptions);
    return tr
  }

  exportExcelModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusModule/ExportExcelModule' , model, httpOptions);
    return tr
  }
}
