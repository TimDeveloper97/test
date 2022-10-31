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
export class ConfigService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchConfig(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Config/SearchConfig', httpOptions);
    return tr
  }

  getConfigInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Config/GetConfigInfo', model, httpOptions);
    return tr
  }

  updateConfig(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Config/UpdateConfig', model, httpOptions);
    return tr
  }
}
