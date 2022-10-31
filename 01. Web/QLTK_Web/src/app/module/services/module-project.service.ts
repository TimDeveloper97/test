import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ModuleProjectService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  SearchProjectModel(searchModule) : Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/SearchProjectModel', searchModule, httpOptions);
    return tr
  }
  SearchTestCriteria(searchModelTest) : Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/SearchTestCriteria', searchModelTest, httpOptions);
    return tr
  }
  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Project/ExportExcel', model, httpOptions);
    return tr
  }
}
