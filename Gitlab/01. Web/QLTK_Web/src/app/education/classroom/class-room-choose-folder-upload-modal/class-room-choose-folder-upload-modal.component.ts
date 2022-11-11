import { Component, OnInit, ViewChild } from '@angular/core';
import { ClassRoomService } from '../../service/class-room.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DxTreeListComponent } from 'devextreme-angular';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { Configuration, MessageService } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-class-room-choose-folder-upload-modal',
  templateUrl: './class-room-choose-folder-upload-modal.component.html',
  styleUrls: ['./class-room-choose-folder-upload-modal.component.scss']
})
export class ClassRoomChooseFolderUploadModalComponent implements OnInit {

  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private onUploadFolderClassRoom = new BroadcastEventListener<any>('uploadFolderClassRoom');
  private notiSub: Subscription;
  private uploadClassRoomSub: Subscription;

  classRoomId: string;
  checkSelect: boolean = false;
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  listCodeRule = [];

  classRoomModel = {
    ClassRoomId: '',
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
    private service: ClassRoomService,
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
    this.signalRService.listen(this.onUploadFolderClassRoom, true);
    this.uploadClassRoomSub = this.onUploadFolderClassRoom.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.LstError && data.LstError.length > 0) {
          var errorMessage = data.LstError.join("<br/>")
          this.messageService.showMessage(errorMessage);
        }
        else {
          data.ClassRoomId = this.classRoomId;
          data.DesignType = this.classRoomModel.DesignType;
          this.service.uploadDesignDocument(data).subscribe((data: any) => {
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
    this.uploadClassRoomSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
    this.signalRService.stopListening(this.onUploadFolderClassRoom);
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
      ClassRoomId: this.classRoomId,
      Token: '',
      DesignType: type,
      ApiUrl: this.config.ServerApi,
    };
    this.checkSelect = true;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetSelectFolderClassRoom', model).subscribe(data => {
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
      this.classRoomModel.Path = data[0].Id;
      this.folderModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e) {
    this.selectedId = e.selectedRowKeys[0];
    this.classRoomModel.Path = e.selectedRowKeys[0];
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
    this.classRoomModel.ClassRoomId = this.classRoomId;
    this.classRoomModel.ApiUrl = this.config.ServerApi;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.classRoomModel.Token = currentUser.access_token;
    }
    if (this.classRoomModel.DesignType) {
      this.signalRService.invoke('UploadClassRoomDesignDocument', this.classRoomModel).subscribe(data => {
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

}
