import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class SolutionAnalysisEstimateService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/SearchSolutionProducts', model, httpOptions);
    return tr;
  }
  createSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/CreateSolutionProduct', model, httpOptions );
    return tr;
  }
  deleteSolutionProduct(solutionProductId): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/DeleteSolutionProduct?solutionProductId='+ solutionProductId, httpOptions);
    return tr;
  }
  updateSolutionProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/UpdateSolutionProduct', model, httpOptions);
    return tr;
  }
  getSolutionProduct(solutionProductId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/GetByIdSolutionProduct?solutionProductId=' + solutionProductId, httpOptions);
    return tr
  }

  getObjectInfo(moduleId): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionAnalysisEstimate/GetObjectInfo', moduleId, httpOptions);
    return tr;
  }
}
