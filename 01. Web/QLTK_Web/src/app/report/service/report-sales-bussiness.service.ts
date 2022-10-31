import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root',
})
export class ReportSaleBussinessService {
  constructor(private http: HttpClient, private config: Configuration) {}

  salesTargetRegion(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/SalesTargetRegion',
      model,
      httpOptions
    );
    return tr;
  }

  salesJob(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/SalesJob',
      model,
      httpOptions
    );
    return tr;
  }

  salesIndustry(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/SalesIndustry',
      model,
      httpOptions
    );
    return tr;
  }

  salesApplication(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/SalesApplication',
      model,
      httpOptions
    );
    return tr;
  }
  departments(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/Departments',
      model,
      httpOptions
    );
    return tr;
  }
  salesDepartments(model: any): Observable<any> {
    var tr = this.http.post<any>(
      this.config.ServerWithApiUrl + 'ReportBussiness/SalesDepartment',
      model,
      httpOptions
    );
    return tr;
  }
}
