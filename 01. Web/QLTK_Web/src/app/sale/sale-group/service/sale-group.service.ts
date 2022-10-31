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
export class SaleGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSaleGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/SearchSaleGroup', model, httpOptions);
    return tr;
  }

  getInfoSaleGroup(id): Observable<any>{
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'SaleGroup/GetInfoSaleGroup/'+ id);
    return tr;
  }

  createSaleGroup(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/CreateSaleGroup', model, httpOptions);
    return tr;
  }

  updateSaleGroup(id,model): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/UpdateSaleGroup/'+id, model, httpOptions);
    return tr;
  }

  deleteSaleGroup(id): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/DeleteSaleGroup/'+ id, httpOptions);
    return tr;
  }

  getListEmployee(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/GetListEmployee', model, httpOptions);
    return tr;
  }

  getListProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SaleGroup/GetListProduct', model, httpOptions);
    return tr;
  }

}
