import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ForecastProjectsService {

  constructor(
    private http: HttpClient,
    private config:Configuration
  ) { }

  getForecastProjects(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ForecastProject/GetForecastProjects', httpOptions);
    return tr
  }
}
