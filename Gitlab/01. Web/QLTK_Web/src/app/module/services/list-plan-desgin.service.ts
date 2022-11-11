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
export class ListPlanDesginService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchListPlanDesgin(moduleId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Module/SearchListPlanDesgin?moduleId='+moduleId, httpOptions);
    return tr
  }

  updateListCheckStatus(module): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UpdateListCheckStatus', module, httpOptions);
    return tr
  }
}
