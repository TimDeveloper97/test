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

export class QuestionService {

  constructor(private http: HttpClient,
    private config: Configuration) { }

  searchQuestionGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question-group/search', model, httpOptions);
    return tr;
  }

  createQuestionGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question-group/create', model, httpOptions);
    return tr;
  }

  updateQuestionGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question-group/update', model, httpOptions);
    return tr;
  }

  getInfoQuestionGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question-group/getQuestionGroupInfo', model, httpOptions);
    return tr;
  }

  deleteQuestionGroup(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question-group/delete', model, httpOptions);
    return tr;
  }

  // Gọi api câu hỏi

  searchQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question/search', model, httpOptions);
    return tr;
  }

  createQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question/create', model, httpOptions);
    return tr;
  }

  updateQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question/update', model, httpOptions);
    return tr;
  }

  getInfoQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question/getQuestionInfo', model, httpOptions);
    return tr;
  }

  deleteQuestion(model: any): Observable<any> {
    var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'question/delete', model, httpOptions);
    return tr;
  }

}
