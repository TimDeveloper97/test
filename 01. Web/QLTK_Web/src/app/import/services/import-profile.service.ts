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
export class ImportProfileService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchImportProfile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/Search', model, httpOptions);
    return tr;
  }

  searchImportProfileFinish(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/SearchFinish', model, httpOptions);
    return tr;
  }

  searchImportProfileKanban(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/SearchKanban', model, httpOptions);
    return tr;
  }

  deleteImportProfile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/Delete', model, httpOptions);
    return tr;
  }

  createImportProfile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/Create', model, httpOptions);
    return tr;
  }

  updateImportProfile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/Update', model, httpOptions);
    return tr;
  }

  getById(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/GetById', model, httpOptions);
    return tr;
  }

  getViewById(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/GetViewById', model, httpOptions);
    return tr;
  }

  nextStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/NextStep', model, httpOptions);
    return tr;
  }

  backStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/BackStep', model, httpOptions);
    return tr;
  }

  getListFile(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/GetListFile', model, httpOptions);
    return tr;
  }

  getImportProfileCode(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportProfile/GetImportProfileCode', httpOptions);
    return tr;
  }

  downloadListFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/DownAllDocumentProcess', model, httpOptions);
    return tr
  }

  searchImportProfileProblemExist(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/SearchImportProfileProblemExist', model, httpOptions);
    return tr;
  }

  createImportProblemExist(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/CreateImportProblemExist', model, httpOptions);
    return tr;
  }
}
