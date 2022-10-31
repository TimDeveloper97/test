import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class UnitService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/SearchUnit', model, httpOptions);
    return tr
  }

  deleteUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/DeleteUnit', model, httpOptions);
    return tr
  }

  createUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/AddUnit', model, httpOptions);
    return tr
  }

  getUnitInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/GetUnitInfo', model, httpOptions);
    return tr
  }

  updateUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/UpdateUnit', model, httpOptions);
    return tr
  }

  getCbbUnit(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListUnit', httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
    return tr
  }

  getListUnit(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Unit/GetListUnit', httpOptions);
    return tr
  }
}
