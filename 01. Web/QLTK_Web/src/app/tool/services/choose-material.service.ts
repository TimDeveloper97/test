
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
export class ChooseMaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }
  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GetMaterials', model, httpOptions);
    return tr
  }
}
