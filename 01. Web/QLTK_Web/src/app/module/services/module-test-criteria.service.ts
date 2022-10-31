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
export class ModuleTestCriteriaService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  createTestCriteria(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/AddTestCriteria', model, httpOptions);
    return tr
  }

  searchTestCriteria(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/SearchTestCriteria', model, httpOptions);
    return tr
  }

  searchTestCriteriaInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetTestCriteria', model, httpOptions);
    return tr
  }
  getTestCriteriaInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetTestCriteriaInfo', model, httpOptions);
    return tr
  }

  getModuleGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/GetGroupModuleInfo', model, httpOptions);
    return tr
  }

  getModuleTestCriteriaInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleTestCriteriInfo', model, httpOptions);
    return tr
  }
}
