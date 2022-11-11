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
export class PracticeService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/SearchPractice', model, httpOptions);
    return tr
  }
  // skill
  searchSkill(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/SearchSkill', model, httpOptions);
    return tr
  }

  createPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/AddPractice', model, httpOptions);
    return tr
  }

  updatePractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/UpdatePractice', model, httpOptions);
    return tr
  }

  deletePractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/DeletePractice', model, httpOptions);
    return tr
  }

  getPracticeInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/GetPracticeInfo', model, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/ExportExcel', model, httpOptions);
    return tr
  }

  getContentPractice(practiceId: string): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/GetContentPractice?practiceId=' + practiceId, httpOptions);
    return tr
  }

  updateContent(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/UpdateContent', model, httpOptions);
    return tr
  }

  //Tab thiết bị phụ trợ
  searchPracticeSupMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/SearchPracticeSubMaterial', model, httpOptions);
    return tr
  }

  importProductModule(file: any, practiceId: any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    formData.append('PracticeId', practiceId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Practice/ImportProductModule', formData);
    return tr
  }


  searchMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/SearchMaterial', model, httpOptions);
    return tr
  }

  searchModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/SearchModule', model, httpOptions);
    return tr
  }

  getPriceModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/GetPriceModule', model, httpOptions);
    return tr
  }

  createPracticeSupMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/AddPracticeSupMaterial', model, httpOptions);
    return tr
  }

  exportExcelPracticeSupMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/ExportExcelPracticeSubMaterial', model, httpOptions);
    return tr
  }

  //Tab vật tư
  searchModuleInPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterial/SearchModuleInPractice', model, httpOptions);
    return tr
  }

  searchPracticeMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterial/SearchPracticeMaterial', model, httpOptions);
    return tr
  }

  searchMaterialPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterial/SearchMaterial', model, httpOptions);
    return tr
  }

  createPracticeMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterial/AddPracticeMaterial', model, httpOptions);
    return tr
  }

  exportExcelPracticeMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterial/ExportExcelPracticeMaterial', model, httpOptions);
    return tr
  }


  // Tab vật tư tiêu hao
  // get vật tư tương tự theo practice 
  searchPracticeMaterialConsumable(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterialConsumable/SearchPracticeMaterialConsumable', model, httpOptions);
    return tr
  }
  ////Chọn vật tư
  searchMaterialPracticeConsumable(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterialConsumable/SearchMaterial', model, httpOptions);
    return tr
  }

  createPracticeMaterialConsumable(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterialConsumable/AddPracticeMaterialConsumable', model, httpOptions);
    return tr
  }

  exportExcelPracticeConsumable(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterialConsumable/ExportExcelPracticeMaterialConsumable', model, httpOptions);
    return tr
  }

  // Tab Thiết bị

  SearchPracticeProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeInProduct/SearchPracticeSubMaterial', model, httpOptions);
    return tr
  }

  //Tab Module
  searchPracticeModule(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeInProduct/SearchPracticeModule', model, httpOptions);
    return tr
  }

  addModuleInPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeInProduct/AddModuleInPractice', model, httpOptions);
    return tr
  }

  exportExcelPracticeProduct(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeInProduct/ExportExcelPracticeProduct', model, httpOptions);
    return tr
  }
  //Tab tài liệu
  getPracticeFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeFile/GetPracticeFileInfo', model, httpOptions);
    return tr
  }

  createPracticeFile(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeFile/AddFile', model, httpOptions);
    return tr
  }

  // Tab Kỹ năng
  getskillInPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSkill/GetskillInPractice', model, httpOptions);
    return tr
  }

  SearchSkillInPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSkill/SearchSkillInPractice', model, httpOptions);
    return tr
  }

  CreateSkillInPractice(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSkill/AddSkillInPractice', model, httpOptions);
    return tr
  }

  importSubMaterial(file){
    let formData: FormData = new FormData();
    formData.append('File', file);
    //formData.append('ProjectId', projectId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeSubMaterial/ImportSubMaterial', formData);
    return tr
  }

  importMaterialConsumable(file){
    let formData: FormData = new FormData();
    formData.append('File', file);
    //formData.append('ProjectId', projectId);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'PracticeMaterialConsumable/ImportMaterialConsumable', formData);
    return tr
  }
}
