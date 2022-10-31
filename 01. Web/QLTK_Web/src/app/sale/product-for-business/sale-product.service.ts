import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Configuration } from '../../shared';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class SaleProductService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchSaleProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/SearchSaleProduct', model, httpOptions);
    return tr
  }
  searchAccessory(idSaleProduct: string, model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/SearchSaleProduct/' + idSaleProduct, model, httpOptions);
    return tr
  }
  createSaleProduct(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/CreateSaleProduct', model, httpOptions);
    return tr
  }
  updateSaleProduct(id: string, model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/UpdateSaleProduct/' + id, model, httpOptions);
    return tr
  }
  SearchJob(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/SearchJob/', model, httpOptions);
    return tr
  }
  SearchApp(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/SearchApp/', model, httpOptions);
    return tr
  }
  getAllSaleProduct(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/getAllSaleProduct', model, httpOptions);
    return tr
  }
  getAppByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getAppByProductId/'+id, httpOptions);
    return tr
  }
  getJobByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getJobByProductId/'+id, httpOptions);
    return tr
  }
  getMediaByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getMediaByProductId/'+id, httpOptions);
    return tr
  }
  getDocumentByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getDocumentByProductId/'+id, httpOptions);
    return tr
  }
  getAccessoryByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getAccessoryByProductId/'+id, httpOptions);
    return tr
  }
  getProductInfoByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getProductInfoByProductId/'+id, httpOptions);
    return tr
  }

  importSaleProduct(file: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('LastModified', file.lastModified);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/ImportFile', formData);
    return tr
  }
  deleteSaleProduct(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/deleteSaleProduct/'+id, httpOptions);
    return tr
  }

  updateStatusSaleProduct(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/updateStatus/'+id, httpOptions);
    return tr
  }

  SearchGroupProduct(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/getAllGroupSaleProduct/', model, httpOptions);
    return tr
  }

  getGroupByProductId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getGroupByProductId/'+id, httpOptions);
    return tr
  }
  getEmployeeByGroupId(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleProduct/getEmployeeByGroupId/'+id, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/ExportExcel', model, httpOptions);
    return tr;
  }

  defautlType(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleProduct/DefaultType', model, httpOptions);
    return tr;
  }
}
