import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from '..';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class HistoryVersionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getDataHistoryVersion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HistoryVersion/GetDataHistoryVersion', model, httpOptions);
    return tr
  }

  updateVersion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HistoryVersion/UpdateVersion', model, httpOptions);
    return tr
  }
}
