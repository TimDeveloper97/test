import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Constants, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
@Component({
  selector: 'app-upload-image-video',
  templateUrl: './upload-image-video.component.html',
  styleUrls: ['./upload-image-video.component.scss']
})
export class UploadImageVideoComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
  ) { }
  modalInfo = {
    Title: 'Upload ảnh / video',
    SaveText: 'Lưu',
  };
 model={
   Type:2,
 }
 listFileMedia:any[]=[];
  isAction: boolean = false;
  fileImage = {
    Path: '',
    FileName: '',
    FileSize: 0,
    Type: 0,
    CreateDate: new Date(),
  }
  listFileResult:any[]=[];
  ngOnInit() {
  }
  save() {
    var listTypeFileUpload = [];
    this.listFileMedia.forEach((element: any) => {
      listTypeFileUpload.push(element.Type);
    });
    
    this.uploadService.uploadListFile(this.listFileMedia, 'ImageProductBusiness/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileImage);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.Type =listTypeFileUpload[index];
          file.CreateDate=new Date();
          this.listFileResult.push(file);
        });
      }
      for (var item of this.listFileResult) {
        if (item.Path == null || item.Path == "") {
          this.listFileMedia.splice(this.listFileMedia.indexOf(item), 1);
        }
      }
      this.activeModal.close(this.listFileResult);
    }, error => {
      this.messageService.showError(error);
    });
  }
  uploadFile($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFileMedia) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listFileMedia, this.model.Type);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listFileMedia, this.model.Type);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listFileMedia, this.model.Type);
    }
  }
  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace, listFile: any[], type: number) {
    var isExist = false;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < listFile.length; index++) {

        if (listFile[index].Id != null) {
          if (file.name == listFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              listFile.splice(index, 1);
            }
          }
        }
        else if (file.name == listFile[index].name) {
          isExist = true;
          if (isReplace) {
            listFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        file.Type = type;
        listFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments, listFile: any[], type: number) {
    for (var file of fileManualDocuments) {
      file.Type = type;
      listFile.push(file);
    }
  }

  showConfirmDeleteFileMedia(index){
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file media này không?").then(
      data => {
        this.listFileMedia.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
