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
export class FunctionGroupsService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchFunctionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FunctionGroups/SearchFunctionGroups', model, httpOptions);
    return tr
  }

  deleteFunctionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FunctionGroups/DeleteFunctionGroups', model, httpOptions);
    return tr
  }

  createFunctionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FunctionGroups/AddFunctionGroups', model, httpOptions);
    return tr
  }

  getFunctionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FunctionGroups/GetFunctionGroupsInfo', model, httpOptions);
    return tr
  }

  updateFunctionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'FunctionGroups/UpdateFunctionGroups', model, httpOptions);
    return tr
  }
}
