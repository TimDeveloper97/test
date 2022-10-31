import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MessageService, Configuration, AppSetting, Constants, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductCreatesService } from '../services/product-create.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-product-tab-result-test',
  templateUrl: './product-tab-result-test.component.html',
  styleUrls: ['./product-tab-result-test.component.scss']
})
export class ProductTabResultTestComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  @Input() Id: string;
  IsTestResult: boolean;
  constructor(
    private messageService: MessageService,
    private config: Configuration,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public constants: Constants,
    private router: Router,
    private productCreatesService: ProductCreatesService,
    private routeA: ActivatedRoute,
    private modalService: NgbModal,
    public fileProcesService: FileProcess,
    private serviceHistory: HistoryVersionService,
    private productService: ProductService


  ) { }
  userName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;

  modelProduct: any = {
    Id: '',
    ProductId: '',
    ListFileTestAttach: [],
  }
  dateNow = new Date();

  listFile: any[] = [];
  ProductId: string;

  ngOnInit() {
    this.modelProduct.Id = this.Id;
    this.modelProduct.ProductId = this.Id;
    this.getListFileTestAttachByProductId();
  }

  uploadFileTestAttach($event) {
    var fileFileTestAttach = this.fileProcesService.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileFileTestAttach) {
      isExist = false;
      for (var ite of this.modelProduct.ListFileTestAttach) {
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
          this.updateAndReplaceFile(fileFileTestAttach, true);
        }, error => {
          this.updateAndReplaceFile(fileFileTestAttach, false);
        });
    }
    else {
      this.updateFileTestAttach(fileFileTestAttach);
    }
  }
  fileProductTestAttach = {
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    IsFileUpload: false,
    CreateDate: null,
    File: null
  };

  updateAndReplaceFile(fileFileTestAttach, isReplace) {
    var isExist = false;
    for (var file of fileFileTestAttach) {
      for (let index = 0; index < this.modelProduct.ListFileTestAttach.length; index++) {
        if (file.name == this.modelProduct.ListFileTestAttach[index].FileName) {
          isExist = true;
          if (isReplace) {
            this.modelProduct.ListFileTestAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        var files = Object.assign({}, this.fileProductTestAttach);
        files.FileName = file.name;
        files.FileSize = file.size;
        files.IsFileUpload = true;
        files.File = file;
        files.CreateDate = this.dateNow;
        this.modelProduct.ListFileTestAttach.push(files);
      }
    }
  }

  showConfirmDeleteFileTestAttach(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file kết quả kiếm tra này không?").then(
      data => {
        this.modelProduct.ListFileTestAttach.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  updateFileTestAttach(fileFileTestAttach) {
    for (var file of fileFileTestAttach) {
      var files = Object.assign({}, this.fileProductTestAttach);
      files.FileName = file.name;
      files.FileSize = file.size;
      files.IsFileUpload = true;
      files.File = file;
      files.CreateDate = this.dateNow;
      this.modelProduct.ListFileTestAttach.push(files);
    }
  }

  downloadTestAttach(file) {
    this.fileProcesService.downloadFileBlob(file.FilePath, file.FileName);
  }

  async showConfirmUploadVersion() {

    if (this.modelProduct.Id) {
      this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
        async data => {
          if (data) {
            await this.showEditContent();
          } else {
            this.save();
          }
        }
      );
    } else {
      this.save();
    }
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.modelProduct.Id;
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

  save() {
    let listFile: any[] = [];
    let listFileUpload: any[] = [];

    this.modelProduct.ListFileTestAttach.forEach((document, index) => {
      if (document.IsFileUpload) {
        document.Index = index;
        listFile.push(document.File);
        listFileUpload.push(document);
      }
    });

    this.uploadService.uploadListFile(listFile, 'ProductFileTestAttach/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          this.modelProduct.ListFileTestAttach[listFileUpload[index].Index].FilePath = item.FileUrl;
        });
      }
      this.productCreatesService.CreateFileTestAttach(this.modelProduct).subscribe(
        () => {
          this.getListFileTestAttachByProductId();
          this.messageService.showSuccess('Cập nhật kết quả thử nghiệm thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    });

  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi']);
  }

  getListFileTestAttachByProductId() {
    this.productCreatesService.getListFileTestAttachByProductId(this.modelProduct).subscribe(data => {
      if (data != null) {
        this.modelProduct.ListFileTestAttach = data.ListFileTestAttach;
        this.IsTestResult = data.IsTestResult
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  modelStatus: any = {
    ProductId: '',
    Status: 0
  }
  Config() {

    this.modelStatus.ProductId = this.Id;
    this.modelStatus.Status = 1;
    this.productService.checkStatusProduct(this.modelStatus).subscribe(
      data => {
        this.messageService.showSuccess('Chưa xác nhận!');
        this.modelStatus.Status = 1;
        this.IsTestResult = true;
        this.getListFileTestAttachByProductId();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  NotConfig() {

    this.modelStatus.ProductId = this.Id;
    this.modelStatus.Status = 0;
    this.productService.checkStatusProduct(this.modelStatus).subscribe(
      data => {
        this.messageService.showSuccess('Đã xác xác nhận!');
        this.modelStatus.Status = 0;
        this.IsTestResult = false;
        this.getListFileTestAttachByProductId();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}