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
export class ConfigMaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  getParameterByGroupId(Id:string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialParameter/GetParameterByGroupId?Id=' + Id, httpOptions);
    return tr
  }

  getValueByParameterId(Id:string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialParameterValue/GetValueByParameterId?Id=' + Id, httpOptions);
    return tr
  }

  addFromSource(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigMaterial/AddFromSource', model, httpOptions);
    return tr
  }

  saveConfig(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigMaterial/SaveConfig', model, httpOptions);
    return tr
  }

  getListMaterialGroup(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ConfigMaterial/GetListMaterialGroup', httpOptions);
    return tr
  }

  checkRelationshipValue(Id:string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialParameter/CheckRelationship?Id=' + Id, httpOptions);
    return tr
  }

  checkRelationshipParam(Id:string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MaterialParameterValue/CheckRelationship?Id=' + Id, httpOptions);
    return tr
  }
}
