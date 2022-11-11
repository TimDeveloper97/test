import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class SurveyMaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSurveyMaterial(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyMaterial/SearchSurveyMaterial', model, httpOptions);
    return tr;
  }
  createSurveyMaterial(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyMaterial/AddSurveyMaterial', model, httpOptions );
    return tr;
  }
  deleteSurveyMaterial(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyMaterial/DeleteSurveyMaterial', model, httpOptions);
    return tr;
  }
  updateSurveyMaterial(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyMaterial/UpdateSurveyMaterial', model, httpOptions);
    return tr;
  }
  getSurveyMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SurveyMaterial/GetSurveyMaterial', model, httpOptions);
    return tr
  }

}
