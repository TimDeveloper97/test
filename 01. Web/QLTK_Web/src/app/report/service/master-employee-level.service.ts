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
export class MasterEmployeeLevelService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchEmployeeLevel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterEmployeeLevel/SearchMasterEmployeeLevel', model, httpOptions);
    return tr
  }

  ExportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterEmployeeLevel/ExportExcel', model, httpOptions);
    return tr
  }
}
