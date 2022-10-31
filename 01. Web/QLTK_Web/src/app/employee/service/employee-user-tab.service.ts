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
export class EmployeeUserTabService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getUserInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'USER/GetUserInfo', model, httpOptions);
    return tr
  }

  create(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'USER/Create', model, httpOptions);
    return tr;
  }

  UpdateUser(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'USER/UpdateUser', model, httpOptions);
    return tr;
  }

  getGroupPermission(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'USER/GetGroupPermission?Id=' + Id, httpOptions);
    return tr;
  }

}
