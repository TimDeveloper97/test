import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../../shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class NsMaterialGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchNSMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/SearchNSMaterialGroup', model, httpOptions);
    return tr
  }

  createNSMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/CreateNSMaterialGroup', model, httpOptions);
    return tr
  }

  updateNSMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/UpdateNSMaterialGroup', model, httpOptions);
    return tr
  }

  getById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/GetById', model, httpOptions);
    return tr
  }

  deleteNSMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/DeleteNSMaterialGroup', model, httpOptions);
    return tr
  }

  getNSMaterialGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'NSMaterialGroup/GetNSMaterialGroup', model, httpOptions);
    return tr
  }
}
