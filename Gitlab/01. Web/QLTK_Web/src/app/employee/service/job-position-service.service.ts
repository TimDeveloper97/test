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
export class JobPositionServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  SearchJobPostitons(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobPosition/SearchJobPostitons', model, httpOptions);
    return tr
  }
  GetJobPositions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobPosition/GetJobPositions', model, httpOptions);
    return tr
  }
  DeleteJobPositions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobPosition/DeleteJobPositions', model, httpOptions);
    return tr
  }
  AddJobPositions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobPosition/AddJobPositions', model, httpOptions);
    return tr
  }
  UpdateJobPositions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'JobPosition/UpdateJobPositions', model, httpOptions);
    return tr
  }
  
}
