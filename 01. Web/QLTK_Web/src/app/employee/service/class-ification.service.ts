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
export class ClassIficationService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchClassIfication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassIfication/SearchClassIfication', model, httpOptions);
    return tr;
  }

  createClassIfication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassIfication/CreateClassIfication', model, httpOptions);
    return tr;
  }

  updateClassIfication(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassIfication/UpdateClassIfication', model, httpOptions);
    return tr;
  }

  GetInforClassIfication(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassIfication/GetInforClassIfication?ificationId=' + id, httpOptions);
    return tr;
  }

  deleteClassIfication(ificationId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassIfication/DeleteClassIfication?ificationId=' + ificationId, httpOptions);
    return tr;
  }
}
