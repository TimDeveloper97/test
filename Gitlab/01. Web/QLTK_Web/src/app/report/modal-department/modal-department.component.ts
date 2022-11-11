import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
  MessageService,
  Constants,
  AppSetting,
} from 'src/app/shared';
import { ReportSaleBussinessService } from '../service/report-sales-bussiness.service';

@Component({
  selector: 'app-modal-employee',
  templateUrl: './modal-department.component.html',
  styleUrls: ['./modal-department.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ModalDepartmentComponent implements OnInit {
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private reportSaleBussinessService: ReportSaleBussinessService
  ) {}
  listBase: any = [];
  listSelect: any = [];
  isAction: boolean = false;
  IsRequest: boolean;
  ListIdSelect: any = [];
  model: any = {
    Department: {},
  };
  listDepartment: any = [];
  ngOnInit() {
    this.getDepartment();
  }

  getDepartment() {
    this.reportSaleBussinessService.departments().subscribe(
      (data) => {
        this.listBase = data;
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      TotalItem: 0,

      Id: '',
      Name: '',
      listBase: [],
      ListIdSelect: [],
      ListIdChecked: [],
    };
    this.model.IsRequest = this.IsRequest;
  }

  choose() {
    this.listBase.forEach((element) => {
      if (element.Checked) {
        this.ListIdSelect.push(element.Code);
      }
    });
    this.activeModal.close({
      listIdSelect: this.ListIdSelect
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
