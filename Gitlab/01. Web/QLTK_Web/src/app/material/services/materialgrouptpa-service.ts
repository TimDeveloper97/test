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
export class MaterialGroupTPAService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMaterialGroupTPA(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroupTPA/SearchRawMaterialGroupTPA', model, httpOptions);
    return tr
  }

  deleteMaterialGroupTPA(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroupTPA/DeleteMaterialGroupTPA', model, httpOptions);
    return tr
  }

  createMaterialGroupTPA(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroupTPA/AddMaterialGroupTPA', model, httpOptions);
    return tr
  }

  getMaterialGroupTPAInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroupTPA/GetMaterialGroupTPAInfo', model, httpOptions);
    return tr
  }

  updateMaterialGroupTPA(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialGroupTPA/UpdateMaterialGroupTPA', model, httpOptions);
    return tr
  }

  // getCbbRawMaterial(): Observable<any> {
  //   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListRawMaterial', httpOptions);
  //   return tr
  // }

  // exportExcel(model:any): Observable<any> {
  //   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
  //   return tr
  // }
}
