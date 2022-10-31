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
export class RecruitmentChannelService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-channel/search', model, httpOptions);
    return tr
  }
  
  getById(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-channel/get-channel-by-id', model, httpOptions);
    return tr
  }

  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-channel/delete', model, httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-channel/create', model, httpOptions);
    return tr
  }

  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'recruitment-channel/update', model, httpOptions);
    return tr
  }
}
