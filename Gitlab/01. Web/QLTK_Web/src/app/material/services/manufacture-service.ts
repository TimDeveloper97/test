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
export class ManufactureService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchManufacturer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/SearchManufacture', model, httpOptions);
    return tr
  }

  searchSupplierManufacture(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/SearchSupplierManufacture', model, httpOptions);
    return tr
  }

  deleteManufacturer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/DeleteManufacture', model, httpOptions);
    return tr
  }

  createManufacture(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/AddManufacture', model, httpOptions);
    return tr
  }

  getManufactureInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/GetManufactureInfo', model, httpOptions);
    return tr
  }

  updateManufacture(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/UpdateManufacture', model, httpOptions);
    return tr
  }

  importFile(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ManufactureImportFile', formData);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/ExportExcel', model, httpOptions);
    return tr
  }

  getListManufacture(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/GetListManufacture', httpOptions);
    return tr
  }

  getGroupInTemplate(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/GetGroupInTemplate', httpOptions);
    return tr
  }

  searchParentId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'QLTKM/SearchParentId', model, httpOptions);
    return tr
  }
}
