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
export class ManufactureGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchManufactureGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ManufactureGroup/SearchManufactureGroup', model, httpOptions);
    return tr
  }

  createManufactureGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ManufactureGroup/AddManufactureGroup', model, httpOptions);
    return tr
  }

  updateManufactureGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ManufactureGroup/UpdateManufactureGroup', model, httpOptions);
    return tr
  }


  deleteManufactureGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ManufactureGroup/DeleteManufactureGroup', model, httpOptions);
    return tr
  }

  getManufactureGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ManufactureGroup/GetManufactureGroupInfo', model, httpOptions);
    return tr
  }
}
