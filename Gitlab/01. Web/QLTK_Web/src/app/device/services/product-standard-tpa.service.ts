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
export class ProductStandardTpaService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProductStandardTPA(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/SearchProductStandardTPA', model, httpOptions);
    return tr;
  }

  createProductStandardTPA(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/CreateProductStandardTPA', model, httpOptions);
    return tr;
  }

  updateProductStandardTPA(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/UpdateProductStandardTPA', model, httpOptions);
    return tr;
  }

  deleteProductStandardTPA(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/DeleteProductStandardTPA', model, httpOptions);
    return tr;
  }

  getProductStandardTPAInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/GetProductStandardTPAInfo', model, httpOptions);
    return tr;
  }

  importProductStandardTPA(file: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/ImportProductStandardTPA', formData);
    return tr
  }

  uploadFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/UploadFile', model, httpOptions);
    return tr;
  }

  getListProductStandardTPAFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/GetListProductStandardTPAFile', model, httpOptions);
    return tr;
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/ExportExcel', model, httpOptions);
    return tr;
  }

  exportExcelAccountant(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/ExportExcelAccountant', model, httpOptions);
    return tr;
  }

  exportExcelBusiness(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/ExportExcelBusiness', model, httpOptions);
    return tr;
  }

  GetSuppliers(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/GetSuppliers', httpOptions);
    return tr;
  }

  importSyncSaleProductStandardTPA(file: any, isConfirm: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('isConfirm', isConfirm);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/ImportFile', formData);
    return tr
  }

  syncSaleProductStandardTPA(check: any, isConfirm: any, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardTPA/SyncSaleProductStandardTPA?check=' + check + '&isConfirm=' + isConfirm, model, httpOptions);
    return tr;
  }
}
