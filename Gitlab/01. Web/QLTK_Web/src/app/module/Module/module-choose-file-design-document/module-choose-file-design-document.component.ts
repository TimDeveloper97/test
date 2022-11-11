import { Component, OnInit, ViewEncapsulation, NgZone } from '@angular/core';
import { FileRepositoryService } from 'src/app/gapi/service/file-repository.service';
import { FileInfo } from 'src/app/gapi/model/fileInfo';
import { ModuleTabDesignDocumentService } from '../../services/module-tab-design-document.service';
import { MessageService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GapiSession } from 'src/app/gapi/service/gapi.session';
declare var $: any;
@Component({
  selector: 'app-module-choose-file-design-document',
  templateUrl: './module-choose-file-design-document.component.html',
  styleUrls: ['./module-choose-file-design-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleChooseFileDesignDocumentComponent implements OnInit {
  files: FileInfo[] = [];
  listFile = [];
  listFileGoogle: FileInfo[];
  listFolder = [];
  ModuleId: string;
  fileModel = {
    Id: '',
    ModuleId: '',
    ParentId: '',
    Name: '',
    Path: '',
    FileType: '',
    FileSize: 0,
    Type: 0,
    Blob: File
  }

  importModel = {
    ListFile: [],
    DesignType: ''
  }

  constructor(
    private fileService: FileRepositoryService,
    private fileInfo: FileInfo,
    private designDocumentService: ModuleTabDesignDocumentService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private gapiservice: GapiSession,
  ) { }

  ngOnInit() {
  }

  dragover(event) {
    event.preventDefault();
  }

  drop(event) {
    if (this.importModel.DesignType != '') {
      const items = event.dataTransfer.items;
      for (let i = 0; i < items.length; i++) {
        const entry = items[i].webkitGetAsEntry();
        this.scanFiles(entry);
        this.uploadFolder(0);
      }
    }
    else {
      this.messageService.showMessage("Bạn chưa chọn loại tài liệu thiết kế!");
    }
  }

  scanFiles(item) {
    if (item.isDirectory) {
      var fileM = Object.assign({}, this.fileModel);
      fileM.Name = item.name;
      fileM.ModuleId = this.ModuleId;
      fileM.Path = item.fullPath;
      fileM.ParentId = item.fullPath.replace('/' + item.name, '');
      fileM.Type = 0;
      this.listFile.push(fileM);
      var directoryReader = item.createReader();
      directoryReader.readEntries(entries => {
        entries.forEach((entry) => this.readEntry(entry));
      });
    }
    else {
      return new Promise((resolve, reject) => {
        item.file(
          file => {
            resolve(file);
            var fileM = Object.assign({}, this.fileModel);
            fileM.Name = item.name;
            fileM.ModuleId = this.ModuleId;
            fileM.ParentId = item.fullPath.replace('/' + item.name, '');
            fileM.Path = item.fullPath;
            fileM.Type = 1;
            fileM.Blob = file;
            this.listFile.push(fileM);
          },
          err => {
            reject(err);
          }
        );
      });
    }
  }

  readEntry(entry) {
    this.scanFiles(entry);
  }

  uploadFolder(index) {
    if (this.gapiservice.isSignedIn) {
      var googleId = 'root';
      if (index < this.listFile.length) {
        for (let i = 0; i < this.listFile.length; i++) {
          if (this.listFile[i].Path == this.listFile[index].ParentId) {
            googleId = this.listFile[i].Id;
            break;
          }
        }
        if (this.listFile[index].Type == 0) {
          this.fileService.create(googleId, this.listFile[index].Name).then((res: FileInfo) => {
            this.listFile[index].Id = res.Id;
            if (googleId != 'root') {
              this.listFile[index].ParentId = googleId;
            }
            this.listFile[index].FileType = "2";
            this.awaitUploadFolder(index + 1);
          });
        } else {
          this.listFile[index].ParentId = googleId;
          this.fileInfo.Name = this.listFile[index].Name;
          this.fileInfo.Blob = this.listFile[index].Blob;
          this.uploadFile(googleId, this.fileInfo, index);

        }
      }
      else {
        this.importModel.ListFile = this.listFile;
        this.designDocumentService.uploadDesignDocument(this.importModel).subscribe((data: any) => {
          this.messageService.showSuccess("Upload file thành công!");
          this.closeModal();
        },
          error => {
            this.messageService.showError(error);
          });
      }
    }
    else {
      this.gapiservice.signIn();
    }

  }

  awaitUploadFolder(index) {
    this.uploadFolder(index);
  }

  creatFolder(folderId, name) {
    this.fileService.create(folderId, name);
  }

  uploadFile(folderId, fileInfo, index) {
    this.fileService.importFile(folderId, fileInfo, (res) => this.onImportComplete(res, index));

  }

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  onImportComplete(res, index) {
    var object = JSON.parse(res);
    this.listFile[index].Id = object.id;
    this.listFile[index].FileType = "1";
    this.awaitUploadFolder(index + 1);
  }

  closeModal() {
    this.activeModal.close();
  }
}
