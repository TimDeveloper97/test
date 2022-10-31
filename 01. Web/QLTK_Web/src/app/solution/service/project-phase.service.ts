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
export class ProjectPhaseService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProjectPhase(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectPhase/SearchProjectPhase', model, httpOptions);
    return tr
  }

  deleteProjectPhase(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectPhase/DeleteProjectPhase', model, httpOptions);
    return tr
  }

  createProjectPhase(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectPhase/AddProjectPhase', model, httpOptions);
    return tr
  }

  getProjectPhase(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectPhase/GetProjectPhase', model, httpOptions);
    return tr
  }

  updateProjectPhase(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectPhase/UpdateProjectPhase', model, httpOptions);
    return tr
  }
}
