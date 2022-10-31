import { Component, Input, OnInit } from '@angular/core';
import { Constants, MessageService, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProductStandardTpaService } from '../../services/product-standard-tpa.service';

@Component({
  selector: 'app-product-standard-tpa-file-view',
  templateUrl: './product-standard-tpa-file-view.component.html',
  styleUrls: ['./product-standard-tpa-file-view.component.scss']
})
export class ProductStandardTpaFileViewComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public constants: Constants,
    private service: ProductStandardTpaService,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
  ) { }

  listFileAttach: any[] = [];
  listCatalog: any[] = [];

  model: any = {
    ProductStandardTPAId: '',
    ListFile: []
  }

  fileModel: any = {
    Id: '',
    FileName: '',
    FileSize: '',
    FilePath: '',
    Note: '',
    Type: 0,
  }

  ngOnInit() {
    this.model.ProductStandardTPAId = this.Id;
    this.getListProductStandardTPAFile();
  }

  uploadFileAttach($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFileAttach) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listFileAttach, 2);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listFileAttach, 2);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listFileAttach, 2);
    }
  }

  uploadFileCatalog($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listCatalog) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listCatalog, 1);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listCatalog, 1);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listCatalog, 1);
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

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file hướng dẫn sử dụng này không?").then(
      data => {
        this.listFileAttach.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  showConfirmDeleteCatalog(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file Catolog này không?").then(
      data => {
        this.listCatalog.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  downloadDocument(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  downloadCatalog(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  getListProductStandardTPAFile() {
    this.service.getListProductStandardTPAFile(this.model).subscribe(
      data => {
        if (data) {
          this.listFileAttach = data.listFile;
          this.listCatalog = data.listCatolog;
        }
      }, error => {
        this.messageService.showError(error);
      });
  }

  save() {
    this.model.ListFile = [];
    this.listCatalog.forEach(element => {
      element.Type = 1;
      this.model.ListFile.push(element);
    });
    this.listFileAttach.forEach(element => {
      element.Type = 2;
      this.model.ListFile.push(element);
    });
    this.uploadService.uploadListFile(this.model.ListFile, 'ProductStandardTPAFile/').subscribe((event: any) => {
      if (event.length > 0) {
        // event.forEach((item, index) => {
        //   var file = Object.assign({}, this.fileModel);
        //   file.FileName = item.FileName;
        //   file.FileSize = item.FileSize;
        //   file.FilePath = item.FileUrl;
        //   file.Type = item.Type;
        //   file.Note = item.Note;
        //   this.model.ListFile.push(file);
        // });
        var count = 0;
        this.model.ListFile.forEach(item => {
          if (!item.FilePath) {
            item.FileName = event[count].FileName;
            item.FileSize = event[count].FileSize;
            item.FilePath = event[count].FileUrl;
            count++;
          }
        })
      }
      for (var item of this.model.ListFile) {
        if (item.FilePath == null || item.FilePath == "") {
          this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
        }
      }
      this.service.uploadFile(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Cập nhật tài liệu thành công!');
          this.getListProductStandardTPAFile();
        }, error => {
          this.messageService.showError(error);
        });
    }, error => {
      this.messageService.showError(error);
    });
  }

}
