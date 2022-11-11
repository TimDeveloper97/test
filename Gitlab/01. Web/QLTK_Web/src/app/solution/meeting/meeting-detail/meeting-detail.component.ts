import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from 'src/app/project/service/customer.service';
import { ComboboxService, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { MeetingService } from '../../service/meeting.service';

@Component({
  selector: 'app-meeting-detail',
  templateUrl: './meeting-detail.component.html',
  styleUrls: ['./meeting-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MeetingDetailComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private combobox: ComboboxService,
    private customerService: CustomerService,
    private meetingService: MeetingService,
    public constant: Constants,
    public fileProcess: FileProcess,
    public dateUtils: DateUtils,
  ) {
  }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'Tên người liên hệ' }];
  columnNameMeetingType: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  listCustomer: any[] = [];
  listCustomerContactByCustomer: any[] = [];
  chars: any[] = ["I", "O"];
  listMeetingType: any[] = [];

  Id: any;
  model: any = {
    ListAttach: [],
    ListContent: [],
    ListCustomerContact: [],
    ListUser: []
  };
  modalInfo: any = {};

  ngOnInit(): void {
    this.modalInfo.Title = 'Thông tin Meeting';
    this.getListCustomer();
    this.getMeetingType();
    this.getById();
  }

  getById() {
    this.meetingService.getById(this.Id).subscribe(
      data => {
        this.model = data;
        this.getCustomerContactByCustomerId();
        if(this.model.MeetingDate)
        {
          this.model.MeetingDate = this.dateUtils.convertDateTimeToObject(this.model.MeetingDate);
        }
        
        this.model.ListContent.forEach(item => {
          item.FinishDate = this.dateUtils.convertDateToObject(item.FinishDate);
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomer() {
    this.combobox.getListCustomer().subscribe(
      data => {
        this.listCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getMeetingType() {
    this.combobox.getMeetingType().subscribe(
      data => {
        this.listMeetingType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerContactByCustomerId() {
    let customer = this.listCustomer.find(t => t.Id === this.model.CustomerId);
    if (customer) {
      this.model.CustomerName = customer.Name;
    }
    this.combobox.getCustomerContact(this.model.CustomerId).subscribe(
      data => {
        this.listCustomerContactByCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close(true);
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

}
