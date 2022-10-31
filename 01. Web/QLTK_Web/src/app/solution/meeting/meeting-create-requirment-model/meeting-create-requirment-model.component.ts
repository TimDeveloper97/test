import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { MeetingService } from '../../service/meeting.service';
import { ActivatedRoute } from '@angular/router';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { forkJoin } from 'rxjs';
import { CustomerRequirementService } from '../../customer-requirement/service/customer-requirement.service';

@Component({
  selector: 'app-meeting-create-requirment-model',
  templateUrl: './meeting-create-requirment-model.component.html',
  styleUrls: ['./meeting-create-requirment-model.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MeetingCreateRequirmentModelComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils,
    public meetingService : MeetingService,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    public customerRequirementService: CustomerRequirementService,


  ) { }
  nameFile = "";
  fileToUpload: File = null;

  ListCustomerContact: any[]=[];
  ModalInfo = {
    Title: 'Thêm mới yêu cầu',
    SaveText: 'Lưu',
  };
  MeetingId:string;
  message :any;
  model: any = {

    Id: '',
    MeetingId:'',
    Code:'',
    Request: '',
    Solution: '',
    FinishDate: null,
    Note:'',
    Index:'',
    CreateBy:'',
    CreateDate: null,
    Checked:'',
    MeetingContentAttaches : [],
  }
  listCreateRequirementModel: any[] = [];
  Id: any='';
  columnCustomer: any[] = [{ Name: 'Name', Title: 'Tên khách hàng' },{ Name: 'Email', Title: 'Email' }];
  listContent : any[] =[];
  MeetingContentAttachs : any[] =[];

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

  ngOnInit(): void{
    if(this.model.Id){
      this.ModalInfo.Title ="Chỉnh sửa yêu cầu";
      this.model.FinishDate = this.dateUtils.convertDateToObject(this.model.FinishDate);
      this.model.CreateDate = this.dateUtils.convertDateToObject(this.model.CreateDate);
    }
    else{
      this.generateCode();
    }
    this.ListCustomerContact = this.ListCustomerContact;
  }
  save() {
    if (this.model.CreateDate) {
      this.model.CreateDate = this.dateUtils.convertObjectToDate(this.model.CreateDate);
    }
    if (this.model.FinishDate) {
      this.model.FinishDate = this.dateUtils.convertObjectToDate(this.model.FinishDate);
    }
    // this.listCreateRequirementModel.push(this.model);
    this.model.MeetingId =this.MeetingId;
    if (this.model.MeetingContentAttaches.length >0) {
      var listFileUpload = [];
      this.model.MeetingContentAttaches.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUpload.push(document.File);
        }
      });
      if (listFileUpload.length > 0 ) {
        let fileAttachs = this.uploadService.uploadListFile(listFileUpload, 'Meeting/');
        forkJoin([fileAttachs]).subscribe(results => {
          var count = 0;
          let data1 = results[0];
          if (data1 && data1.length > 0) {
            count++;
            listFileUpload.forEach((item, index) => {
              this.model.MeetingContentAttaches[item.index].FilePath = data1[index].FileUrl;
            });

            this.meetingService.createUpdateMeetingRequimentNeedHandle(this.model).subscribe(
              data =>{
                this.activeModal.close(true);
              },
              error =>{
        
              }
            );
          }
        });
      }else{
        this.meetingService.createUpdateMeetingRequimentNeedHandle(this.model).subscribe(
          data =>{
            this.activeModal.close(true);
          },
          error =>{
    
          }
        );
      }
    }else{
      this.meetingService.createUpdateMeetingRequimentNeedHandle(this.model).subscribe(
        data =>{
              this.activeModal.close(true);
        },
        error =>{
  
        }
      );
    }
    
  }
  closeModal() {
    this.activeModal.close(false);
  }


  saveFile() {
    if (this.model.MeetingContentAttaches.length >0) {
      var listFileUpload = [];
      this.model.MeetingContentAttaches.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUpload.push(document.File);
        }
      });
      if (listFileUpload.length > 0 ) {
        let fileAttachs = this.uploadService.uploadListFile(listFileUpload, 'Meeting/');
        forkJoin([fileAttachs]).subscribe(results => {
          var count = 0;
          let data1 = results[0];
          if (data1 && data1.length > 0) {
            count++;
            listFileUpload.forEach((item, index) => {
              this.model.MeetingContentAttaches[item.index].FilePath = data1[index].FileUrl;
            });
          }
        });
      }
    }
    else {
      this.messageService.showMessage(this.message);
    }
  }

  uploadFile($event: any) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    var list;
    list = this.model.MeetingContentAttaches;
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;
    let docuemntTemplate;
      for (var file of fileManualDocuments) {
        for (let index = 0; index < this.model.MeetingContentAttaches.length; index++) {

          if (this.model.MeetingContentAttaches[index].Id != null) {
            if (file.name == this.model.MeetingContentAttaches[index].FileName) {
              isExist = true;
              if (isReplace) {
                this.model.ListAttach.splice(index, 1);
              }
            }
          }
          else if (file.name == this.model.MeetingContentAttaches[index].name) {
            isExist = true;
            if (isReplace) {
              this.model.MeetingContentAttaches.splice(index, 1);
            }
          }
        }

        if (!isExist || isReplace) {
          docuemntTemplate = Object.assign({}, this.fileTemplate);
          docuemntTemplate.File = file;
          docuemntTemplate.FileName = file.name;
          docuemntTemplate.FileSize = file.size;
          this.model.MeetingContentAttaches.push(docuemntTemplate);
        }
      }

  }

  updateFileManualDocument(files) {
    let documentTemplate;
    for (var file of files) {
      documentTemplate = Object.assign({}, this.fileTemplate);
      documentTemplate.File = file;
      documentTemplate.FileName = file.name;
      documentTemplate.FileSize = file.size;
      documentTemplate.CreateDate = new Date();
      this.model.MeetingContentAttaches.push(documentTemplate);
    }
  }
  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }
  showConfirmDeleteFileMeetingContentAttach(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        var model ={
          Id:this.model.MeetingContentAttaches[index].Id
        }
        this.meetingService.deleteMeetingFileRequimentNeedHandle(model).subscribe(
          data =>{
            this.model.MeetingContentAttaches.splice(index, 1);
          },
          error =>{
    
          }
        );
      },
      error => {

      }
    );
  }

  getRequimentContent(){
    this.meetingService.getRequimentContent(this.MeetingId).subscribe(
      data => {
        this.listCreateRequirementModel =data.ListContent;
        this.listCreateRequirementModel.forEach(item => {
          item.FinishDate = this.dateUtils.convertDateToObject(item.FinishDate);
        });

        this.listCreateRequirementModel.forEach(item => {
          item.CreateDate = this.dateUtils.convertDateToObject(item.CreateDate);
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  generateCode() {
    this.customerRequirementService.generateCode().subscribe((data: any) => {
      if (data) {
        this.model.Code = data.Code;
        this.model.Index = data.Index;
      }
    });
  }

}
