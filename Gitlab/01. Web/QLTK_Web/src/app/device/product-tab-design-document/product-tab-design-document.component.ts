import { Component, OnInit, Input, OnDestroy, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService, Configuration, ComponentService } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { ProductService } from '../services/product.service';
import { NgBlockUI, BlockUI } from 'ng-block-ui';

import { ProductChooseFolderUploadModalComponent } from '../product-choose-folder-upload-modal/product-choose-folder-upload-modal.component';

@Component({
  selector: 'app-product-tab-design-document',
  templateUrl: './product-tab-design-document.component.html',
  styleUrls: ['./product-tab-design-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductTabDesignDocumentComponent implements OnInit, OnDestroy {
  private onDownload = new BroadcastEventListener<any>('downloadFileProductDesignDocument');
  private onDownloadFolder = new BroadcastEventListener<any>('downloadFolderProductDesignDocument');
  private downloadSub: Subscription;
  private downloadFolderSub: Subscription;
  @BlockUI() blockUI: NgBlockUI;
  @Input() Id: string;

  startIndex = 1;
  productModel = {
    ProductId: '',
    FolderId: '',
    Path: '',
    ApiUrl: '',
    Token: ''
  };

  folderId: string;
  ListFolder = [];
  ListFile = [];
  ListDesignDocumentId = [];
  folderHeight = 0;
  items = [
    { Id: 1, text: 'Download', icon: 'fa fa-arrow-down text-success' }
  ];

  constructor(
    public constants: Constants,
    private modalService: NgbModal,
    private signalRService: SignalRService,
    private productService: ProductService,
    private messageService: MessageService,
    private config: Configuration,
    private componentService: ComponentService
  ) { }

  ngOnInit() {
    this.folderHeight = window.innerHeight - 140;
    this.productModel.ProductId = this.Id;
    this.getListFolder();

    this.signalRService.listen(this.onDownload, true);
    this.downloadSub = this.onDownload.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {

          this.messageService.showMessage('Download thành công!');
        }
      }
    });

    this.signalRService.listen(this.onDownloadFolder, true);
    this.downloadFolderSub = this.onDownloadFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {
          this.messageService.showMessage('Download thành công!');
        }
      }
    });
  }

  getListFolder() {
    this.productService.getListFolderProduct(this.Id).subscribe((data: any) => {
      if (data) {
        this.ListFolder = data;
        console.log(this.ListFolder);
        for (var item of this.ListFolder) {
          this.ListDesignDocumentId.push(item.Id);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showChooseFolderUpload() {
    let activeModal = this.modalService.open(ProductChooseFolderUploadModalComponent, { container: 'body', windowClass: 'product-choose-folder-upload-modal', backdrop: 'static' });
    activeModal.componentInstance.productId = this.Id;
    activeModal.result.then((result) => {
      this.getListFolder();
    }, (reason) => {
    });
  }

  ngOnDestroy() {
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
    this.downloadFolderSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadFolder);
  }

  onSelectionChanged(e) {
    this.searchFile(e.selectedRowKeys[0]);
    this.folderId = e.selectedRowKeys[0];
  }

  searchFile(folderId: string) {
    this.productModel.FolderId = folderId;
    this.productService.getListFileProduct(folderId).subscribe((data: any) => {
      if (data) {
        this.ListFile = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  downloadFile(id, name, serverPath, designType: any) {
    this.componentService.showChooseFolder(1, 1, this.Id).subscribe(result => {
      if (result) {
        var file = {
          Id: id,
          Name: name,
          ServerPath: serverPath,
          DownloadPath: result,
          Token: '',
          ApiUrl: this.config.ServerApi
        };
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          file.Token = currentUser.access_token;
        }

        this.signalRService.invoke('DownloadFileProductDesignDocument', file).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    });

  }

  itemClick(e) {
    if (!this.folderId) {
      this.messageService.showMessage("Bạn chưa chọn thư mục cần download!")
    } else {
      this.componentService.showChooseFolder(1, 1, this.Id).subscribe(result => {
        if (result) {
          var file = {
            Id: this.folderId,
            DownloadPath: result,
            ObjectId: this.Id,
            Token: '',
            ApiUrl: this.config.ServerApi
          };
          let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
          if (currentUser && currentUser.access_token) {
            file.Token = currentUser.access_token;
          }

          this.signalRService.invoke('DownloadFolderProductDesignDocument', file).subscribe(data => {
            if (data) {
              this.blockUI.start();
            }
            else {
              this.messageService.showMessage("Không kết nối được service");
            }
          });
        }
      });
    }
  }
}
