import { Component, OnInit, Input } from '@angular/core';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Constants, MessageService, Configuration, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { ProductChooseFolderUploadModalComponent } from 'src/app/device/product-choose-folder-upload-modal/product-choose-folder-upload-modal.component';
import { ClassRoomService } from '../../service/class-room.service';
import { ClassRoomChooseFolderUploadModalComponent } from '../class-room-choose-folder-upload-modal/class-room-choose-folder-upload-modal.component';

@Component({
  selector: 'app-class-room-design-document',
  templateUrl: './class-room-design-document.component.html',
  styleUrls: ['./class-room-design-document.component.scss']
})
export class ClassRoomDesignDocumentComponent implements OnInit {

  private onDownload = new BroadcastEventListener<any>('downloadFileClassRoomDesignDocument');
  private onDownloadFolder = new BroadcastEventListener<any>('downloadFolderClassRoomDesignDocument');
  private downloadSub: Subscription;
  private downloadFolderSub: Subscription;
  @BlockUI() blockUI: NgBlockUI;
  @Input() Id: string;

  startIndex = 1;
  classRoomModel = {
    ClassRoomId: '',
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
    private service: ClassRoomService,
    private messageService: MessageService,
    private config: Configuration,
    private componentService: ComponentService
  ) { }

  ngOnInit() {
    this.folderHeight = window.innerHeight - 140;
    this.classRoomModel.ClassRoomId = this.Id;
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
    this.service.getListFolderClassRoom(this.Id).subscribe((data: any) => {
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
    let activeModal = this.modalService.open(ClassRoomChooseFolderUploadModalComponent, { container: 'body', windowClass: 'class-room-choose-folder-upload-modal', backdrop: 'static' });
    activeModal.componentInstance.classRoomId = this.Id;
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
  designTypes: number;
  onSelectionChanged(e) {
    this.searchFile(e.selectedRowKeys[0]);
    this.folderId = e.selectedRowKeys[0];
    this.designTypes = e.selectedRowsData[0].DesignType;
  }

  searchFile(folderId: string) {
    this.classRoomModel.FolderId = folderId;
    this.service.getListFileClassRoom(folderId).subscribe((data: any) => {
      if (data) {
        this.ListFile = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  downloadFile(id, name, serverPath, designType: any) {
    this.componentService.showChooseFolder(2, designType, this.Id).subscribe(result => {
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

        this.signalRService.invoke('DownloadFileClassRoomDesignDocument', file).subscribe(data => {
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
      this.componentService.showChooseFolder(2, this.designTypes, this.Id).subscribe(result => {
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

          this.signalRService.invoke('DownloadFolderClassRoomDesignDocument', file).subscribe(data => {
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
