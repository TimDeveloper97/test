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

export class SubjectsService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSubjects(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/SearchSubjects', model, httpOptions);
    return tr;
  }

  searchClassRoom(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/SearchClassRoom', model, httpOptions);
    return tr;
  }

  searchSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/SearchSkill', model, httpOptions);
    return tr;
  }

  createSubjects(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/AddSubjects', model, httpOptions );
    return tr;
  }
  deleteSubjects(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/DeleteSubjects', model, httpOptions);
    return tr;
  }
  updateSubjects(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/UpdateSubjects', model, httpOptions);
    return tr;
  }
  getSubjects(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/GetSubjects', model, httpOptions);
    return tr
  }
  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Subjects/ExportExcel', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }
}
