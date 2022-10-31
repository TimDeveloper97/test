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
export class ProductCreatesService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  createProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/AddProduct', model, httpOptions);
    return tr
  }

  getProductInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetProductInfo', model, httpOptions);
    return tr
  }

  updateProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/UpdateProduct', model, httpOptions);
    return tr
  }

  searchModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetModule', model, httpOptions);
    return tr
  }

  deleteProductModuleUpdate(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/DeleteProductModuleUpdate?id=' + id, httpOptions);
    return tr
  }

  getListFileTestAttachByProductId(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetListFileTestAttachByProductId', model, httpOptions);
    return tr
  }
  CreateFileTestAttach(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/CreateFileTestAttach', model, httpOptions);
    return tr
  }

  getProductDocumentAttachs(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/GetProductDocumentAttachs', model, httpOptions);
    return tr
  }

  updateProductDocument(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/UpdateProductDocument', model, httpOptions);
    return tr
  }

  getProductNeedPublications(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Product/getProductNeedPublications', model, httpOptions);
    return tr
  }
}
