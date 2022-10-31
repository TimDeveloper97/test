import { Injectable } from '@angular/core';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class CustomerRequirementService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  search(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/search', model, httpOptions);
    return tr
  }

  generateCode(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/GenerateCode', httpOptions);
    return tr
  }

  getDomain(customerId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/GetDomain?customerId=' + customerId, httpOptions);
    return tr
  }

  getCustomerContactById(customerId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/GetCustomerContactById?customerId=' + customerId, httpOptions);
    return tr
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/create', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-by-id/'+ id, httpOptions);
    return tr
  }

  getCustomerId(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/getCustomerId?id='+ id, httpOptions);
    return tr
  }

  update(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/update/'+id, model, httpOptions);
    return tr
  }

  updateCustomerRequirementContent(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/UpdateCustomerRequirementContent?id='+id, model, httpOptions);
    return tr
  }

  CreateCustomerRequirementContent(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/CreateCustomerRequirementContent?id='+id, model, httpOptions);
    return tr
  }

  delete(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/delete/'+ id, httpOptions);
    return tr
  }

  nextStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/next-status/'+id, httpOptions);
    return tr
  }
  nextDoubleStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/next-double-status/'+id, httpOptions);
    return tr
  }

  backStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/back-status/'+id, httpOptions);
    return tr
  }

  backDoubleStatus(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/back-double-status/'+id, httpOptions);
    return tr
  }

  getCustomerrequirementCode(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/getCustomerrequirementCode',model, httpOptions);
    return tr
  }

  nextStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/NextStep', model, httpOptions);
    return tr;
  }

  backStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/BackStep', model, httpOptions);
    return tr;
  }
  nextThreeStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/NextThreeStep', model, httpOptions);
    return tr;
  }
  backThreeStep(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/BackThreeStep', model, httpOptions);
    return tr;
  }

  checkDeleteSurvey(customerRequirementId): Observable<any> {
    var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'Expert/Checkdelete/'+ customerRequirementId);
    return tr
  }


  getProductNeedSolutionById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-product-need-solution/'+ id, httpOptions);
    return tr
  }

  getProductNeedPriceById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-product-need-price/'+ id, httpOptions);
    return tr
  }



  getCustomerRequirementProductSolutionById(model) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-customer-requirement-product-solution', model, httpOptions);
    return tr
  }


 
  createUpdate(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/createUpdateCustomerRequirementContent', model, httpOptions);
    return tr
  }
  deleteCustomerRequirementContent(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/deleteCustomerRequirementContent/'+ id, httpOptions);
    return tr
  }

  getRequestName(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/GetRequestName?id='+ id, httpOptions);
    return tr
  }

  SearchCustomerRequirementContentModelById(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/SearchCustomerRequirementContentModelById?id='+ model, httpOptions);
  return tr
  }

  getCustomerContact(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/GetCustomerContact', httpOptions);
    return tr
  }


  updateStatusSolution(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/update-status-solution', model,httpOptions);
    return tr
  }
  getMeetingByCustomerRequirementId(model) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-meeting-by-customer-requirement-id', model, httpOptions);
    return tr
  }
  getProjectByCustomerRequirementId(model) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-project-by-customer-requirement-id', model, httpOptions);
    return tr
  }
  getSolutionByCustomerId(model) : Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'customer-requirement/get-solution-by-customer-id', model, httpOptions);
    return tr
  }
}
