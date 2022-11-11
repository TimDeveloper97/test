import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class WorkPlaceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchWorkPlace(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkPlace/SearchWorkPlace', model, httpOptions);
    return tr;
  }
  createWorkPlace(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkPlace/AddWorkPlace', model, httpOptions );
    return tr;
  }
  deleteWorkPlace(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkPlace/DeleteWorkPlace', model, httpOptions);
    return tr;
  }
  updateWorkPlace(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkPlace/UpdateWorkPlace', model, httpOptions);
    return tr;
  }
  getWorkPlace(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'WorkPlace/GetWorkPlace', model, httpOptions);
    return tr
  }

}
