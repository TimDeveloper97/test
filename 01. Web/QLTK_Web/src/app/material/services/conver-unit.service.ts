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
export class ConverUnitService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getListConverUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConverUnit/GetListConverUnit', model, httpOptions);
    return tr
  }

  addConverUnit(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConverUnit/AddConverUnit', model, httpOptions);
    return tr
  }

  getUnitName(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ConverUnit/GetUnitName', model, httpOptions);
    return tr
  }
}
