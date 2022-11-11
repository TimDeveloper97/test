import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class GeneralTemplateService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  //Biểu mẫu kiểm tra bản vẽ mạch in
  GeneralElectronic(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralElectronic', model, httpOptions);
    return tr
  }

  // Biểu mẫu kiểm tra nguyên lý
  GeneralPrinciples(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralPrinciples', model, httpOptions);
    return tr
  }

  // Biểu mẫu bản vẽ nguyên lý - Bảng tính toán
  GeneralPrinciplesCalculate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralPrinciplesCalculate', model, httpOptions);
    return tr
  }

  // Kiểm tra chất lượng sản phẩm mạch điện tử
  GeneralCheckElectronic(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralCheckElectronic', model, httpOptions);
    return tr
  }

  // Biểu mẫu hướng dẫn lắp ráp mạch điện tử
  GeneralElectronicCircuitAssembly(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralElectronicCircuitAssembly', model, httpOptions);
    return tr
  }

  // Biểu mẫu vật tư
  GeneralMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralMaterial', model, httpOptions);
    return tr
  }

  // Phương án thiết kế - Mô tả chung - Sơ đồ khối
  GeneralDesignOptions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralDesignOptions', model, httpOptions);
    return tr
  }

  // Phương án thiết kế - Danh mục khối chức năng
  GeneralFunctionDesignOptions(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralFunctionDesignOptions', model, httpOptions);
    return tr
  }

  // Phương án thiết kế - Linh kiện chính, thông số mạch
  GeneralFunctionDesignMaterial(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralFunctionDesignMaterial', model, httpOptions);
    return tr
  }

  // Hồ sơ điện
  GeneralElectronicRecord(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralElectronicRecord', model, httpOptions);
    return tr
  }

  // Hồ sơ cơ khí
  GeneralMechanicalRecord(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralMechanicalRecord', model, httpOptions);
    return tr
  }

  //Hồ sơ thiết kế
  GeneralDesignRecord(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralDesignRecord', model, httpOptions);
    return tr
  }

  // Danh mục kiểm tra
  GeneralCheckList(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralCheckList', model, httpOptions);
    return tr
  }

  // Xác nhận danh mục vật tư điện - điện tử
  GeneralConfirmElectronicRecord(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralConfirmElectronicRecord', model, httpOptions);
    return tr
  }

  // Bảng dữ liệu lập trình
  GeneralProgramableData(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralProgramableData', model, httpOptions);
    return tr
  }

  // Vẽ sơ đồ thuật toán điều khiển
  GeneralDrawControlAlgorithmModel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralDrawControlAlgorithmModel', model, httpOptions);
    return tr
  }

  //Danh sách hàm chức năng
  GeneralListFunction(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralListFunction', model, httpOptions);
    return tr
  }

  // Quá trình thử nghiệm
  GeneralTestProcess(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralTestProcess', model, httpOptions);
    return tr
  }
  // Danh mục thiết bị theo chức năng
  GeneralEquipmentByFunction(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralEquipmentByFunction', model, httpOptions);
    return tr
  }
  //Lập thông số kỹ thuật
  GeneralSetUpSpecification(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralSetUpSpecification', model, httpOptions);
    return tr
  }

  //Kiểm tra phương án thiết kế
  CheckDesignPlan(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/CheckDesignPlan', model, httpOptions);
    return tr
  }

  // Tạo phác thảo thiết kế
  GeneralDegignMechanical(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralDegignMechanical', model, httpOptions);
    return tr
  }

  // Dự toán sơ bộ
  GeneralPreliminaryEstimate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralPreliminaryEstimate', model, httpOptions);
    return tr
  }
  // upload Ảnh
  uploadImage(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model', JSON.stringify(model));
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
    return tr
  }
  //Hồ sơ thiết kế mạch điện tử
  GeneralProfileElectronicDesign(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralProfileElectronicDesign', model, httpOptions);
    return tr
  }

  //Tổng hợp thiết kế
  GeneralDesign(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GeneralDesign/GeneralDesign', model, httpOptions);
    return tr
  }

  GetCustomerByProjectId(ProjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GeneralDesign/GetCustomerByProjectId?ProjectId=' + ProjectId, httpOptions);
    return tr
  }

  GetModuleByProjectproductId(ProjectProductId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GeneralDesign/GetModuleByProjectproductId?ProjectProductId=' + ProjectProductId, httpOptions);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'GeneralDesign/ExpoetGeneralDesign', model, httpOptions);
    return tr
  }
  // kiểm tra nguyên lý điện
  GeneralCheckPrinciplesElectric(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralCheckPrinciplesElectric', model, httpOptions);
    return tr
  }

  // Dữ liệu cài đặt 
  GeneralDataProgramElectric(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/GeneralDataProgramElectric', model, httpOptions);
    return tr
  }

  // Hạng mục thiết kế
  GeneralDesignArticleElectric(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'General/DesignArticleElectric', model, httpOptions);
    return tr
  }
}
