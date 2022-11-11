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
export class ImportConfigService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  getConfigByType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DocumentConfig/GetDocumentConfigByType', model, httpOptions);
    return tr;
  }

  updateConfig(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'DocumentConfig/UpdateDocumentConfig', model, httpOptions);
    return tr;
  }

}
