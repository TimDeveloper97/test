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
export class EmployeeUpdateService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
    ) { }
    LockEmployee(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/LockEmployee', model, httpOptions);
      return tr
    }
    updateEmployee(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/UpdateEmployee', model, httpOptions);
      return tr
    }
    getByIdupdate(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetEmployeeInfo', model, httpOptions);
      return tr
    }
    
    getListCourse(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetListCourse', model, httpOptions);
      return tr
    }

    getListEmployeeTraining(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetListEmployeeTraining', model, httpOptions);
      return tr
    }

    uploadImage(file: any, model): Observable<any> {
      let formData: FormData = new FormData();
      formData.append('Model',JSON.stringify(model) );
      formData.append('File' , file);
      var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
      return tr
    }

    getListRegulation(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetRegulation', model, httpOptions);
      return tr
    }

    getListProcedure(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetProcedure', model, httpOptions);
      return tr
    }

    getListWork(model:any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Employee/GetWorkList', model, httpOptions);
      return tr
    }
}

