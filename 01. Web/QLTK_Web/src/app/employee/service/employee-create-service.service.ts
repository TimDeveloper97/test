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
export class EmployeeCreateServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }
  CreateEmployee(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/CreateEmployee', model, httpOptions);
    return tr
  }
  getById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetEmployeeInfo', model, httpOptions);
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

