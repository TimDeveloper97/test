import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { forkJoin } from 'rxjs';
import { DocumentService } from '../services/document.service';
import { ChooseDocumentReferenceComponent } from '../choose-document-reference/choose-document-reference.component';

@Component({
  selector: 'app-document-file-tab',
  templateUrl: './document-file-tab.component.html',
  styleUrls: ['./document-file-tab.component.scss']
})
export class DocumentFileTabComponent implements OnInit {

  constructor(public constants: Constants,
    private modalService: NgbModal,
    public fileProcess: FileProcess,
    private fileService: UploadfileService,
    private messageService: MessageService,
    private router: Router,
    private documentService: DocumentService) { }

  @Input() id: string;
  @Input() documentStatus: number;

  documentModel: any = {
    DocumentId: '',
    DocumentFiles: [],
    DocumentReferences: []
  };

  fileModel: any = {
    FileName: '',
    Path: '',
    PDFPath: '',
    FileSize: null,
    CreateBy: null,
    CreateDate: null,
    File: null
  };
  documentFilesUpload: any[] = [];
  user: any;

  ngOnInit(): void {
    let userLocal = localStorage.getItem('qltkcurrentUser');
    if (userLocal) {
      this.user = JSON.parse(userLocal);
    }
    this.documentModel.DocumentId = this.id;
    this.getInfo();
  }

  getInfo() {
    this.documentService.getDocumentFileInfo({ DocumentId: this.id }).subscribe(
      result => {
        this.documentModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteFile(row: any, index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        if (row.Id) {
          row.IsDelete = true;
        }
        else {
          this.documentModel.DocumentFiles.splice(index, 1);
        }
        this.messageService.showSuccess('Xóa file thành công!');
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileReference(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu tham khảo này không?").then(
      data => {
        this.documentModel.DocumentReferences.splice(index, 1);
        this.messageService.showSuccess('Xóa tài liệu tham khảo thành công!');
      },
      error => {

      }
    );
  }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;

    for (var item of fileDataSheet) {

      var fileExist = this.documentModel.DocumentFiles.find(a => a.FileName == item.name);
      if (fileExist != null) {
        isExist = true;
      }
    }

    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceDocuments(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceDocuments(fileDataSheet, false);
        });
    }
    else {
      this.updateFileDocuments(fileDataSheet);
    }
  }

  updateFileAndReplaceDocuments(fileAttachs, isReplace) {
    var isExist = false;
    for (var file of fileAttachs) {
      for (let index = 0; index < this.documentModel.DocumentFiles.length; index++) {
        if (file.name == this.documentModel.DocumentFiles[index].FileName) {
          isExist = true;
          if (isReplace) {
            if (this.documentModel.DocumentFiles[index].Id != null) {
              this.documentModel.DocumentFiles[index].IsDelete = true;
            }
            else {
              this.documentModel.DocumentFiles.splice(index, 1);
            }
          }
        }
      }

      if (!isExist || isReplace) {
        var fileNew = Object.assign({}, this.fileModel);
        fileNew.FileName = file.name;
        fileNew.FileSize = file.size;
        fileNew.CreateBy = this.user.employeeName;
        fileNew.CreateDate = new Date();
        fileNew.File = file
        this.documentModel.DocumentFiles.push(fileNew);
      }
    }
  }

  updateFileDocuments(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      var fileNew = Object.assign({}, this.fileModel);
      fileNew.FileName = file.name;
      fileNew.FileSize = file.size;
      fileNew.CreateBy = this.user.employeeName;
      fileNew.CreateDate = new Date();
      fileNew.File = file
      this.documentModel.DocumentFiles.push(fileNew);
    }
  }

  save() {

    this.documentFilesUpload = [];
    this.documentModel.DocumentFiles.forEach(element => {
      if (element.File && !element.IsDelete) {
        this.documentFilesUpload.push(element.File);
      }
    });

    if (this.documentFilesUpload.length > 0) {
      let questionFiles = this.fileService.uploadListFilePDF(this.documentFilesUpload, 'DocumentFile/');
      forkJoin([questionFiles]).subscribe(results => {
        if (results[0].length > 0) {
          results[0].forEach(item => {
            var file = this.documentModel.DocumentFiles.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
            file.Path = item.FileUrl;
            file.PDFPath = item.FilePDFUrl;
          });
        }
        this.updateDocumentFile();
      }, error => {
        this.messageService.showError(error);
      });
    } else {
      this.updateDocumentFile();
    }
  }

  updateDocumentFile() {
    this.documentService.updateDocumentFile(this.documentModel).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật file tài liệu thành công!');
        this.documentFilesUpload = [];
        this.getInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  chooseDocument() {
    let activeModal = this.modalService.open(ChooseDocumentReferenceComponent, { container: 'body', windowClass: 'choose-document-reference-model', backdrop: 'static' });
    var listIdSelect = [];
    this.documentModel.DocumentFiles.forEach(element => {
      listIdSelect.push(element.Id);
    });
    listIdSelect.push(this.id);

    activeModal.componentInstance.ListIdSelect = listIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.documentModel.DocumentReferences.push(element);
        });
      }
    }, (reason) => {

    });
  }

  closeModal() {
    this.router.navigate(['tai-lieu/quan-ly-tai-lieu']);
  }

  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.Path, row.FileName);
  }

}
