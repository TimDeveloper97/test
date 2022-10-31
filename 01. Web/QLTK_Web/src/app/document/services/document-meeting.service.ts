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
export class DocumentMeetingService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-meeting/search', model, httpOptions);
    return tr
  }
  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-meeting/getMeetingInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-meeting/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-meeting/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-meeting/update', model, httpOptions);
    return tr
  }
}
