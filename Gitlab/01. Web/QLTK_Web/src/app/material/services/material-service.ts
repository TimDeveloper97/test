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
export class MaterialService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/SearchMaterial', model, httpOptions);
    return tr
  }

  deleteMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/DeleteMaterial', model, httpOptions);
    return tr
  }

  createMaterial(model, file: any): Observable<any> {
    let formData: FormData = new FormData();
    var ff = JSON.stringify(model);
    formData.append('Model', ff);
    for (var i = 0; i < file.length; i++) {
      if (file[i].isDel == false) {
        formData.append('File' + i, file[i].objFile);
      }
    }
    return this.http.post(this.config.ServerWithApiUrl + 'Material/AddMaterial', formData);
  }

  DownloadAFile(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'HandlingImage/DownloadFile', model, httpOptions);
    return tr
  }

  getMaterialInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/GetMaterialInfo', model, httpOptions);
    return tr
  }

  updateMaterial(model, file: any): Observable<any> {
    let formData: FormData = new FormData();
    var ff = JSON.stringify(model);
    formData.append('Model', ff);
    for (var i = 0; i < file.length; i++) {
      if (file[i].isDel == false) {
        formData.append('File' + i, file[i].objFile);
      }
    }
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/UpdateMaterial', formData);
    return tr
  }

  // importFile(model:any): Observable<any> {
  //   var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ImportFile', model, httpOptions);
  //   return tr
  // }

  importFile(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    //formData.append('ProjectId', projectId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ImportFile', formData);
    return tr
  }

  creatNewBuyHistory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/CreatNewBuyHistory', model, httpOptions);
    return tr
  }

  overwriteBuyHistory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/OverwriteBuyHistory', model, httpOptions);
    return tr
  }

  getHistoryByMaterialId(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/GetHistoryByMaterialId', model, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ExportExcel', model, httpOptions);
    return tr
  }

  importFileMaterial(file): Observable<any> {
    // var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ImportFileMaterial', model, httpOptions);
    // return tr
    let formData: FormData = new FormData();
    formData.append('File', file);
    //formData.append('ProjectId', projectId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/MaterialImportFile', formData);
    return tr
  }

  getListMaterial(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/GetListMaterial', httpOptions);
    return tr
  }

  importFile3D(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ImportFile3D', model, httpOptions);
    return tr
  }

  getGroupInTemplate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/GetGroupInTemplate', model, httpOptions);
    return tr
  }

  exportDMVTNotDB(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ExportDMVTNotDB', model, httpOptions);
    return tr
  }

  checkPriceMaterial(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    //formData.append('ProjectId', projectId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/CheckPriceMaterial', formData);
    return tr
  }

  exportCheckPrice(materials): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ExportCheckPrice', materials, httpOptions);
    return tr
  }

  importSyncSaleMaterial(file: any, isConfirm: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('isConfirm', isConfirm);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/ImportFileSync', formData);
    return tr;
  }

  syncSaleMaterial(check: any, isConfirm: boolean, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/SyncSaleMaterial?check=' + check + '&isConfirm=' + isConfirm, model, httpOptions);
    return tr;
  }
  getGroupMaterialCodeInTemplate(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/GetGroupMaterialCodeInTemplate', httpOptions);
    return tr
  }

  importFileMaterialCode(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/MaterialCodeImportFile', formData);
    return tr
  }

  importFileMateria(file,projectProductId,moduleId,isExit,confirm): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Material/MaterialImportFileBOM?projectProductId='+projectProductId+'&moduleId='+moduleId+'&isExit='+isExit +'&confirm='+confirm, formData);
    return tr
  }
}
