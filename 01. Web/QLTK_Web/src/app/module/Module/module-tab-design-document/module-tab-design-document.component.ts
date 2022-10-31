import { Component, OnInit, Input, ViewChild, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ModuleTabDesignDocumentService } from '../../services/module-tab-design-document.service';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModuleChooseFileDesignDocumentComponent } from '../module-choose-file-design-document/module-choose-file-design-document.component';
import { ChooseFolderModalComponent } from '../../choose-folder-modal/choose-folder-modal.component';
import { ChooseFolderUploadModalComponent } from '../../choose-folder-upload-modal/choose-folder-upload-modal.component';
import { ModuleChooseFolderDownloadComponent } from '../module-choose-folder-download/module-choose-folder-download.component';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ListPlanDesginComponent } from '../list-plan-desgin/list-plan-desgin.component';
import { ShowChooseProductModuleUpdateComponent } from '../show-choose-product-module-update/show-choose-product-module-update.component';

@Component({
  selector: 'app-module-tab-design-document',
  templateUrl: './module-tab-design-document.component.html',
  styleUrls: ['./module-tab-design-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleTabDesignDocumentComponent implements OnInit, OnDestroy {
  private onDownload = new BroadcastEventListener<any>('downloadFileModuleDesignDocument');
  private onDownloadFolder = new BroadcastEventListener<any>('downloadFolderModuleDesignDocument');
  private downloadSub: Subscription;
  private downloadFolderSub: Subscription;
  @Input() Id: string;
  @Input() Code: string;
  @Input() GroupId: string;
  @BlockUI() blockUI: NgBlockUI;
  startIndex = 1;
  moduleModel = {
    Id: '',
    Code: '',
    GroupId: '',
  };

  documentModel = {
    Id: ''
  }

  folderId: string;
  ListFolder = [];
  ListFile = [];
  ListDesignDocumentId = [];
  listData: any[] = [];
  folderHeight = 400;
  items = [
    { Id: 1, text: 'Download', icon: 'fa fa-arrow-down text-success' }
  ];

  constructor(
    public constants: Constants,
    private designDocumentService: ModuleTabDesignDocumentService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private signalRService: SignalRService,
    private config: Configuration
  ) { }

  ngOnInit() {
    this.folderHeight = window.innerHeight - 140;
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
    this.moduleModel.Id = this.Id;
    this.moduleModel.Code = this.Code;
    this.moduleModel.GroupId = this.GroupId;
    this.designDocumentService.getListFolderModule(this.moduleModel).subscribe((data: any) => {
      if (data) {
        this.ListFolder = data;
        for (var item of this.ListFolder) {
          this.ListDesignDocumentId.push(item.Id);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }
  designType: number;
  onSelectionChanged(e) {
    this.searchFile(e.selectedRowKeys[0]);
    this.folderId = e.selectedRowKeys[0];
    var data = this.ListFolder.filter(i => i.Id == this.folderId);
    if (data.length > 0) {
      this.designType = data[0].DesignType;
    }
  }

  searchFile(folderId: string) {
    this.documentModel.Id = folderId;
    this.designDocumentService.getListFileModule(this.documentModel).subscribe((data: any) => {
      if (data) {
        this.ListFile = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmdeleteModuleDesignDocument(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thư mục này không?").then(
      data => {
        this.deleteModuleDesignDocument(Id);
      },
      error => {
        
      }
    );
  }

  deleteModuleDesignDocument(Id: string) {
    this.designDocumentService.deleteModuleDesignDocument({ Id: Id }).subscribe(
      data => {
        this.getListFolder();
        this.messageService.showSuccess('Xóa thư mục thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showChooseFolderUpload() {
    let activeModal = this.modalService.open(ChooseFolderUploadModalComponent, { container: 'body', windowClass: 'choose-folder-upload-modal', backdrop: 'static' });
    activeModal.componentInstance.ModuleId = this.moduleModel.Id;
    activeModal.componentInstance.ModuleCode = this.moduleModel.Code;
    activeModal.componentInstance.ModuleGroupId = this.moduleModel.GroupId;
    activeModal.result.then((result) => {
      this.getListFolder();
    }, (reason) => {
    });
  }

  showChooseFolder() {
    let activeModal = this.modalService.open(ModuleChooseFolderDownloadComponent, { container: 'body', windowClass: 'module-choose-folder-download', backdrop: 'static' });
    activeModal.componentInstance.ModuleId = this.moduleModel.Id;
    activeModal.componentInstance.FolderId = this.folderId;
  }

  downloadFile(id, name, serverPath) {
    let activeModal = this.modalService.open(ChooseFolderModalComponent, { container: 'body', windowClass: 'choose-folder-modal', backdrop: 'static' });
    activeModal.componentInstance.designType = this.designType;
    activeModal.componentInstance.moduleCode = this.moduleModel.Code;
    activeModal.result.then((result) => {
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

        this.signalRService.invoke('DownloadFileModuleDesignDocument', file).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    }, (reason) => {

    });

  }

  itemClick(e) {
    if (!this.folderId) {
      this.messageService.showMessage("Bạn chưa chọn thư mục cần download!")
    } else {
      let activeModal = this.modalService.open(ChooseFolderModalComponent, { container: 'body', windowClass: 'choose-folder-modal', backdrop: 'static' });
      activeModal.componentInstance.designType = this.designType;
      activeModal.componentInstance.moduleCode = this.moduleModel.Code;
      activeModal.result.then((result) => {
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

          this.signalRService.invoke('DownloadFolderModuleDesignDocument', file).subscribe(data => {
            if (data) {
              this.blockUI.start();
            }
            else {
              this.messageService.showMessage("Không kết nối được service");
            }
          });
        }
      }, (reason) => {

      });
    }
  }

  ngOnDestroy() {
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
    this.downloadFolderSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadFolder);
  }
}
