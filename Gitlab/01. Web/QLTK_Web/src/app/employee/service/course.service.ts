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


export class CourseService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  searchCourse(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/SearchCourse', model, httpOptions);
    return tr;
  }

  SearchCourseSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/SearchCourseSkill', model, httpOptions);
    return tr;
  }

  createCourse(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/CreateCourse', model, httpOptions);
    return tr;
  }

  updateCourse(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/UpdateCourse', model, httpOptions);
    return tr;
  }

  getInforCourse(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/GetInfoCourse', model, httpOptions);
    return tr;
  }

  deleteCourse(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/DeleteCourse', model, httpOptions);
    return tr;
  }

  getListParentCourse(): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Course/GetParentCourse', httpOptions);
    return tr;
  }  
}
