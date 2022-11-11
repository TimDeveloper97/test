import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class PracticeExpertService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  //Tab chuyên gia
  loadPracticeExpert(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeExpertChoose/LoadPracticeExperts', model, httpOptions);
    return tr
  }

  searchPracticeExpert(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeExpertChoose/SearchPracticeExperts', model, httpOptions);
    return tr
  }

  searchExpert(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeExpertChoose/SearchExpert', model, httpOptions);
    return tr
  }

  createPracticeExpert(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeExpertChoose/AddPracticeExperts', model, httpOptions);
    return tr
  }
}
