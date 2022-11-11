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
export class WorkTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTypes/SearchWorkTypes', model, httpOptions);
    return tr;
  }
  createWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTypes/CreateWorkType', model, httpOptions );
    return tr;
  }
  deleteWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTypes/DeleteWorkType', model, httpOptions);
    return tr;
  }
  updateWorkType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTypes/UpdateWorkType', model, httpOptions);
    return tr;
  }
  getWorkTypeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkTypes/GetByIdWorkType', model, httpOptions);
    return tr
  }
  
}
