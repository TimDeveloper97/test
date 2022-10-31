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
export class SolutionGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSolutionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionGroup/SearchSolutionGroup', model, httpOptions);
    return tr
  }

  getSolutionGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionGroup/GetSolutionGroupInfo', model, httpOptions);
    return tr
  }

  createSolutionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionGroup/AddSolutionGroup', model, httpOptions);
    return tr
  }

  updateSolutionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionGroup/UpdateSolutionGroup', model, httpOptions);
    return tr
  }

  deleteSolutionGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionGroup/DeleteSolutionGroup', model, httpOptions);
    return tr
  }
}
