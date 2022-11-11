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
export class BankService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  
  searchBank(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Bank/SearchBank', model, httpOptions);
    return tr;
  }

  deleteBank(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Bank/DeleteBank', model, httpOptions);
    return tr;
  }

  getBank(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Bank/GetBank', model, httpOptions);
    return tr
  }
}
