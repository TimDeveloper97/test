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
export class MaterialGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/SearchMaterialGroup', model, httpOptions);
    return tr
  }

  deleteMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/DeleteMaterialGroup', model, httpOptions);
    return tr
  }

  createMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/AddMaterialGroup', model, httpOptions);
    return tr
  }

  getMaterialGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/GetMaterialGroupInfo', model, httpOptions);
    return tr
  }

  updateMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/UpdateMaterialGroup', model, httpOptions);
    return tr
  }

  getCbbTPA(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListTPA', httpOptions);
    return tr
  }

  getCbbMaterialGroup(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKMG/GetCbbMaterialGroupFullChild?Id=' + Id, httpOptions);
    return tr
  }

  getListMaterialGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroup/GetListMaterialGroup', httpOptions);
    return tr
  }

  getMaterialGroups(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroup/GetMaterialGroups', httpOptions);
    return tr
  }

  // exportExcel(model:any): Observable<any> {
  //   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
  //   return tr
  // }
}
