import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Constants, MessageService, FileProcess, Configuration } from 'src/app/shared';
import { ProductCreatesService } from '../services/product-create.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { async } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ShowDocumentComponent } from 'src/app/module/Module/show-document/show-document.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DownloadService } from 'src/app/shared/services/download.service';
import { ModuleCreateService } from 'src/app/module/services/module-create-service';

@Component({
  selector: 'app-product-tab-document-attach',
  templateUrl: './product-tab-document-attach.component.html',
  styleUrls: ['./product-tab-document-attach.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductTabDocumentAttachComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public constants: Constants,
    private productService: ProductCreatesService,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private router: Router,
    private modalService: NgbModal,
    private config: Configuration,
    private dowloadService: DownloadService,
    private moduleService: ModuleCreateService
  ) { }

  ListFileAttach = [];
  ListFileCatalog = [];
  ListGuidePractice = [];
  ListQuotation = [];
  ListDrawingLayout = [];
  ListDMVT = [];
  ListDMBTH = [];
  ListGuideMaintenance = [];

  ListFileDocument = [];

  productModel = {
    Id: ''
  }

  updateModel = {
    Id: '',
    ListFileDocument: [],
    ListFileCatalog: []
  }

  fileProductDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
    IsFileUpload: false,
    UpdateDate: null,
    File: null
  }

  fileProductCatalog = {
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
    IsFileUpload: false,
    UpdateDate: null,
    File: null
  };

  ngOnInit() {
    this.getProductDocumentAttachs();
  }

  getProductDocumentAttachs() {
    this.productModel.Id = this.Id;
    this.productService.getProductDocumentAttachs(this.productModel).subscribe(
      data => {
        this.ListFileAttach = data.ListFileAttach;
        this.ListFileCatalog = data.ListFileCatalog;
        this.ListGuidePractice = data.ListGuidePractice;
        this.ListQuotation = data.ListQuotation;
        this.ListDrawingLayout = data.ListDrawingLayout;
        this.ListDMVT = data.ListDMVT;
        this.ListDMBTH = data.ListDMBTH;
        this.ListGuideMaintenance = data.ListGuideMaintenance;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadFileAttach($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListFileAttach) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListFileAttach, 0, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListFileAttach, 0, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListFileAttach, 0);
    }
  }

  uploadFileGuidePractice($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListGuidePractice) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListGuidePractice, 1, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListGuidePractice, 1, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListGuidePractice, 1);
    }
  }

  uploadFileQuotation($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListQuotation) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListQuotation, 2, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListQuotation, 2, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListQuotation, 2);
    }
  }

  uploadFileDrawingLayout($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListDrawingLayout) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListDrawingLayout, 3, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListDrawingLayout, 3, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListDrawingLayout, 3);
    }
  }

  uploadFileDMVT($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListDMVT) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListDMVT, 4, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListDMVT, 4, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListDMVT, 4);
    }
  }

  uploadFileDMBTH($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListDMBTH) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListDMBTH, 5, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListDMBTH, 5, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListDMBTH, 5);
    }
  }

  uploadFileGuideMaintenance($event) {
    var fileAttach = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileAttach) {
      isExist = false;
      for (var ite of this.ListGuideMaintenance) {
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
          this.updateFileAndReplaceFile(fileAttach, this.ListGuideMaintenance, 6, true);
        }, error => {
          this.updateFileAndReplaceFile(fileAttach, this.ListGuideMaintenance, 6, false);
        });
    }
    else {
      this.updateFileDocumentAttach(fileAttach, this.ListGuideMaintenance, 6);
    }
  }

  updateFileAndReplaceFile(fileProductDocuments, listFile, fileType, isReplace) {
    var isExist = false;
    for (var file of fileProductDocuments) {
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
        var fileDocument = Object.assign({}, this.fileProductDocument);
        fileDocument.FileName = file.name;
        fileDocument.FileSize = file.size;
        fileDocument.FileType = fileType;
        fileDocument.IsFileUpload = true;
        fileDocument.File = file;
        fileDocument.UpdateDate = new Date();

        listFile.push(fileDocument);
      }
    }
  }

  updateFileDocumentAttach(fileProductDocuments, listFile, type) {
    for (var file of fileProductDocuments) {
      var fileDocument = Object.assign({}, this.fileProductDocument);
      fileDocument.FileName = file.name;
      fileDocument.FileSize = file.size;
      fileDocument.FileType = type;
      fileDocument.IsFileUpload = true;
      fileDocument.File = file;
      fileDocument.UpdateDate = new Date();

      listFile.push(fileDocument);
    }
  }


  uploadFileCatalog($event) {
    var filecatalog = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of filecatalog) {
      isExist = false;
      for (var ite of this.ListFileCatalog) {
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
          this.updateFileCatalogAndReplaceManualDocument(filecatalog, true);
        }, error => {
          this.updateFileCatalogAndReplaceManualDocument(filecatalog, false);
        });
    }
    else {
      this.updateFileCatalogManualDocument(filecatalog);
    }
  }

  updateFileCatalogAndReplaceManualDocument(filecatalog, isReplace) {
    var isExist = false;
    for (var file of filecatalog) {
      for (let index = 0; index < this.ListFileCatalog.length; index++) {
        if (file.name == this.ListFileCatalog[index].FileName) {
          isExist = true;
          if (isReplace) {
            this.ListFileCatalog.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        var fileCatalog = Object.assign({}, this.fileProductCatalog);
        fileCatalog.FileName = file.name;
        fileCatalog.FileSize = file.size;
        fileCatalog.IsFileUpload = true;
        fileCatalog.File = file;
        fileCatalog.UpdateDate = new Date();
        this.ListFileCatalog.push(fileCatalog);
      }
    }
  }

  updateFileCatalogManualDocument(filecatalog) {
    for (var file of filecatalog) {
      var fileCatalog = Object.assign({}, this.fileProductCatalog);
      fileCatalog.FileName = file.name;
      fileCatalog.FileSize = file.size;
      fileCatalog.IsFileUpload = true;
      fileCatalog.File = file;
      fileCatalog.UpdateDate = new Date();
      this.ListFileCatalog.push(fileCatalog);
    }
  }

  async save() {
    const uploadCatalogStatus = <any>await this.uploadCatalog();
    const uploadDocumentStatus = <any>await this.uploadDocument();
    if (uploadCatalogStatus && uploadDocumentStatus) {
      this.saveDocument();
    }
  }

  saveDocument() {
    this.updateModel.Id = this.productModel.Id;
    this.updateModel.ListFileCatalog = this.ListFileCatalog;
    this.updateModel.ListFileDocument = this.ListFileDocument;
    this.productService.updateProductDocument(this.updateModel).subscribe(
      (data: any) => {
        if (data) {
          this.messageService.showSuccess('Cập nhật thiết bị thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadCatalog(): Promise<any> {
    return new Promise((resolve, reject) => {
      var listFile: any[] = [];
      var listFileUpload: any[] = [];

      this.ListFileCatalog.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
          listFileUpload.push(document);
        }
      });

      if (listFile.length > 0) {
        this.uploadService.uploadListFile(listFile, 'FileCatalog/').subscribe(async (event: any) => {
          if (event.length > 0) {
            event.forEach((item, index) => {
              this.ListFileCatalog[listFileUpload[index].Index].FilePath = item.FileUrl;
            });
            resolve(true);
          }
          else {
            resolve(false);
          }
        }, error => {
          reject('');
          this.messageService.showError(error);
        });
      }
      else {
        resolve(true);
      }
    });
  }

  uploadDocument(): Promise<any> {
    return new Promise((resolve, reject) => {
      var listFile: any[] = [];
      this.ListFileAttach.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListGuidePractice.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListQuotation.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListDrawingLayout.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListDMVT.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListDMBTH.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      this.ListGuideMaintenance.forEach((document, index) => {
        if (document.IsFileUpload) {
          document.Index = index;
          listFile.push(document.File);
        }
        this.ListFileDocument.push(document);
      });

      if (listFile.length > 0) {
        this.uploadService.uploadListFile(listFile, 'ProductManualDocument/').subscribe(async (event: any) => {
          if (event.length > 0) {
            event.forEach((item, index) => {
              var file = this.ListFileDocument.find(t => t.FileName == item.FileName);
              if (file != null) {
                file.Path = item.FileUrl;
              }
            });
            resolve(true);
          }
          else {
            resolve(false);
          }
        }, error => {
          reject('');
          this.messageService.showError(error);
        });
      }
      else {
        resolve(true);
      }
    });
  }

  downloadDocument(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  downloadCatalog(file) {
    if (!file.IsDocument) {
      this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
    } else {
      this.getListFile(file);
    }
  }

  downloadGuidePractice(file) {
    if (!file.IsDocument) {
      this.fileProcess.downloadFileBlob(file.Path, file.FileName);
    } else {
      this.getListFile(file);
    }
  }

  downloadQuotation(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  downloadDrawingLayout(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  downloadDMVT(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  downloadDMBTH(file) {
    if (!file.IsDocument) {
      this.fileProcess.downloadFileBlob(file.Path, file.FileName);
    } else {
      this.getListFile(file)
    }
  }

  downloadGuideMaintenance(file) {
    if (!file.IsDocument) {
      this.fileProcess.downloadFileBlob(file.Path, file.FileName);
    } else {
      this.getListFile(file)
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
        this.ListFileAttach.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileCatalog(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file catalog đính kèm này không?").then(
      data => {
        this.ListFileCatalog.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileGuidePractice(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file hướng dẫn thực hành này không?").then(
      data => {
        this.ListGuidePractice.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileQuotation(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file báo giá này không?").then(
      data => {
        this.ListQuotation.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileDrawingLayout(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file bản vẽ layout này không?").then(
      data => {
        this.ListDrawingLayout.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileDMVT(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file danh mục vật tư này không?").then(
      data => {
        this.ListDMVT.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileDMBTH(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file danh mục bài thực hành này không?").then(
      data => {
        this.ListDMBTH.splice(index, 1);
      },
      error => {

      }
    );
  }

  showConfirmDeleteFileGuideMaintenance(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file hướng dẫn bảo trì này không?").then(
      data => {
        this.ListGuideMaintenance.splice(index, 1);
      },
      error => {

      }
    );
  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi']);
  }

  showDocument(groupCode: string) {
    let activeModal = this.modalService.open(ShowDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var listIdSelect = [];
    if (groupCode === this.constants.GroupDocument.Product_Catelog) {
      this.ListFileCatalog.forEach(element => {
        if (element.IsDocument) {
          listIdSelect.push(element.Id);
        }
      });
    } else if (groupCode === this.constants.GroupDocument.Product_GuidePractive) {
      this.ListGuidePractice.forEach(element => {
        if (element.IsDocument) {
          listIdSelect.push(element.Id);
        }
      });
    } else if (groupCode === this.constants.GroupDocument.Product_DMBTH) {
      this.ListDMBTH.forEach(element => {
        if (element.IsDocument) {
          listIdSelect.push(element.Id);
        }
      });
    } else if (groupCode === this.constants.GroupDocument.Product_GuideMaintenance) {
      this.ListGuideMaintenance.forEach(element => {
        if (element.IsDocument) {
          listIdSelect.push(element.Id);
        }
      });
    }

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.componentInstance.groupCode = groupCode;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        if (groupCode === this.constants.GroupDocument.Product_Catelog) {
          result.forEach(element => {
            this.ListFileCatalog.push(element);
          });
        } else if (groupCode === this.constants.GroupDocument.Product_GuidePractive) {
          result.forEach(element => {
            this.ListGuidePractice.push(element);
          });
        } else if (groupCode === this.constants.GroupDocument.Product_DMBTH) {
          result.forEach(element => {
            this.ListDMBTH.push(element);
          });
        } else if (groupCode === this.constants.GroupDocument.Product_GuideMaintenance) {
          result.forEach(element => {
            this.ListGuideMaintenance.push(element);
          });
        }
      }
    }, (reason) => {

    });
  }
}
