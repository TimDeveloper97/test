import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ExportAndKeepService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchExportAndKeep(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/Search', model, httpOptions);
    return tr
  }

  SearchExportAndKeepHistory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/SearchExportAndKeepHistory', model, httpOptions);
    return tr
  }

  CreateExportAndKeep(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/Create', model, httpOptions);
    return tr
  }

  UpdateExportAndKeep(id, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/UpdateExportAndKeep', model, httpOptions);
    return tr
  }

  GetInfoByIdExportAndKeep(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetInfoByIdExportAndKeep/' + id, httpOptions);
    return tr
  }

  GetExportAndKeepViewById(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetExportAndKeepViewById/' + id, httpOptions);
    return tr
  }


  deleteExportAndKeep(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/DeleteUpdateExportAndKeep/' + id, httpOptions);
    return tr
  }

  GenerateCode(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GenerateCode', httpOptions);
    return tr
  }

  GenerateCodeCustomer(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GenerateCodeCustomer', httpOptions);
    return tr
  }

  GetSaleProducts(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetSaleProducts', model, httpOptions);
    return tr
  }

  GetSaleProductById(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetSaleProductById/' + id, httpOptions);
    return tr
  }

  createCustomer(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/CreateCustomer', model, httpOptions);
    return tr
  }

  GetCustomerTypes(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetCustomerTypes', httpOptions);
    return tr
  }

  GetListCustomers(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetListCustomer', httpOptions);
    return tr
  }

  ManumitExportAndKeep(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/ManumitExportAndKeep/' + id, httpOptions);
    return tr
  }

  SoldExportAndKeep(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/SoldExportAndKeep/' + id, httpOptions);
    return tr
  }

  GetListExportDetailBySaleProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ExportAndKeep/GetListExportDetailBySaleProductId/' + id, httpOptions);
    return tr
  }

  PrintCustomer(id: string): Observable<any> {
    var apiPath = this.config.ServerWithApiUrl + 'ExportAndKeep/PrintCustomer?id=' + id;
    var tr = this.http.get(apiPath, {
      responseType: "blob"
    });
    return tr
  }
}
