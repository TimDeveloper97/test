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
export class WorkDiaryService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/SearchWorkDiary', model, httpOptions);
    return tr;
  }

  addWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/AddWorkDiary', model, httpOptions);
    return tr;
  }

  getByIdWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/GetByIdWorkDiary', model, httpOptions);
    return tr;
  }

  getWorkDiaryView(workDiaryId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/GetWorkDiaryView?id=' + workDiaryId, httpOptions);
    return tr;
  }

  updateWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/UpdateWorkDiary', model, httpOptions);
    return tr;
  }
  deleteWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/DeleteWorkDiary', model, httpOptions);
    return tr;
  }

  ExcelWorkDiary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/Excel', model, httpOptions);
    return tr;
  }

  GetCbbEmployeeByUser(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/GetCbbEmployeeByUser', httpOptions);
    return tr;
  }

  GetCbbprojectByUser(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/GetCbbprojectByUser', httpOptions);
    return tr;
  }

  searchWorkingTime(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkDiary/SearchWorkingTime', model, httpOptions);
    return tr;
  }

}
