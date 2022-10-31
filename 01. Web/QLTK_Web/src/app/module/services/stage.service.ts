import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class StageService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/SearchStage', model, httpOptions);
    return tr
  }

  searchListStage(projectProductId: string): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Stage/SearchListStage?projectProductId=' + projectProductId, httpOptions);
    return tr
  }

  delete(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/DeleteStage', model, httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/AddStage', model, httpOptions);
    return tr
  }

  createIndex(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/createIndex', model, httpOptions);
    return tr
  }

  getInfoById(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/GetStageInfo', model, httpOptions);
    return tr
  }

  update(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Stage/UpdateStage', model, httpOptions);
    return tr
  }
}
