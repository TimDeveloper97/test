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
export class SummaryQuotesService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  getCustomerById(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetCustomerById?Id=' + Id, httpOptions);
    return tr
  }
  
  getCustomerRequire(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetCustomerRequire', httpOptions);
    return tr
  }

  getRequireByNumberYCKH(number): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetRequireByNumberYCKH?Number=' + number, httpOptions);
    return tr
  }
  getQuotesBySBU(SBUid): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotesBySBU?SBUid=' + SBUid, httpOptions);
    return tr
  }

  checkSoldQuotation(quotationId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/CheckSoldQuotation?quotationId=' + quotationId, httpOptions);
    return tr
  }

  createQuote(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/AddQuote', model, httpOptions);
    return tr
  }

  updateQuote(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/UpdateQuote', model, httpOptions);
    return tr
  }

  ChangeStatusQuotation(quotationId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeStatusQuotation?quotationId='+ quotationId, httpOptions);
    return tr
  }

  getQuotationById(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationById?Id=' + Id, httpOptions);
    return tr
  }

  getAllQuotationInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetAllQuotationInfo', model, httpOptions);
    return tr
  }

  getQuotationByCustomerId(model: any, CustomerId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationByCustomerId?CustomerId=' + CustomerId, model, httpOptions);
    return tr
  }

  deleteQuotation(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/DeleteQuotationByQuotationId?Id=' + Id, httpOptions);
    return tr
  }

  getFileInfor(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetFileInfor?Id=' + Id, httpOptions);
    return tr
  }

  changeIndustry(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeIndustry?Id=' + Id, httpOptions);
    return tr
  }

  changeModule(ObjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeModule?ObjectId=' + ObjectId, httpOptions);
    return tr
  }

  changeProduct(ObjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeProduct?ObjectId=' + ObjectId, httpOptions);
    return tr
  }

  changeSaleProduct(ObjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeSaleProduct?ObjectId=' + ObjectId, httpOptions);
    return tr
  }

  changeMaterial(ObjectId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ChangeMaterial?ObjectId=' + ObjectId, httpOptions);
    return tr
  }

  AddProduct(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/AddProduct', model, httpOptions);
    return tr
  }

  UpdateProduct(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/UpdateProduct', model, httpOptions);
    return tr
  }

  deleteProduct(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/DeleteProduct?Id=' + Id, httpOptions);
    return tr
  }

  getQuotationProduct(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationProduct?Id=' + Id, httpOptions);
    return tr
  }

  getQuotationProductInfor(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationProductInfor?Id=' + Id, httpOptions);
    return tr
  }

  getQuotationPlan(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationPlan?Id=' + Id, httpOptions);
    return tr
  }

  getEmployeeCharge(quotationId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetEmployeeCharge?quotationId=' + quotationId, httpOptions);
    return tr
  }

  getListEmployee(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetListEmployee', httpOptions);
    return tr
  }

  getGroupInTemplate(): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetGroupInTemplate', httpOptions);
    return tr
  }

  importFile(file, QuotationId): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('File', file);
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/QuotationProductImportFile?QuotationId=' + QuotationId, formData);
    return tr
  }

  getQuotesSelected(QuoteId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotesSelected?QuoteId=' + QuoteId, httpOptions);
    return tr
  }

  createQuotationPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/CreateQuotationPlan', model, httpOptions);
    return tr
  }

  updateQuotationPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/UpdateQuotationPlan', model, httpOptions);
    return tr
  }

  getQuotationPlanById(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/GetQuotationPlanById?Id=' + Id, httpOptions);
    return tr
  }

  DeleteQuotation(Id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/DeleteQuotation?Id=' + Id, httpOptions);
    return tr
  }

  exportExcel(model:any, QuotationId): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'SummaryQuotes/ExportExcel?QuotationId=' + QuotationId, model, httpOptions);
    return tr
  }
}
