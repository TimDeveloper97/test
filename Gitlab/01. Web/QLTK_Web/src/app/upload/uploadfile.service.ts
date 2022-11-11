import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UploadfileService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  uploadListFile(file, filefolder): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('filefolder', filefolder);
    file.forEach(item => {
      formData.append(item.name, item);
    });
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadListFile', formData);
    return tr
  }

  uploadFile(file: any, filefolder): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('filefolder', filefolder);
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadFile', formData);
    return tr
  }

  uploadListFilePDF(file, filefolder): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('filefolder', filefolder);
    file.forEach(item => {
      formData.append(item.name, item);
    });
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/upload-list-file-convert-pdf', formData);
    return tr
  }

  uploadImage(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model',JSON.stringify(model) );
    formData.append('File' , file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
    return tr
  }
}
