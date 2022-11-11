import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-problem-exist-history',
  templateUrl: './problem-exist-history.component.html',
  styleUrls: ['./problem-exist-history.component.scss']
})
export class ProblemExistHistoryComponent implements OnInit {

  @Input() Id: string;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private errorHistoryService: ErrorService,
    public constants: Constants,
    private router: Router,
  ) { }

  listErrorHistory: any[] = [];
  model: any = {
    Id: '',
    ErrorId: '',
  }
  ngOnInit() {
    this.model.ErrorId = this.Id;
    this.searchErrorHistory();
  }


  searchErrorHistory() {
    this.errorHistoryService.searchErrorHistory(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listErrorHistory = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-van-de']);
  }

}
