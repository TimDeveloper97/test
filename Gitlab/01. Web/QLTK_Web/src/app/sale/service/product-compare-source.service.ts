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
export class ProductCompareSourceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProductCompareSource(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductCompareSource/SearchProductCompareSource', model, httpOptions);
    return tr
  }

  getProductCompareSourceById(id): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ProductCompareSource/GetProductCompareSourceById/'+ id);
    return tr
  }

  UpdateListSaleProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductCompareSource/UpdateListSaleProduct', model, httpOptions);
    return tr
  }

  UpdateSaleProduct(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductCompareSource/UpdateSaleProduct/'+ id, httpOptions);
    return tr
  }
}
