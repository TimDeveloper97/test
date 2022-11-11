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
export class DesignStructureService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/SearchDesignStructure', model, httpOptions);
    return tr
  }

  createDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/CreateDesignStructure', model, httpOptions);
    return tr
  }

  getInfoDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/GetInfoDesignStructure', model, httpOptions);
    return tr
  }

  updateDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/UpdateDesignStructure', model, httpOptions);
    return tr
  }

  deleteDesignStructure(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/DeleteDesignStructure', model, httpOptions);
    return tr
  }

  createDesignStructureFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/CreateDesignStructureFile', model, httpOptions);
    return tr
  }

  deleteDesignStructureFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/DeleteDesignStructureFile', model, httpOptions);
    return tr
  }

  updateDesignStructureFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/UpdateDesignStructureFile', model, httpOptions);
    return tr
  }

  getInfoDesignStructureFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/GetInfoDesignStructureFile', model, httpOptions);
    return tr
  }

  getInfoDesignStructureCreate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/GetInfoDesignStructureCreate', model, httpOptions);
    return tr
  }
  
  getFolderParent(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DesignStructure/GetFolderParent', model, httpOptions);
    return tr
  }
}
