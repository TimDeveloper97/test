import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProductAccessoriesService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }

  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/SearchMaterial', model, httpOptions);
    return tr
  }
  addProductAccessories(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/AddProductAccessories', model, httpOptions);
    return tr
  }

  updateProductAccessories(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/UpdateProductAccessories', model, httpOptions);
    return tr
  }

  getProductAccessoriesInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/SearchProductAccessories', model, httpOptions);
    return tr
  }
  deleteProductAccessories(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/DeleteProductAccessories', model, httpOptions);
    return tr
  }
  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductAccessories/ExportExcel', model, httpOptions);
    return tr
  }
}
