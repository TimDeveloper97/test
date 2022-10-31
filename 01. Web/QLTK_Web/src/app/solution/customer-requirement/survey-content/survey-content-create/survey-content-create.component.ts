import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SurveyContentService } from '../../service/survey-content.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';
import { ViewDocumentSurveyContentComponent } from '../view-document-survey-content/view-document-survey-content.component';
import { element } from 'protractor';
@Component({
  selector: 'app-survey-content-create',
  templateUrl: './survey-content-create.component.html',
  styleUrls: ['./survey-content-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SurveyContentCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private surveyContentService: SurveyContentService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    public constant: Constants,
    private routeA: ActivatedRoute,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới dụng cụ khảo sát',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  isAdd = false;
  Id: string;
  surveyId: string;
  listSurveyContent: any[] = [];
  listUserId: any[] = [];
  listRequest: any[] = [];
  listUser: any[] = [];
  listfolder: any[] = [
    
  ]

  file: any = {
    name: "",
    size: '',
    dateModified: '',
    thumbnail: null,
  };

  columnName: any[] = [{ Name: 'Name', Title: 'Tên' }, { Name: 'Code', Title: 'Mã' },];

  model: any = {
    Id: '',
    Conten: '',
    Result: '',
    SurveyId: '',
    ListSurveyContentAttach: [],
    ListUser: [],
    EmployeeId: '',
    Level: 3,
  }

  fileTemplate: any = {
    Id: '',
    Name: "",
    Note: '',
    Type: '',
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  listSurveyLevel: any[] = [
    { Id: 1, Name: "Khẩn cấp" },
    { Id: 2, Name: "Cao" },
    { Id: 3, Name: "Trung bình" },
    { Id: 4, Name: "Thấp" },
  ]

  ngOnInit() {
    this.model.SurveyId = this.surveyId;
    this.getlistUser();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa nội dung khảo sát';
      this.ModalInfo.SaveText = 'Lưu';
      this.getById();
     
    }
    else {
      // this.ModalInfo.Title = "Thêm mới dụng cụ khảo sát";
      this.ModalInfo.Title = "Thêm mới nội dung khảo sát";

    }
  }

  getlistUser(){
    this.comboboxService.getListEmployees().subscribe(
      data => {
        this.listUser = data;
      },
    )
  }

  getById() {
    this.model.Id = this.Id;
    this.surveyContentService.getById(this.model.Id).subscribe(
      data => {
        this.model = data;
        //this.model.ListSurveyContentAttach
        let docuemntTemplate;
        for (var file of this.model.ListSurveyContentAttach) {
          docuemntTemplate = Object.assign({}, this.file);
          docuemntTemplate.name = file.FileName;
          docuemntTemplate.size = file.FileSize;
          docuemntTemplate.thumbnail = file.FilePath;
          this.listfolder.push(docuemntTemplate);
        }


      },
    )
  }

  // closeModal(isOK: boolean) {
  //   this.activeModal.close(isOK ? isOK : this.isAction);
  // }

  closeModal(isOK: boolean) {
    this.activeModal.close({ isAdd: true, modelTemp: this.listRequest });
  }

  saveAndContinue() {
    if(this.model.EmployeeId){
      this.listUser.forEach(element => {
        if(element.Id == this.model.EmployeeId){
          this.model.Name = element.Name;
        }
      });

    }

    this.save(true);
  }

  // addSurveyContent(isContinue) {

  //   this.model.SurveyId = this.surveyId;
  //   this.surveyContentService.create(this.model).subscribe(
  //     data => {
  //       if (isContinue) {
  //         this.isAction = true;
  //         this.model = {
  //           Id: '',
  //           Conten: '',
  //           Result: '',
  //           SurveyId: '',
  //           ListSurveyContentAttach: [],
  //         };
  //         this.messageService.showSuccess('Thêm mới thành công!');
  //       }
  //       else {
  //         this.messageService.showSuccess('Thêm mới thành công!');
  //         this.closeModal(data);
  //       }
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }

  save(isContinue: boolean) {

    var contentFiles = [];
    this.model.ListSurveyContentAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        contentFiles.push(document.File);
      }
    });
    if ( contentFiles.length > 0 ) {
      let filesContent = this.uploadService.uploadListFilePDF(contentFiles, 'SurveyContent/');
      forkJoin([ filesContent]).subscribe(results => {
        if (results[0].length > 0) {
          contentFiles.forEach((item, index) => {
            if(results[0][index].FilePDFUrl != null){
            this.model.ListSurveyContentAttach[item.index].FilePath = results[0][index].FilePDFUrl;
            }else if(results[0][index].FileUrl != null){
            this.model.ListSurveyContentAttach[item.index].FilePath = results[0][index].FileUrl;
            }
          });
          if(this.model.SurveyId){
          this.saveSurveyContent();
          }else if (this.isAdd == true) {
            this.listRequest.push(this.model)
            if (isContinue == true) {
              this.activeModal.close({ isAdd: true, modelTemp: this.listRequest });
            } else {
              this.model = {};
            }
      
          }
          
        }
      })
    }else

    if (this.model.SurveyId)
    {
      this.saveSurveyContent();
    }
    else if (this.isAdd == true) {
      this.listRequest.push(this.model)
      if (isContinue == true) {
        this.activeModal.close({ isAdd: true, modelTemp: this.listRequest });
      } else {
        this.model = {};
      }

    } else {
      this.activeModal.close({ isAdd: false, modelTemp: this.model });
      this.messageService.showSuccess('Cập nhật người liên hệ!');
    }

  }



  saveSurveyContent() {
    this.surveyContentService.update(this.model.Id, this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadFileContent($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.model.ListSurveyContentAttach) {
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
    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateReplaceFileContent(fileDataSheet, true);
        }, error => {
          this.updateReplaceFileContent(fileDataSheet, false);
        });
    }
    else {
      this.updateFileContent(fileDataSheet);
    }
  }

  updateReplaceFileContent(file, isReplace) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of file) {
      for (let index = 0; index < this.model.ListSurveyContentAttach.length; index++) {

        if (this.model.ListSurveyContentAttach[index].Id != null) {
          if (file.name == this.model.ListSurveyContentAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.ListSurveyContentAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.ListSurveyContentAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.ListSurveyContentAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        docuemntTemplate.Type = 1;
        this.model.ListSurveyContentAttach.push(docuemntTemplate);
      }
    }
  }

  updateFileContent(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      docuemntTemplate.Type = 1;
      this.model.ListSurveyContentAttach.push(docuemntTemplate);
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
      this.model.ListSurveyContentAttach.splice(index, 1);
    }
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }


  viewDocument(Id: string ){
    let activeModal = this.modalService.open(ViewDocumentSurveyContentComponent, { container: 'body', windowClass: 'view-document-survey-content-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        //this.getProjectAttach();
      }
    }, (reason) => {
    });
  }
}
