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
export class RecruitmentRequestService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/search', model, httpOptions);
    return tr
  }

  generateCode(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/GenerateCode', httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/create', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'recruitment-request/get-by-id/'+ id, httpOptions);
    return tr
  }

  getWorkTypeSalaryById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'recruitment-request/get-worktype-salary-by-id/'+ id, httpOptions);
    return tr
  }

  update(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/update/'+id, model, httpOptions);
    return tr
  }

  delete(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/delete/'+ id, httpOptions);
    return tr
  }

  cancelStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/cancel-status/'+id, httpOptions);
    return tr
  }

  nextStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/next-status/'+id, httpOptions);
    return tr
  }

  backStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-request/back-status/'+id, httpOptions);
    return tr
  }
}
