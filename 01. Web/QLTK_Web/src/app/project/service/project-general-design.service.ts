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
export class ProjectGeneralDesignService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchProjectGeneralDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/SearchProjectGeneralDesign', model, httpOptions);
    return tr;
  }

  searchProjectProductExport(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/SearchProjectProductExport', model, httpOptions);
    return tr;
  }

  addProjectGeneralDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/AddProjectGeneralDesign', model, httpOptions);
    return tr;
  }

  updateProjectGeneralDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/UpdateProjectGeneralDesign', model, httpOptions);
    return tr;
  }

  deleteProjectGeneralDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/DeleteProjectGeneralDesign', model, httpOptions);
    return tr;
  }

  getProjectGeneralDesignInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GetProjectGeneralDesignInfo', model, httpOptions);
    return tr;
  }

  generalDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GeneralDesign', model, httpOptions);
    return tr;
  }

  searchMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/SearchMaterial', model, httpOptions);
    return tr;
  }

  searchModuleSale(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/SearchModuleSale', model, httpOptions);
    return tr;
  }

  getListDepartment(sbuId: string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GetListDepartment?sbuId=' + sbuId, httpOptions);
    return tr;
  }

  getData(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GetData', httpOptions);
    return tr;
  }

  exportGeneralDesign(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/ExpoetGeneralDesign', model, httpOptions);
    return tr;
  }

  exportExcelManage(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/exportExcelManage', model, httpOptions);
    return tr;
  }

  exportExcelBOM(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/ExportExcelBOM', model, httpOptions);
    return tr;
  }

  exportBOM(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/ExportBOM', model, httpOptions);
    return tr;
  }

  updateApproveStatus(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/UpdateApproveStatus', model, httpOptions);
    return tr;
  }

  getListPlanByProjectProduct(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GetListPlanByProjectProduct', model, httpOptions);
    return tr;
  }

  checkApproveStatus(projectProductId: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/CheckApproveStatus?projectProductId=' + projectProductId, httpOptions);
    return tr;
  }
  
  getListMaterialOfModule(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/GetListMaterialOfModule', model, httpOptions);
    return tr;
  }

  UpdateMaterialImportBOM(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectGeneralDesign/UpdateMaterialImportBOM', model, httpOptions);
    return tr;
  }
}
