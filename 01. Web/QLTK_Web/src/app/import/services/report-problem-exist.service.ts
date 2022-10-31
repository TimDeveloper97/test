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
export class ReportProblemExistService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchReportProblemExist(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/SearchReportProblemExist', model, httpOptions);
    return tr;
  }

  createReportProblemExist(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/CreateReportProblemExist', model, httpOptions );
    return tr;
  }

  deleteReportProblemExist(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/DeleteReportProblemExist', model, httpOptions);
    return tr;
  }

  updateReportProblemExist(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/UpdateReportProblemExist', model, httpOptions);
    return tr;
  }

  getListImportProfileProblemExist(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProblemExist/GetListImportProfileProblemExist', model, httpOptions);
    return tr
  }

  excel(model:any) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl +'ReportProblemExist/ExcelReportProblemExist', model, httpOptions);
    return tr;
  }
}
