import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { identifierModuleUrl } from '@angular/compiler';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class SolutionService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/SearchSolution', model, httpOptions);
    return tr
  }

  getSolutionInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/GetSolutionInfo', model, httpOptions);
    return tr
  }

  createSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/AddSolution', model, httpOptions);
    return tr
  }

  updateSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/UpdateSolution', model, httpOptions);
    return tr
  }

  deleteSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/DeleteSolution', model, httpOptions);
    return tr
  }

  deleteConten(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/DeleteConten/' + id, httpOptions);
    return tr
  }

  deleteMaterial(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/DeleteMaterial/' + id, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/ExportExcel', model, httpOptions);
    return tr
  }

  searchProjectSolution(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/SearchProjectSolution', model, httpOptions);
    return tr
  }

  getSolutionCode(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/GetSolutionCode', model, httpOptions);
    return tr
  }

  uploadDesignDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/UploadDesignDocument', model, httpOptions);
    return tr;
  }

  uploadFileDesignDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/UploadFileDesignDocument', model, httpOptions);
    return tr;
  }


  getListFolderSolution(solutionId: any, curentVersion: number): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Solution/GetListFolderSolution?solutionId=' + solutionId + '&&curentVersion=' + curentVersion, httpOptions);
    return tr;
  }

  getListFileSolution(modal: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/GetListFileSolution/', modal, httpOptions);
    return tr;
  }

  getSolutionOldVersion(solutionId: any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Solution/GetSolutionOldVersion?solutionId=' + solutionId, httpOptions);
    return tr;
  }

  getDomainById(id: any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Solution/GetDomainById?id=' + id, httpOptions);
    return tr;
  }

  getSurveyContentId(id: any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Solution/GetSurveyContentId/'+id, httpOptions);
    return tr
  }

  checkDeleteSurvey(id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/Checkdelete/'+ id, httpOptions);
    return tr
  }

  saveMaterial(id: any ,modal: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Solution/saveMaterial?id=' + id , modal, httpOptions);
    return tr;
  }

  getSurveyMaterialId(id: any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Solution/GetSurveyMaterialId/'+id, httpOptions);
    return tr
  }
}
