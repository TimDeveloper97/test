import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class TestcriteriaService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  SearchTestCriteria(model:any) :Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'TestCriteria/SearchTestCriteria', model, httpOptions);
    return tr
  }
  DeleteTestCriteria(model:any) : Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+ 'TestCriteria/DeleteTestCriteria', model,httpOptions);
    return tr
  }
  GetTestCriteriaInfo(model:any) : Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'TestCriteria/GetTestCriteriaInfo', model,httpOptions);
    return tr
  }
  AddTestCriteria(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'TestCriteria/AddTestCriteria', model, httpOptions);
    return tr
  }
  UpdateTestCriteria(model:any) : Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'TestCriteria/UpdateTestCriteria', model, httpOptions);
    return tr
  }

  // 04-02-2020 * thêm mới xuất excel
  excel(model:any) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl +'TestCriteria/ExcelTestCriteria', model, httpOptions);
    return tr;
  }
  
}
