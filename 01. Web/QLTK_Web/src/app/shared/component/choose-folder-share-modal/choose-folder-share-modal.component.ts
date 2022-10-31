import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { MessageService } from '../../services/message.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Configuration } from '../../config/configuration';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-choose-folder-share-modal',
  templateUrl: './choose-folder-share-modal.component.html',
  styleUrls: ['./choose-folder-share-modal.component.scss']
})
export class ChooseFolderShareModalComponent implements OnInit, OnDestroy {
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private folderSub: Subscription;

  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  selectedId = '';
  ListFolderId = [];
  type: number = 0;
  designType: number = 0;
  id: string;
  checkSelect: boolean = false;

  folderModel = {
    Id: '',
    Path: '',
    SelectPath: '',
    ListFolder: []
  }

  moduleCode: string = '';
  model: any = {}

  constructor(
    private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private config: Configuration
  ) { }

  ngOnInit() {
    if (this.type > 0 || this.moduleCode) {
      this.getLink(this.type);
    } else {
      this.getFolder();
    }
    this.signalRService.listen(this.onSendListFolder, false);
    this.folderSub = this.onSendListFolder.subscribe((data: any) => {
      this.blockUI.stop();
      this.folderModel.ListFolder = data.ListForder;
      if (this.checkSelect && this.type > 0) {
        this.folderModel.SelectPath = data.Path;
        this.selectPathForder();
      }
    });
    this.folderModel.Path = 'D:\\';
  }

  ngOnDestroy() {
    this.folderSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
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
    if (type == 0 && this.moduleCode) {
      this.model = {
        ModuleCode: this.moduleCode,
        Token: '',
        Type: this.designType,
        ApiUrl: this.config.ServerApi,
      };
      this.checkSelect = true;
      let userStore = localStorage.getItem('qltkcurrentUser');
      let currentUser: any = null;
      if (userStore) {
        currentUser = JSON.parse(userStore);
      }
      if (currentUser && currentUser.access_token) {
        this.model.Token = currentUser.access_token;
      }
      this.signalRService.invoke('GetSelectFolder',  this.model).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
    if (type == 1) {
      this.model = {
        ProductId: this.id,
        Token: '',
        DesignType: this.designType,
        ApiUrl: this.config.ServerApi,
      };
      this.checkSelect = true;
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser && currentUser.access_token) {
        this.model.Token = currentUser.access_token;
      }
      this.signalRService.invoke('GetSelectFolderProduct', this.model).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    } else if (type == 2) {
      this.model = {
        ClassRoomId: this.id,
        Token: '',
        DesignType: this.designType,
        ApiUrl: this.config.ServerApi,
      };
      this.checkSelect = true;
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser && currentUser.access_token) {
        this.model.Token = currentUser.access_token;
      }
      this.signalRService.invoke('GetSelectFolderClassRoom', this.model).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    } else if (type == 3) {
      this.model = {
        SolutionId: this.id,
        Token: '',
        DesignType: type,
        ApiUrl: this.config.ServerApi,
      };
      this.checkSelect = true;
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser && currentUser.access_token) {
        this.model.Token = currentUser.access_token;
      }
      this.signalRService.invoke('GetSelectFolderSolution',  this.model).subscribe(data => {
        if (data) {
          this.blockUI.start();
        }
        else {
          this.messageService.showMessage("Không kết nối được service");
        }
      });
    }
  }

  selectPathForder() {
    var data = this.folderModel.ListFolder.filter(i => i.Id == this.folderModel.SelectPath);
    if (data.length > 0) {
      this.selectedId = data[0].Id;
      this.folderModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e) {
    this.folderModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e) {
    this.checkSelect = true;
    this.folderModel.Id = e.key;
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  chooseFolder() {
    this.activeModal.close(this.folderModel.Path);
  }
}
