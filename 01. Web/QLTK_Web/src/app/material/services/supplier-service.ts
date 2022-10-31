import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class SupplierService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSupplier(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/SearchSupplier', model, httpOptions);
    return tr
  }

  searchManufacture(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/SearchManufacture', model, httpOptions);
    return tr
  }

  searchSupplierManufacture(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/SearchSupplierManufacture', model, httpOptions);
    return tr
  }

  deleteSupplier(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/DeleteSupplier', model, httpOptions);
    return tr
  }

  createSupplier(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/AddSupplier', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  getSupplierInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/GetSupplierInfo', model, httpOptions);
    return tr
  }

  getSupplierContractInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/GetSupplierContractInfo', model, httpOptions);
    return tr
  }

  getSupplierCode(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/GetSupplierCode', httpOptions);
    return tr
  }

  updateSupplier(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/UpdateSupplier', model, httpOptions);
    return tr
  }

  getCbbManufacture(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ComboboxCommon/GetListManufacture', httpOptions);
    return tr
  }

  importFile(file): Observable<any> {
    // var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/ImportFile', model, httpOptions);
    // return tr
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/ImportFile', formData);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/ExportExcel', model, httpOptions);
    return tr
  }

  getGroupInTemplate(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/GetGroupInTemplate', httpOptions);
    return tr
  }

  CreateSupplierContract(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/CreateSupplierContract', model, httpOptions);
    return tr
  }

  updateSupplierContract(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/UpdateSupplierContract', model, httpOptions);
    return tr
  }

  deleteSupplierContract(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Supplier/DeleteSupplierContract', model, httpOptions);
  return tr
  }
}
