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
export class SolutionAnalysisSupplierService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }


  getlistSupplier(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SolutionSupplier/GetlistSupplier', model, httpOptions);
    return tr;
  }
}
