import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class SimilarMaterialConfigService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSimilarMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/SearchSimilarMaterial', model, httpOptions);
    return tr
  }

  searchMaterialInSimilarMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/SearchMaterialInSimilarMaterial', model, httpOptions);
    return tr
  }

  getSimilarMaterialInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/GetSimilarMaterialInfo', model, httpOptions);
    return tr
  }

  createSimilarMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/AddSimilarMaterial', model, httpOptions);
    return tr
  }

  updateSimilarMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/UpdateSimilarMaterial', model, httpOptions);
    return tr
  }

  deleteSimilarMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterial/DeleteSimilarMaterial', model, httpOptions);
    return tr
  }

  //SimilarMaterialConfig
  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/SearchMaterial', model, httpOptions);
    return tr
  }

  searchSimilarMaterialConfig(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/SearchSimilarMaterialConfig', model, httpOptions);
    return tr
  }

  createSimilarMaterialConfig(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/AddSimilarMaterialConfig', model, httpOptions);
    return tr
  }

  updateSimilarMaterialConfig(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/UpdateSimilarMaterialConfig', model, httpOptions);
    return tr
  }

  deleteSimilarMaterialConfig(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/DeleteSimilarMaterialConfig', model, httpOptions);
    return tr
  }

  getMaterialInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/GetMaterialInfo', model, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SimilarMaterialConfig/ExportExcel', model, httpOptions);
    return tr
  }
}
