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
export class ReportStatusProductService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusProduct/GetProduct' , model, httpOptions);
    return tr
  }

  
  excel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusProduct/ExportExcel' , model, httpOptions);
    return tr
  }

  exportExcelProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportStatusProduct/ExportExcelProduct' , model, httpOptions);
    return tr
  }
}
