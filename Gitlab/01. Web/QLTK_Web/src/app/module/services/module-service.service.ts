import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ModuleServiceService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  SearchModul(modelSearch): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/SearchModuls', modelSearch, httpOptions);
    return tr
  }
  ExPort(lst): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/ExPort', lst, httpOptions);
    return tr
  }
  DeleteGroupModul(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/DeleteModul', model, httpOptions);
    return tr
  }
  AddModule(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/AddModule', httpOptions);
    return tr
  }
  UpdateModule(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UpdateModule', model, httpOptions);
    return tr
  }
  UpdateModuleIsDMTV(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UpdateModuleIsDMTV', model, httpOptions);
    return tr
  }
  GetModuleInfo(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleInfo', httpOptions);
    return tr
  }
  ExportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/ExportExcel', model, httpOptions);
    return tr
  }
  SearchTestCriteria(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/Criteria', model, httpOptions);
    return tr
  }
  ExportExcelCriteria(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/ExportExcelCriteria', model, httpOptions);
    return tr
  }
  GetModuleGroupExcepted(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleGroupExcepted', model, httpOptions);
    return tr
  }
  GetModuleId(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleId', model, httpOptions);
    return tr
  }
  GetListTestCriteria(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetListTestCriteria', model, httpOptions);
    return tr
  }
  searchProductStandard(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/Productstandard', model, httpOptions);
    return tr
  }
  AddModuleGroupProductStandard(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/AddModuleGroupProductStandard', model, httpOptions);
    return tr
  }
  searchEmployee(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Designer/SearchEmployee', model, httpOptions);
    return tr
  }

  AddDesigner(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Designer/AddDesigner', model, httpOptions);
    return tr
  }

  getDesigners(moduleId): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Module/GetDesigners?moduleId=' + moduleId, httpOptions);
    return tr
  }

  DeleteDesigner(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Designer/DeleteDesigner', model, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Designer/ExportExcel', model, httpOptions);
    return tr
  }

  searchModuleErrors(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleError/SearchModuleErrors', model, httpOptions);
    return tr
  }

  searchModuleMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/SearchModuleMaterial', model, httpOptions);
    return tr
  }

  searchModuleMaterialsSetup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/SearchModuleMaterialsSetup', model, httpOptions);
    return tr
  }

  searchSimilarMaterialConfig(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/SearchSimilarMaterialConfig', model, httpOptions);
    return tr
  }

  searchSimilarMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/SearchSimilarMaterial', model, httpOptions);
    return tr
  }

  exportExcelModuleMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/ExportExcel', model, httpOptions);
    return tr
  }

  getErrorInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleError/GetErrorInfo', model, httpOptions);
    return tr
  }
  getModuleProductStandardInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetModuleProductStandardInfo', model, httpOptions);
    return tr
  }

  getContentModule(moduleId: string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/GetContentModule?moduleId=' + moduleId, httpOptions);
    return tr
  }

  updateContent(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/UpdateContent', model, httpOptions);
    return tr
  }

  importSyncSaleModule(file: any, isConfirm: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('isConfirm', isConfirm);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/ImportFile', formData);
    return tr;
  }

  syncSaleModule(check: any, isConfirm: any, model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/SyncSaleModule?check=' + check + '&isConfirm=' + isConfirm, model, httpOptions);
    return tr;
  }

  searchDocument(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Module/SearchDocument', model, httpOptions);
    return tr
}
exportExcelMaterialBOMDraft(model: any): Observable<any> {
  var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ModuleMaterial/ExportExcelMaterialBOMDraft', model, httpOptions);
  return tr
}
}
