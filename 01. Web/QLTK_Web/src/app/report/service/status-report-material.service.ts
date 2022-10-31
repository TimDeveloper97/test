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
export class StatusReportMaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getStatusReportMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'StatusReportMaterial/GetStatusReportMaterial', model, httpOptions);
    return tr
  }
  reportModuleMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'StatusReportMaterial/ReportModuleMaterial',model, httpOptions);
    return tr
  }
  moduleMaterialCheck3D(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'StatusReportMaterial/ModuleMaterialCheck3D', httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'StatusReportMaterial/ExportExcel',model, httpOptions);
    return tr
  }
}
