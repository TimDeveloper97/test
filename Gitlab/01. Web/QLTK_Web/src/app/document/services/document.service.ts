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
export class DocumentService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

  manageSearch(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search', model, httpOptions);
    return tr
  }

  documentSearch(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-search/search', model, httpOptions);
    return tr
  }

  getInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/getDocumentInfo', model, httpOptions);
    return tr
  }
  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/delete', model, httpOptions);
    return tr
  }
  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/create', model, httpOptions);
    return tr
  }
  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/update', model, httpOptions);
    return tr
  }

  searchDepartment(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-department', model, httpOptions);
    return tr
  }

  searchWorkType(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-worktype', model, httpOptions);
    return tr
  }

  searchTask(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-task', model, httpOptions);
    return tr
  }

  updateDocumentFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/update-document-file', model, httpOptions);
    return tr
  }

  searchChooseDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-choose-document', model, httpOptions);
    return tr
  }

  getDocumentFileInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/getDocumentFile', model, httpOptions);
    return tr
  }

  searchPromulgate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-document-promulgate', model, httpOptions);
    return tr
  }

  deletePromulgate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/delete-promulgate', model, httpOptions);
    return tr
  }
  createPromulgate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/create-promulgate', model, httpOptions);
    return tr
  }
  updatePromulgate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/update-promulgate', model, httpOptions);
    return tr
  }

  getPromulgateInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/getDocumentPromulgateInfo', model, httpOptions);
    return tr
  }

  searchProblem(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-problem/search', model, httpOptions);
    return tr
  }
  getProblemInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-problem/getProblemInfo', model, httpOptions);
    return tr
  }
  deleteProblem(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-problem/delete', model, httpOptions);
    return tr
  }
  createProblem(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-problem/create', model, httpOptions);
    return tr
  }
  updateProblem(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document-problem/update', model, httpOptions);
    return tr
  }

  searchDocumentFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/search-document-file', model, httpOptions);
    return tr
  }

  cancelPromulgate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/cancel-promulgate', model, httpOptions);
    return tr
  }

  reviewDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/review-document', model, httpOptions);
    return tr
  }

  getDocumentTags(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'document/get-documenttags', httpOptions);
    return tr
  }
}
