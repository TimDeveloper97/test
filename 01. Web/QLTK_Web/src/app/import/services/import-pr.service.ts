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

export class ImportPrService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchImportPR(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportPR/Search', model, httpOptions);
    return tr;
  }

  searchChoosePR(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportPR/SearchChoose', model, httpOptions);
    return tr;
  }

  deleteProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportPR/DeleteProduct', model, httpOptions);
    return tr;
  }

  importFile(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ImportPR/ImportFile', formData);
    return tr
  }
}
