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
export class CodeRuleService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  saveCodeRule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CodeRule/SaveCodeRule', model, httpOptions);
    return tr
  }

  getCodeRules(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CodeRule/GetCodeRules', httpOptions);
    return tr
  }

  searchCodeRule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CodeRule/SearchCodeRule', model, httpOptions);
    return tr
  }
}
