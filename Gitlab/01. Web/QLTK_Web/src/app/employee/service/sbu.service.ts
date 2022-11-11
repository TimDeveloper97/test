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
export class SbuService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSBU(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/SearchSBU', model, httpOptions);
    return tr
  }

  searchDepartment(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/SearchDepartment', model, httpOptions);
    return tr
  }

  deleteSBU(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/DeleteSBU', model, httpOptions);
    return tr
  }

  createSBU(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/AddSBU', model, httpOptions);
    return tr
  }

  getSBUInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/GetSBUInfo', model, httpOptions);
    return tr
  }

  updateSBU(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SBU/UpdateSBU', model, httpOptions);
    return tr
  }
}
