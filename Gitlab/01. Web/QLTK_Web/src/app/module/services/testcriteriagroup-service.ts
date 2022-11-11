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
export class TestcriteriagroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
    ) { }
    searchTestCriterialGroup(model:any): Observable<any>{
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteriaGroup/SearchRawTestCriteriaGroup', model, httpOptions);
    return tr
    }
    deleteTestCriteralGroup(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteriaGroup/DeleteTestCriteralGroup', model, httpOptions);
      return tr
    }
  
    createTestCriteralGroup(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteriaGroup/AddTestCriteralGroup', model, httpOptions);
      return tr
    }
  
    getTestCriteralGroup(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteriaGroup/GetTestCriteralGroup', model, httpOptions);
      return tr
    }
  
    updateTestCriteralGroup(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteriaGroup/UpdateTestCriteralGroup', model, httpOptions);
      return tr
    }
}
