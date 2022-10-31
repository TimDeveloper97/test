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

export class ConfigScanFileService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchConfigScanFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigScanFile/SearchConfigScanFile', model, httpOptions);
    return tr
  }

  createConfigScanFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigScanFile/AddConfigScanFile', model, httpOptions);
    return tr
  }

  updateConfigScanFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigScanFile/UpdateConfigScanFile', model, httpOptions);
    return tr
  }

  deleteConfigScanFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigScanFile/DeleteConfigScanFile', model, httpOptions);
    return tr
  }

  getConfigScanFileInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConfigScanFile/GetConfigScanFileInfo', model, httpOptions);
    return tr
  }

}
