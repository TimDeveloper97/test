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
export class TestDesignStructureService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchDesign(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchList3D', httpOptions);
    return tr
  }

  searchMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchMaterial', httpOptions);
    return tr
  }

  searchManufacture(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchManufacture', httpOptions);
    return tr
  }

  searchUnit(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchUnit', httpOptions);
    return tr
  }

  searchRawMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListRawMaterial', httpOptions);
    return tr
  }

  searchListModuleDesignDocument(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListModuleDesignDocument', httpOptions);
    return tr
  }

  searchListModuleErrorNotDone(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListModuleErrorNotDone', httpOptions);
    return tr
  }

  searchListConvertUnit(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListConvertUnit', httpOptions);
    return tr
  }

  searchListDesignStructure(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListDesignStructure', httpOptions);
    return tr
  }

  searchListDesignStructureFile(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListDesignStructureFile', httpOptions);
    return tr
  }

  searchModule(moduleCode): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchModuleByCode?moduleCode=' + moduleCode, httpOptions);
    return tr
  }

  searchListModule(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchModule', httpOptions);
    return tr
  }

  searchListDesignStrcture(): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WebService/SearchListDesignStrcture', httpOptions);
    return tr
  }
}
