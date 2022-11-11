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
export class CustomerTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchCustomerType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CustomerType/SearchCustomerType', model, httpOptions);
    return tr
  }

  getCustomerTypeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CustomerType/GetCustomerTypeInfo', model, httpOptions);
    return tr
  }

  createCustomerType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CustomerType/AddCustomerType', model, httpOptions);
    return tr
  }

  updateCustomerType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CustomerType/UpdateCustomerType', model, httpOptions);
    return tr
  }

  deleteCustomerType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'CustomerType/DeleteCustomerType', model, httpOptions);
    return tr
  }
}
