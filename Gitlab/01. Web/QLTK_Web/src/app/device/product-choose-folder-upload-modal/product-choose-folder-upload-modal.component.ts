import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';

import { Subscription } from 'rxjs';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgBlockUI, BlockUI } from 'ng-block-ui';

import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { MessageService, Configuration, Constants } from 'src/app/shared';
import { ProductService } from '../services/product.service';
import { DxTreeListComponent } from 'devextreme-angular';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-product-choose-folder-upload-modal',
  templateUrl: './product-choose-folder-upload-modal.component.html',
  styleUrls: ['./product-choose-folder-upload-modal.component.scss']
})
export class ProductChooseFolderUploadModalComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private onUploadFolderProduct = new BroadcastEventListener<any>('uploadFolderProduct');
  private notiSub: Subscription;
  private uploadProductSub: Subscription;

  productId: string;
  checkSelect: boolean = false;
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  listCodeRule = [];

  productModel = {
    ProductId: '',
    FolderId: '',
    Path: '',
    ApiUrl: '',
    Token: '',
    DesignType: 0
  };

  folderModel = {
    Id: '',
    Path: '',
    SelectPath: '',
    ListFolder: []
  }

  uploadModel = {
    ModuleId: '',
    ListFile: [],
    DesignType: 0,
    LstError: [],
    Status: false
  }

  searchCodeRuleModel = {
    Code: ''
  }

  constructor(
    private config: Configuration,
    private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private productService: ProductService,
    private modalService: NgbModal,
    public constant: Constants,
    private serviceHistory: HistoryVersionService
  ) { }


  ngOnInit() {
    this.getFolder();
    this.signalRService.listen(this.onSendListFolder, false);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.folderModel.ListFolder = data.ListForder;
        if (this.checkSelect) {
          this.folderModel.SelectPath = data.Path;
          this.selectPathForder();
        }
      }
    });
    this.signalRService.listen(this.onUploadFolderProduct, true);
    this.uploadProductSub = this.onUploadFolderProduct.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.LstError && data.LstError.length > 0) {
          var errorMessage = data.LstError.join("<br/>")
          this.messageService.showMessage(errorMessage);
        }
        else {
          data.ProductId = this.productId;
          data.DesignType = this.productModel.DesignType;
          this.productService.uploadDesignDocument(data).subscribe((data: any) => {
            this.messageService.showSuccess('Upload tài liệu thiết kế thành công!');
          },
            error => {
              this.messageService.showError(error);
            });
        }
      }
    });
  }

  ngOnDestroy() {
    this.notiSub.unsubscribe();
    this.uploadProductSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
    this.signalRService.stopListening(this.onUploadFolderProduct);
  }

  getFolder() {
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  getLink(type) {
    var model = {
      ProductId: this.productId,
      Token: '',
      DesignType: type,
      ApiUrl: this.config.ServerApi,
    };
    this.checkSelect = true;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetSelectFolderProduct', model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  selectPathForder() {
    var data = this.folderModel.ListFolder.filter(i => i.Id == this.folderModel.SelectPath);
    if (data.length > 0) {
      this.selectedId = data[0].Id;
      this.productModel.Path = data[0].Id;
      this.folderModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e) {
    this.selectedId = e.selectedRowKeys[0];
    this.productModel.Path = e.selectedRowKeys[0];
    this.folderModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e) {
    this.checkSelect = false;
    this.folderModel.Id = e.key;
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
    // this.notiSub = this.onSendListFolder.subscribe((data: any) => {
    //   this.folderModel.ListFolder = data;
    // });
  }

  closeModal() {
    this.activeModal.close(true);
  }

  chooseFolder() {
    this.productModel.ProductId = this.productId;
    this.productModel.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.productModel.Token = currentUser.access_token;
    }
    if (this.productModel.DesignType) {
      this.signalRService.invoke('UploadProductDesignDocument', this.productModel).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
    else {
      this.messageService.showMessage("Chưa chọn loại thiết kế. Vui lòng kiểm tra lại!")
    }
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.chooseFolder();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.productId;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Product;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.chooseFolder();
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
