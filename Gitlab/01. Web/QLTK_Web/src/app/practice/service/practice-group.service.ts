import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class PracticeGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchPracticeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/SearchPracticeGroup', model, httpOptions);
    return tr
  }

  searchPracticeGroupById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/SearchPracticeGroupById', model, httpOptions);
    return tr
  }

  deletePracticeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/DeletePracticeGroup', model, httpOptions);
    return tr
  }

  getCbbPracticeGroupForUpdate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/SearchPracticeGroupExpect', model, httpOptions);
    return tr
  }

  createPracticeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/AddPracticeGroup', model, httpOptions);
    return tr
  }

  searchPracticeStandard(model:any): Observable<any> {
   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchPracticeStandard', model, httpOptions);
   return tr
  }

  getPracticeGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/GetPracticeGroupInfo', model, httpOptions);
    return tr
  }

  updatePracticeGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeGroup/UpdatePracticeGroup', model, httpOptions);
    return tr
  }
}