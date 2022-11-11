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
export class IndustryService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchIndustry(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Industry/SearchIndustry', model, httpOptions);
    return tr
  }

  deleteIndustry(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Industry/DeleteIndustry', model, httpOptions);
    return tr
  }

  createIndustry(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Industry/AddIndustry', model, httpOptions);
    return tr
  }

  getIndustry(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Industry/GetIndustry', model, httpOptions);
    return tr
  }

  updateIndustry(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Industry/UpdateIndustry', model, httpOptions);
    return tr
  }
}
