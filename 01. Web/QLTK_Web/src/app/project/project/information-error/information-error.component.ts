import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { ProjectErrorService } from '../../service/project-error.service';

@Component({
  selector: 'app-information-error',
  templateUrl: './information-error.component.html',
  styleUrls: ['./information-error.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InformationErrorComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ProjectErrorService
  ) { }

  Id: string;
  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Description: '',
    ErrorGroupId: '',
    ErrorGroupName: '',
    DepartmentId: '',
    DepartmentName: '',
    AuthorId: '',
    AuthorName: '',
    ErrorBy: '',
    PlanStartDate: '',
    ObjectId: '',
    ModuleErrorVisualId: '',
    DepartmentProcessId: '',
    StageId: '',
    FixBy: '',
    ProjectId: '',
    ProjectName: '',
    Status: '',
    Solution: '',
    Note: '',
    ErrorCost: '',
    ActualStartDate: '',
    ActualFinishDate: '',
    Type: null,
    strHistory: '',
    ListImage: [],
    ListFile: [],
  }

  ngOnInit() {
    if (this.Id) {
      this.getErrorInfo();
    }
  }

  getErrorInfo() {
    this.service.getErrorInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.activeModal.close();
  }
}
