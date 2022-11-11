import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared/config/configuration';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class DegreeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchDegree(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Degree/SearchDegree', model, httpOptions);
    return tr;
  }
  createDegree(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Degree/AddDegree', model, httpOptions );
    return tr;
  }
  deleteDegree(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Degree/DeleteDegree', model, httpOptions);
    return tr;
  }
  updateDegree(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Degree/UpdateDegree', model, httpOptions);
    return tr;
  }
  getDegree(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Degree/GetDegree', model, httpOptions);
    return tr
  }
}
