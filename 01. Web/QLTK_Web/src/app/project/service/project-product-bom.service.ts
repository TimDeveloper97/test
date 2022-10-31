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
export class ProjectProductBomService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchBOMDesignTwo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/SearchBOMDesignTwo', model, httpOptions);
    return tr
  }

  createBOMDesignTwo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/CreateBOMDesignTwo', model, httpOptions);
    return tr
  }

  deleteBOMDesignTwo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/DeleteBOMDesignTwo', model, httpOptions);
    return tr
  }

  importMaterialElectric(file, bomDesignTwoId, projectProductId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('BOMDesignTwoId', bomDesignTwoId);
    formData.append('ProjectProductId', projectProductId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ImportMaterialElectric', formData);
    return tr
  }

  importMaterialManufacture(file, bomDesignTwoId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('BOMDesignTwoId', bomDesignTwoId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ImportMaterialManufacture', formData);
    return tr
  }

  importMaterialTPA(file, bomDesignTwoId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('BOMDesignTwoId', bomDesignTwoId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ImportMaterialManufacture', formData);
    return tr
  }

  importMaterialOther(file, bomDesignTwoId, projectProductId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('BOMDesignTwoId', bomDesignTwoId);
    formData.append('ProjectProductId', projectProductId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ImportMaterialOther', formData);
    return tr
  }

  importListFile(listFileImport, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model',JSON.stringify(model) );
    listFileImport.forEach(item => {
      formData.append(item.Index, item.File);
    });
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ImportData', formData);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/ExportExcel', model, httpOptions);
    return tr
  }

  getVersion(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'BOMDesignTwo/GetVersion', model, httpOptions);
    return tr
  }

  searchModuleMaterial(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ProjectProduct/SearchModuleMaterial', model, httpOptions);
    return tr
  }
}
