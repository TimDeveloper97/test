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
export class SolutionTabProductService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/SearchSolutionProducts', model, httpOptions);
    return tr;
  }
  createSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/CreateSolutionProduct', model, httpOptions );
    return tr;
  }
  deleteSolutionProduct(solutionProductId): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/DeleteSolutionProduct?solutionProductId='+ solutionProductId, httpOptions);
    return tr;
  }
  updateSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/UpdateSolutionProduct', model, httpOptions);
    return tr;
  }
  getSolutionProduct(solutionProductId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/GetByIdSolutionProduct?solutionProductId=' + solutionProductId, httpOptions);
    return tr
  }

  getObjectInfo(moduleId): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionProduct/GetObjectInfo', moduleId, httpOptions);
    return tr;
  }
}
