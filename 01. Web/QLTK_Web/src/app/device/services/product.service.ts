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
export class ProductService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }


  searchProduct(modelSearch): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/SearchProduct', modelSearch, httpOptions);
    return tr;
  }

  searchProductError(modelSearch): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/SearchProductErrors', modelSearch, httpOptions);
    return tr;
  }

  createProductSketches(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/AddProductSketches', model, httpOptions);
    return tr;
  }

  getInfoSketches(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetProductSketchesInfo', model, httpOptions);
    return tr;
  }

  importProductModule(file: any, productId: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('ProductId', productId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/ImportProductModule', formData);
    return tr
  }

  deleteProduct(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/DeleteProduct', model, httpOptions);
    return tr;
  }

  deleteProductGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/DeleteProductGroup', model, httpOptions);
    return tr;
  }

  uploadDesignDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/UploadDesignDocument', model, httpOptions);
    return tr;
  }

  getListFolderProduct(productId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Product/GetListFolderProduct?productId=' + productId, httpOptions);
    return tr;
  }

  getListFileProduct(folderId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Product/GetListFileProduct?folderId=' + folderId, httpOptions);
    return tr;
  }

  getModulePrice(listModuleId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetModulePrice', listModuleId, httpOptions);
    return tr;
  }

  synchronizedPractice(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/SynchronizedPractice', model, httpOptions);
    return tr
  }

  checkStatusProduct(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/CheckStatusProduct', model, httpOptions);
    return tr
  }

  getListMatarial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetListMatarial', model, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/ExportExcel', model, httpOptions);
    return tr
  }

  getContentProduct(productId: string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetContentProduct?productId=' + productId, httpOptions);
    return tr;
  }

  updateContent(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/UpdateContent', model, httpOptions);
    return tr;
  }

  importSyncSaleProduct(file: any, isConfirm: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('isConfirm', isConfirm);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/ImportFile', formData);
    return tr
  }

  syncSaleProduct(check: any, isConfirm: any, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/SyncSaleProduct?check=' + check + '&isConfirm=' + isConfirm, model, httpOptions);
    return tr;
  }

  getModuleByProductId(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetModuleByProduct', model, httpOptions);
    return tr;
  }

  downAllDocumentProcess(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/DownAllDocumentProcess', model, httpOptions);
    return tr
  }

  updateNewPrice(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/UpdateNewPrice', httpOptions);
    return tr;
  }
}
