import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService, FileProcess, Configuration } from 'src/app/shared';
import { CustomerService } from '../../service/customer.service';

@Component({
  selector: 'app-customer-contact-create',
  templateUrl: './customer-contact-create.component.html',
  styleUrls: ['./customer-contact-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerContactCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    public CustomerService: CustomerService,
    public fileProcess: FileProcess,
    private config: Configuration,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới người liên hệ',
    SaveText: 'Lưu',
  };
  listCreate: any[] = [];
  listExpert: any[] = [];
  isAction: boolean = false;
  Id: string;
  customerId: string;
  row: any = {};
  isAdd = false;
  model: any = {
    Id: '',
    CustomerId: '',
    Name: '',
    Address: '',
    PhoneNumber: '',
    Note: '',
    Status: 1,
    Gender: '1',
    Avatar: '',
    customerAvatar:'',
  }


  listData: any[] = []
  listTemp: any[] = [];
  dateOfBirth = null;

  ngOnInit() {
    if (this.isAdd == false) {
      this.model = this.row;
      if (this.model.Avatar) {
        this.model.customerAvatar = this.config.ServerFileApi + this.model.Avatar;
      }
      this.ModalInfo.Title = 'Chỉnh sửa người liên hệ';
      this.ModalInfo.SaveText = 'Lưu';
      this.dateOfBirth = this.dateUtils.convertDateToObject(this.model.DateOfBirth)
    } else if (this.customerId) {
      this.ModalInfo.Title = 'Thêm người liên hệ';
    }
    else {
      this.Id = '';
      this.ModalInfo.Title = 'Thêm mới người liên hệ';
    }
  }

  isCheck = true;
  save(isContinue: boolean) {

    if (this.dateOfBirth) {
      this.model.DateOfBirth = this.dateUtils.convertObjectToDate(this.dateOfBirth)
    }

    if (this.customerId) {
      let modelFile = {
        FolderName: 'CustomerContact/'
      };
      if (this.fileProcess.FileDataBase != null) {
        this.CustomerService.uploadImage(this.fileProcess.FileDataBase, modelFile).subscribe(
          data => {
            this.model.Avatar = data.FileUrl;
            this.updateCustomerContact();
          },
          error => {
            this.messageService.showError(error);
          });
          this.activeModal.close({ isAdd: false, modelTemp: this.model });
      } else {
        this.updateCustomerContact();
      }
    }

    else {
      this.activeModal.close({ isAdd: false, modelTemp: this.model });
      this.messageService.showSuccess('Cập nhật người liên hệ!');
    }

  }


  updateCustomerContact() {
    this.CustomerService.updateCustomerContact(this.model, this.customerId).subscribe(
      data => {
        this.activeModal.close({ isAdd: false, modelTemp: this.model });
        this.messageService.showSuccess('Cập nhật người liên hệ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  closeModal(isOK: boolean) {
    this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
    if ($event.target.files && $event.target.files.length > 0) {
      let files = $event.target.files;
      let reader = new FileReader();
      reader.readAsDataURL(files[0]);
      
          reader.onload = () => {
              var filer = {
                  Name: files[0].name,
                  DataURL: reader.result,
                  Size: files[0].size
              }
              this.model.customerAvatar = filer.DataURL;
          };
      }
  }
}
