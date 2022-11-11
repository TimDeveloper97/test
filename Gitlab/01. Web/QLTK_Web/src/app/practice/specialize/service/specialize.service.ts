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


export class SpecializeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }



  searchSpecialize(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Specialize/SearchSpecialize', model, httpOptions);
    return tr;
  }
  createSpecialize(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Specialize/AddSpecialize', model, httpOptions );
    return tr;
  }
  deleteSpecialize(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Specialize/DeleteSpecialize', model, httpOptions);
    return tr;
  }
  updateSpecialize(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Specialize/UpdateSpecialize', model, httpOptions);
    return tr;
  }
  getSpecialize(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Specialize/GetSpecialize', model, httpOptions);
    return tr
  }
}
