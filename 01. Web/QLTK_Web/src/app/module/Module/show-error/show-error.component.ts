import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ModuleServiceService } from '../../services/module-service.service';

@Component({
  selector: 'app-show-error',
  templateUrl: './show-error.component.html',
  styleUrls: ['./show-error.component.scss']
})
export class ShowErrorComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private serviceError: ModuleServiceService,
  ) { }
  Id: string;
  isAction: boolean = false;
  modelError: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    ErrorGroupId: '',
    ErrorGroupName: '',
    ObjectId: '',
    DepartmentProcessId: '',
    DepartmentProcessName: '',
    DepartmentId: '',
    DepartmentName: '',
    Code: '',
    Subject: '',
    AuthorId: '',
    AuthorName: '',
    FixBy: '',
    FixByName: '',
    StageId: '',
    StageName: '',
    Description: '',
    Status: '',
    ErrorCost: '',
    CreateDate: ''
  }
  ngOnInit() {
    this.getErrorInfo();
  }

  getErrorInfo() {
    this.serviceError.getErrorInfo({ Id: this.Id }).subscribe(data => {
      this.modelError = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
