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
export class ProjectErrorService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchModuleErrors(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectError/SearchModuleErrors', model, httpOptions);
    return tr
  }

  getErrorInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectError/GetErrorInfo', model, httpOptions);
    return tr
  }

  exportExcelError(Id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectError/ExportExcelError/' + Id, httpOptions);
    return tr
  }
}
