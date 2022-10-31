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
export class SaleProductTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProductType/SearchType', model, httpOptions);
    return tr
  }

  deleteType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProductType/DeleteType', model, httpOptions);
    return tr
  }

  createType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProductType/AddType', model, httpOptions);
    return tr
  }

  getTypeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProductType/GetTypeInfo', model, httpOptions);
    return tr
  }

  updateType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProductType/UpdateType', model, httpOptions);
    return tr
  }

  getCbbType(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetCBBSaleProductType', httpOptions);
    return tr
  }
}
