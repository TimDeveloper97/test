import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class WorkSkillService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/SearchWorkSkill', model, httpOptions);
    return tr;
  }

  searchWorkSkillGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/SearchWorkSkillGroup', model, httpOptions);
    return tr;
  }

  createWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/AddWorkSkill', model, httpOptions);
    return tr;
  }

  updateWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/UpdateWorkSkill', model, httpOptions);
    return tr;
  }

  getInforWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/GetWorkSkillInfo', model, httpOptions);
    return tr;
  }

  deleteWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/DeleteWorkSkill', model, httpOptions);
    return tr;
  }

  getInforWorkSkillGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/GetWorkSkillGroupInfo', model, httpOptions);
    return tr;
  }

  updateWorkSkillGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/UpdateWorkSkillGroup', model, httpOptions);
    return tr;
  }
  
  deleteWorkSkillGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/DeleteWorkSkillGroup', model, httpOptions);
    return tr;
  }

  createWorkSkillGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/AddWorkSkillGroup', model, httpOptions);
    return tr;
  }

  searcSelecthWorkSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkSkill/SearchSelectWorkSkill', model, httpOptions);
    return tr;
  }
}
