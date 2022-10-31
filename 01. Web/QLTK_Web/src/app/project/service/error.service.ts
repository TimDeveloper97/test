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
export class ErrorService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchErrorGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ErrorGroup/SearchErrorGroup', model, httpOptions);
    return tr
  }

  getErrorGroupInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ErrorGroup/GetErrorGroupInfo', model, httpOptions);
    return tr
  }

  createErrorGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ErrorGroup/AddErrorGroup', model, httpOptions);
    return tr
  }

  updateErrorGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ErrorGroup/UpdateErrorGroup', model, httpOptions);
    return tr
  }

  deleteErrorGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ErrorGroup/DeleteErrorGroup', model, httpOptions);
    return tr
  }

  //Error
  searchError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchError', model, httpOptions);
    return tr
  }

  searchErrorHistory(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchErrorHistory', model, httpOptions);
    return tr
  }

  searchChangedPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchChangedPlan', model, httpOptions);
    return tr
  }

  searchModule(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchModule', model, httpOptions);
    return tr
  }

  searchProject(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchProject', model, httpOptions);
    return tr
  }

  getErrorInfo(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/GetErrorInfo', model, httpOptions);
    return tr
  }

  createError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/AddError', model, httpOptions);
    return tr
  }

  updateError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateError', model, httpOptions);
    return tr
  }

  updateErrorConfirm(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateErrorConfirm', model, httpOptions);
    return tr
  }

  updateErrorPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateErrorPlan', model, httpOptions);
    return tr
  }

  updateErrorProcess(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateErrorProcess', model, httpOptions);
    return tr
  }

  updateErrorQC(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateErrorQC', model, httpOptions);
    return tr
  }

  deleteError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/DeleteError', model, httpOptions);
    return tr
  }

  confirmRequest(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/ConfirmRequest', model, httpOptions);
    return tr
  }

  //problem
  searchProblemExist(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/SearchProblemExist', model, httpOptions);
    return tr
  }

  exportExcel(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/ExportExcel', model, httpOptions);
    return tr
  }

  cancelRequest(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelRequest', model, httpOptions);
    return tr
  }

  confirm(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/Confirm', model, httpOptions);
    return tr
  }

  cancelConfirm(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelConfirm', model, httpOptions);
    return tr
  }

  confirmPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/ConfirmPlan', model, httpOptions);
    return tr
  }

  cancelConfirmPlan(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelConfirmPlan', model, httpOptions);
    return tr
  }

  completeProccessing(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CompleteProccessing', model, httpOptions);
    return tr
  }

  cancelCompleteProccessing(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelCompleteProccessing', model, httpOptions);
    return tr
  }

  cancelResultQC(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelResultQC', model, httpOptions);
    return tr
  }

  qcOK(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/QCOK', model, httpOptions);
    return tr
  }

  qcNG(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/QCNG', model, httpOptions);
    return tr
  }

  done(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/UpdateErrorDone', model, httpOptions);
    return tr
  }

  cancelDone(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelDone', model, httpOptions);
    return tr
  }

  closeError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CloseError', model, httpOptions);
    return tr
  }

  cancelCloseError(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Error/CancelCloseError', model, httpOptions);
    return tr
  }

}
