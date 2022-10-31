import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Configuration } from '../../shared';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private http: HttpClient, private config: Configuration) { }

  login(loginData: any): Observable<any> {
    var data = "grant_type=password&username=" + loginData.username + "&password=" + loginData.password + "&client_id=" + this.config.ClientId+"&client_secret="+this.config.ClientSecret;

    return this.http.post<any>(this.config.ServerApi + 'token', data, httpOptions);
  }

  ChangePassword(model:any): Observable<any> {
    return this.http.post<any>(this.config.ServerWithApiUrl + 'USER/ChangePassword', model, httpOptionsJson);      
  } 

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('qltkcurrentUser');
  }

  /**
    * Handle Http operation that failed.
    * Let the app continue.
    * @param operation - name of the operation that failed
    * @param result - optional value to return as the observable result
  */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead


      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

}