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
export class GroupUserService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchGroupUser(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupUser/SearchGroupUser', model, httpOptions);
    return tr;
  }

  createGroupUser(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupUser/CreateGroupUser', model, httpOptions);
    return tr;
  }

  updateGroupUser(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupUser/UpdateGroupUser', model, httpOptions);
    return tr;
  }

  deleteGroupUser(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupUser/DeleteGroupUser', model, httpOptions);
    return tr;
  }

  getGroupUserInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupUser/GetGroupUserInfo', model, httpOptions);
    return tr;
  }
}
