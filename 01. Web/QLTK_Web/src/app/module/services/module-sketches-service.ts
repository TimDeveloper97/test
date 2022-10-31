import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class ModuleSketchesService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }


  createSketchFunction(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/AddSketchesFunction', model, httpOptions);
    return tr
  }

  searchFunction(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/GetFunction', model, httpOptions);
    return tr
  }

  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/GetMaterial', model, httpOptions);
    return tr
  }

  getSketchesInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/GetSketchAttachInfo', model, httpOptions);
    return tr
  }

  addFileSketches(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/AddFile', model, httpOptions);
    return tr
  }

  getSketchesHistoryInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/GetSketchHistoryInfo', model, httpOptions);
    return tr
  }

  searchSketchHistory(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/SearchSketchHistory', model, httpOptions);
    return tr
  }

  importFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/ImportFile', model, httpOptions);
    return tr
  }

  searchSketchesMaterialElectronic(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/SearchSketchesMaterialElectronic', model, httpOptions);
    return tr
  }
  
  searchSketchesMaterialMechanical(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/SearchSketchesMaterialMechanical', model, httpOptions);
    return tr
  }

  deleteSketchMaterialElectronic(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/DeleteSketchMaterialElectronic', model, httpOptions);
    return tr
  }

  deleteSketchMaterialMechanical(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Sketches/DeleteSketchMaterialMechanical', model, httpOptions);
    return tr
  }
}
