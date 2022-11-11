import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class TiviComboboxService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCbbDepartmentUse(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TiviCombobox/GetListDepartmentUse', httpOptions);
    return tr
  }

}
