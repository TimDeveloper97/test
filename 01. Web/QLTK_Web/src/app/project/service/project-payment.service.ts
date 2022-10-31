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
export class ProjectPaymentService {

  constructor(
    private http: HttpClient,
    private config: Configuration,
  ) { }
  UpdatePayment(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/UpdatePayment', model, httpOptions);
  return tr
  }

  DeletePayment(id:any): Observable<any>{
    var url = this.config.ServerWithApiUrl + 'Payment/DeleteById?Id=';
    var tr = this.http.post<any>(url+ id, httpOptions);
  return tr
  }


  SearchPayment(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/SearchPaymentModel?projectId='+ model, httpOptions);
  return tr
  }

  GetTotalActualAmount(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/GetTotal?projectId='+ model, httpOptions);
  return tr
  }

  SearchPaymentById(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/SearchPaymentModelById', model, httpOptions);
  return tr
  }
  GetPaymentByPlanId(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/GetPaymentByPlanId?PlanId='+ model, httpOptions);
  return tr
  }
  UpdatePlanPayment(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/UpdatePlanPayment', model, httpOptions);
  return tr
  }
  UpdatePlanDate(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/UpdatePlanDate', model, httpOptions);
  return tr
  }
  GetPlanByPaymentId(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/GetPlanByPaymentId?PaymentId='+ model, httpOptions);
  return tr
  }
  DeletePlanById(model:any): Observable<any>{
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/DeletePlanById?PlanId='+ model, httpOptions);
  return tr
  }
  UpdatePlanPaymentDate(idProject: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Payment/UpdatePlanPayment?projectId=' + idProject, httpOptions);
    return tr;
}
}
