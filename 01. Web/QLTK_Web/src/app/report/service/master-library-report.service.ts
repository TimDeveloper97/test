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
export class MasterLibraryReportService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchMasterLibrary(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterLibrary/SearchMasterLibrary', model, httpOptions);
    return tr
  }

  ExportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MasterLibrary/ExportExcel', model, httpOptions);
    return tr
  }
}
