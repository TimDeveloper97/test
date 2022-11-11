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
export class OutputResultService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }
  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/search', model, httpOptions);
    return tr
  }
  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/getOutputResultInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/update', model, httpOptions);
    return tr
  }

  searchFlowStage(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/search-flowstage', model, httpOptions);
    return tr
  }

  searchDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'output-result/search-document', model, httpOptions);
    return tr
  }
}
