import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProjectSolutionService {

  constructor(private http: HttpClient,
    private config: Configuration
  ) { }

  searchProjectSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectSolution/SearchProjectSolution', model, httpOptions);
    return tr
  }

  searchSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectSolution/SearchSolution', model, httpOptions);
    return tr
  }

  addProjectSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectSolution/AddProjectSolution', model, httpOptions);
    return tr
  }

  getProjectProductByProjectId(projectId, projectSolutionId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectSolution/getProjectProductByProjectId?projectId='+ projectId +'&projectSolutionId=' + projectSolutionId, httpOptions);
    return tr
  }

  StatusSolutionProduct(projectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectSolution/StatusSolutionProduct?projectId=' + projectId, httpOptions);
    return tr
  }
} 
