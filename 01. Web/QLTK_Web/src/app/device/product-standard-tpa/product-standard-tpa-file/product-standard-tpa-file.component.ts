import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { ProductStandardTpaService } from '../../services/product-standard-tpa.service';
import { Constants, MessageService, FileProcess, Configuration } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ShowDocumentComponent } from 'src/app/module/Module/show-document/show-document.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModuleCreateService } from 'src/app/module/services/module-create-service';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-product-standard-tpa-file',
  templateUrl: './product-standard-tpa-file.component.html',
  styleUrls: ['./product-standard-tpa-file.component.scss']
})
export class ProductStandardTpaFileComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public constants: Constants,
    private service: ProductStandardTpaService,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private modalService: NgbModal,
    private moduleCreateService: ModuleCreateService,
    private dowloadService: DownloadService,
    private config: Configuration,
  ) { }

  @ViewChild('fileInputCOCQ') inputFileCOCQ: ElementRef
  listFileAttach: any[] = [];
  listCatalog: any[] = [];
  listCO_CQ: any[] = [];

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
  documentTemplateId = '';

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

  uploadFileDocument(id) {
    this.documentTemplateId = id;
    this.inputFileCOCQ.nativeElement.click();
  }

  uploadFileCOCQ($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    this.listCO_CQ.forEach(element => {
      if (this.documentTemplateId == element.DocumentTemplateId) {
        element.FilePath = '';
        element.FileSize = fileDataSheet[0].size;
        element.File = fileDataSheet[0];
      }
    });
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
      }
    );
  }

  showConfirmDeleteCatalog(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file Catolog này không?").then(
      data => {
        this.listCatalog.splice(index, 1);
      }
    );
  }

  showConfirmDeleteCOCQ(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file CO/CQ này không?").then(
      data => {
        this.listCO_CQ.splice(index, 1);
      }
    );
  }

  downloadDocument(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  downloadCatalog(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  downloadCOCQ(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  DownloadFileDocument(file) {
    this.moduleCreateService.getListDocumentFile(file.Id).subscribe(
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

  getListProductStandardTPAFile() {
    this.service.getListProductStandardTPAFile(this.model).subscribe(
      data => {
        if (data) {
          this.listFileAttach = data.listFile;
          this.listCatalog = data.listCatolog;
          this.listCO_CQ = data.listFileCOCQ;
        }
      }, error => {
        this.messageService.showError(error);
      });
  }

  save() {
    let files = [];
    this.model.ListFile = [];
    this.listCatalog.forEach(element => {
      element.Type = 1;
      this.model.ListFile.push(element);
      files.push(element);
    });
    this.listFileAttach.forEach(element => {
      element.Type = 2;
      this.model.ListFile.push(element);
      if (!element.IsDocument) {
        files.push(element);
      }
    });
    this.listCO_CQ.forEach(element => {
      if (element.File || element.FilePath) {
        element.Type = 3;
        this.model.ListFile.push(element);
      }

      if (element.File) {
        files.push(element.File);
      }
    });
    this.uploadService.uploadListFile(files, 'ProductStandardTPAFile/').subscribe((event: any) => {
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
          if (!item.FilePath && !item.IsDocument) {
            item.FileName = event[count].FileName;
            item.FileSize = event[count].FileSize;
            item.FilePath = event[count].FileUrl;
            count++;
          }
        })
      }

      for (var item of this.model.ListFile) {
        if (!item.IsDocument && (item.FilePath == null || item.FilePath == "")) {
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

  showDocumentAttach() {
    let activeModal = this.modalService.open(ShowDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var listIdSelect = [];
    this.listFileAttach.forEach(element => {
      if (element.IsDocument) {
        listIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.componentInstance.groupCode = this.constants.GroupDocument.Module_DocumentHDSD;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listFileAttach.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showDocumentCatalog() {
    let activeModal = this.modalService.open(ShowDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var listIdSelect = [];
    this.listCatalog.forEach(element => {
      if (element.IsDocument) {
        listIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.componentInstance.groupCode = this.constants.GroupDocument.Product_Catelog;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listCatalog.push(element);
        });
      }
    }, (reason) => {

    });
  }
}
