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
export class ApplicationService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchApplication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Application/SearchApplication', model, httpOptions);
    return tr;
  }

  createApplication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Application/CreateApplication', model, httpOptions);
    return tr;
  }

  updateApplication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Application/UpdateApplication', model, httpOptions);
    return tr;
  }

  GetInforApplication(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Application/GetInforApplication?id=' + id, httpOptions);
    return tr;
  }

  deleteApplication(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Application/DeleteApplication?id=' + id, httpOptions);
    return tr;
  }
}
