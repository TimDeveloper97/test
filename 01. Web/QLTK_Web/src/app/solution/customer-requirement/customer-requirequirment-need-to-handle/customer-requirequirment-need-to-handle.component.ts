import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService } from 'src/app/shared';
import { CustomerRequirementService } from '../service/customer-requirement.service';


@Component({
  selector: 'app-customer-requirequirment-need-to-handle',
  templateUrl: './customer-requirequirment-need-to-handle.component.html',
  styleUrls: ['./customer-requirequirment-need-to-handle.component.scss']
})
export class CustomerRequirequirmentNeedToHandleComponent implements OnInit {

  ModalInfo = {
    Title: 'Thêm mới yêu cầu',
    SaveText: 'Lưu',
  };

  model: any = {
    Id: '',
    MeetingContentId: '',
    CustomerRequirementId: '',
    Code: '',
    Request: '',
    Solution: '',
    FinishDate: null,
    Note: '',
    RequestBy: '',
    RequestName: '',
    CreateDate: null,
    index: ''
  }
  columnCustomerContact: any[] = [{ Name: 'Name', Title: 'Tên' }, { Name: 'Email', Title: 'Email' }];
  createDate = null;
  finishDate = null;
  listContentModel: any[] = [];
  RequirementId: any;
  Id: any;
  index: -1;
  listCreateRequirementModel: any;
  Create: boolean;
  ListCustomerRequirement: any[] = [];
  listCustomerContact: any[] = [];

  listTemp: any[] = [];
  listCreate: any[] = [];

  constructor(
    private activeModal: NgbActiveModal,
    private customerRequirmentService: CustomerRequirementService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private dateUtils: DateUtils,

  ) { }

  ngOnInit(): void {
    this.Id = this.Id;
    if (this.Create == false) {
      this.model = this.model;
      this.ModalInfo.Title = "Cập nhật yêu cầu";
      this.getInfor();
    }
    else {
      this.ModalInfo.Title = "Thêm mới yêu cầu";
    }
    if(this.listCustomerContact)
    {
      this.listCustomerContact = this.listCustomerContact;
    }
  }

  getInfor() {
    if (this.Create == false && this.Id) {
      this.customerRequirmentService.SearchCustomerRequirementContentModelById(this.Id).subscribe(data => {
        this.model = data;
        if (this.model.CreateDate) {
          this.createDate = this.dateUtils.convertDateToObject(this.model.CreateDate);
        }
        if (this.model.FinishDate) {
          this.finishDate = this.dateUtils.convertDateToObject(this.model.FinishDate);
        }
      });
    }

    if (this.Create == false && !this.Id) {
        if (this.model.CreateDate) {
          this.createDate = this.model.CreateDate;
        }
        if (this.model.FinishDate) {
          this.finishDate = this.model.FinishDate;
        }
    }
    
  }

  closeModal() {
    this.activeModal.close();
    this.model = {
      Id: '',
      MeetingContentId: '',
      CustomerRequirementId: '',
      Code: '',
      Request: '',
      Solution: '',
      FinishDate: null,
      Note: '',
      RequestBy: '',
      RequestName: '',
      CreateDate: null,
      index: ''
    };
  }

  checkCreateDate = null;
  checkFinishDate = null;
  save() {
    if (this.Create == false) {
      if (this.createDate) {
        this.model.CreateDate = this.createDate;
        this.checkCreateDate = this.dateUtils.convertObjectToDate(this.createDate);
      }
      if (this.finishDate) {
        this.checkFinishDate = this.dateUtils.convertObjectToDate(this.finishDate);
        this.model.FinishDate = this.finishDate;
      }
      if (this.checkCreateDate > this.checkFinishDate) {
        this.messageService.showConfirm('Thời điểm hoàn thành phải lớn hơn ngày yêu cầu!');
      }
      else {
        this.activeModal.close({ modelTemp: this.model });
        this.model = {
          Id: '',
          MeetingContentId: '',
          CustomerRequirementId: '',
          Code: '',
          Request: '',
          Solution: '',
          FinishDate: null,
          Note: '',
          RequestBy: '',
          RequestName: '',
          CreateDate: null,
          index: ''
        };
      }
    }
    if (this.Create == true) {
      if (this.createDate) {
        this.model.CreateDate = this.createDate;
      }
      if (this.finishDate) {
        this.model.FinishDate = this.finishDate;
      }
      if (this.createDate > this.finishDate) {
        this.messageService.showConfirm('Thời điểm hoàn thành phải lớn hơn ngày yêu cầu!');
      }
      else {
        this.listCreate.push(this.model)
        this.model = {
          Id: '',
          MeetingContentId: '',
          CustomerRequirementId: '',
          Code: '',
          Request: '',
          Solution: '',
          FinishDate: null,
          Note: '',
          RequestBy: '',
          RequestName: '',
          CreateDate: null,
          index: ''
        };
        this.activeModal.close({ modelTemp: this.listCreate });
      }
    }

  }

  Request(Id) {
    if(Id)
    {
      this.customerRequirmentService.getRequestName(Id).subscribe(
        data => {
          this.model.RequestName = data.Name;
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else{
      this.model.RequestName = '';
    }
  }

}
