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


export class FunctionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }



  searchFunction(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Function/SearchFunction', model, httpOptions);
    return tr;
  }
  createFunction(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Function/AddFunction', model, httpOptions );
    return tr;
  }
  deleteFunction(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Function/DeleteFunction', model, httpOptions);
    return tr;
  }
  updateFunction(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Function/UpdateFunction', model, httpOptions);
    return tr;
  }
  getFunction(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Function/GetFunction', model, httpOptions);
    return tr;
  }

  getListGroup(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListFunctionGroup', httpOptions);
    return tr;
  }
}
