import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerContactService } from 'src/app/project/service/customer-contact.service';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { AddContactCustomerMeetingComponent } from '../add-contact-customer-meeting/add-contact-customer-meeting.component';

@Component({
  selector: 'app-choose-customer-contact',
  templateUrl: './choose-customer-contact.component.html',
  styleUrls: ['./choose-customer-contact.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseCustomerContactComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public constant: Constants,
    private customerContact: CustomerContactService
  ) { }

  listCustomerContact = [];

  listCustomerContactSelect = [];
  listCustomerContactId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

  modelSearch: any = {
    Name: '',
    CustomerContactId: '',
    ListCustomerContactId: [],
  }

  searchOptions = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên người liên hệ ...',
    Items: [
      {
        Name: 'Công ty',
        FieldName: 'CustomerId',
        Placeholder: 'Công ty',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã công ty' }, { Name: 'Name', Title: 'Tên công ty' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  }
  checkeds: boolean=false;

  
  CustomerId ='';
  ngOnInit() {
    this.modelSearch.ListCustomerContactId = this.listCustomerContactId;
    this.modelSearch.Status = 1;
    this.modelSearch.CustomerId =this.CustomerId;
    this.searchCustomerContact(); 
    this.checkeds =false;
  }

  imagePath: string;
  searchCustomerContact() {
    this.listCustomerContactSelect.forEach(item => {
      this.modelSearch.ListCustomerContactId.push(item.Id);
    })
    this.customerContact.searchCustomerContact(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listCustomerContact = data.ListResult;
        this.listCustomerContact.forEach((element, index) => {
          element.Index = index + 1;
          element.Checked =false;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  // checkAll(isCheck: any) {
  //   if (isCheck) {
  //     this.listCustomerContact.forEach(element => {
  //       if (this.checkedTop) {
  //         element.Checked = true;
  //       } else {
  //         element.Checked = false;
  //       }
  //     });
  //   } else {
  //     this.listCustomerContactSelect.forEach(element => {
  //       if (this.checkedBot) {
  //         element.Checked = true;
  //       } else {
  //         element.Checked = false;
  //       }
  //     });
  //   }
  // }

  // addRow() {
  //   this.listCustomerContact.forEach(element => {
  //     if (element.Checked) {
  //       element.Quantity = 1;
  //       this.listCustomerContactSelect.push(element);
  //     }
  //   });
  //   this.listCustomerContactSelect.forEach(element => {
  //     var index = this.listCustomerContact.indexOf(element);
  //     if (index > -1) {
  //       this.listCustomerContact.splice(index, 1);
  //     }
  //   });
  // }

  // removeRow() {
  //   this.listCustomerContactSelect.forEach(element => {
  //     if (element.Checked) {
  //       this.listCustomerContact.push(element);
  //     }
  //   });
  //   this.listCustomerContact.forEach(element => {
  //     var index = this.listCustomerContactSelect.indexOf(element);
  //     if (index > -1) {
  //       this.listCustomerContactSelect.splice(index, 1);
  //     }
  //   });
  // }

  choose() {
    this.listCustomerContact.forEach(element => {
      if(element.Checked ==true){
        this.listCustomerContactSelect.push(element);
      }
    });
    this.activeModal.close(this.listCustomerContactSelect);
  }

  clear() {
    this.modelSearch = {
      Name: '',
      CustomerContactId: '',
      ListCustomerContactId: [],
    }
    this.searchCustomerContact();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showCreateUpdates(Id: string) {
    let activeModal = this.modalService.open(AddContactCustomerMeetingComponent, { container: 'body', windowClass: 'productgroupcreate-model', backdrop: 'static' })
    activeModal.componentInstance.CustomerId = this.CustomerId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchCustomerContact(); 
      }
    }, (reason) => {
    });
  }


  selectAllFunction() {
    this.listCustomerContact.forEach(element => {
      element.Checked = this.checkeds;
    });
  }



}
