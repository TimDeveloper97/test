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
export class SupplierGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSupplierGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SupplierGroup/SearchSupplierGroup', model, httpOptions);
    return tr
  }

  createSupplierGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SupplierGroup/AddSupplierGroup', model, httpOptions);
    return tr
  }

  updateSupplierGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SupplierGroup/UpdateSupplierGroup', model, httpOptions);
    return tr
  }


  deleteSupplierGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SupplierGroup/DeletesupplierGroup', model, httpOptions);
    return tr
  }

  getSupplierGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SupplierGroup/GetSupplierGroupInfo', model, httpOptions);
    return tr
  }
}
