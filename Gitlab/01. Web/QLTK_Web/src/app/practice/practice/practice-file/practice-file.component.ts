import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, Configuration, Constants, FileProcess, PermissionService } from 'src/app/shared';
import { PracticeService } from '../../service/practice.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { ShowDocumentComponent } from 'src/app/module/Module/show-document/show-document.component';
import { ModuleCreateService } from 'src/app/module/services/module-create-service';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-practice-file',
  templateUrl: './practice-file.component.html',
  styleUrls: ['./practice-file.component.scss']
})
export class PracticeFileComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private service: PracticeService,
    public constants: Constants,
    public permissionService: PermissionService,
    private modalService: NgbModal,
    private serviceHistory: HistoryVersionService,
    private dowloadService: DownloadService,
    private moduleService: ModuleCreateService
  ) { }
  DateNow = new Date();
  UserName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  StartIndex = 1;
  ListFile: any[] = [];
  check = 0;
  model: any = {
    ListFile: [],
  }
  fileModel: any = {
    Id: '',
    PracticeId: '',
    FilePath: '',
    FileName: '',
    Size: '',
    Description: '',
    CreateByName: '',
    CreateDate: '',
  }
  ngOnInit() {
    this.model.PracticeId = this.Id;
    this.getPracticeFileInfo();
  }

  getPracticeFileInfo() {
    if (!this.permissionService.checkPermission(['F040720', 'F040721', 'F040722'])) {
      this.service.getPracticeFile(this.model).subscribe(data => {
        this.model.ListFile = data.ListFile;
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  uploadFileClick($event) {
    //   this.fileProcess.onFileChange($event);
    //   var a = 0;
    //   this.fileProcess.FilesDataBase.forEach(element => {
    //     for (var item of this.model.ListFile) {
    //       a++;
    //       if (item.FileName == element.name) {
    //         this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //           data => {
    //             this.model.ListFile.splice(a, 1);
    //           });
    //       }
    //     }
    //   });
    //   if (this.fileProcess.totalbyte > 5000000) {
    //     this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
    //     this.fileProcess.totalbyte = 0;
    //   }
    //   else {
    //     this.fileProcess.FilesDataBase.forEach(element =>{
    //       var file = Object.assign({}, this.fileModel);
    //       file.FileName = element.name;
    //       file.Size = element.size;
    //       file.CreateByName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
    //       file.CreateDate = new Date();
    //       this.model.ListFile.push(file);
    //     })

    //   }
    //   this.ListFile = this.fileProcess.FilesDataBase;
    //   this.fileProcess.FilesDataBase = [];
    // }
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.model.ListFile) {
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
      for (let index = 0; index < this.model.ListFile.length; index++) {

        if (this.model.ListFile[index].Id != null) {
          if (file.name == this.model.ListFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.ListFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.ListFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.model.ListFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.model.ListFile.push(file);
    }
  }
  DownloadAFile(file) {
    if (!file.IsDocument) {
      this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
    } else {
      this.getListFile(file);
    }
  }

  getListFile(file: any) {
    this.moduleService.getListDocumentFile(file.Id).subscribe(
      data => {
        if (data && data.length > 0) {
          this.downAllDocumentFile(file, data);
        } else {
          this.messageService.showMessage("Không có file để tải");
          return;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  downAllDocumentFile(row: any, files: any) {
    var listFilePath: any[] = [];
    files.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });
    var modelDowload = {
      Name: row.FileName,
      ListDatashet: listFilePath
    };

    if (!modelDowload.Name) {
      modelDowload.Name = row.name;
    }

    this.dowloadService.downloadAll(modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
      //this.fileProcessDataSheet.downloadFileBlob(data.PathZip, row.FileName);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.model.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {

      }
    );
  }

  updatePracticeFile() {
    var listFileUpload: any[] = [];
    this.model.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });

    this.uploadService.uploadListFile(this.model.ListFile, 'PracticeFile/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.Size = item.FileSize;
          file.FilePath = item.FileUrl;
          file.Description = listFileUpload[index].Description;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (!item.FilePath && !item.IsDocument) {
          this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
        }
      }
      this.service.createPracticeFile(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Lưu tài liệu thành công!');
          this.getPracticeFileInfo();
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.updatePracticeFile();
  }

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Practice;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showDocument() {
    let activeModal = this.modalService.open(ShowDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var listIdSelect = [];
    this.model.ListFile.forEach(element => {
      if (element.IsDocument) {
        listIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.componentInstance.groupCode = this.constants.GroupDocument.Product_GuidePractive;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.model.ListFile.push(element);
        });
      }
    }, (reason) => {

    });
  }
}
