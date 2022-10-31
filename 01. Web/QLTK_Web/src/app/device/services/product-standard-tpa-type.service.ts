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
export class ProductStandardTpaTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/SearchType', model, httpOptions);
    return tr
  }

  deleteType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/DeleteType', model, httpOptions);
    return tr
  }

  createType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/AddType', model, httpOptions);
    return tr
  }

  getTypeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/GetTypeInfo', model, httpOptions);
    return tr
  }

  updateType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/UpdateType', model, httpOptions);
    return tr
  }

  getCbbType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBProductStandardTPATypeIndex',model, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
    return tr
  }

  getListType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTpaType/GetListType', httpOptions);
    return tr
  }
}
