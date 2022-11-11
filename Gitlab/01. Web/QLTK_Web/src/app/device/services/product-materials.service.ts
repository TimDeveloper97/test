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
export class ProductMaterialsService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  searchProductMaterials(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/SearchProductMaterials', model, httpOptions);
    return tr
  }

  searchProductMaterialsIsSetup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/SearchProductMaterialsSetup', model, httpOptions);
    return tr
  }

  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/SearchMaterials', model, httpOptions);
    return tr
  }

  deleteProductMaterials(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/DeleteProductMaterials', model, httpOptions);
    return tr
  }

  updateProductMaterials(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/UpdateProductMaterials', model, httpOptions);
    return tr
  }

  addProductMaterials(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/AddProductMaterials', model, httpOptions);
    return tr
  }

  getProductInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/GetProductInfo', model, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductMaterials/ExportExcel', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  downAllDocumentProcess(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/DownAllDocumentProcess', model, httpOptions);
    return tr
  }
}
