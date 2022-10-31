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
export class ModuleGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchModuleGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchGroupModules', model, httpOptions);
    return tr
  }

  searchModuleGroupById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchGroupModulesById', model, httpOptions);
    return tr
  }

  deleteModuleGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/DeleteGroupModule', model, httpOptions);
    return tr
  }

  getCbbModuleGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetGroupModuleParentChild', httpOptions);
    return tr
  }

  getCbbModuleGroupForUpdate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchGroupModulesExpect', model, httpOptions);
    return tr
  }

  createModuleGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/AddGroupModule', model, httpOptions);
    return tr
  }

  searchProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchProductStandard', model, httpOptions);
    return tr
  }

  SearchStage(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchStage', model, httpOptions);
    return tr
  }

  getModuleGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/GetGroupModuleInfo', model, httpOptions);
    return tr
  }

  updateMmoduleGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/UpdateGroupModule', model, httpOptions);
    return tr
  }

  deleteProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/DeleteProductStandard', model, httpOptions);
    return tr
  }

}
