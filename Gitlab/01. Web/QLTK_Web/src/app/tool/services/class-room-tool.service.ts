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
export class ClassRoomToolService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getPracticeAndSkill(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/GetPracticeAndSkill', model, httpOptions);
    return tr
  }

  getPracticeAndProduct(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/GetPracticeAndProduct', model, httpOptions);
    return tr
  }

  addClassRoomTool(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/AddClassRoomTool', model, httpOptions);
    return tr
  }

  getClassRoomToolInfo(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/GetClassRoomToolInfo', httpOptions);
    return tr
  }

  getAutoPracticeWithSkill(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/GetAutoPracticeWithSkill', model, httpOptions);
    return tr
  }

  getAutoProductWithPractice(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoomTool/GetAutoProductWithPractice', model, httpOptions);
    return tr
  }
}
