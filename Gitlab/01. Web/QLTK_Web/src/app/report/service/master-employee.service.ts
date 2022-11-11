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
export class MasterEmployeeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  SearchEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterEmployee/SearchMasterEmployee', model, httpOptions);
    return tr
  }

  ExportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterEmployee/ExportExcel', model, httpOptions);
    return tr
  }
}
