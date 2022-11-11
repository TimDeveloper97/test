import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class QuoteStepService {
  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  
  searchQuoteStep(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/SearchQuoteStep', model, httpOptions);
    return tr
  }

  getInfoById(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/GetQuoteInfo', model, httpOptions);
    return tr
  }

  getIndex(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/GetIndex', httpOptions);
    return tr
  }

  createQuote(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/CreateQuote', model, httpOptions);
    return tr
  }

  updateQuote(model:any):Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl+'Categories/UpdateQuote',model,httpOptions);
    return tr
  }

  createIndex(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/CreateIndex', model, httpOptions);
    return tr
  }

  deleteQuote(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Categories/DeleteQuote', model, httpOptions);
    return tr
  }
}
