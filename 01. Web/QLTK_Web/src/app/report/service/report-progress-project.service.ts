import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ReportProgressProjectService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  GetReportProgressProject(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ReportProgressProject/GetReportProcessProject',model, httpOptions);
    return tr
  }
}
