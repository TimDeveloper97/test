import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchCustomer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/SearchCustomer', model, httpOptions);
    return tr
  }

  getCustomerInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetCustomerInfo', model, httpOptions);
    return tr
  }

  getCustomerMeetings(model:any, id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetCustomerMeetings?id=' + id, model, httpOptions);
    return tr
  }

  getCustomerQuotation(model:any, id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetCustomerQuotation?id=' + id, model, httpOptions);
    return tr
  }

  getCustomerCode(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetCustomerCode',model, httpOptions);
    return tr
  }
  createCustomer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/AddCustomer', model, httpOptions);
    return tr
  }

  updateCustomer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/UpdateCustomer', model, httpOptions);
    return tr
  }

  deleteCustomer(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/DeleteCustomer', model, httpOptions);
    return tr
  }

  getGroupInTemplate(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetGroupInTemplate', httpOptions);
    return tr
  }

  importFile(file): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/ImportFile', formData);
    return tr
  }

  exportExcel(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/ExportExcel', model, httpOptions);
    return tr
  }
  
  checkDeleteCustomerContact(customerContactId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/CheckDeletes/'+ customerContactId, httpOptions);
    return tr
  }

  getCustomerContactInfo(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Customer/GetCustomerContact/'+ id, httpOptions);
    return tr
  }

  getListCustomerContact(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Customer/contact/'+ id, httpOptions);
    return tr
  }

  updateCustomerContact(model:any, id: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/UpdateCustomerContact/' + id, model, httpOptions);
    return tr
  }
  customerContactupdate(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/UpdateCustomerContact', model, httpOptions);
    return tr
  }

  uploadImage(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model',JSON.stringify(model) );
    formData.append('File' , file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
    return tr
  }
  
  searchCustomerProject(model:any, id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/SearchCustomerProject?id=' + id, model, httpOptions);
    return tr
  }

  getListCustomerRequiment(model:any, id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Customer/GetListCustomerRequirement?id=' + id, model, httpOptions);
    return tr
  }
}
