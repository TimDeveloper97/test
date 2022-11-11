import { Component, OnInit, Input } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ErrorService } from 'src/app/project/service/error.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-project-error-history',
  templateUrl: './project-error-history.component.html',
  styleUrls: ['./project-error-history.component.scss']
})
export class ProjectErrorHistoryComponent implements OnInit {

  @Input() Id: string;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private errorHistoryService: ErrorService,
    public constants: Constants,
    private activeModal: NgbActiveModal,
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
    this.activeModal.close(true);
  }

}
