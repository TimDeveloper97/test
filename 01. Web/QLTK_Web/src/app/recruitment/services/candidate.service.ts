import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class CandidateService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  searchCandidate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/search', model, httpOptions);
    return tr
  }

  searchCandidatesByRecruitmentRequestId(id: any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'candidate/search-by-recruitment-request-id/'+id, httpOptions);
    return tr
  }

  deleteCandidate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/delete-candidate', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'candidate/get-candidate-by-id/'+ id, httpOptions);
    return tr
  }

  create(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/create-candidate', model, httpOptions);
    return tr
  }

  update(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/update-candidate/'+id, model, httpOptions);
    return tr
  }

  updateFollow(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/update-follow/'+id, model, httpOptions);
    return tr
  }

  getFollow(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'candidate/get-follow/'+id, httpOptions);
    return tr
  }
  
  getApplys(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'candidate/get-applys/'+id, httpOptions);
    return tr
  }

  generateCode(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'candidate/GenerateCode', httpOptions);
    return tr
  }

  cancel(id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'candidate/cancel-candidate/'+ id, httpOptions);
    return tr
  }
}
