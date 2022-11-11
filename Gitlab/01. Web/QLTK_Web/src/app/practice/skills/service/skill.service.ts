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

export class SkillService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/SearchSkills', model, httpOptions);
    return tr
  }

  searchPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/SearchPractice', model, httpOptions);
    return tr
  }

  deleteSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/DeleteSkills', model, httpOptions);
    return tr
  }

  deletePracticeSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/DeletePracticeSkill', model, httpOptions);
    return tr
  }

  createSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/AddSkills', model, httpOptions);
    return tr
  }

  addPracticeSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/AddPracticeSkill', model, httpOptions);
    return tr
  }
  getSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/GetSkillsInfo', model, httpOptions);
    return tr
  }

  getPracticeSkillInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/GetPracticeSkillInfo', model, httpOptions);
    return tr
  }

  GetPracticeSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/GetPracticeSkill', model, httpOptions);
    return tr
  }

  updateSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/UpdateSkills', model, httpOptions);
    return tr
  }
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Skills/ExportExcel', model, httpOptions);
    return tr
  }
  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }
}
