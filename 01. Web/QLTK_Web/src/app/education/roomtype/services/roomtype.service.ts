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

export class RoomtypeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchRoomType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Roomtype/SearchRoomType', model, httpOptions);
    return tr
  }

  deleteRoomType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Roomtype/DeleteRoomType', model, httpOptions);
    return tr
  }

  createRoomType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Roomtype/AddRoomType', model, httpOptions);
    return tr
  }

  getRoomType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Roomtype/GetRoomType', model, httpOptions);
    return tr
  }

  updateRoomType(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Roomtype/UpdateRoomType', model, httpOptions);
    return tr
  }

}
