import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryAnimation, NgxGalleryOptions, NgxGalleryImage } from '@kolkov/ngx-gallery';

import { MessageService, Configuration, AppSetting } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DxTreeListComponent } from 'devextreme-angular';
import { ConfigScanFileComponent } from '../config-scan-file/config-scan-file.component';
import { ChooseConfigScanFileComponent } from '../choose-config-scan-file/choose-config-scan-file.component';
import { RenameFileComponent } from '../rename-file/rename-file.component';

@Component({
  selector: 'app-scan-file',
  templateUrl: './scan-file.component.html',
  styleUrls: ['./scan-file.component.scss']
})
export class ScanFileComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;

  constructor(
    private messageService: MessageService,
    private signalRService: SignalRService,
    public appSetting: AppSetting,
    private config: Configuration,
    private modalService: NgbModal,
  ) {
    this.items = [
      { Id: 1, text: 'Đổi tên file', icon: 'fas fa-copy text-info' },
      { Id: 2, text: 'Di chuyển file', icon: 'fas fa-file-export text-warning' },
      { Id: 3, text: 'Xóa file', icon: 'fas fa-times text-danger' }
    ];
  }

  private onScanFileJPG = new BroadcastEventListener<any>('scanfileJPG');
  private getScanFileJPG: Subscription;
  private onDeleteFile = new BroadcastEventListener<any>('getDelete');
  private getDeleteFile: Subscription;
  private onClear = new BroadcastEventListener<any>('clearCode');
  private getDataScanFile: Subscription;
  private onBase64 = new BroadcastEventListener<any>('getBase64');
  private getBase64: Subscription;

  items: any;
  height = 0;
  check: boolean = false;
  nameFile: string;
  path: string;
  pathForder: string;
  link: string;
  linkZoom: string;
  scanFileId: number;
  listFile: any[] = [];
  listFileId: any[] = [];
  listFolderScan: any[] = [];
  listFoderId: any[] = [];
  gridBoxValue: number[] = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  selectedItems: any[] = [];
  selectedFolder: any[] = [];
  myThumbnail = "https://wittlock.github.io/ngx-image-zoom/assets/thumb.jpg";
  myFullresImage = "https://wittlock.github.io/ngx-image-zoom/assets/fullres.jpg";
  model: any = {
    ModuleCode: '',
    ApiUrl: '',
    CheckDelete: false,
    ListFile: [],
    ListFolderScan: [],
    ListFileScan: []
  }

  fileImage = {
    Id: '',
    Path: '',
    ThumbnailPath: ''
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Đổi tên file";
    this.height = window.innerHeight - 250;
    this.clear();
    this.signalRService.listen(this.onClear, false);
    this.getDataScanFile = this.onClear.subscribe((data: any) => {
      if (data) {
        this.listFile = data.ListFile;
        this.listFolderScan = data.ListFolderScan;
        for (var item of this.listFile) {
          this.listFileId.push(item.Id);
        }
        for (var item of this.listFolderScan) {
          this.listFoderId.push(item.ID);
        }
      }
      else {
        this.messageService.showMessage('Không tồn tại thiết kế cho sản phẩm này trong ổ D!');
      }
      this.blockUI.stop();
    });

    this.signalRService.listen(this.onScanFileJPG, false);
    this.getScanFileJPG = this.onScanFileJPG.subscribe((data: any) => {
      this.blockUI.stop();
      if (data.StatusCode == 1) {
        if (data.Data) {
          this.showConfirmScanFileJPG();
        } else {
          this.messageService.showSuccess('Đổi tên file thành công!');
          this.path = '';
          this.model.CheckDelete = false;
          this.clear();
        }
      } else if (data.StatusCode == 2) {
        this.messageService.showMessage(data.Message);
      }
      else {
        this.messageService.showSuccess('Đổi tên file thất bại!');
      }
    });
    //Xóa file
    this.signalRService.listen(this.onDeleteFile, false);
    this.getDeleteFile = this.onDeleteFile.subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Xóa file thành công!');
        this.link = '';
        this.clear();
      }
      else {
        this.messageService.showWarning('Xóa file thất bại!');
        this.clear();
      }
      this.blockUI.stop();
    });
    //Lấy Base64 ảnh
    this.signalRService.listen(this.onBase64, false);
    this.getBase64 = this.onBase64.subscribe((data: any) => {
      if (data) {
        this.link = 'data:image/jpeg;base64,' + data;
        this.linkZoom = this.link;
      }
      this.blockUI.stop();
    });
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
    this.onBase64.unsubscribe();
    this.onDeleteFile.unsubscribe();
    this.onScanFileJPG.unsubscribe();
    this.onClear.unsubscribe();
    this.signalRService.stopListening(this.onBase64);
    this.signalRService.stopListening(this.onDeleteFile);
    this.signalRService.stopListening(this.onScanFileJPG);
    this.signalRService.stopListening(this.onClear);
  }

  itemClick(e) {
    if (this.scanFileId == null) {
      this.messageService.showMessage("Bạn chưa chọn file!")
    } else {
      if (e.itemData.Id == 1) {
        this.showChooseConfigScanFile();
      } else if (e.itemData.Id == 2) {
        this.showMoveFile();
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteScanFile();
      }
    }
  }

  clear() {
    this.model.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('Clear', this.model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  scanFileJPG() {
    //this.model.ListFile = this.listFile;
    //this.model.ListFolderScan = this.listFolderScan;
    this.model.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('ScanFileJPG', this.model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  showConfirmDeleteScanFile() {
    if (!this.path) {
      this.messageService.showMessage("Bạn chưa chọn File!");
      return;
    }
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        this.signalRService.invoke('DeleteFileScan', this.path).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      },
      error => {

      }
    );
  }

  showConfirmScanFileJPG() {
    this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không?").then(
      data => {
        this.model.CheckDelete = true;
        this.scanFileJPG();
      },
      error => {

      }
    );
  }

  getLinkBase64() {
    this.signalRService.invoke('GetBase64', this.path).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  onSelectionChanged(e) {
    //this.link = 'data:image/jpeg;base64,' + e.selectedRowKeys[0].Base64;
    this.nameFile = e.selectedRowKeys[0].FileName;
    this.path = e.selectedRowKeys[0].FilePath;
    this.scanFileId = e.selectedRowKeys[0].Id;
    this.check = e.selectedRowKeys[0].End;
    if (this.check) {
      this.getLinkBase64();
    }
  }

  onSelectionChangedFolder(e) {
    if (e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.pathForder = e.selectedRowsData[0].FilePath;
    }
  }

  showCreateUpdateConfigScanFile(Id: string) {
    let activeModal = this.modalService.open(ConfigScanFileComponent, { container: 'body', windowClass: 'config-scan-file', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.clear();
      }
    }, (reason) => {
    });
  }

  showChooseConfigScanFile() {
    let activeModal = this.modalService.open(ChooseConfigScanFileComponent, { container: 'body', windowClass: 'choose-config-scan-file', backdrop: 'static' })
    activeModal.componentInstance.Id = this.scanFileId;
    activeModal.componentInstance.moduleCode = this.model.ModuleCode;
    activeModal.result.then((result) => {
      if (result) {
        this.scanFileId = null;
        this.clear();
      }
    }, (reason) => {
    });
  }

  showMoveFile() {
    let activeModal = this.modalService.open(RenameFileComponent, { container: 'body', windowClass: 'rename-file', backdrop: 'static' })
    activeModal.componentInstance.name = this.nameFile;
    activeModal.componentInstance.path = this.path;
    activeModal.componentInstance.pathForder = this.pathForder;
    activeModal.result.then((result) => {
      if (result) {
        this.clear();
      }
    }, (reason) => {
    });
  }
}
