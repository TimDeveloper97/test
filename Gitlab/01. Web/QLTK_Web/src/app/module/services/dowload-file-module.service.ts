import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class DowloadFileModuleService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  ListFileDowload(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DowloadFileModule/ListFileDowload', model, httpOptions);
    return tr
  }

  GetListModuleDesignDocument(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DowloadFileModule/GetListModuleDesignDocument', httpOptions);
    return tr
  }

  GetListModuleMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DowloadFileModule/GetListModuleMaterial',model, httpOptions);
    return tr
  }
  GetListModuleMaterials(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DowloadFileModule/GetListModuleMaterials',model , httpOptions);
    return tr
  }
  GetListFileByModuleId(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DowloadFileModule/GetListFileByModuleId', model, httpOptions);
    return tr
  }

}
