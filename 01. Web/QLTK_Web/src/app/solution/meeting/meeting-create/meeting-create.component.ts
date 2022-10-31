import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { NgbActiveModal, NgbModal, ModalDismissReasons, } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';
import { DateTimeAdapter } from 'ng-pick-datetime';
import { forkJoin } from 'rxjs';

import { CustomerService } from 'src/app/project/service/customer.service';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';
import { AppSetting, ComboboxService, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { MeetingService } from '../../service/meeting.service';
import { AddContactCustomerMeetingComponent } from '../add-contact-customer-meeting/add-contact-customer-meeting.component';
import { ChooseCustomerContactComponent } from '../choose-customer-contact/choose-customer-contact.component';
import { EndMeetingComponent } from '../end-meeting/end-meeting.component';
import { MeetingCreateRequirmentModelComponent } from '../meeting-create-requirment-model/meeting-create-requirment-model.component';

@Component({
  selector: 'app-meeting-create',
  templateUrl: './meeting-create.component.html',
  styleUrls: ['./meeting-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MeetingCreateComponent implements OnInit {
  constructor(
    private routeA: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    private combobox: ComboboxService,
    private customerService: CustomerService,
    private meetingService: MeetingService,
    public constant: Constants,
    private modalService: NgbModal,
    public fileProcess: FileProcess,
    public dateUtils: DateUtils,
    private config: Configuration,
    private uploadService: UploadfileService,
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
  ) {
  }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'Tên người liên hệ' }];
  columnNameMeetingType: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  listCustomer: any[] = [];
  listCustomerContactByCustomer: any[] = [];
  chars: any[] = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
  listMeetingType: any[] = [];
  listContent: any[] = [];
  isAction: boolean = false;

  listCustomerContact: any[] = [];

  Id: any = '';
  selectIndex = -1;
  IdCheck: null;
  model: any = {
    Status: 0,
    Time: 0,
    ListAttach: [],
    ListContent: [],
    ListCustomerContact: [],
    ListUser: [],
    ListAttachPerformStep: []
  };


  cancelModel: any = {
    Id: '',
    ReasonCancel: ''
  };

  message: any;
  indexNumber: number = 0;
  modalInfo: any = {};
  listUserId: any[] = [];
  listCustomerContactId: any[] = [];

  fileTemplate: any = {
    Id: '',
    Note: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: '',
    MeetingId:'',
    MeetingContentId:''
  };

  contentModel: any = {
    Request: '',
    Solution: '',
    FinishDate: null,
    Note: '',
    Code: '',
    Checked: false,
  };
  modelMeetingContact: any = {
    Name: '',
    Position: '',
    Company: '',
    Phone: '',
    Email: ''
  };

  meetingCustomerContactModel: any = {
    CustomerContactId: '',
    MeetingId: ''
  };

  meetingUserModel: any = {
    UserId: '',
    MeetingId: ''
  };

  MeetingContentAttachs: any[];
  SelectIndex = -1;

  CheckChoose: boolean = false;
  nameFile = "";
  fileToUpload: File = null;
  startIndex = 0;
  listData: any[] = [];
  MeetingContentId: '';
  ListContentCheck : any[] =[];

  ngOnInit(): void {
    this.appSetting.PageTitle = " THÊM MỚI MEETING, CHỈNH SỬA MEETING"
    this.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getListCustomer();
    this.getMeetingType();
    this.searchMeeting('');
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa Meeting';
      this.modalInfo.SaveText = 'Lưu';
      this.appSetting.PageTitle = "CHỈNH SỬA MEETING"
      this.getById();
    }
    else {
      this.model.Step = 0;
      this.modalInfo.Title = 'Thêm mới Meeting';
      this.modalInfo.SaveText = 'Lưu';
      this.appSetting.PageTitle = " THÊM MỚI MEETING"
    }

  }

  getById() {
    this.meetingService.getById(this.Id).subscribe(
      data => {
        this.model = data;
        this.model.ListContent.forEach(item =>{
          item.status =item.Checked;
        });
        this.select(this.constant.MeetingStep[this.model.Step], this.model.Step);
        this.getCustomerContactByCustomerId();
        if (this.model.MeetingDate) {
          this.model.MeetingDate = this.dateUtils.convertDateTimeToObject(this.model.MeetingDate);
        }

        // this.model.ListContent.forEach(item => {
        //   item.FinishDate = this.dateUtils.convertDateToObject(item.FinishDate);
        // });

        // this.model.ListContent.forEach(item => {
        //   item.CreateDate = this.dateUtils.convertDateToObject(item.CreateDate);
        // });


        this.model.ListUser.forEach(item => {
          this.listUserId.push(item.UserId);
        });

        this.model.ListCustomerContact.forEach(item => {
          this.listCustomerContactId.push(item.CustomerContactId);
        });

        this.MeetingContentAttachs = [];
        this.model.ListContent.forEach(element =>{
          element.MeetingContentAttaches.forEach(e=>{
          this.MeetingContentAttachs.push(e);
        })
      });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  generateCode() {
    this.meetingService.generateCode({ CodeChar: this.model.CodeChar }).subscribe((data: any) => {
      if (data) {
        this.model.Code = data.Code;
        this.model.Index = data.Index;
      }
    });
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
  getMeetingTypeCode() {
    this.getMeetingType();
    let meeting = this.listMeetingType.find(m => m.Id === this.model.MeetingTypeId);
    let number = '';
    if (meeting != null) {
      meeting.Index = meeting.Index + 1;
      this.model.Index = meeting.Index;
      this.model.CodeChar = meeting.Code;
      let index = '' + meeting.Index;
      for (let i = index.length; i < 4; i++) {
        number = number + '0'
      }
      if (!this.Id) {
        this.model.Code = meeting.Code + number + meeting.Index;
      }
    }

  }

  getCustomerContactInfo() {
    this.customerService.getCustomerContactInfo(this.model.CustomerContactId).subscribe(
      data => {
        this.model.Email = data.Email;
        this.model.PhoneNumber = data.PhoneNumber;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  saveV() {
    // this.showConfirmCancelMeeting();
    this.cancelMeeting();
    // this.closeModal(true);
    this.close();

  }

  saveModal() {
    this.save(false);
  }

  saveAll() {
    this.save(false);
    this.router.navigate(['giai-phap/meeting']).then((result) => {
      if (result) {
        this.searchMeeting('');
      }
      this.searchMeeting('');
    }
    );
  }

  save(isExport : boolean) {
    let check = this.valid();
    let checkPerformStep = this.validCheckAttachPerformStep();
    if (check && checkPerformStep) {
      var listFileUpload = [];
      this.model.ListAttach.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUpload.push(document.File);
        }
      });

      var listFileUploadPerformStep = [];
      this.model.ListAttachPerformStep.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUploadPerformStep.push(document.File);
        }
      });
      if (this.model.MeetingDate) {
        this.model.MeetingDate = this.dateUtils.convertObjectToDate(this.model.MeetingDate);
      }
      this.model.ListContent.forEach(item => {
        item.FinishDate = this.dateUtils.convertObjectToDate(item.FinishDate);
      });

      this.model.ListContent.forEach(item => {
        item.CreateDate = this.dateUtils.convertObjectToDate(item.CreateDate);
      });
      
      if (listFileUpload.length > 0 || listFileUploadPerformStep.length > 0) {
        let fileAttachs = this.uploadService.uploadListFile(listFileUpload, 'Meeting/');
        let fileAttachPerforms = this.uploadService.uploadListFile(listFileUploadPerformStep, 'Meeting/');
        forkJoin([fileAttachs, fileAttachPerforms]).subscribe(results => {
          var count = 0;
          let data1 = results[0];
          let data2 = results[1];
          if (data1 && data1.length > 0) {
            count++;
            listFileUpload.forEach((item, index) => {
              this.model.ListAttach[item.index].FilePath = data1[index].FileUrl;
            });
          }
          if (data2 && data2.length > 0) {
            count++;
            listFileUploadPerformStep.forEach((item, index) => {
              this.model.ListAttachPerformStep[item.index].FilePath = data2[index].FileUrl;
            });
          }
          if (count > 0) {
            if (this.Id) {
              this.update(isExport);
              this.getById();
            }
            else {
              this.create();

            }
          }
        });
      }
      else {
        if (this.Id) {
          this.update(isExport)
          this.getById();
        }
        else {
          this.create();
        }
      }
    }
    else {
      this.messageService.showMessage(this.message);
    }
    // this.router.navigate(['giai-phap/meeting']);
  }

  valid(): boolean {
    let check = true;
    // this.model.ListAttach.forEach(item => {
    //   if (check) {
    //     if (!item.Name) {
    //       this.message = 'Chưa nhập đầy đủ tên tài liệu';
    //       check = false;
    //     }
    //   }
    // });
    return check;
  }

  validCheckAttachPerformStep(): boolean {
    let check = true;
    this.model.ListAttachPerformStep.forEach(item => {
      if (check) {
        if (!item.Name) {
          this.message = 'Chưa nhập đầy đủ tên tài liệu';
          check = false;
        }
      }
    });
    return check;
  }

  create() {
    this.meetingService.createMeeting(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới meeting thành công!');
        // this.close();
        // this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update(isExport : boolean) {
    this.meetingService.update(this.Id, this.model).subscribe(
      data => {
        if(!isExport){
          this.messageService.showSuccess('Cập nhật meeting thành công!');
        }else{
          if (this.model.MeetingDate) {
            this.model.MeetingDate = this.dateUtils.convertObjectToDate(this.model.MeetingDate);
          }
          this.meetingService.exportExcel(this.Id, this.model).subscribe(d => {
            var link = document.createElement('a');
            link.setAttribute("type", "hidden");
            link.href = this.config.ServerApi + d;
            link.download = 'Download.xlsx';
            document.body.appendChild(link);
            // link.focus();
            link.click();
            document.body.removeChild(link);
          }, e => {
            this.messageService.showError(e);
          });
          this.getById();
        }
        // this.close();
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.searchMeeting('');

  }

  close() {


    this.router.navigate(['giai-phap/meeting']);
    this.searchMeeting('');
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });
    this.model.ListUser.forEach(element => {
      this.listUserId.push(element.Id);
    });

    activeModal.componentInstance.listEmployeeId = this.listUserId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsNew = true;
          this.model.ListUser.push(element);
        });
        var j = 1;
        this.model.ListUser.forEach(element => {
          element.index = j++;
        });
      }
    }, (reason) => {

    });
  }

  showComfirmDeleteEmployee(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên tham dự này không?").then(
      data => {
        this.model.ListUser.splice(index, 1);
      },
      error => {

      }
    );
  }

  showSelectCustomerContact() {
    let activeModal = this.modalService.open(ChooseCustomerContactComponent, { container: 'body', windowClass: 'choose-customer-contact-modal', backdrop: 'static' });
    this.model.ListCustomerContact.forEach(element => {
      this.listCustomerContactId.push(element.Id);
    });

    activeModal.componentInstance.listCustomerContactId = this.listCustomerContactId;
    activeModal.componentInstance.CustomerId = this.model.CustomerId;
    activeModal.componentInstance.listCustomerContactSelect = this.model.ListCustomerContact;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsNew = true;
        });
        var j = 1;
        this.model.ListCustomerContact.forEach(element => {
          element.index = j++;
        });
        var model ={
          MeetingId : this.Id,
          ListCustomerContact: this.model.ListCustomerContact,
        }
        // this.meetingService.createUpdateCustomerContacts(model).subscribe(
        //   data =>{
        //     this.getById();
        //   },
        //   error =>{
    
        //   }
        // );
      }
    }, (reason) => {

    });
  }

  showComfirmDeleteCustomerContact(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người liên hệ tham dự này không?").then(
      data => {
        this.model.ListCustomerContact.splice(index, 1);
      },
      error => {

      }
    );
  }

  uploadFile($event: any, type: any) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    var list;
    if (type == 0) {
      list = this.model.ListAttach;
    }
    else if (type == 2) {
      list = this.MeetingContentAttachs;
    }
    else {
      list = this.model.ListAttachPerformStep;
    }
    for (var file of fileDataSheet) {
      isExist = false;
      if (list != null) {
        for (var ite of list) {
          if (ite.Id != null) {
            if (file.name == ite.FileName) {
              isExist = true;
            }
          }
          else {
            if (file.name == ite.name) {
              isExist = true;
            }
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, type);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, type);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, type);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace, type) {
    var isExist = false;
    let docuemntTemplate;
    if (type == 0) {
      for (var file of fileManualDocuments) {
        for (let index = 0; index < this.model.ListAttach.length; index++) {

          if (this.model.ListAttach[index].Id != null) {
            if (file.name == this.model.ListAttach[index].FileName) {
              isExist = true;
              if (isReplace) {
                this.model.ListAttach.splice(index, 1);
              }
            }
          }
          else if (file.name == this.model.ListAttach[index].name) {
            isExist = true;
            if (isReplace) {
              this.model.ListAttach.splice(index, 1);
            }
          }
        }

        if (!isExist || isReplace) {
          docuemntTemplate = Object.assign({}, this.fileTemplate);
          docuemntTemplate.File = file;
          docuemntTemplate.FileName = file.name;
          docuemntTemplate.FileSize = file.size;
          this.model.ListAttach.push(docuemntTemplate);
        }
      }

    } else if (type == 2) {
      for (var file of fileManualDocuments) {
        for (let index = 0; index < this.MeetingContentAttachs.length; index++) {

          if (this.model.ListAttach[index].Id != null) {
            if (file.name == this.MeetingContentAttachs[index].FileName) {
              isExist = true;
              if (isReplace) {
                this.MeetingContentAttachs.splice(index, 1);
              }
            }
          }
          else if (file.name == this.MeetingContentAttachs[index].name) {
            isExist = true;
            if (isReplace) {
              this.MeetingContentAttachs.splice(index, 1);
            }
          }
          this.MeetingContentAttachs.forEach((files) =>{
            let fileMeetingContentAttachs = this.uploadService.uploadListFile(files, 'Meeting/');
            forkJoin([fileMeetingContentAttachs]).subscribe(results => {
              let data1 = results[0];
              if (data1 && data1.length > 0) {
                files.forEach((item, index) => {
                  this.model.ListContent[files[0].indexContent].MeetingContentAttaches[item.index].FilePath = data1[index].FileUrl;
                });
              }
            });
          });
        }

        if (!isExist || isReplace) {
          docuemntTemplate = Object.assign({}, this.fileTemplate);
          docuemntTemplate.File = file;
          docuemntTemplate.FileName = file.name;
          docuemntTemplate.FileSize = file.size;
          this.MeetingContentAttachs.push(docuemntTemplate);
        }
      }
    } else {
      for (var file of fileManualDocuments) {
        for (let index = 0; index < this.model.ListAttachPerformStep.length; index++) {

          if (this.model.ListAttach[index].Id != null) {
            if (file.name == this.model.ListAttachPerformStep[index].FileName) {
              isExist = true;
              if (isReplace) {
                this.model.ListAttachPerformStep.splice(index, 1);
              }
            }
          }
          else if (file.name == this.model.ListAttachPerformStep[index].name) {
            isExist = true;
            if (isReplace) {
              this.model.ListAttachPerformStep.splice(index, 1);
            }
          }
        }

        if (!isExist || isReplace) {
          docuemntTemplate = Object.assign({}, this.fileTemplate);
          docuemntTemplate.File = file;
          docuemntTemplate.FileName = file.name;
          docuemntTemplate.FileSize = file.size;
          this.model.ListAttachPerformStep.push(docuemntTemplate);
        }
      }
    }

  }

  updateFileManualDocument(files, type) {
    let documentTemplate;
    for (var file of files) {
      documentTemplate = Object.assign({}, this.fileTemplate);
      documentTemplate.File = file;
      documentTemplate.FileName = file.name;
      documentTemplate.FileSize = file.size;
      documentTemplate.CreateDate = new Date();
      if (type == 0) {
        this.model.ListAttach.push(documentTemplate);
      }
      else
        this.model.ListAttachPerformStep.push(documentTemplate);

    }
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  showConfirmDeleteFilePerformStep(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteFilePerformStep(document, index);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileMeetingContentAttach(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        var model ={
          Id:this.MeetingContentAttachs[index].Id
        }
        this.meetingService.deleteMeetingFileRequimentNeedHandle(model).subscribe(
          data =>{
            this.MeetingContentAttachs.splice(index, 1);
            // this.getById();
          },
          error =>{
    
          }
        );
      },
      error => {

      }
    );
  }

  deleteFilePerformStep(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.model.ListAttachPerformStep.splice(index, 1);
    }
  }

  deleteFileMeetingContentAttach(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.model.ListContent.MeetingContentAttaches.splice(index, 1);
    }
  }

  showConfirmDeleteFile(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteFile(document, index);
      },
      error => {

      }
    );
  }

  deleteFile(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.model.ListAttach.splice(index, 1);
    }
  }




  deleteContent(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nội dung này không?").then(
      data => {
        var model ={
          Id:this.model.ListContent[index].Id
        }
        this.meetingService.deleteMeetingRequimentNeedHandle(model).subscribe(
          data =>{
            // this.getById();
            this.model.ListContent.splice(index, 1);
          },
          error =>{
    
          }
        );
      },
      error => {

      }
    );
  }

  addRow() {
    if (!this.contentModel.Request) {
      this.messageService.showMessage("Bạn không được để trống yêu cầu!");
    }
    else {
      var row = Object.assign({}, this.contentModel);
      row.Request = this.contentModel.Request;
      row.Solution = this.contentModel.Solution;
      row.FinishDate = this.contentModel.FinishDate;
      row.Note = this.contentModel.Note;
      row.Code = this.contentModel.Code;
      row.Checked = this.contentModel.Checked;
      this.model.ListContent.push(row);
      this.contentModel = {
        Request: '',
        Solution: '',
        FinishDate: null,
        Note: '',
        Code: '',
        Checked: false,
      }
    }
    this.save(false);
  }

  select(item: any, index: number) {
    if (item) {
      if (item.Id <= this.model.Step) {
        this.indexNumber = index;
      }
    }
  }

  meetingTimeChange() {
    if (this.model.StartTime && this.model.StartTime.minute >= 0 && this.model.Time >= 0) {
      let minute = Number(this.model.StartTime.minute) + Number(this.model.Time);
      let hourBonus = Math.floor(minute / 60);
      let hourEnd = this.model.StartTime.hour + hourBonus;
      let minuteEnd = Number(minute % 60);
      this.model.EndTime = { hour: hourEnd, minute: minuteEnd, second: this.model.StartTime.second };
    }

  }

  showConfirmDoMeeting() {
    this.messageService.showConfirm("Bạn có chắc muốn thực hiện meeting này không?").then(
      data => {
        this.doMeeting();
      },
      error => {

      }
    );
  }

  doMeeting() {
    this.meetingService.doMeeting(this.Id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật meeting thành công!');
        this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmFinish(status) {
    var mess ='';
    if(status ==0){
      mess ='Bạn có chắc muốn kết thúc meeting này không?';
        this.messageService.showConfirm(mess).then(
          data => {
            this.addCustomerRequirement();

            this.model.ListContent.forEach(a =>{
              if(a.Checked==true){
                this.ListContentCheck.push(a);
              }
            });
            if(this.ListContentCheck.length >0){
              let activeModal = this.modalService.open(EndMeetingComponent, { container: 'body', windowClass: 'end-meeting', backdrop: 'static' });
              activeModal.componentInstance.requirementModel.MeetingCode = this.model.Code;
              activeModal.componentInstance.requirementModel.CustomerId = this.model.CustomerId;
              activeModal.componentInstance.requirementModel.CustomerContactId = this.model.CustomerContactId;
              activeModal.componentInstance.requirementModel.Code = this.ListContentCheck[0].Code;
              activeModal.componentInstance.requirementModel.Name = this.ListContentCheck[0].Request;
              activeModal.componentInstance.requirementModel.ListContent = this.ListContentCheck;
              activeModal.componentInstance.requirementModel.MeetingId = this.Id;
              activeModal.result.then((result) => {
                if (result ) {
                }
                this.getById();
                this.ListContentCheck =[];
                this.CheckChoose =false;
              }, (reason) => {

              });   
            }      
          },
            error => {

            }
        );
      }else{
            this.model.ListContent.forEach(a =>{
              if(a.Checked==true && a.status ==false){
                this.ListContentCheck.push(a);
              }
            });
            if(this.ListContentCheck.length >0){
              let activeModal = this.modalService.open(EndMeetingComponent, { container: 'body', windowClass: 'end-meeting', backdrop: 'static' });
              activeModal.componentInstance.requirementModel.MeetingCode = this.model.Code;
              activeModal.componentInstance.requirementModel.CustomerId = this.model.CustomerId;
              activeModal.componentInstance.requirementModel.CustomerContactId = this.model.CustomerContactId;
              activeModal.componentInstance.requirementModel.Code = this.ListContentCheck[0].Code;
              activeModal.componentInstance.requirementModel.Name = this.ListContentCheck[0].Request;
              activeModal.componentInstance.requirementModel.ListContent = this.ListContentCheck;
              activeModal.componentInstance.requirementModel.MeetingId = this.Id;
              activeModal.result.then((result) => {
                if (result ) {
                }
                this.getById();
                this.ListContentCheck =[];
                this.CheckChoose =false;
              }, (reason) => {

              });   
            }  
      }
    
  }

  finishMeeting() {
    this.meetingService.finishMeeting(this.Id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật meeting thành công!');
        this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmCancelMeeting() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy meeting này không?").then(
      data => {
        this.cancelMeeting();
        //#region thaint
        //#endregion
      },
      error => {

      }
    );
  }

  closeResult = '';
  open(content) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;


    },
      (reason) => {
        this.closeResult = `dismissed ${this.getDismissReason(reason)}`;


      }
    );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  cancelMeeting() {
    this.cancelModel.Id = this.Id;
    this.meetingService.cancelMeeting(this.cancelModel).subscribe(
      data => {
        this.messageService.showSuccess('Hủy meeting thành công!');
        this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.router.navigate(['giai-phap/meeting']);
  }


  // popup thêm mới và chỉnh sửa
  showCreateUpdates(Id: string) {
    let activeModal = this.modalService.open(AddContactCustomerMeetingComponent, { container: 'body', windowClass: 'productgroupcreate-model', backdrop: 'static' })
    // activeModal.componentInstance.idUpdate = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.model.ListCustomerContact.push(result);
      }
    }, (reason) => {
    });
  }

  // popup thêm mới và chỉnh sửa
  // ChoosenCustomer() {
  //   let activeModal = this.modalService.open(AddContactCustomerMeetingComponent, { container: 'body', windowClass: 'productgroupcreate-model', backdrop: 'static' })
  //   // activeModal.componentInstance.idUpdate = Id;
  //   activeModal.result.then((result) => {
  //     if (result) {
  //       this.model.ListCustomerContact.push(result);
  //     }
  //   }, (reason) => {
  //   });
  // }



  showCreateUpdate(Id: string, Row: any, IdCustomer: string) {
    let activeModal = this.modalService.open(AddContactCustomerMeetingComponent, { container: 'body', windowClass: 'choose-question', backdrop: 'static' });
    activeModal.componentInstance.listQuestion = this.model.ListCustomerContact;
    activeModal.componentInstance.idUpdate = Row;
    activeModal.componentInstance.checkType = IdCustomer;

    if (Row) {
      activeModal.componentInstance.model = Row;
    }
    activeModal.result.then((result) => {
      if (result) {
        result.forEach((element: any) => {
          // this.listContact.push(element);
          this.model.ListCustomerContact.push(element);
        });
        // this.setInfo();
      }
    }, (reason) => {

    });
  }

  ExportExcel() {
    this.save(true);
  }
  //#region thaint
  checkeds: boolean = false;
  listCheck: any[] = [];
  selectAllFunction() {
    this.model.ListContent.forEach(element => {
      element.Checked = this.checkeds;
    });
    var count =0;
    this.model.ListContent.forEach(element => {
      if(element.Checked== true && element.status ==false){
        count ++;
      }
    });
    if(count > 0){
      this.CheckChoose = true;
    }else{
      this.CheckChoose =false;
    }
  }

  selectCustomerRequimentNeedToHandle(index) {
    this.selectIndex == index;
  }

  selectContent(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      var ListFileAttach = this.model.ListContent;
      this.MeetingContentAttachs = this.model.ListContent[index].MeetingContentAttaches;
      var IdContent = ListFileAttach[index].Id;
      this.IdCheck = IdContent;
    }
    else {
      this.selectIndex = -1;
      // this.SolutionByDevice = [];
      this.IdCheck = null;
      this.MeetingContentAttachs = [];
      this.model.ListContent.forEach(element =>{
        element.MeetingContentAttaches.forEach(e=>{
          this.MeetingContentAttachs.push(e);
        })
      });
    }
  }

  CountCheck: number = 0;
  pushChecker(row: any) {
    if (row.Checkeds) {
      this.listCheck.push(row);

    } else {
      this.checkeds = false;
      this.listCheck.splice(this.listCheck.indexOf(row), 1);
    }
    this.model.ListContent.forEach(element => {
      if (element.Checked == true && element.status== false) {
        this.CountCheck++;
      }
    });
    if (this.CountCheck > 0) {
      this.CheckChoose = true;

    } else {
      this.CheckChoose = false;
    }
    this.CountCheck = 0;
  }

  saveMeetingContent() {
    let check = this.valid();
    let checkPerformStep = this.validCheckAttachPerformStep();
    if (check && checkPerformStep) {
      var listFileUpload = [];
      this.model.ListAttach.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUpload.push(document.File);
        }
      });

      var listFileUploadPerformStep = [];
      this.model.ListAttachPerformStep.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUploadPerformStep.push(document.File);
        }
      });
      if (this.model.MeetingDate) {
        this.model.MeetingDate = this.dateUtils.convertObjectToDate(this.model.MeetingDate);
      }

      if (listFileUpload.length > 0 || listFileUploadPerformStep.length > 0) {
        let fileAttachs = this.uploadService.uploadListFile(listFileUpload, 'Meeting/');
        let fileAttachPerforms = this.uploadService.uploadListFile(listFileUploadPerformStep, 'Meeting/');
        forkJoin([fileAttachs, fileAttachPerforms]).subscribe(results => {
          var count = 0;
          let data1 = results[0];
          let data2 = results[1];
          if (data1 && data1.length > 0) {
            count++;
            listFileUpload.forEach((item, index) => {
              this.model.ListAttach[item.index].FilePath = data1[index].FileUrl;
            });
          }
          if (data2 && data2.length > 0) {
            count++;
            listFileUploadPerformStep.forEach((item, index) => {
              this.model.ListAttachPerformStep[item.index].FilePath = data2[index].FileUrl;
            });
          }
          if (this.Id) {
            this.update1();
            this.getById();
          }
          else {
            this.create();
          }
        });
      }
      else {
        if (this.Id) {
          this.update1()
        }
        else {
          this.create();
        }
      }
    }
    else {
      this.messageService.showMessage(this.message);
    }
  }

  update1() {
    this.meetingService.update1(this.Id, this.model)
      .subscribe(
        data => {
          this.messageService.showSuccess('Cập nhật meeting thành công!');
          // this.close();
        },
        error => {
          this.messageService.showError(error);
        }
      );
  }

  showConfirmFinish1() {
    this.messageService.showConfirm("Bạn có  muốn thêm yêu cầu khách hàng không?").then(
      data => {
        this.finishMeeting1();
      },
      error => {
      }
    );
  }

  finishMeeting1() {
    this.meetingService.finishMeeting(this.Id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật meeting thành công!');
        this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  addCustomerRequirement() {
    // this.saveMeetingContent();
    // this.showConfirmFinish1();
    // this.save();
    var model ={
      ListContent:this.model.ListContent
    }
    this.meetingService.addCustomerRequirement(this.Id,model).subscribe(
      data => {
        // this.messageService.showSuccess('Cập nhật meeting thành công!');
        this.getById();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showCreateCustomerRequirement(Id: string, index) {
    let activeModal = this.modalService.open(MeetingCreateRequirmentModelComponent, { container: 'body', windowClass: 'project-payment-create-modal', backdrop: 'static' });
    activeModal.componentInstance.ListCustomerContact = this.model.ListCustomerContact;
    activeModal.componentInstance.listCreateRequirementModel = this.model.ListContent;
    activeModal.componentInstance.MeetingId = this.Id;
    
    activeModal.result.then((result) => {
      if (result) {
        this.getRequimentContent();
      }
    }, (reason) => {
    });
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
    this.getById();
  }


  searchMeeting(meetingTypeId: string) {
    // this.model.MeetingTypeId = meetingTypeId;
    this.meetingService.searchMeeting(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
        this.listData.forEach(item => {
          if (item.Type == 1) {
            item.TypeName = "Online";
          }
          else {
            item.TypeName = "Trực tiếp";
          }

          if (item.StartTime) {
            item.StartTime = item.StartTime.hour + ":" + (item.StartTime.minute < 10 ? '0' : '') + item.StartTime.minute;
          }

          if (item.EndTime) {
            item.EndTime = item.EndTime.hour + ":" + (item.EndTime.minute < 10 ? '0' : '') + item.EndTime.minute;
          }

          if (item.RealStartTime) {
            item.RealStartTime = item.RealStartTime.hour + ":" + (item.RealStartTime.minute < 10 ? '0' : '') + item.RealStartTime.minute;
          }

          if (item.RealEndTime) {
            item.RealEndTime = item.RealEndTime.hour + ":" + (item.RealEndTime.minute < 10 ? '0' : '') + item.RealEndTime.minute;
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  // #endregion thaint

  updateContent(i){
    let activeModal = this.modalService.open(MeetingCreateRequirmentModelComponent, { container: 'body', windowClass: 'project-payment-create-modal', backdrop: 'static' });
    activeModal.componentInstance.model = this.model.ListContent[i];
    activeModal.componentInstance.listCreateRequirementModel = this.model.ListContent;
    activeModal.componentInstance.ListCustomerContact = this.model.ListCustomerContact
    activeModal.componentInstance.MeetingId = this.Id;
    
    activeModal.result.then((result) => {
      if (result) {
        this.getRequimentContent();
      }
    }, (reason) => {
    });
  }

  handleFileInput(files: FileList,type) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
    if(type ==0){
      var isExist =false;
      var Index =0;
      var Id ='';
      this.MeetingContentAttachs.forEach((element,i) =>{
        if(element.FileName == this.fileToUpload.name){
          Id = element.Id;
          isExist= true;
          Index =i;
        }
      });
      if (isExist) {
        this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
          data => {
            var model ={
              Id: Id
            }
            this.meetingService.deleteMeetingFileRequimentNeedHandle(model).subscribe(
              data =>{
                this.MeetingContentAttachs.splice(Index,1),
                this.uploadFileMeetingContentAttach();
              },
              error =>{
        
              }
            );
          }, error => {
          });
      }else{
        this.uploadFileMeetingContentAttach();
      }
    }
  }

  uploadFileMeetingContentAttach() {
    let documentTemplate;
    documentTemplate = Object.assign({}, this.fileTemplate);
    documentTemplate.File = this.fileToUpload;
    documentTemplate.FileName = this.fileToUpload.name;
    documentTemplate.FileSize = this.fileToUpload.size;
    documentTemplate.CreateDate = new Date();
    this.uploadService.uploadFile(this.fileToUpload, 'Meeting/').subscribe(
      data => {
        if (data != null) {
          documentTemplate.MeetingId = this.Id;
          documentTemplate.MeetingContentId = this.IdCheck;
          documentTemplate.FilePath = data.FileUrl;
          this.MeetingContentAttachs.push(documentTemplate);
          this.meetingService.createUpdateMeetingFileRequimentNeedHandle(documentTemplate).subscribe(
            data =>{
              this.getById();
            },
            error =>{
      
            }
          );
          
        } else {
          this.messageService.showMessage(data.mess);
          this.nameFile = '';
        }

      },
      error => {
        this.messageService.showMessage(error);
        this.nameFile = '';
      }
    );
  }

  getRequimentContent(){
    this.meetingService.getRequimentContent(this.Id).subscribe(
      data => {
        this.model.ListContent =data.ListContent;
        this.model.ListContent.forEach(item =>{
          item.Checked = false;
          item.status =item.Checked;
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
