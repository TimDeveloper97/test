import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';



const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})

export class ExpertService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  searchExpert(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/SearchExpert', model, httpOptions);
    return tr;
  }

  searchWorkPlace(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/SearchWorkPlace', model, httpOptions);
    return tr;
  }

  searchSpecialize(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/SearchSpecialize', model, httpOptions);
    return tr;
  }

  createExpert(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/AddExpert', model, httpOptions );
    return tr;
  }
  deleteExpert(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/DeleteExpert', model, httpOptions);
    return tr;
  }
  updateExpert(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/UpdateExpert', model, httpOptions);
    return tr;
  }
  getExpert(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/GetExpert', model, httpOptions);
    return tr
  }
  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Expert/ExportExcel', model, httpOptions);
    return tr
  }
  
  checkDeleteBank(expertId): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'Expert/Checkdelete/'+ expertId);
    return tr
  }
}
