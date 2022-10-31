import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProductStandardGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProductStandardGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardGroup/SearchProductStandardGroup', model, httpOptions);
    return tr
  }

  deleteProductStandardGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardGroup/DeleteProductStandardGroup', model, httpOptions);
    return tr
  }

  createProductStandardGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardGroup/AddProductStandardGroup', model, httpOptions);
    return tr
  }

  getProductStandardGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardGroup/GetProductStandardGroupInfo', model, httpOptions);
    return tr
  }

  updateProductStandardGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductStandardGroup/UpdateProductStandardGroup', model, httpOptions);
    return tr
  }

}
