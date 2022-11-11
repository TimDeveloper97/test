import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../config/configuration';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class DownloadService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  downloadFileBlob(path, fileName): Observable<any> {
    let apiPath = this.config.ServerFileApiUrl + 'download/download-file?pathFile=' + path + '&fileName=' + fileName+'&keyAuthorize=nhantinsoft123456!!';
    var tr = this.http.get(apiPath, {
      responseType: "blob"
    });
    return tr
  }

  downloadFileBlobNew(path, fileName): Observable<any> {
    let apiPath = this.config.ServerFileApiUrl + 'download/download-file-new?pathFile=' + path +'&keyAuthorize=nhantinsoft123456!!';
    var tr = this.http.get(apiPath, {
      responseType: "blob"
    });
    return tr
  }

  downloadAll(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/DownAllDocumentProcess', model, httpOptions);
    return tr
  }
}
