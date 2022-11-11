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
export class RawMaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchRawMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/SearchRawMaterial', model, httpOptions);
    return tr
  }

  deleteRawMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/DeleteRawMaterial', model, httpOptions);
    return tr
  }

  createRawMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/AddRawMaterial', model, httpOptions);
    return tr
  }

  getRawMaterialInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/GetRawMaterialInfo', model, httpOptions);
    return tr
  }

  updateRawMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/UpdateMaterial', model, httpOptions);
    return tr
  }

  getCbbRawMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListRawMaterial', httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
    return tr
  }

  getListRawMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'RawMaterial/GetListRawMaterial', httpOptions);
    return tr
  }
}
