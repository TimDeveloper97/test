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
export class DownloadListModuleService {

  constructor(
    private http: HttpClient,
    private config: Configuration
    
  ) { }

  searchModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadmodule/SearchModule', model, httpOptions);
    return tr
  }

  importExcelListModel(model,file): Observable<any> {
    let formData: FormData = new FormData();
    var ff = JSON.stringify(model);
    formData.append('Model', ff);
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'downloadmodule/ImportExcelListModel', formData);
    return tr
  }
}
