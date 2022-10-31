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
export class SurveyContentService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyContent/search', model, httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyContent/create', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SurveyContent/get-by-id/'+ id, httpOptions);
    return tr
  }

  update(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyContent/update/'+id, model, httpOptions);
    return tr
  }

  delete(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyContent/delete/'+ id, httpOptions);
    return tr
  }

  searchDocumentFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyContent/search-document-file', model, httpOptions);
    return tr
  }

}
