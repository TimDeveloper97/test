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

export class MeetingService {

  constructor(
    private http: HttpClient,
    private config: Configuration
  ) { }

  searchMeeting(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/SearchMeeting', model, httpOptions);
    return tr
  }

  searchMeetingFinish(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/SearchMeetingFinish', model, httpOptions);
    return tr
  }

  createMeeting(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/CreateMeeting', model, httpOptions);
    return tr
  }

  generateCode(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/GenerateCode', model, httpOptions);
    return tr
  }

  getById(id:any): Observable<any> {
    var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'Meeting/get-by-id/'+ id, httpOptions);
    return tr
  }

  update(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/update/'+id, model, httpOptions);
    return tr
  }

  update1(id, model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/update-customer-requirment/'+id, model, httpOptions);
    return tr
  }

  addCustomerRequirement(id,model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/add-customer-requirement/'+id, model, httpOptions);
    return tr
  }

  delete(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/delete/'+ id, httpOptions);
    return tr
  }

  doMeeting(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/do-meeting/'+id, httpOptions);
    return tr
  }

  cancelMeeting(model): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/cancel-meeting', model, httpOptions);
    return tr
  }

  finishMeeting(id): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/finish-meeting/'+id, httpOptions);
    return tr
  }

  getMeetingCustomerContactInfo(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/getMeetingCustomerContactInfo', model, httpOptions);
    return tr
  }

  exportExcel(id,model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/ExportExcel/'+id, model, httpOptions);
    return tr;
  }

  createUpdateMeetingRequimentNeedHandle(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/UpdateMeetingRequimentNeedHandle', model, httpOptions);
    return tr
  }

  deleteMeetingRequimentNeedHandle(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/DeleteMeetingRequimentNeedHandle', model, httpOptions);
    return tr
  }

  createUpdateMeetingFileRequimentNeedHandle(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/UpdateMeetingFileRequimentNeedHandle', model, httpOptions);
    return tr
  }

  deleteMeetingFileRequimentNeedHandle(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/DeleteMeetingFileRequimentNeedHandle', model, httpOptions);
    return tr
  }

  getRequimentContent(id:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/GetRequimentContent/'+id, httpOptions);
    return tr
  }

  createMeetingCustomerContact(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/CreateMeetingCustomerContact', model, httpOptions);
    return tr
  }
  uploadImage(file: any, model): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('Model',JSON.stringify(model) );
    formData.append('File' , file);
    var tr = this.http.post<any>(this.config.ServerFileApiUrl + 'HandlingImage/UploadImage', formData);
    return tr
  }

  createCustomerRequimentMeetingContent(model:any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Meeting/CreateCustomerRequimentMeetingContent', model, httpOptions);
    return tr
  }
}
