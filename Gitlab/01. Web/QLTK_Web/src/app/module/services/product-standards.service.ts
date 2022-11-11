import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProductStandardsService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/SearchProductStandard', model, httpOptions);
    return tr
  }

  deleteProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/DeleteProductStandard', model, httpOptions);
    return tr
  }

  createProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/AddProductStandard', model, httpOptions);
    return tr
  }

  getProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/GetProductStandardInfo', model, httpOptions);
    return tr
  }

  getSBUIdandDeepartmentId(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/GetSBUIdandDeepartmentId', httpOptions);
    return tr
  }

  updateProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/UpdateProductStandard', model, httpOptions);
    return tr
  }
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/ExportExcel', model, httpOptions);
    return tr
  }
  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  getShowQCProductStandard(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandard/GetShowQCProductStandardInfo', model, httpOptions);
    return tr
  }

}
