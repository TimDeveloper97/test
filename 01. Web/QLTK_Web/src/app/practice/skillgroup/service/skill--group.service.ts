import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class SkillGroupService {

  constructor(

    private http: HttpClient,
    private config: Configuration
  ) { }
  searchSkillGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/SearchListSkilltGroup', model, httpOptions);
    return tr;
  }
  // searchSkillGroup(model:any): Observable<any>{
  //   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/SearchSkillGroup', model, httpOptions);
  //   return tr;
  // }
  getSkillGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/GetSkillGroupInfo', model, httpOptions);
    return tr
  }

  getCbbSkillGroupForUpdate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/SearchSkillGroupExpect', model, httpOptions);
    return tr
  }

  createSkillGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/AddSkillGroup', model, httpOptions);
    return tr;
  }

  deleteSkillGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/DeleteSkilGroup', model, httpOptions);
    return tr;
  }

  updateSkillGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/UpdateSkillGroup', model, httpOptions);
    return tr;
  }

  getSkillGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SkillGroup/GetSkillGroup', model, httpOptions);
    return tr
  }
}
