import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Configuration, Constants, FileProcess, ComboboxService } from 'src/app/shared';
import { EducationProgramService } from '../../service/education-program.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SelectJobComponent } from '../select-job/select-job/select-job.component';

@Component({
  selector: 'app-education-program-create',
  templateUrl: './education-program-create.component.html',
  styleUrls: ['./education-program-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EducationProgramCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private educationProgramService: EducationProgramService,
    private config: Configuration,
    public constant: Constants,
    private modalService: NgbModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private comboboxService: ComboboxService,
    public fileProcess: FileProcess,
    public uploadfileService: UploadfileService,
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa chương trình đào tạo';
      this.ModalInfo.SaveText = 'Lưu';
      this.getEducationProgramInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới chương trình đào tạo";
    }
    this.getJob();
  }

  ModalInfo = {
    Title: 'Thêm mới phòng học',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  ListJob = [];
  ListFile = [];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Documents: '',
    JobId: '',
    ListJob: [],
    ListFile: []
  }

  fileListFileModuleManualDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
  }
  getEducationProgramInfo() {
    this.educationProgramService.getEducationProgram({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.listBase = data.ListJob;
      this.ListFile = data.ListFile;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }



  saveAndContinue() {
    this.save(true);
  }
  createEducationProgram(isContinue) {
    this.model.ListJob = this.listBase;
    var listFileUpload = [];
    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.uploadfileService.uploadListFile(this.ListFile, 'EducationProgram/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileModuleManualDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.ListFile.splice(this.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.addEducationProgram(isContinue);
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.addEducationProgram(isContinue);
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  addEducationProgram(isContinue) {
    this.educationProgramService.createEducationProgram(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            JobId: '',
            JobName: '',
            ListJob: [],
            ListFile: [],
          };
          this.listBase = [];
          this.messageService.showSuccess('Thêm mới phòng học thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới phòng học thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateEducationProgram();
    }
    else {
      this.createEducationProgram(isContinue);
    }
  }

  saveEducationProgram() {
    this.educationProgramService.updateEducationProgram(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chương trình đào tạo thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateEducationProgram() {
    //kiểm tra ký tự đặc việt trong Mã
    this.model.ListJob = this.listBase;
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var listFileUpload = [];

    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadfileService.uploadListFile(this.ListFile, 'EducationProgram/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileModuleManualDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.ListFile.splice(this.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.saveEducationProgram();
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
            this.saveEducationProgram();
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  getJob() {
    this.comboboxService.getListJob().subscribe(
      data => {
        this.ListJob = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listBase: any[] = [];
  showSelectJob() {
    let activeModal = this.modalService.open(SelectJobComponent, { container: 'body', windowClass: 'select-job-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listBase.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listBase.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteJob(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng học này không?").then(
      data => {
        this.removeRowJob(row);
      },
      error => {
        
      }
    );
  }

  removeRowJob(row) {
    var index = this.listBase.indexOf(row);
    if (index > -1) {
      this.listBase.splice(index, 1);
    }
  }

  //UPLOAD FILE
  StartIndex = 1;
  uploadFileClick($event) {
    //   var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    //   for (var item of fileDataSheet) {
    //     var a=0;
    //     for(var ite of this.ListFile){
    //       if(ite.Id != null){
    //         if(item.name == ite.FileName){
    //           var b =a;
    //           this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //             data => {
    //               // this.model.ListFile.splice(b, 1);
    //               this.ListFile.splice(b, 1);
    //             });
    //         }
    //       }
    //       else{
    //         if(item.name == ite.name){
    //           var b =a;
    //           this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //             data => {
    //               // this.model.ListFile.splice(b, 1);
    //               this.ListFile.splice(b, 1);
    //             });
    //         }
    //       }

    //       a ++;
    //     }
    //     item.IsFileUpload = true;
    //     this.ListFile.push(item);
    //   }
    // }
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.ListFile) {
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

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.ListFile.length; index++) {

        if (this.ListFile[index].Id != null) {
          if (file.name == this.ListFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.ListFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.ListFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.ListFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.ListFile.push(file);
    }
  } 
  
  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }
}
