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
export class DownloadDmvtSapService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  GetModuleInProjectProductByProjectId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadDMVTSAP/GetModuleInProjectProductByProjectId', model, httpOptions);
    return tr
  }

  GenerateMaterialSAP(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadDMVTSAP/GenerateMaterialSAP', model, httpOptions);
    return tr
  }
  DownAllDocumentProcess(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadDMVTSAP/DownAllDocumentProcess', model, httpOptions);
    return tr
  }

  importModule(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadDMVTSAP/ImportModule', formData);
    return tr
  }
}
