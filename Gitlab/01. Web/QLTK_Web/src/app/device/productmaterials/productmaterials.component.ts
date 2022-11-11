import { Component, OnInit, Input } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, FileProcess, Constants, AppSetting, Configuration } from 'src/app/shared';
import { ProductMaterialsService } from '../services/product-materials.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router } from '@angular/router';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-productmaterials',
  templateUrl: './productmaterials.component.html',
  styleUrls: ['./productmaterials.component.scss']
})
export class ProductmaterialsComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    private config: Configuration,
    public constants: Constants,
    public appset: AppSetting,
    private router: Router,
    private serviceHistory: HistoryVersionService,
    private serviceProductmaterials: ProductMaterialsService
  ) { }

  listData: any[] = [];
  ListFileSetup = [];
  ListFileDatasheet = [];
  ListFileProductDocument = [];
  manufactureId: string;
  modelProductMaterials: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'MaterialCode',
    OrderType: true,
    Id: '',
    ProductId: '',
    MaterialId: '',
    SetupFilePath: '',
    DatasheetPath: '',
    ListFileSetup: [],
    ListFileDatasheet: [],
    ListFielDocument: [],
    leadtime: '',
  }

  fileSetup: any = {
    Path: ''
  }

  fileDatasheet: any = {
    ManufactureId: '',
    FileName: '',
    FilePath: '',
    Size: '',
  }

  model: any = {
    ListDatashet: []
  }

  fileListFileProductDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: ''
  }

  ngOnInit() {
    this.fileProcess.FilesDataBase = [];
    this.modelProductMaterials.ProductId = this.Id;
    this.searchProductMaterials();
    this.getById();
  }

  getById() {
    this.serviceProductmaterials.getProductInfo(this.modelProductMaterials).subscribe(
      data => {
        this.modelProductMaterials.ListFielDocument = data.ListFielDocument;
        this.ListFileProductDocument = data.ListFielDocument;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchProductMaterials() {
    this.serviceProductmaterials.searchProductMaterialsIsSetup(this.modelProductMaterials).subscribe(data => {
      this.listData = data.ListResult;
      this.model.leadtime = data.Status1;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  uploadFileClickSetup($event, Id: string) {
    this.fileProcess.onFileChange($event);
    if (this.fileProcess.totalbyte > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.totalbyte = 0;
    }
    else {
      this.ListFileSetup = this.fileProcess.FilesDataBase;
      this.modelProductMaterials.Id = Id;
    }
    this.fileProcess.FilesDataBase = [];
  }

  uploadFileClickDatasheet($event, Id: string, MaterialId: string) {
    this.fileProcess.onFileChange($event);
    if (this.fileProcess.totalbyte > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.totalbyte = 0;
    }
    else {
      this.ListFileDatasheet = this.fileProcess.FilesDataBase;
      this.manufactureId = Id;
      this.modelProductMaterials.MaterialId = MaterialId;
    }
    this.fileProcess.FilesDataBase = [];
  }

  updateProduct() {
    var listFileUpload = [];
    this.ListFileProductDocument.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });

    this.uploadService.uploadListFile(this.ListFileProductDocument, 'ModuleManualDocument/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileProductDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.FileType = "1";
          file.Note = listFileUpload[index].Note;
          this.modelProductMaterials.ListFielDocument.push(file);
        });
      }
      for (var item of this.modelProductMaterials.ListFielDocument) {
        if (item.Path == null || item.Path == "") {
          this.ListFileProductDocument.splice(this.ListFileProductDocument.indexOf(item), 1);
        }
      }
      this.uploadService.uploadListFile(this.ListFileSetup, 'ProductMaterials/').subscribe((event: any) => {
        if (event.length > 0) {
          event.forEach(item => {
            var file = Object.assign({}, this.fileSetup);
            file.Path = item.FileUrl;
            file.FileName = item.FileName;
            this.modelProductMaterials.ListFileSetup.push(file);
          });
        }
        this.uploadService.uploadListFile(this.ListFileDatasheet, 'DataSheet/').subscribe((event: any) => {
          if (event.length > 0) {
            event.forEach(item => {
              var file = Object.assign({}, this.fileDatasheet);
              file.FileName = item.FileName;
              file.Size = item.FileSize;
              file.FilePath = item.FileUrl;
              file.ManufactureId = this.manufactureId;
              this.modelProductMaterials.ListFileDatasheet.push(file);
            });
          }
          this.serviceProductmaterials.updateProductMaterials(this.modelProductMaterials).subscribe(
            data => {
              this.messageService.showSuccess('Cập nhật vật tư thành công!');
              this.searchProductMaterials();
              this.getById();
            },
            error => {
              this.messageService.showError(error);
            }
          );

        }, error => {
          this.messageService.showError(error);
        });

      }, error => {
        this.messageService.showError(error);
      });

    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFileProductDocument.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  save() {
    this.updateProduct();
  }

  DownloadAFileSetup(listFileSetup) {
    if (listFileSetup.length == 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    else {
      // var link = document.createElement('a');
      // link.setAttribute("type", "hidden");
      // link.href = this.config.ServerFileApi + SetupFilePath;
      // link.download = "aaaaa";
      // document.body.appendChild(link);
      // link.focus();
      // link.click();
      var listFilePath: any[] = [];
      listFileSetup.forEach(element => {
        listFilePath.push({ Path: element.Path, FileName: element.FileName });
      });
      this.model.Name = "FileSetup";
      this.model.ListDatashet = listFilePath;
      this.serviceProductmaterials.downAllDocumentProcess(this.model).subscribe(data => {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerFileApi + data.PathZip;
        link.download = 'Download.zip';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }, e => {
        this.messageService.showError(e);
      });
    }
  }

  // DownloadAFileDatasheet(ListDatashet) {
  //   if (ListDatashet == null) {
  //     this.messageService.showMessage("Không có file để tải");
  //     return;
  //   }
  //   this.DownloadAllFile(ListDatashet, 0)
  // }

  // DownloadAllFile(ListDatashet, index: number) {
  //   if (index >= ListDatashet.length) {
  //     return;
  //   }
  //   var test = ListDatashet[index].FilePath;
  //   var link = document.createElement('a');
  //   link.setAttribute("type", "hidden");
  //   link.href = this.config.ServerFileApi + test;
  //   link.download = "aaaaa";
  //   document.body.appendChild(link);

  //   setTimeout(() => {
  //     this.DownloadAllFile(ListDatashet, index + 1);
  //   }, 500);
  //   link.click();
  // }

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({ Path: element.FilePath, FileName: element.FileName });
    });
    this.model.Name = "Datasheet";
    this.model.ListDatashet = listFilePath;
    this.serviceProductmaterials.downAllDocumentProcess(this.model).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi']);
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
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Product;
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
}
