import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration } from 'src/app/shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class GuidelineService {

  constructor(private http: HttpClient,
    private config: Configuration,) { }

    getGuidelineInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Guideline/GetGuidelineInfo', model, httpOptions);
    return tr
    }
    UpdateGuidelineInfo(model: any): Observable<any> {
      var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Guideline/UpdateGuidelineInfo', model, httpOptions);
      return tr
  }
}
