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
export class EducationProgramService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchEducationProgram(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/SearchEducationProgram', model, httpOptions);
    return tr;
  }

  searchJob(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/SearchJob', model, httpOptions);
    return tr;
  }

  createEducationProgram(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/AddEducationProgram', model, httpOptions );
    return tr;
  }
  deleteEducationProgram(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/DeleteEducationProgram', model, httpOptions);
    return tr;
  }
  updateEducationProgram(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/UpdateEducationProgram', model, httpOptions);
    return tr;
  }
  getEducationProgram(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/GetEducationProgram', model, httpOptions);
    return tr
  }
  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'EducationProgram/ExportExcel', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }
}
