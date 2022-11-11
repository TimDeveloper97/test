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
export class DataDistributionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchDataDistribution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/SearchDataDistribution', model, httpOptions);
    return tr
  }

  createDataDistribution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/AddDataDistribution', model, httpOptions);
    return tr
  }

  getDataDistributionInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/GetDataDistributionInfo', model, httpOptions);
    return tr
  }

  updateDataDistribution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/UpdateDataDistribution', model, httpOptions);
    return tr
  }

  deleteDataDistribution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/DeleteDataDistribution', model, httpOptions);
    return tr
  }

  searchDataDistributionFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/SearchDataDistributionFile', model, httpOptions);
    return tr
  }

  deleteDataDistributionFileLink(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/DeleteDataDistributionFileByFolder', model, httpOptions);
    return tr
  }

  getDataDistributionFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/GetDataDistributionFile', model, httpOptions);
    return tr
  }

  createDataDistributionFileLink(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/AddDataDistributionFileLink', model, httpOptions);
    return tr
  }

  createDataDistributionFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/AddDataDistributionFile', model, httpOptions);
    return tr
  }

  updateDataDistributionFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/UpdateDataDistributionFile', model, httpOptions);
    return tr
  }

  getDataDistributionFileInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/GetDataDistributionFileInfo', model, httpOptions);
    return tr
  }
  
  deleteDataDistributionFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DataDistribution/DeleteDataDistributionFile', model, httpOptions);
    return tr
  }
}
