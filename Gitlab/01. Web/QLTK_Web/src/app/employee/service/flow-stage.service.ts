import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class FlowStageService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/search', model, httpOptions);
    return tr
  }
  
  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/getFlowStageInfo', model, httpOptions);
    return tr
  }

  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/delete', model, httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/create', model, httpOptions);
    return tr
  }

  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/update', model, httpOptions);
    return tr
  }

  searchOutputResult(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'flow-stage/search-outputresult', model, httpOptions);
    return tr
  }
}
