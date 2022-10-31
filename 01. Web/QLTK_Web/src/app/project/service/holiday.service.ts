import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}
@Injectable({
  providedIn: 'root'
})
export class HolidayService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCalendarOfYear(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Holiday/GetCalendarOfYear', model, httpOptions);
    return tr;
  }

  createHoliday(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Holiday/CreateHoliday', model, httpOptions);
    return tr;
  }
}
