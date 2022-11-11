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

export class ClassRoomService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchClassRoom(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchClassRoom', model, httpOptions);
    return tr;
  }

  searchRoomType(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchRoomType', model, httpOptions);
    return tr;
  }

  searchMaterial(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchMaterial', model, httpOptions);
    return tr;
  }

  searchModule(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchModule', model, httpOptions);
    return tr;
  }

  searchProduct(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchProduct', model, httpOptions);
    return tr;
  }

  searchPractice(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchPractice', model, httpOptions);
    return tr;
  }

  searchSkill(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/SearchSkill', model, httpOptions);
    return tr;
  }

  createClassRoom(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/AddClassRoom', model, httpOptions );
    return tr;
  }
  deleteClassRoom(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/DeleteClassRoom', model, httpOptions);
    return tr;
  }
  updateClassRoom(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/UpdateClassRoom', model, httpOptions);
    return tr;
  }
  getClassRoom(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetClassRoom', model, httpOptions);
    return tr
  }
  ExportExcel(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/ExportExcel', model, httpOptions);
    return tr
  }

  DownloadAFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  uploadDesignDocument(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/UploadDesignDocument', model, httpOptions);
    return tr;
  }

  getListFolderClassRoom(classRoomId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetListFolderClassRoom?classRoomId=' + classRoomId, httpOptions);
    return tr;
  }

  getListFileClassRoom(folderId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetListFileClassRoom?folderId=' + folderId, httpOptions);
    return tr;
  }

  getPriceModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetPriceModule', model, httpOptions);
    return tr
  }

  GetPricePractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetPricePractice', model, httpOptions);
    return tr
  }

  GetPriceProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/GetPriceProduct', model, httpOptions);
    return tr
  }

  updatePriceClass(): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ClassRoom/UpdatePriceClassRoom', httpOptions);
    return tr
  }


}
