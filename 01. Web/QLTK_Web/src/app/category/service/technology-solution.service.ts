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
export class TechnologySolutionService {

  constructor(
    private http: HttpClient,
    private config: Configuration,)
  { }

  searchTech(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/SearchTech', model, httpOptions);
    return tr
  }

  createIndex(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/CreateTechIndex', model, httpOptions);
    return tr
  }

  deleteTech(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/DeleteTech', model, httpOptions);
    return tr
  }

  getIndex(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/GetTechIndex', httpOptions);
    return tr
  }

  createTech(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/CreateTech', model, httpOptions);
    return tr
  }

  updateTech(model:any):Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'Categories/UpdateTech',model,httpOptions);
    return tr
  }

  
  getInfoById(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/GetTechInfo', model, httpOptions);
    return tr
  }
}
