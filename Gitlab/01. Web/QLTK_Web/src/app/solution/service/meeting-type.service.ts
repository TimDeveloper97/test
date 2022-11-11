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

export class MeetingTypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMeetingType(): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'MeetingType/SearchMeetingType', httpOptions);
    return tr
  }

  createMeeting(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MeetingType/CreateMeetingType', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'MeetingType/GetById/'+ id, httpOptions);
    return tr
  }

  update(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MeetingType/UpdateMeetingType', model, httpOptions);
    return tr
  }

  delete(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'MeetingType/DeleteMeetingType/'+ id, httpOptions);
    return tr
  }
}
