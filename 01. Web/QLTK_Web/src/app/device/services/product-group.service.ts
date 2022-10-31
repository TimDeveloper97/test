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
export class ProductGroupService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProductGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/SearchProductGroup', model, httpOptions);
    return tr
  }

  searchProductGroupById(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/SearchProductGroupById', model, httpOptions);
    return tr
  }

  deleteProductGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/DeleteProductGroup', model, httpOptions);
    return tr
  }

  getCbbProductGroupForUpdate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/SearchProductGroupExpect', model, httpOptions);
    return tr
  }

  createProductGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/AddProductGroup', model, httpOptions);
    return tr
  }

  //searchProductStandard(model:any): Observable<any> {
  //  var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GroupModule/SearchProductStandard', model, httpOptions);
  //  return tr
  //}

  getProductGroupInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/GetProductGroupInfo', model, httpOptions);
    return tr
  }

  updateProductGroup(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProductGroups/UpdateProductGroup', model, httpOptions);
    return tr
  }
}
